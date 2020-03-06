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
            // Load Storage
            /*
            string xmlValue;
            if (1==2 && MyAPIGateway.Utilities.GetVariable("DSC_Storage_Factions", out xmlValue))
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Load xml=>" + xmlValue);
                Storage = MyAPIGateway.Utilities.SerializeFromXML<DSC_Storage_Factions>(xmlValue);
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("No Storage exists on loading");
                // Create default values
                Storage = new DSC_Storage_Factions
                {
                    FactionsPlayer = new Dictionary<string, long>(),
                    FactionsNPC = new Dictionary<string, long>(),
                    FactionPlayers = new Dictionary<long, List<long>>(),
                    FactionTechs = new Dictionary<long, List<string>>(),
                    FactionBlocks = new Dictionary<long, List<string>>(),
                };
            }
            */
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_FACTIONS", typeof(DSC_Storage_Factions)))
            {
                try
                {
                    var reader = MyAPIGateway.Utilities.ReadBinaryFileInWorldStorage("DSC_FACTIONS", typeof(DSC_Storage_Factions));
                    Storage = MyAPIGateway.Utilities.SerializeFromBinary<DSC_Storage_Factions>(reader.ReadBytes((int)reader.BaseStream.Length));

                    //runningScripts = MyAPIGateway.Utilities.SerializeFromXML<Dictionary<long, long>>(serialized);
                }
                catch (Exception e)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_FACTIONS loading failed");
                }
            }
            else
            {
                // Create default values
                Storage = new DSC_Storage_Factions
                {
                    FactionsPlayer = new Dictionary<string, long>(),
                    FactionsNPC = new Dictionary<string, long>(),
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

        public void FactionStateChaned(MyFactionStateChange change, long fromFactionId, long toFactionId, long playerId, long senderId)
        {

            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("FactionState=> change:"+change.ToString()+" | fromFaction:"+ fromFactionId.ToString()+" | toFaction:"+toFactionId.ToString()+" | player:"+playerId.ToString()+" | sender:"+senderId.ToString());

            //MyFactionCollection.


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

        /*
         * Save all data to savegame 
         */
        public void Save()
         {
            // Save Storage
            

            byte[] serialized = MyAPIGateway.Utilities.SerializeToBinary<DSC_Storage_Factions>(Storage);
            System.IO.BinaryWriter writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage("DSC_FACTIONS", typeof(DSC_Storage_Factions));
            writer.Write(serialized);

            /*
            var xmlValue = MyAPIGateway.Utilities.SerializeToXML(Storage);
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Unload xml=>" + xmlValue);

            MyAPIGateway.Utilities.SetVariable("DSC_Storage_Factions", xmlValue);
            */
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
        public bool AddTechLevel(long factionID, string techLevel)
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

        public bool RemoveTechLevel(long factionID, string techLevel)
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

        public bool checkTechBlockFaction(long factionID, string techBlock)
        {
            // Check if hashset with this types exists
            if (Storage.FactionBlocks[factionID].Contains(techBlock))
                return true;

            return false;
        }

        /*
        public bool checkTechBlockPlayer(long playerID, string techBlock)
        {
            // Check if hashset with this types exists
            if (Storage.FactionBlocks[factionID].Contains(techBlock))
                return true;

            return false;
        }
        */

        /* Events
         * --------------------------
         */

        //Event - Add grid handler to new grid #TODO Check also the first block
        private void AddGridEvent(IMyEntity ent)
        {
            var grid = ent as MyCubeGrid;
            if (grid?.Physics == null) return;

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
            try
            {
                MyVisualScriptLogicProvider.SendChatMessage("Build by=>" + block.BuiltBy.ToString() + " - " + MyVisualScriptLogicProvider.GetPlayersName(block.BuiltBy) + " | ", "[Server]");
            }
            catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("ERROR: " + ex.Message, "[Server]", 0); }

            MyVisualScriptLogicProvider.SendChatMessage("Def=>" + block.BlockDefinition.ToString(), "[Server]", 0);



        }

        #endregion


        #region faction functions & events


        // Add a new faction to the storage
        public bool AddFaction(string factionTag, bool isNPC)
        {
            if (null != factionTag)
                return false;

            if (MyAPIGateway.Session.Factions.FactionTagExists(factionTag))
            {

                IMyFaction factionObj = MyAPIGateway.Session.Factions.TryGetFactionByTag(factionTag);
                if(null != factionObj)
                {

                    if (isNPC)
                    {
                        Storage.FactionsNPC.Add(factionTag, factionObj.FactionId);
                    }
                    else
                    {
                        Storage.FactionsPlayer.Add(factionTag, factionObj.FactionId);
                    }

                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
 