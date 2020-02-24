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
    class DSC_Grids
    {
        private static DSC_Grids _instance;
        public Dictionary<string, long> _gridReference = new Dictionary<string, long>();

        public static DSC_Grids Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DSC_Grids();
                return _instance;
            }
        }

        public readonly string FileName = "DSC_Grids.txt";

        public Dictionary<string, long> GridReference
        { 
            get
            {
                return _gridReference;
            }
        }

        // Constructor
        private DSC_Grids()
        {
            _instance = this;
        }

        #region load/unload

        // Load all saved data
        public void load()
        {
            // Load last block reference
            if (MyAPIGateway.Utilities.GetVariable< Dictionary<string, long>>(FileName, out _gridReference))
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo($"Blocklist: Loaded from savegame (Filename: {FileName})");

                // Loop through all blocks
                foreach (KeyValuePair<string, long> entry in _gridReference)
                {
                    // Check if this block still exists
                    if(null == MyAPIGateway.Entities.GetEntityById(entry.Value))
                    {
                        // Remove block from reference
                        _gridReference.Remove(entry.Key);
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
        public void unload()
        {
            MyAPIGateway.Utilities.SetVariable(FileName, _gridReference);
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo($"Blocklist: Saved to savegame (Filename: {FileName})");
        }

        #endregion


        #region grid functions

        /// <summary>
        /// Checks if the grid with that name exists and returns the id
        /// </summary>
        /// <param name="gridName">Name of block to be found</param>
        /// <returns>List of block ids with that name</returns>
        public List<long> FindGridsWithID(string gridName)
        {
            List<long> reference = new List<long>();

            // Check for missing blockname
            if (null == gridName || gridName == "")
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
                        if (grid.CustomName.Equals(gridName))
                            reference.Add(grid.EntityId);
                    }
                    catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message, "SYSTEM", 0, "Red"); }
                }
            }

            return reference;
        }

        /// <summary>
        /// Searches for the grids with name gridName
        /// </summary>
        /// <param name="gridName">Name of the grid to search</param>
        /// <returns>
        /// -2, if gridName is empty or null
        /// -1, if multiple grids where found
        /// else the grid id as long
        /// </returns>
        public long AddGridWithName(string gridName)
        {
            // Check for missing blockname
            if (string.IsNullOrEmpty(gridName))
                return -2;

            long result = -1;
            List<long> blocks = FindGridsWithID(gridName);
            // Check if we found only one block! Blockreferences are unique
            if(blocks.Count == 1)
            {
                // Check if this block is allready added
                if (!_gridReference.ContainsKey(gridName))
                {
                    // Add block to reference
                    _gridReference.Add(gridName, blocks[0]);
                }
                result = blocks[0];
            }

            return result;
        }

        /// <summary>
        /// Searches for the grid with name gridName
        /// </summary>
        /// <param name="gridName">Name of the grid to search</param>
        /// <returns>
        /// -2, if gridName is empty or null
        /// -1, if gridName is not in _gridReference list
        /// else the block id as long
        /// </returns>
        public long GetGridWithName(string gridName)
        {
            // Check for missing blockname
            if (string.IsNullOrEmpty(gridName))
                return -2;

            // Check if block exists
            if (_gridReference.ContainsKey(gridName))
            {
                // Return id
                return _gridReference[gridName];
            }

            return -1;
        }
        /// <summary>
        /// Searches for the grid with name gridName
        /// </summary>
        /// <param name="gridName">Name of the grid to search</param>
        /// <returns>
        /// true, if gridName was in the list
        /// false, if gridName was empty, null or not found
        /// </returns>
        public bool RemoveGridWithName(string gridName)
        {
            // Check for missing blockname
            if (string.IsNullOrEmpty(gridName))
                return false;

            // Check if block exists
            if (_gridReference.ContainsKey(gridName))
            {
                _gridReference.Remove(gridName);
                return true;
            }

            return false;
        }

        #endregion



    }
}
