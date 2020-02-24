using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Collections;
using VRage.Game;
using VRage.ModAPI;
using VRage.Game.Entity;
using VRage.Game.ModAPI;

namespace DSC
{
    class DSC_Blocks
    {
        private static DSC_Blocks _instance;
        private Dictionary<string, long> _blockReference = new Dictionary<string, long>();

        public static DSC_Blocks Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DSC_Blocks();
                return _instance;
            }
        }

        public readonly string FileName = "DSC_Blocks.txt";

        public Dictionary<string, long> BlockReference
        { 
            get
            {
                return _blockReference;
            }
        }

        // Constructor
        private DSC_Blocks()
        {
            _instance = this;
        }

        #region load/unload

        // Load all saved data
        public void Load()
        {
            // Load last block reference
            if (MyAPIGateway.Utilities.GetVariable< Dictionary<string, long>>(FileName, out _blockReference))
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo($"Blocklist: Loaded from savegame (Filename: {FileName})");

                // Loop through all blocks
                foreach (KeyValuePair<string, long> entry in _blockReference)
                {
                    // Check if this block still exists
                    if(null == MyAPIGateway.Entities.GetEntityById(entry.Value))
                    {
                        // Remove block from reference
                        _blockReference.Remove(entry.Key);
                        DeepSpaceCombat.Instance.ServerLogger.WriteWarning($"Blocklist: Block with name {entry.Key} did not exist, so its deleted from BlockReference (Filename: {FileName})");
                    }
                }

                DeepSpaceCombat.Instance.ServerLogger.WriteInfo($"Blocklist: Loading finished (Filename: {FileName})");
            }
            else
            {
                // Could not load block references
                DeepSpaceCombat.Instance.ServerLogger.WriteError($"Blocklist: Could not load from savegame (Filename: {FileName})");
            }
        }

        // Save data
        public void Save()
        {
            MyAPIGateway.Utilities.SetVariable(FileName, _blockReference);
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo($"Blocklist: Saved to savegame (Filename: {FileName})");
        }

        #endregion


        #region block functions

        /// <summary>
        /// Checks if the block with that name exists and returns a list of ids
        /// </summary>
        /// <param name="blockName">Name of block to be found</param>
        /// <returns>List of block ids with that name</returns>
        public List<long> FindBlocksWithID(string blockName)
        {
            List<long> reference = new List<long>();

            // Check for missing blockname
            if (null == blockName || blockName == "")
                return null;

            // Get Reader
            DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> defset = MyDefinitionManager.Static.GetAllDefinitions();

            // Get all entities
            MyConcurrentHashSet<MyEntity> allEntities = MyEntities.GetEntities();

            // Loop through all entities
            foreach (IMyEntity entity in allEntities)
            {
                //Get All grid-entities
                if (entity is IMyCubeGrid)
                {
                    IMyCubeGrid grid = (IMyCubeGrid)entity;

                    //Possible Null-Pointer-Exception
                    try
                    {
                        //Get Terminal Blocks. (Use FatBlocks instead?)
                        List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks = new List<Sandbox.ModAPI.Ingame.IMyTerminalBlock>();
                        Sandbox.ModAPI.Ingame.IMyGridTerminalSystem gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(grid);
                        gts.GetBlocks(blocks);

                        foreach (Sandbox.ModAPI.Ingame.IMyTerminalBlock block in blocks)
                        {
                            //Look for tagged Terminal blocks
                            if (block.CustomName.Contains(blockName))
                            {
                                reference.Add(block.EntityId);
                            }
                        }
                    }
                    catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message, "SYSTEM", 0, "Red"); }
                }
            }

            return reference;
        }

        /// <summary>
        /// Searches for the block with name blockName
        /// </summary>
        /// <param name="blockName">Name of the block to search</param>
        /// <returns>
        /// -2, if blockName is empty or null
        /// -1, if multiple blocks where found
        /// else the block id as long
        /// </returns>
        public long AddBlockWithName(string blockName)
        {
            // Check for missing blockname
            if (string.IsNullOrEmpty(blockName))
                return -2;

            long result = -1;
            List<long> blocks = FindBlocksWithID(blockName);
            // Check if we found only one block! Blockreferences are unique
            if(blocks.Count == 1)
            {
                // Check if this block is allready added
                if (!_blockReference.ContainsKey(blockName))
                {
                    // Add block to reference
                    _blockReference.Add(blockName, blocks[0]);
                }
                result = blocks[0];
            }

            return result;
        }

        /// <summary>
        /// Searches for the block with name blockName
        /// </summary>
        /// <param name="blockName">Name of the block to search</param>
        /// <returns>
        /// -2, if blockName is empty or null
        /// -1, if blockName is not in _blockReference list
        /// else the block id as long
        /// </returns>
        public long GetBlockWithName(string blockName)
        {
            // Check for missing blockname
            if (string.IsNullOrEmpty(blockName))
                return -2;

            // Check if block exists
            if (_blockReference.ContainsKey(blockName))
            {
                // Return id
                return _blockReference[blockName];
            }

            return -1;
        }
        /// <summary>
        /// Searches for the block with name blockName
        /// </summary>
        /// <param name="blockName">Name of the block to search</param>
        /// <returns>
        /// true, if blockName was in the list
        /// false, if blockName was empty, null or not found
        /// </returns>
        public bool RemoveBlockWithName(string blockName)
        {
            // Check for missing blockname
            if (string.IsNullOrEmpty(blockName))
                return false;

            // Check if block exists
            if (_blockReference.ContainsKey(blockName))
            {
                _blockReference.Remove(blockName);
                return true;
            }

            return false;
        }

        #endregion



    }
}
