using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.ModAPI;
using VRageMath;

namespace DSC
{
    class RespawnManager
    {

        public RespawnManager() { }



        public void load()
        {

            IMyButtonPanel test;
            test.ButtonPressed += ButtonPressed;

            // Add respawn 
            long blockId = DeepSpaceCombat.Instance.DSCReference.AddBlockWithName(DSC_Config.SpawnBlock);
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
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("Factions::LoadResearchStations: Could not change ownership of block=>" + DSC_Config.SpawnBlock);
                    }

                    // Get Position
                    Vector3D pos = blockEntity.GetPosition();

                    // Create area
                    MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(pos, 5, DSC_Config.SpawnBlock);

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

        public void unload()
        {

        }


        public void ButtonPressed(int test)
        {

        }


    }
}
