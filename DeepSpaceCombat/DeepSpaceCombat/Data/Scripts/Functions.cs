using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using VRage.ObjectBuilders;
using VRage.Collections;
using Sandbox.Game.SessionComponents;

namespace DSC
{
    class DSC_Functions
    {
        // References



        // Get faction trade data
        public void SetContract(string fTag)
        {
            HashSet<IMyEntity> entList = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entList, e => e is IMyCubeGrid);
            if (entList.Count == 0)
            {
                MyVisualScriptLogicProvider.SendChatMessage("No grids found", "", 0, "Red");
                return;
            }
            foreach (var ent in entList)
            {
                var grid = ent as IMyCubeGrid;
                var id = grid.EntityId;

                if (grid.DisplayName == "DSC_TARGET")
                {
                    MyVisualScriptLogicProvider.SendChatMessage("Target grid found" + " | CustomName=>" + grid.CustomName, "", 0, "Red");

                    if (adminBlocks["DSC_CONTRACT"] > 0)
                    {
                        MyVisualScriptLogicProvider.SendChatMessage("Contract Point found" + " | ID=>" + adminBlocks["DSC_CONTRACT"].ToString(), "", 0, "Red");

                        long contractId;
                        MyVisualScriptLogicProvider.AddSearchContract(adminBlocks["DSC_CONTRACT"], 5000, 0, 5000, grid.EntityId, 50, out contractId);

                        MyVisualScriptLogicProvider.SendChatMessage("Contract added with id =>" + contractId.ToString(), "", 0, "Red");
                    }

                }
            }



            return;
        }


    }
}
