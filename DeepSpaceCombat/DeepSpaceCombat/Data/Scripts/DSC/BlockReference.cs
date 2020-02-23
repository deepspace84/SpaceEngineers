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
        public readonly string referenceName;
        private Dictionary<string, long> BlockReference = new Dictionary<string, long>();

        // Constructor
        private DSC_Blocks(string refname)
        {
            referenceName = refname;
        }

        #region load/unload

        // Load all saved data
        public void load()
        {
            // Load last block reference
            if (MyAPIGateway.Utilities.GetVariable< Dictionary<string, long>>("DSC_Blocks_"+ referenceName, out BlockReference))
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Blocklist: Loaded from savegame");

                // Loop through all blocks
                foreach (KeyValuePair<string, long> entry in BlockReference)
                {
                    // Check if this block still exists
                    if(null == MyAPIGateway.Entities.GetEntityById(entry.Value))
                    {
                        // Remove block from reference
                        BlockReference.Remove(entry.Key);
                        DeepSpaceCombat.Instance.ServerLogger.WriteWarning("Blocklist: Block with name "+entry.Key+" did not exist, so its deleted from BlockReference");
                    }
                }

                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Blocklist: Loading finished");
            }
            else
            {
                // Could not load block references
                DeepSpaceCombat.Instance.ServerLogger.WriteError("Blocklist: Could not load from savegame");
            }
        }

        // Save data
        public void unload()
        {
            MyAPIGateway.Utilities.SetVariable("DSC_Blocks_"+ referenceName, BlockReference);
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Blocklist: Saved to savegame");
        }

        #endregion


        #region block functions

        /*
         * getBlockId
         * 
         * Checks if the block with that name exists and returns the first occurence
         * If nothing is found its returning 0
         * 
         */
        // Returns the block entity id by name
        public List<long> getBlockId(string blockName)
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

        public long addBlock(string blockName)
        {
            // Check for missing blockname
            if (null == blockName || blockName == "")
                return 0;

            List<long> blocks = getBlockId(blockName);
            // Check if we found only one block! Blockreferences are unique
            if(blocks.Count == 1)
            {
                // Check if this block is allready added
                if (!BlockReference.ContainsKey(blockName))
                {
                    // Add block to reference
                    BlockReference.Add(blockName, blocks[0]);
                }
            }

            return 0;
        }

        public long getBlock(string blockName)
        {
            // Check for missing blockname
            if (null == blockName || blockName == "")
                return 0;

            // Check if block exists
            if (BlockReference.ContainsKey(blockName))
            {
                // Return id
                return BlockReference[blockName];
            }

            return 0;
        }

        public bool removeBlock(string blockName)
        {
            // Check for missing blockname
            if (null == blockName || blockName == "")
                return false;

            // Check if block exists
            if (BlockReference.ContainsKey(blockName))
            {
                // Return id
                BlockReference.Remove(blockName);
                return true;
            }

            return false;
        }

        #endregion



    }
}
