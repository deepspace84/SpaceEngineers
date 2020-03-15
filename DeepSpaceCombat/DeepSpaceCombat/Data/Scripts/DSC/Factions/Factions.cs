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

namespace DSC
{
    public class DSC_Factions
    {
        private static DSC_Factions _instance;
        private DSC_Storage_Factions Storage;

        public static DSC_Factions Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DSC_Factions();
                return _instance;
            }
        }


        #region load/unload/save functions for the core.cs
        /*
         * Load all data from savegame and register handlers 
         */
        public void load()
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
         public void unload()
         {
            // Remove block event for progression
            MyAPIGateway.Entities.OnEntityAdd -= AddGridEvent;
            MyAPIGateway.Entities.OnEntityRemove -= RemoveGridEvent;

            // Remove faction state event
            MyAPIGateway.Session.Factions.FactionStateChanged -= FactionStateChaned;
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
            // Delete old entries
            Storage.FactionBlocks.Clear();

            // Loop through factions
            foreach (long factionID in Storage.FactionTechs.Keys)
            {
                // Loop through all tech levels and add block list
                foreach(string techLevel in Storage.FactionTechs[factionID])
                {
                    // Add list 
                    Storage.FactionBlocks[factionID].AddList(DeepSpaceCombat.Instance.Techtree.TechLevels[techLevel].Blocks);
                }
            }
        }

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

        private bool checkTechBlockFaction(long factionID, string techBlock)
        {
            // Check if hashset with this types exists
            if (Storage.FactionBlocks[factionID].Contains(techBlock))
                return true;

            return false;
        }

        /* Events
         * --------------------------
         */

        //Event - Add grid handler to new grid #TODO Check also the first block
        private void AddGridEvent(IMyEntity ent)
        {
            var grid = ent as MyCubeGrid;
            if (grid?.Physics == null) return;

            // Check if its the first block
            if(grid.BlocksCount == 1)
            {
                IMySlimBlock block = grid.CubeBlocks.FirstElement();

                if (!checkTechBlockFaction(Storage.PlayersToFaction[block.BuiltBy], block.BlockDefinition.ToString()))
                {
                    MyVisualScriptLogicProvider.SendChatMessage("You are not allowed to build this block!", "[Server]", block.BuiltBy);

                    // Add item back to player
                    MyVisualScriptLogicProvider.AddToPlayersInventory(block.BuiltBy, DSC_Definitions.Blocks[block.BlockDefinition.ToString()].buildComponent, 1);

                    // Remove block
                    block.CubeGrid.RemoveBlock(block);
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
            MyVisualScriptLogicProvider.SendChatMessage("Blocktype=>"+ block.BlockDefinition.ToString(), "[Server]", block.BuiltBy);

            // Check if block is in definitions
            if (!DSC_Definitions.Blocks.ContainsKey(block.BlockDefinition.ToString()))
            {
                MyVisualScriptLogicProvider.SendChatMessage("This block is not added to the blockreference at all. Please contact an administrator. Block=>"+ block.BlockDefinition.ToString(), "[Server]", block.BuiltBy);
            }

            // Check if player is in a player faction, if not dont allow building at all
            if (!Storage.PlayersToFaction.ContainsKey(block.BuiltBy))
            {
                // Remove block
                block.CubeGrid.RemoveBlock(block);

                // Add item back to player
                MyVisualScriptLogicProvider.AddToPlayersInventory(block.BuiltBy, DSC_Definitions.Blocks[block.BlockDefinition.ToString()].buildComponent , 1);

                return;
            }

            // Check if block building is allowed
            if (!checkTechBlockFaction(Storage.PlayersToFaction[block.BuiltBy], block.BlockDefinition.ToString()))
            {
                MyVisualScriptLogicProvider.SendChatMessage("You are not allowed to build this block!", "[Server]", block.BuiltBy);

                // Add item back to player
                MyVisualScriptLogicProvider.AddToPlayersInventory(block.BuiltBy, DSC_Definitions.Blocks[block.BlockDefinition.ToString()].buildComponent, 1);

                // Remove block
                block.CubeGrid.RemoveBlock(block);
            }
        }

        #endregion


        #region faction functions & events


        // Add a new faction to the storage
        public bool AddFaction(string factionTag, bool isNPC)
        {
            if (null == factionTag)
                return false;

            // Check if faction exists
            if (MyAPIGateway.Session.Factions.FactionTagExists(factionTag))
            {
                // get faction object and check if the id is allready added
                IMyFaction factionObj = MyAPIGateway.Session.Factions.TryGetFactionByTag(factionTag);

                if (Storage.PlayerFactions.ContainsKey(factionObj.FactionId) || Storage.NPCFactions.ContainsKey(factionObj.FactionId))
                    return false;

                if(null != factionObj)
                {
                    // Check if it should be a npc faction
                    if (isNPC)
                    {
                        Storage.NPCFactions.Add(factionObj.FactionId, factionTag);
                    }
                    else
                    {
                        // Add Faction PlayerFactions
                        Storage.PlayerFactions.Add(factionObj.FactionId, factionTag);

                        // Load all existing players and save them to the FactionPlayers reference
                        //Storage.FactionPlayers.Add(factionObj.FactionId, MyVisualScriptLogicProvider.GetFactionMembers(factionTag));

                        foreach(long playerId in factionObj.Members.Keys)
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

            return false;
        }



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
    }
}
 