﻿using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.ModAPI;
using Sandbox.Game.Entities;
using VRage.Game.Entity;
using Sandbox.Game.Multiplayer;
using Sandbox.Definitions;
using VRageMath;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Contracts;

namespace DSC
{
    public class DSC_Factions
    {
        private DSC_Storage_Factions Storage;
        private Dictionary<long, List<string>> FactionNextTech = new Dictionary<long, List<string>>();

        private Dictionary<string, List<long>> ResearchStationsPlayers = new Dictionary<string, List<long>>();
        private Dictionary<string, Dictionary<long, DSC_ResearchContract>> ResearchStationsContracts = new Dictionary<string, Dictionary<long, DSC_ResearchContract>>();


        public bool freeBuild = false;

        public DSC_Factions(){}

        #region load/unload/save functions for the core.cs
        /*
         * Load all data from savegame and register handlers 
         */
        public void Load()
        {
            // Check if file exists
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_Storage_Factions", typeof(DSC_Storage_Factions)))
            {
                try
                {
                    var reader = MyAPIGateway.Utilities.ReadBinaryFileInWorldStorage("DSC_Storage_Factions", typeof(DSC_Storage_Factions));
                    Storage = MyAPIGateway.Utilities.SerializeFromBinary<DSC_Storage_Factions>(reader.ReadBytes((int)reader.BaseStream.Length));
                    reader.Dispose();
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DSC_Storage_Factions found and loaded");
                }
                catch (Exception e)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_Storage_Factions loading failed");
                }
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("No DSC_Storage_Factions found, create default");
                // Create default values
                Storage = new DSC_Storage_Factions
                {
                    PlayerFactions = new Dictionary<long, string>(),
                    NPCFactions = new Dictionary<long, string>(),
                    FactionPlayers = new Dictionary<long, List<long>>(),
                    FactionTechs = new Dictionary<long, List<string>>(),
                    FactionBlocks = new Dictionary<long, List<string>>(),
                };
            }

            // Add block event for progression
            MyAPIGateway.Entities.OnEntityAdd += AddGridEvent;
            MyAPIGateway.Entities.OnEntityRemove += RemoveGridEvent;

            // Add Faction state changed event
            MyAPIGateway.Session.Factions.FactionStateChanged += FactionStateChaned;

            // Add gridhandlers to all existing grids
            AddGridHandlers();

            // Calculate FactionNextTech
            foreach(long factionId in Storage.PlayerFactions.Keys)
            {
                RecalulateNextTech(factionId);
            }

            // Load research blocks
            LoadResearchStations();

            // Register Area handlers
            MyVisualScriptLogicProvider.AreaTrigger_Entered += Event_Area_Entered;
            MyVisualScriptLogicProvider.AreaTrigger_Left += Event_Area_Left;

            // Register Contract handlers
            MyAPIGateway.ContractSystem.CustomActivateContract += Event_CustomActivateContract;

        }

        /*
         * Save all data to savegame 
         */
        public void Save()
         {
            // Save Storage
            byte[] serialized = MyAPIGateway.Utilities.SerializeToBinary<DSC_Storage_Factions>(Storage);
            System.IO.BinaryWriter writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage("DSC_Storage_Factions", typeof(DSC_Storage_Factions));
            writer.Write(serialized);
            writer.Flush();
            writer.Dispose();
        }

        /*
         * Unregister handlers 
         */
         public void Unload()
         {
            // Remove Area handlers
            MyVisualScriptLogicProvider.AreaTrigger_Entered -= Event_Area_Entered;
            MyVisualScriptLogicProvider.AreaTrigger_Left -= Event_Area_Left;

            // Remove block event for progression
            MyAPIGateway.Entities.OnEntityAdd -= AddGridEvent;
            MyAPIGateway.Entities.OnEntityRemove -= RemoveGridEvent;

            // Remove faction state event
            MyAPIGateway.Session.Factions.FactionStateChanged -= FactionStateChaned;

            // Remove Contract handlers
            MyAPIGateway.ContractSystem.CustomActivateContract -= Event_CustomActivateContract;
        }
         #endregion


        #region Progression functions & events
        /* Functions
         * --------------------------
         */

        // Adds to all grids the prorgression grid handler at server start
        private void AddGridHandlers()
        {
            HashSet<IMyEntity> entList = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entList, e => e is IMyCubeGrid);
            if (entList.Count == 0)
                return;

            // Loop through all Grids
            foreach (IMyEntity ent in entList)
            {
                MyCubeGrid grid = ent as MyCubeGrid;
                AddGridEvent(grid);
            }
        }

        // Rebuild tech blocks dependend on current TechLevels
        private void RebuildTechBlocks()
        {
            // Loop through factions
            foreach (long factionID in Storage.FactionTechs.Keys)
            {
                // Delete old entries
                Storage.FactionBlocks[factionID].Clear();

                // Loop through all tech levels and add block list
                foreach (string techLevel in Storage.FactionTechs[factionID])
                {
                    // Add list 
                    Storage.FactionBlocks[factionID].AddList(DeepSpaceCombat.Instance.Techtree.TechLevels[techLevel].Blocks);
                }
            }
        }

        private bool checkTechBlockFaction(long factionID, string techBlock)
        {
            // Check for freeBuild
            if (freeBuild) return true;

            // Check if hashset with this types exists
            if (Storage.FactionBlocks[factionID].Contains(techBlock))
                return true;

            return false;
        }

        /* Events
         * --------------------------
         */

        //Event - Add grid handler to new grid
        private void AddGridEvent(IMyEntity ent)
        {
            var grid = ent as MyCubeGrid;
            if (grid?.Physics == null) return;

            // Check if its the first block
            if(grid.BlocksCount == 1)
            {
                IMySlimBlock block = grid.CubeBlocks.FirstElement();

                if (!Storage.PlayersToFaction.ContainsKey(block.BuiltBy))
                {
                    block.CubeGrid.RemoveBlock(block);
                    return;
                }

                if (!checkTechBlockFaction(Storage.PlayersToFaction[block.BuiltBy], block.BlockDefinition.ToString()))
                {
                    MyVisualScriptLogicProvider.ShowNotification("You are not allowed to build this block!", 2500 ,MyFontEnum.Red, block.BuiltBy);

                    // Remove block
                    block.CubeGrid.RemoveBlock(block);

                    return;
                }
            }

            grid.OnBlockAdded += GridBlockAddedEvent;
        }

        // Event - Remove grid handler from removed grid
        private void RemoveGridEvent(IMyEntity ent)
        {
            var grid = ent as MyCubeGrid;
            if (grid?.Physics == null) return;
            
            grid.OnBlockAdded -= GridBlockAddedEvent;
        }

        // Event - New block added to grid | Progression check
        private void GridBlockAddedEvent(IMySlimBlock block)
        {

            // Check if block is in definitions
            if (!DeepSpaceCombat.Instance.Definitions.Blocks.ContainsKey(block.BlockDefinition.ToString()))
            {
                MyVisualScriptLogicProvider.SendChatMessage("This block is not added to the blockreference at all. Please contact an administrator. Block=>"+ block.BlockDefinition.ToString(), "[Server]", block.BuiltBy);
            }

            // Check if player is in a player faction, if not dont allow building at all
            if (!Storage.PlayersToFaction.ContainsKey(block.BuiltBy))
            {
                // Remove block
                block.CubeGrid.RemoveBlock(block);

                // Add item back to player
                MyVisualScriptLogicProvider.AddToPlayersInventory(block.BuiltBy, DeepSpaceCombat.Instance.Definitions.Blocks[block.BlockDefinition.ToString()].buildComponent , 1);

                return;
            }

            // Check if block building is allowed
            if (!checkTechBlockFaction(Storage.PlayersToFaction[block.BuiltBy], block.BlockDefinition.ToString()))
            {
                MyVisualScriptLogicProvider.ShowNotification("You are not allowed to build this block!", 2500 ,MyFontEnum.Red, block.BuiltBy);

                // Add item back to player
                MyVisualScriptLogicProvider.AddToPlayersInventory(block.BuiltBy, DeepSpaceCombat.Instance.Definitions.Blocks[block.BlockDefinition.ToString()].buildComponent, 1);

                // Remove block
                block.CubeGrid.RemoveBlock(block);
            }
        }

        #endregion


        #region faction functions & events


        // Add a new faction to the storage
        public bool AddFaction(string factionTag, bool isNPC)
        {

            try
            {
                if (null == factionTag)
                    return false;

                // Debug
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Faction Tag not null =>"+factionTag.ToString());

                // Check if faction exists
                if (MyAPIGateway.Session.Factions.FactionTagExists(factionTag))
                {
                    // Debug
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Faction tag exists");

                    // get faction object and check if the id is allready added
                    IMyFaction factionObj = MyAPIGateway.Session.Factions.TryGetFactionByTag(factionTag);

                    if (null != factionObj)
                    {
                        // Debug
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Faction object not null");

                        if (Storage.PlayerFactions.ContainsKey(factionObj.FactionId) || Storage.NPCFactions.ContainsKey(factionObj.FactionId))
                            return false;
                        // Debug
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Faction not in storage");

                        // Check if it should be a npc faction
                        if (isNPC)
                        {
                            Storage.NPCFactions.Add(factionObj.FactionId, factionTag);
                        }
                        else
                        {
                            // Debug
                            if(DeepSpaceCombat.Instance.isDebug)DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Faction Added");

                            // Add Faction PlayerFactions
                            Storage.PlayerFactions.Add(factionObj.FactionId, factionTag);

                            // Prepare defaults
                            Storage.FactionBlocks.Add(factionObj.FactionId, new List<string>());
                            Storage.FactionPlayers.Add(factionObj.FactionId, new List<long>());
                            Storage.FactionTechs.Add(factionObj.FactionId, new List<string>());

                            // Add Faction Default tech
                            AddTechLevel(factionObj.FactionId, "LBasic");
                            AddTechLevel(factionObj.FactionId, "SBasic");

                            // Calculate next tech
                            RecalulateNextTech(factionObj.FactionId);

                            // Load all existing players and save them to the FactionPlayers reference
                            foreach (long playerId in factionObj.Members.Keys)
                            {
                                // Only add real players
                                if (MyAPIGateway.Players.TryGetSteamId(playerId) > 0)
                                {
                                    // Add to FactionPlayers list
                                    Storage.FactionPlayers[factionObj.FactionId].Add(playerId);

                                    // Add players to PlayersToFaction
                                    Storage.PlayersToFaction.Add(playerId, factionObj.FactionId);
                                }
                            }

                        }

                    }

                    return true;
                }

            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Add faction failed");
            }
            

            return false;
        }

        // Event for faction changes
        private void FactionStateChaned(MyFactionStateChange change, long fromFactionId, long toFactionId, long playerId, long senderId)
        {

            // Player entered Faction
            if(change == MyFactionStateChange.FactionMemberAcceptJoin)
            {
                // Check if faction exists in storage
                if (Storage.PlayerFactions.ContainsKey(toFactionId))
                {
                    // Add player to reference
                    Storage.FactionPlayers[toFactionId].Add(playerId);
                    Storage.PlayersToFaction.Add(playerId, toFactionId);

                    // Calculate PCU TODO

                }
            }


            // Player left Faction
            if (change == MyFactionStateChange.FactionMemberLeave)
            {
                // Check if faction exists in storage
                if (Storage.PlayerFactions.ContainsKey(toFactionId))
                {
                    // Remove Player from references
                    Storage.FactionPlayers[toFactionId].Remove(playerId);
                    Storage.PlayersToFaction.Remove(playerId);

                    // Remove PCU TODO

                }
            }

            if(change == MyFactionStateChange.RemoveFaction)
            {
                // Add Faction PlayerFactions
                Storage.PlayerFactions.Remove(fromFactionId);

                // Prepare defaults
                Storage.FactionBlocks.Remove(fromFactionId);
                Storage.FactionPlayers.Remove(fromFactionId);
                Storage.FactionTechs.Remove(fromFactionId);
            }


            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("FactionState=> change:" + change.ToString() + " | fromFaction:" + fromFactionId.ToString() + " | toFaction:" + toFactionId.ToString() + " | player:" + playerId.ToString() + " | sender:" + senderId.ToString());
            //MyAPIGateway.Session.Factions.;

            //MyFactionStateChange
            /*
        RemoveFaction = 0,
        SendPeaceRequest = 1,
        CancelPeaceRequest = 2,
        AcceptPeace = 3,
        DeclareWar = 4,
        SendFriendRequest = 5,
        CancelFriendRequest = 6,
        AcceptFriendRequest = 7,
        FactionMemberSendJoin = 8,
        FactionMemberCancelJoin = 9,
        FactionMemberAcceptJoin = 10,
        FactionMemberKick = 11,
        FactionMemberPromote = 12,
        FactionMemberDemote = 13,
        FactionMemberLeave = 14,
        FactionMemberNotPossibleJoin = 15
        */
        }


        #endregion


        #region Research function&events

        // Add new techlevel to faction
        private bool AddTechLevel(long factionID, string techLevel)
        {
            // Check if techlevel exists
            if (DeepSpaceCombat.Instance.Techtree.TechLevels.ContainsKey(techLevel))
            {
                // Add to faction levels
                Storage.FactionTechs[factionID].Add(techLevel);

                // Rebuild tech blocks
                RebuildTechBlocks();

                // Recalculate next tech levels
                RecalulateNextTech(factionID);

                return true;
            }

            return false;
        }

        private bool RemoveTechLevel(long factionID, string techLevel)
        {

            // Check if techlevel exists
            if (DeepSpaceCombat.Instance.Techtree.TechLevels.ContainsKey(techLevel))
            {
                // Remove from faction levels
                Storage.FactionTechs[factionID].Remove(techLevel);

                // Rebuild tech blocks
                RebuildTechBlocks();

                return true;
            }

            return false;
        }

        private void RecalulateNextTech(long factionId)
        {

            // Remove all current if exists
            if (FactionNextTech.ContainsKey(factionId))
            {
                FactionNextTech[factionId].Clear();
            }
            else
            {
                // Add default
                FactionNextTech.Add(factionId, new List<string>());
            }

            // Add now all available
            foreach(string levelName in DeepSpaceCombat.Instance.Techtree.TechLevels.Keys)
            {
                DSC_TechLevel levelObj = DeepSpaceCombat.Instance.Techtree.TechLevels[levelName];

                // Check if allready researched
                if (Storage.FactionTechs[factionId].Contains(levelName))
                {
                    continue;
                }

                // Check if dependend part is available
                if (Storage.FactionTechs[factionId].Contains(levelObj.DependsOn))
                {
                    // Its avail for research
                    FactionNextTech[factionId].Add(levelName);
                }
            }
         }

        private void LoadResearchStations()
        {
            // Loop through all needed blocks
            foreach (string blockName in DSC_Config.ResearchBlocks.Keys)
            {
                // Add block to the global storage
                long blockId = DeepSpaceCombat.Instance.DSCReference.AddBlockWithName(blockName);
                if ( blockId > 0)
                {
                    // Get Block Position
                    IMyEntity blockEntity;
                    if (MyAPIGateway.Entities.TryGetEntityById(blockId, out blockEntity))
                    {
                        // Change owner to our npc
                        MyCubeBlock block = MyAPIGateway.Entities.GetEntityById(blockId) as MyCubeBlock;
                        if (null != block)
                        {
                            block.ChangeOwner(DeepSpaceCombat.Instance.NPCPlayerID, MyOwnershipShareModeEnum.All);
                        }
                        else
                        {
                            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::LoadResearchStations: Could not change ownership of block=>" + blockName);
                        }

                        // Get Position
                        Vector3D pos = blockEntity.GetPosition();

                        // Create area
                        MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(pos, 5, blockName);

                        // Add to reference with empty user list
                        ResearchStationsPlayers.Add(blockName, new List<long>());
                        ResearchStationsContracts.Add(blockName, new Dictionary<long, DSC_ResearchContract>());

                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::LoadResearchStations: Successfully added Researchblock=>" + blockName );
                    }
                    else
                    {
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::LoadResearchStations: Could not find entity with id=>"+ blockId.ToString());
                    }
                }
                else
                {
                    // Block could not be added
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::LoadResearchStations: Could not add/find block in Reference. Blockname=>" + blockName+" | Error=>"+blockId.ToString());
                }
            }
        }

        private void UnloadReserchStations()
        {
            // Loop through reference and remove all areas 
            foreach(string areaName in ResearchStationsPlayers.Keys)
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::LoadResearchStations: Remove Researchblock=>" + areaName);
                MyVisualScriptLogicProvider.RemoveTrigger(areaName);
            }
            ResearchStationsPlayers = null; 
        }

        private void Event_Area_Entered(string name, long playerId)
        {
            // Check if player is in an active faction
            if (!Storage.PlayersToFaction.ContainsKey(playerId)) return;

            // Check if this area is for research
            if (ResearchStationsPlayers.ContainsKey(name))
            {
                // Check if someone is in the area
                if(ResearchStationsPlayers[name].Count > 0)
                {
                    // Check if all from the same faction
                    bool factionCheck = true;
                    foreach (long playerIdCheck in ResearchStationsPlayers[name])
                    {
                        if (Storage.PlayersToFaction[playerId] != Storage.PlayersToFaction[playerIdCheck])
                        {
                            // Not from the same ally
                            factionCheck = false;
                            break;
                        }
                    }

                    if (!factionCheck)
                    {
                        // Someone else joined the area, so delete all contracts
                        RemoveContracts(name);
                    }

                    // Add player to list
                    ResearchStationsPlayers[name].Add(playerId);
                }
                else
                {
                    // Noone is in the area, so build the contracts for this ally
                    AddContracts(name, Storage.PlayersToFaction[playerId]);

                    // Add player to list
                    ResearchStationsPlayers[name].Add(playerId);
                }
            }
         }

        private void Event_Area_Left(string name, long playerId)
        {

            // Check if player is in an active faction
            if (!Storage.PlayersToFaction.ContainsKey(playerId)) return;

            // Check if this area is for research
            if (ResearchStationsPlayers.ContainsKey(name))
            {
                // Remove player and check new conditions
                ResearchStationsPlayers[name].Remove(playerId);

                // Check if someone is left in the are
                if (ResearchStationsPlayers[name].Count > 0)
                {
                    // Check if all from the same faction
                    bool factionCheck = true;
                    long targetFaction = 0;
                    foreach (long playerIdCheck in ResearchStationsPlayers[name])
                    {
                        // Set the faction id, if we fail, we dont use it
                        targetFaction = Storage.PlayersToFaction[playerId];

                        foreach (long playerIdCheckAgain in ResearchStationsPlayers[name])
                        {
                            if (Storage.PlayersToFaction[playerId] != Storage.PlayersToFaction[playerIdCheckAgain])
                            {
                                // Not from the same ally
                                factionCheck = false;
                                break;
                            }
                        }
                    }

                    if (!factionCheck)
                    {
                        // Someone else joined the area, so delete all contracts
                        RemoveContracts(name);
                    }
                    else
                    {
                        AddContracts(name, targetFaction);
                    }
                }
                else
                {
                    // Noone is in the area now so delete all contracts
                    RemoveContracts(name);
                }
            }
        }

        private void Event_CustomActivateContract(long contractId, long playerId)
        {
            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::Event_CustomActivateContract: Contract=>" + contractId.ToString() +" | playe=>"+ playerId.ToString());

            // Get the contract object from our reference
            foreach (string areaName in ResearchStationsContracts.Keys)
            {
                if (ResearchStationsContracts[areaName].ContainsKey(contractId))
                {
                    DSC_ResearchContract contract = ResearchStationsContracts[areaName][contractId];

                    // Check user inventory
                    if(MyVisualScriptLogicProvider.GetPlayersInventoryItemAmount(playerId, DeepSpaceCombat.Instance.Definitions.Compontents["ResearchPoint"]) >= contract.ResearchPoints)
                    {
                        // Remove Research points
                        MyVisualScriptLogicProvider.RemoveFromPlayersInventory(playerId, DeepSpaceCombat.Instance.Definitions.Compontents["ResearchPoint"], contract.ResearchPoints);

                        // Add techlevel to faction and recalculate everything
                        AddTechLevel(contract.FactionId, contract.TechLevel);

                        // Update area with new tech data
                        RemoveContracts(contract.ContractBlockName);
                        AddContracts(contract.ContractBlockName, contract.FactionId);

                        // Send chat message
                        MyVisualScriptLogicProvider.ShowNotification("You have successfully researched the Techlevel: " + contract.TechLevel, 5000, MyFontEnum.Red, playerId);
                    }
                    else
                    {
                        // Player has not enough ResearchPoints

                        // Rebuild tech contracts
                        RemoveContracts(contract.ContractBlockName);
                        AddContracts(contract.ContractBlockName, contract.FactionId);

                        // Send chat message
                        MyVisualScriptLogicProvider.ShowNotification("You have not enough ResearchPoints to research the Techlevel: " + contract.TechLevel + ". Needed Researchpoints:" + contract.ResearchPoints.ToString(), 5000, MyFontEnum.Red, playerId);
                    }

                    // Delete this contract
                    MyAPIGateway.ContractSystem.RemoveContract(contract.ContractId);

                    break;
                }
            }
        }

        private void AddContracts(string areaName, long factionId)
        {
            // get blockid
            long contractBlock = DeepSpaceCombat.Instance.DSCReference.GetBlockWithName(areaName);

            MyDefinitionId def_id;
            MyDefinitionId.TryParse("MyObjectBuilder_ContractTypeDefinition/CustomContract", out def_id);

            // Loop through available techs
            foreach (string techLevel in FactionNextTech[factionId])
            {
                // Check if this tech is available for this block
                if (!DSC_Config.ResearchBlocks[areaName].Contains(DeepSpaceCombat.Instance.Techtree.TechLevels[techLevel].TechArea))
                {
                    continue;
                }

                MyAddContractResultWrapper result = new MyAddContractResultWrapper();
                int reward = 0;
                int collateral = 0;
                int duration = 0;
                int researchPoints = CalculateResearchpoints(techLevel);
                string contract_name = "Research Techlevel: "+techLevel;
                string contract_description = "You can research this Techlevel for "+researchPoints.ToString()+" ResearchPoints. Accept this contract while you have the needed amount of ResearchPoints in your inventory.";
                MyContractCustom contract = new Sandbox.ModAPI.Contracts.MyContractCustom(def_id, contractBlock, reward, collateral, duration, contract_name, contract_description, 0, 0, null);
                // Add conctract
                result = MyAPIGateway.ContractSystem.AddContract(contract);

                // Add contract to reference
                ResearchStationsContracts[areaName].Add(result.ContractId, new DSC_ResearchContract(result.ContractId, areaName, techLevel, researchPoints, factionId));
            }
        }

        private void RemoveContracts(string areaName)
        {
            foreach(long contractId in ResearchStationsContracts[areaName].Keys)
            {
                // Remove contract
                MyAPIGateway.ContractSystem.RemoveContract(contractId);
            }

            // Remove all contracts from this block
            ResearchStationsContracts[areaName].Clear();
        }

        private int CalculateResearchpoints(string techLevel)
        {
            int finalPoints = DeepSpaceCombat.Instance.Techtree.TechLevels[techLevel].ResearchPoints;
            int factionCount = 0;

            // Check for others
            foreach(long factionId in Storage.FactionTechs.Keys)
            {
                if (Storage.FactionTechs[factionId].Contains(techLevel))
                {
                    factionCount++;
                }
            }

            // Calculate points on the ResearchSteps config values
            finalPoints = (int) Math.Floor(finalPoints * DSC_Config.ResearchSteps[factionCount]);

            return finalPoints;
        }

        #endregion
    }

    public class DSC_ResearchContract
    {
        public readonly long ContractId;
        public readonly string ContractBlockName;
        public readonly string TechLevel;
        public readonly int ResearchPoints;
        public readonly long FactionId;

        public DSC_ResearchContract(long contractId, string contractBlockName, string techLevel, int researchPoints, long factionId)
        {
            ContractId = contractId;
            ContractBlockName = contractBlockName;
            TechLevel = techLevel;
            ResearchPoints = researchPoints;
            FactionId = factionId;
        }
    }
}
 