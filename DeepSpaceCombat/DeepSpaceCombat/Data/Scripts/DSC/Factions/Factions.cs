using Sandbox.Game;
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
using VRage.Collections;
using System.Linq;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI.Weapons;
using SpaceEngineers.Game.ModAPI;

namespace DSC
{
    public class DSC_Factions
    {
        public DSC_Storage_Factions Storage;
        private Dictionary<long, List<string>> FactionNextTech = new Dictionary<long, List<string>>();

        private Dictionary<string, List<long>> ResearchStationsPlayers = new Dictionary<string, List<long>>();
        private Dictionary<string, Dictionary<long, DSC_ResearchContract>> ResearchStationsContracts = new Dictionary<string, Dictionary<long, DSC_ResearchContract>>();

        private Dictionary<long, long> WarheadCache = new Dictionary<long, long>(); // EntityId - AttackerId
        private Dictionary<long, List<DamageEventCache>> DamageCache = new Dictionary<long, List<DamageEventCache>>(); // playerId, DamageEvents
        private Dictionary<long, ulong> DamagePreCache = new Dictionary<long, ulong>(); // playerId, damage

        private readonly ConcurrentCachingList<DamageEvent> _preDamageEvents = new ConcurrentCachingList<DamageEvent>();

        public bool freeBuild = true;


        // Damage strut
        private struct DamageEvent
        {
            public readonly long ShipId;
            public readonly long BlockId;
            public readonly long PlayerId;
            public readonly long Tick;

            public DamageEvent(long shipId, long blockId, long playerId, long tick)
            {
                ShipId = shipId;
                BlockId = blockId;
                PlayerId = playerId;
                Tick = tick;
            }

            private bool Equals(DamageEvent other)
            {
                return ShipId == other.ShipId && BlockId == other.BlockId && PlayerId == other.PlayerId && Tick + 2 >= other.Tick;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is DamageEvent && Equals((DamageEvent)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = ShipId.GetHashCode();
                    hashCode = (hashCode * 397) ^ BlockId.GetHashCode();
                    hashCode = (hashCode * 397) ^ PlayerId.GetHashCode();
                    hashCode = (hashCode * 397) ^ Tick.GetHashCode();
                    return hashCode;
                }
            }

            public override string ToString()
            {
                return $"{ShipId} | {BlockId} | {PlayerId} | {Tick}";
            }
        }

        private struct DamageEventCache
        {
            public readonly long TargetId;
            public readonly long AttackerId;
            public readonly float Amount;

            public DamageEventCache(long targetId, long attackerId, float amount)
            {
                TargetId = targetId;
                AttackerId = attackerId;
                Amount = amount;
            }
        }


        public DSC_Factions() { }

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
                    PlayersToFaction = new Dictionary<long, long>(),
                    FactionTechs = new Dictionary<long, List<string>>(),
                    FactionBlocks = new Dictionary<long, List<string>>(),
                    PlayersToPCU = new Dictionary<long, List<long>>(),
                    PlayerDamage = new Dictionary<long, ulong>(),
                    FactionDamage = new Dictionary<long, ulong>(),

                };
            }

            // Add block event for progression
            MyAPIGateway.Entities.OnEntityAdd += AddGridEvent;
            MyAPIGateway.Entities.OnEntityRemove += RemoveGridEvent;

            // Add Faction state changed event
            MyAPIGateway.Session.Factions.FactionStateChanged += FactionStateChaned;
            MyAPIGateway.Session.Factions.FactionCreated += FactionCreated;

            // Add gridhandlers to all existing grids
            AddGridHandlers();

            // Calculate FactionNextTech
            foreach (long factionId in Storage.PlayerFactions.Keys)
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

            // Register Damage handlers
            MyAPIGateway.Session.DamageSystem.RegisterBeforeDamageHandler(1, BeforeDamageHandler);

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
            try
            {
                MyAPIGateway.ContractSystem.CustomActivateContract -= Event_CustomActivateContract;
            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Still could not unload contract handlers");
            }


            // Clear damage caching
            _preDamageEvents?.ClearList();
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
            if (grid.BlocksCount == 1)
            {
                IMySlimBlock block = grid.CubeBlocks.FirstElement();

                if (!Storage.PlayersToFaction.ContainsKey(block.BuiltBy))
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("AddGridEvent:: Block builder not in a faction =>" + block.BuiltBy.ToString());
                    if (block.BuiltBy == 0)
                    {
                        DeepSpaceCombat.Instance.ServerLogger.WriteInfo("AddGridEvent:: Block built by none");
                        return;
                    }

                    block.CubeGrid.RemoveBlock(block);
                    return;
                }

                if (!checkTechBlockFaction(Storage.PlayersToFaction[block.BuiltBy], block.BlockDefinition.ToString()))
                {
                    MyVisualScriptLogicProvider.ShowNotification("You are not allowed to build this block!", 2500, MyFontEnum.Red, block.BuiltBy);

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
                MyVisualScriptLogicProvider.SendChatMessage("This block is not added to the blockreference at all. Please contact an administrator. Block=>" + block.BlockDefinition.ToString(), "[Server]", block.BuiltBy);
            }

            // Check if player is in a player faction, if not dont allow building at all
            if (!Storage.PlayersToFaction.ContainsKey(block.BuiltBy))
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("GridBlockAddedEvent:: Block builder not in a faction =>" + block.BuiltBy.ToString());
                if (block.BuiltBy == 0)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("GridBlockAddedEvent:: Block built by none");
                    return;
                }
                // Remove block
                block.CubeGrid.RemoveBlock(block);

                // Add item back to player
                MyVisualScriptLogicProvider.AddToPlayersInventory(block.BuiltBy, DeepSpaceCombat.Instance.Definitions.Blocks[block.BlockDefinition.ToString()].buildComponent, 1);

                return;
            }

            // Check if block building is allowed
            if (!checkTechBlockFaction(Storage.PlayersToFaction[block.BuiltBy], block.BlockDefinition.ToString()))
            {
                MyVisualScriptLogicProvider.ShowNotification("You are not allowed to build this block!", 2500, MyFontEnum.Red, block.BuiltBy);

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
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Faction Tag not null =>" + factionTag.ToString());

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
                            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Faction Added");

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
            if (change == MyFactionStateChange.FactionMemberAcceptJoin)
            {
                // Check if faction exists in storage
                if (Storage.PlayerFactions.ContainsKey(toFactionId))
                {
                    // Add player to reference
                    Storage.FactionPlayers[toFactionId].Add(playerId);
                    Storage.PlayersToFaction.Add(playerId, toFactionId);

                    // Calculate PCU TODO

                }
                else
                {
                    // Player wants to join a no player faction => Kick him TODO
                    MyVisualScriptLogicProvider.SetPlayersFaction(playerId);
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

            if (change == MyFactionStateChange.RemoveFaction)
            {
                // Remove Faction PlayerFactions
                Storage.PlayerFactions.Remove(fromFactionId);

                // Remove defaults
                Storage.FactionBlocks.Remove(fromFactionId);
                Storage.FactionPlayers.Remove(fromFactionId);
                Storage.FactionTechs.Remove(fromFactionId);
                Storage.FactionDamage.Remove(fromFactionId);
            }


            if (change == MyFactionStateChange.DeclareWar)
            {
                // Check if change is between neutral npc
                if (DeepSpaceCombat.Instance.NPCFactionID == toFactionId)
                {
                    // Reset peace
                    MyAPIGateway.Session.Factions.SendPeaceRequest(fromFactionId, toFactionId);
                    MyAPIGateway.Session.Factions.AcceptPeace(fromFactionId, toFactionId);

                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("FactionState restored");
                }

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

        private void FactionCreated(long factionID)
        {
            // Directly set npc faction states
            MyAPIGateway.Session.Factions.SendPeaceRequest(factionID, DeepSpaceCombat.Instance.NPCFactionID);
            MyAPIGateway.Session.Factions.AcceptPeace(factionID, DeepSpaceCombat.Instance.NPCFactionID);
            MyAPIGateway.Session.Factions.DeclareWar(DeepSpaceCombat.Instance.EnemyFactionID, factionID);
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
            foreach (string levelName in DeepSpaceCombat.Instance.Techtree.TechLevels.Keys)
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
                if (blockId > 0)
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

                        // Because we cant remove triggers at unload any longer, we have to be sure that no old is active, so delete it
                        MyVisualScriptLogicProvider.RemoveTrigger(blockName);

                        // Create area
                        MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(pos, 5, blockName);

                        // Add to reference with empty user list
                        ResearchStationsPlayers.Add(blockName, new List<long>());
                        ResearchStationsContracts.Add(blockName, new Dictionary<long, DSC_ResearchContract>());

                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::LoadResearchStations: Successfully added Researchblock=>" + blockName);
                    }
                    else
                    {
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::LoadResearchStations: Could not find entity with id=>" + blockId.ToString());
                    }
                }
                else
                {
                    // Block could not be added
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::LoadResearchStations: Could not add/find block in Reference. Blockname=>" + blockName + " | Error=>" + blockId.ToString());
                }
            }
        }

        private void Event_Area_Entered(string name, long playerId)
        {
            

            // Check if player is in an active faction
            if (!Storage.PlayersToFaction.ContainsKey(playerId)) return;

            // Check if this area is for research
            if (ResearchStationsPlayers.ContainsKey(name))
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::Event_Area_Entered called");

                // Check if someone is in the area
                if (ResearchStationsPlayers[name].Count > 0)
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
                DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::Event_Area_Left called");

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
            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::Event_CustomActivateContract: Contract=>" + contractId.ToString() + " | playe=>" + playerId.ToString());

            // Get the contract object from our reference
            foreach (string areaName in ResearchStationsContracts.Keys)
            {
                if (ResearchStationsContracts[areaName].ContainsKey(contractId))
                {
                    DSC_ResearchContract contract = ResearchStationsContracts[areaName][contractId];

                    // Check user inventory
                    if (MyVisualScriptLogicProvider.GetPlayersInventoryItemAmount(playerId, DeepSpaceCombat.Instance.Definitions.Compontents["ResearchPoint"]) >= contract.ResearchPoints)
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
                string contract_name = "Research Techlevel: " + techLevel;
                string contract_description = "You can research this Techlevel for " + researchPoints.ToString() + " ResearchPoints. Accept this contract while you have the needed amount of ResearchPoints in your inventory.";
                MyContractCustom contract = new Sandbox.ModAPI.Contracts.MyContractCustom(def_id, contractBlock, reward, collateral, duration, contract_name, contract_description, 0, 0, null);
                // Add conctract
                result = MyAPIGateway.ContractSystem.AddContract(contract);

                // Add contract to reference
                ResearchStationsContracts[areaName].Add(result.ContractId, new DSC_ResearchContract(result.ContractId, areaName, techLevel, researchPoints, factionId));
            }
        }

        private void RemoveContracts(string areaName)
        {
            foreach (long contractId in ResearchStationsContracts[areaName].Keys)
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
            foreach (long factionId in Storage.FactionTechs.Keys)
            {
                if (Storage.FactionTechs[factionId].Contains(techLevel))
                {
                    factionCount++;
                }
            }

            // Calculate points on the ResearchSteps config values
            finalPoints = (int)Math.Floor(finalPoints * DSC_Config.ResearchSteps[factionCount]);

            return finalPoints;
        }

        #endregion

        #region Damage controller

        // Damage handler
        private void BeforeDamageHandler(object target, ref MyDamageInformation info)
        {
            if (info.IsDeformation) return;
            IMySlimBlock block = target as IMySlimBlock;
            if (block == null) return;
            long blockId = 0;
            IMyCubeBlock fatBlock = block.FatBlock;
            if (fatBlock != null) blockId = fatBlock.EntityId;

            ProcessPreDamageReporting(new DamageEvent(block.CubeGrid.EntityId, blockId, info.AttackerId, (int)DeepSpaceCombat.Instance.TickCounter), info);
        }

        // Check for duplicate events
        private void ProcessPreDamageReporting(DamageEvent damage, MyDamageInformation info)
        {
            if (_preDamageEvents.Contains(damage)) return;
            _preDamageEvents.Add(damage);
            _preDamageEvents.ApplyAdditions();
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::ProcessPreDamageReporting success");
            IdentifyDamageDealer(damage.ShipId, damage.BlockId, info);
        }

        private void IdentifyDamageDealer(long damagedEntity, long damagedBlock, MyDamageInformation damageInfo)
        {
            // Deformation damage must be allowed here since it handles grid collision damage
            // One idea may be scan loaded mods and grab their damage types for their ammo as well?  We'll see... 
            // Missiles from vanilla launchers track their damage id back to the player, so if unowned or npc owned, they will have no owner - need to entity track missiles, woo! (on entity add)

            try
            {

                if (damageInfo.AttackerId == 0)
                {
                    // If the attacker is unkown its not a player so we don't track it
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::IdentifyDamageDealer no attacker identified");
                    return;
                }

                IMyEntity attackingEntity;
                if (!MyAPIGateway.Entities.TryGetEntityById(damageInfo.AttackerId, out attackingEntity)) return;
                FindTheAsshole(damagedEntity, damagedBlock, attackingEntity, damageInfo);
            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DamageController::IdentifyDamageDealer");
            }
        }


        private void FindTheAsshole(long damagedEntity, long damagedBlock, IMyEntity attacker, MyDamageInformation damageInfo)
        {
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::FindTheAsshole called damagedEntity->" + damagedEntity.ToString() + "  -  damagedBlock->" + damagedBlock.ToString() + "  -  attacker->" + attacker.EntityId.ToString());

            IMyWarhead warhead = MyAPIGateway.Entities.GetEntityById(damagedBlock) as IMyWarhead;
            if (warhead != null)
            {
                // Check for same id
                if (damagedEntity == attacker.EntityId)
                {
                    // Explosion grid warhead
                    // Only add it one time. Reason is that if you have 2 warheads together on one grid, the second warhead will create a warhead damage event too
                    if (!WarheadCache.ContainsKey(attacker.EntityId))
                    {

                        IMyCubeGrid myCubeGrid = attacker?.GetTopMostParent() as IMyCubeGrid;
                        if (myCubeGrid == null) return;



                        if (myCubeGrid.BigOwners.Count == 0)
                        {   // This should only trigger when a player is being a cheeky fucker
                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::FindTheAsshole => No warhead owners found");
                            return;
                        }

                        IMyIdentity myIdentity = GetIdentityById(myCubeGrid.BigOwners.FirstOrDefault());
                        if (myIdentity != null)
                        {
                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::FindTheAsshole => Warhead added to cache, owner is=>" + MyVisualScriptLogicProvider.GetPlayersName(myIdentity.IdentityId));
                            WarheadCache.Add(attacker.EntityId, myIdentity.IdentityId);
                            return;
                        }
                    }
                }

                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::BeforeDamageHandler => Warhead2 detected =>Entity:" + damagedBlock.ToString());
            }


            if (attacker.GetType() == typeof(MyCubeGrid))
            {

                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::FindTheAsshole MyCubeGrid");
                IdentifyOffendingIdentityFromEntity(damagedEntity, damagedBlock, attacker, damageInfo);
                return;
            }

            if (attacker is IMyLargeTurretBase)
            {
                IdentifyOffendingIdentityFromEntity(damagedEntity, damagedBlock, attacker, damageInfo);
                return;
            }

            IMyCharacter myCharacter = attacker as IMyCharacter;
            if (myCharacter != null)
            {
                AddToDamageQueue(damagedEntity, damagedBlock, myCharacter.EntityId, damageInfo);
                return;
            }

            IMyAutomaticRifleGun myAutomaticRifle = attacker as IMyAutomaticRifleGun;
            if (myAutomaticRifle != null)
            {
                AddToDamageQueue(damagedEntity, damagedBlock, myAutomaticRifle.OwnerIdentityId, damageInfo);
                return;
            }

            IMyAngleGrinder myAngleGrinder = attacker as IMyAngleGrinder;
            if (myAngleGrinder != null)
            {
                AddToDamageQueue(damagedEntity, damagedBlock, myAngleGrinder.OwnerIdentityId, damageInfo);
                return;
            }

            IMyHandDrill myHandDrill = attacker as IMyHandDrill;
            if (myHandDrill != null)
            {
                AddToDamageQueue(damagedEntity, damagedBlock, myHandDrill.OwnerIdentityId, damageInfo);
                return;
            }

            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::FindTheAsshole Asshole not identified =>  It was a: " + attacker.GetType().ToString());
        }



        private void IdentifyOffendingIdentityFromEntity(long damagedEntity, long damagedBlock, IMyEntity offendingEntity, MyDamageInformation damageInfo)
        {
            try
            {
                IMyCubeGrid myCubeGrid = offendingEntity?.GetTopMostParent() as IMyCubeGrid;
                if (myCubeGrid == null) return;



                if (myCubeGrid.BigOwners.Count == 0)
                {
                    // Check warhead cache
                    if (WarheadCache.ContainsKey(offendingEntity.EntityId))
                    {
                        DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::IdentifyOffendingIdentityFromEntity => Warhead cache Data found");
                        AddToDamageQueue(damagedEntity, damagedBlock, WarheadCache[offendingEntity.EntityId], damageInfo);
                        return;
                    }

                    // This should only trigger when a player is being a cheeky fucker
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::IdentifyOffendingIdentityFromEntity => No owners found");
                    return;
                }

                IMyIdentity myIdentity = GetIdentityById(myCubeGrid.BigOwners.FirstOrDefault());
                if (myIdentity != null)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::IdentifyOffendingIdentityFromEntity => Owner found");
                    AddToDamageQueue(damagedEntity, damagedBlock, myIdentity.IdentityId, damageInfo);
                    return;
                }

                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::IdentifyOffendingIdentityFromEntity => Could not find Identity =>" + myCubeGrid.BigOwners.FirstOrDefault().ToString());
            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DamageController::IdentifyOffendingIdentityFromEntity");
            }
        }


        private void AddToDamageQueue(long shipId, long blockId, long playerId, MyDamageInformation damageInfo)
        {
            // Check if player is in a registered faction
            if (!Storage.PlayersToFaction.ContainsKey(playerId))
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::AddToDamageQueue => Attacker is in no player faction =>" + playerId.ToString() + " - " + MyVisualScriptLogicProvider.GetPlayersName(playerId));
                return;
            }

            MyCubeBlock block = MyAPIGateway.Entities.GetEntityById(blockId) as MyCubeBlock;
            if (null != block)
            {
                // Stop here if the block owner is empty
                if (block.OwnerId == 0) return;

                // Check if target is in a faction
                if (Storage.PlayersToFaction.ContainsKey(block.OwnerId))
                {
                    // Check if its the same faction as the attacker
                    if (Storage.PlayersToFaction.ContainsKey(block.OwnerId) == Storage.PlayersToFaction.ContainsKey(playerId))
                    {
                        DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::AddToDamageQueue => Self damage, stop now");
                        return;
                    }

                }

                // Check if player is in DamageCache
                if (!DamageCache.ContainsKey(playerId))
                {
                    DamageCache.Add(playerId, new List<DamageEventCache>());
                }

                // Add Damage to cache
                DamageCache[playerId].Add(new DamageEventCache(block.OwnerId, playerId, damageInfo.Amount));

                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::AddToDamageQueue => target block entity found->" + block.OwnerId.ToString() + " - " + MyVisualScriptLogicProvider.GetPlayersName(block.OwnerId));
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::AddToDamageQueue => attacker entity found->" + playerId.ToString() + " - " + MyVisualScriptLogicProvider.GetPlayersName(playerId));
                return;

            }
            else
            {
                // Its not a fatBlock, so check grid
                MyCubeGrid grid = MyAPIGateway.Entities.GetEntityById(shipId) as MyCubeGrid;
                if (null != grid)
                {
                    if (grid.BigOwners.Count == 0)
                    {   // This should only trigger when a player is being a cheeky fucker
                        DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::AddToDamageQueue => No attacker grid owner found");
                        return;
                    }

                    // Check if target is in a faction
                    if (Storage.PlayersToFaction.ContainsKey(grid.BigOwners.FirstOrDefault()))
                    {
                        // Check if its the same faction as the attacker
                        if (Storage.PlayersToFaction.ContainsKey(grid.BigOwners.FirstOrDefault()) == Storage.PlayersToFaction.ContainsKey(playerId))
                        {
                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::AddToDamageQueue => Self damage, stop now");
                            return;
                        }

                    }

                    // Check if player is in DamageCache
                    if (!DamageCache.ContainsKey(playerId))
                    {
                        DamageCache.Add(playerId, new List<DamageEventCache>());
                    }

                    // Add Damage to cache
                    DamageCache[playerId].Add(new DamageEventCache(grid.BigOwners.FirstOrDefault(), playerId, damageInfo.Amount));

                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::AddToDamageQueue => Grid entity found" + grid.BigOwners.FirstOrDefault().ToString() + " - " + MyVisualScriptLogicProvider.GetPlayersName(grid.BigOwners.FirstOrDefault()));

                    return;
                }

                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DamageController::AddToDamageQueue => blockId->" + blockId + " Entity not found in the block and grid");
            }
        }


        public void DamageController()
        {

            // Copy & Clear Cache
            Dictionary<long, List<DamageEventCache>> DamageCheck = new Dictionary<long, List<DamageEventCache>>(DamageCache);
            DamageCache.Clear();

            Dictionary<long, ulong> PreCheck = new Dictionary<long, ulong>(DamagePreCache);
            DamagePreCache.Clear();

            // Check damages
            foreach (KeyValuePair<long, List<DamageEventCache>> kvp in DamageCheck)
            {

                ulong playerTotal = 0;

                // Loop through damage events
                foreach (DamageEventCache dmgEvent in kvp.Value)
                {
                    // Check if damage is done to any faction member
                    int storageDamge = (int)dmgEvent.Amount;

                    if (Storage.PlayersToFaction.ContainsKey(dmgEvent.TargetId))
                    {
                        // Add faction multiplier
                        storageDamge = storageDamge * DSC_Config.DamageAllyMultiplier;
                    }

                    // Check if player is in storage
                    if (!Storage.PlayerDamage.ContainsKey(dmgEvent.AttackerId))
                    {
                        Storage.PlayerDamage.Add(dmgEvent.AttackerId, 0);
                    }

                    // Store damage to storage
                    Storage.PlayerDamage[dmgEvent.AttackerId] += (ulong)storageDamge;

                    // Add to total
                    playerTotal += (ulong)dmgEvent.Amount;
                }

                // Check for preDamage
                if (PreCheck.ContainsKey(kvp.Key))
                {
                    MyVisualScriptLogicProvider.ShowNotification(playerTotal.ToString() + " - " + PreCheck[kvp.Key], 1000, "Red", kvp.Key);

                    // Add to new precheck
                    DamagePreCache.Add(kvp.Key, playerTotal + PreCheck[kvp.Key]);
                }
                else
                {
                    MyVisualScriptLogicProvider.ShowNotification(playerTotal.ToString(), 1000, "Red", kvp.Key);

                    // Add to new precheck
                    DamagePreCache.Add(kvp.Key, playerTotal);
                }
            }

        }

        private IMyIdentity GetIdentityById(long playerId)
        {
            List<IMyIdentity> identityList = new List<IMyIdentity>();
            MyAPIGateway.Players.GetAllIdentites(identityList);
            return identityList.FirstOrDefault(x => x.IdentityId == playerId);



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
