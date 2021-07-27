using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRage.Game.Entity;
using VRage.Voxels;

namespace JetpackStation
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_HydrogenEngine), true, "HydroSupport")]
    public class CloneBlock : MyGameLogicComponent
    {

        Sandbox.ModAPI.IMyTerminalBlock TerminalalBlock;
        List<IMyPlayer> AllPlayers = new List<IMyPlayer>();
        Vector3D TerminalPos;
        int Counter = 0;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            // Save block entity
            TerminalalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;

            // Get init position
            TerminalPos = TerminalalBlock.CubeGrid.GetPosition();

            MyAPIGateway.Players.GetPlayers(AllPlayers);
        }

        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();

            // Check if block is active
            if (TerminalalBlock.IsWorking)
            {
                // Update players and block location each 10 ticks / 1000 real ticks => equals each second
                Counter++;
                if (Counter == 10)
                {
                    MyAPIGateway.Players.GetPlayers(AllPlayers);
                    TerminalPos = TerminalalBlock.CubeGrid.GetPosition();

                }

                foreach (IMyPlayer player in AllPlayers)
                {
                    if(player.GetRelationTo(player.IdentityId) != MyRelationsBetweenPlayerAndBlock.Enemies)

                    if (Vector3D.Distance(player.GetPosition(), TerminalPos) <= 200)
                    {
                        MyVisualScriptLogicProvider.SetPlayersHydrogenLevel(player.IdentityId, 1);
                    }
                }
            }
        }
    }
}