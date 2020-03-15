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
    //Class name should equal filename
    public class DSC_Reference
    {
        private static DSC_Reference _instance;
        private DSC_Storage_Reference Storage;

        public static DSC_Reference Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DSC_Reference();
                }
                return _instance;
            }
        }


        // Constructor
        public DSC_Reference(){}

        #region load/unload

        // Load all saved data
        public void Load()
        {
            // Check if file exists
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_Storage_Reference", typeof(DSC_Storage_Reference)))
            {
                try
                {
                    var reader = MyAPIGateway.Utilities.ReadBinaryFileInWorldStorage("DSC_Storage_Reference", typeof(DSC_Storage_Reference));
                    Storage = MyAPIGateway.Utilities.SerializeFromBinary<DSC_Storage_Reference>(reader.ReadBytes((int)reader.BaseStream.Length));
                    reader.Dispose();
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DSC_Storage_Reference found and loaded");
                }
                catch (Exception e)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_Storage_Reference loading failed");
                }
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("No DSC_Storage_Reference found, create default");
                // Create default values
                Storage = new DSC_Storage_Reference
                {
                    Blocks = new Dictionary<string, long>(),
                    Grids = new Dictionary<string, long>(),
                };
            }

        }

        // Save data
        public void Save()
        {
            // Save Storage
            byte[] serialized = MyAPIGateway.Utilities.SerializeToBinary<DSC_Storage_Reference>(Storage);
            System.IO.BinaryWriter writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage("DSC_Storage_Reference", typeof(DSC_Storage_Reference));
            writer.Write(serialized);
            writer.Flush();
            writer.Dispose();
        }

        // Unload
        public void Unload()
        {

        }

        #endregion


        #region block functions

        /// <summary>
        /// Checks if the block with that name exists and returns a list of ids
        /// </summary>
        /// <param name="blockName">Name of block to be found</param>
        /// <returns>List of block ids with that name</returns>
        public List<long> FindBlocksWithName(string blockName)
        {
            List<long> reference = new List<long>();

            // Check for missing blockname
            if (null == blockName || blockName == "")
                return null;

            // Get all grid entities
            HashSet<IMyEntity> entList = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entList, e => e is IMyCubeGrid);
            if (entList.Count == 0)
                return null;

            // IMyEntity myEntity = MyAPIGateway.Entities.GetEntityByName(blockName);not wirking if multiple blocks with that name exists

            // Loop through all Grids
            foreach (IMyEntity ent in entList)
            {
                MyCubeGrid grid = ent as MyCubeGrid;
                long gridId = grid.EntityId;

                foreach (MyCubeBlock fb in grid.GetFatBlocks())
                {
                    //Look for tagged Terminal blocks
                    if (blockName.Equals(fb.DisplayNameText))
                    {
                        reference.Add(fb.EntityId);
                    }
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
            List<long> blocks = FindBlocksWithName(blockName);
            // Check if we found only one block! Blockreferences are unique
            if(blocks.Count == 1)
            {
                // Check if this block is allready added
                if (!Storage.Blocks.ContainsKey(blockName))
                {
                    // Add block to reference
                    Storage.Blocks.Add(blockName, blocks[0]);
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
        /// -1, if multiple blocks where found
        /// else the block id as long
        /// </returns>
        public long GetBlockWithName(string blockName)
        {
            // Check for missing blockname
            if (string.IsNullOrEmpty(blockName))
                return -2;

            // Check if block exists
            if (Storage.Blocks.ContainsKey(blockName))
            {
                // Return id
                return Storage.Blocks[blockName];
            }
            else
            {
                return AddBlockWithName(blockName);
            }
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
            if (Storage.Blocks.ContainsKey(blockName))
            {
                Storage.Blocks.Remove(blockName);
                return true;
            }

            return false;
        }

        #endregion


        #region grid functions

        /// <summary>
        /// Checks if the grid with that name exists and returns the id
        /// </summary>
        /// <param name="gridName">Name of block to be found</param>
        /// <returns>List of block ids with that name</returns>
        public List<long> FindGridsWithName(string gridName)
        {
            List<long> reference = new List<long>();

            // Check for missing blockname
            if (null == gridName || gridName == "")
                return null;

            // Get all grid entities
            HashSet<IMyEntity> entList = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entList, e => e is IMyCubeGrid);
            if (entList.Count == 0)
                return null;

            // Loop through all Grids
            foreach (IMyEntity ent in entList)
            {
                MyCubeGrid grid = ent as MyCubeGrid;

                if (gridName.Equals(grid.DisplayName))
                {
                    reference.Add(grid.EntityId);
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
            List<long> blocks = FindGridsWithName(gridName);
            // Check if we found only one block! Blockreferences are unique
            if (blocks.Count == 1)
            {
                // Check if this block is allready added
                if (!Storage.Grids.ContainsKey(gridName))
                {
                    // Add block to reference
                    Storage.Grids.Add(gridName, blocks[0]);
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
        /// -1, if multiple grids where found
        /// else the block id as long
        /// </returns>
        public long GetGridWithName(string gridName)
        {
            // Check for missing blockname
            if (string.IsNullOrEmpty(gridName))
                return -2;

            // Check if block exists
            if (Storage.Grids.ContainsKey(gridName))
            {
                // Return id
                return Storage.Grids[gridName];
            }

            return AddGridWithName(gridName);
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
            if (Storage.Grids.ContainsKey(gridName))
            {
                Storage.Grids.Remove(gridName);
                return true;
            }

            return false;
        }

        #endregion
    }
}
