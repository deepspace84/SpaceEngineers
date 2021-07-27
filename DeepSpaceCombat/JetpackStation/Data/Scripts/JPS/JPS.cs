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
using ProtoBuf;

namespace JetpackStation
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_HydrogenEngine), true, "JetpackStation")]
    public class CloneBlock : MyGameLogicComponent
    {

        private Sandbox.ModAPI.IMyTerminalBlock TerminalalBlock;
        private List<IMyPlayer> AllPlayers = new List<IMyPlayer>();
        private Vector3D TerminalPos;
        private int Counter = 0;
        private int StationRange = 1000;
        private int ShipRange = 500;
        private int CheckRange = 0;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            // Save block entity
            TerminalalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
        }

        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();

            if(Counter == 5)
            {
                // Check if block engine is working
                if (TerminalalBlock.IsWorking)
                {
                    // Update players and block position
                    MyAPIGateway.Players.GetPlayers(AllPlayers);
                    TerminalPos = TerminalalBlock.CubeGrid.GetPosition();
                    Counter = 0;

                    if (AllPlayers.Count == 0) return;

                    // Check always range so we don't need to catch events
                    if (TerminalalBlock.CubeGrid.IsStatic)
                    {
                        CheckRange = StationRange;
                    }else CheckRange = ShipRange;

                    foreach (IMyPlayer player in AllPlayers)
                    {
                        if (player.GetRelationTo(player.IdentityId) != MyRelationsBetweenPlayerAndBlock.Enemies)
                        {
                            if (Vector3D.Distance(player.GetPosition(), TerminalPos) <= CheckRange)
                            {
                                MyVisualScriptLogicProvider.SetPlayersHydrogenLevel(player.IdentityId, 1);
                            }
                        }
                    }
                }

            }else Counter++;
        }
    }
}