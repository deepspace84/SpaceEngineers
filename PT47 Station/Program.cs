using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        int _step = 0;
        bool toggle = true;

        int StepCounter = 0;

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.None; // Set Tick update to 100
        }

        IMyTextPanel LCDObject;

        public void Save()
        {

        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (argument.Equals("print"))
            {
                LCDObject = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("Torp_LCD");

                Runtime.UpdateFrequency = UpdateFrequency.Update100;
                return;
            }else if (argument.Equals("start"))
            {
                IMyShipMergeBlock MergeObject = (IMyShipMergeBlock)GridTerminalSystem.GetBlockWithName("Torpedo Merger #A#");
                if (MergeObject != null)
                {
                    MergeObject.ApplyAction("OnOff_Off");
                }
                else
                {
                    WriteError("Not found => Torpedo Merger #A#");
                }
                Echo("Torpedo rdy");
                return;
            }

            StepCounter--;
            if(StepCounter <= 0)
            {
                _step++;
            }
            else
            {
                if (toggle)
                {
                    Echo("Step "+ StepCounter+" | " + _step + " +");
                    LCDObject.WriteText("Step " + StepCounter + " | " + _step + " +");
                    toggle = false;
                }
                else
                {
                    Echo("Step "+ StepCounter + " | " + _step + " -");
                    LCDObject.WriteText("Step " + StepCounter + " | " + _step + " -");
                    toggle = true;
                }
                
                return;
            }

            switch (_step)
            {
                case 1: // Projector on

                    IMyProjector ProjectorObject = (IMyProjector)GridTerminalSystem.GetBlockWithName("TorpProjector");
                    if (ProjectorObject != null)
                    {
                        ProjectorObject.ApplyAction("OnOff_On");

                        StepCounter = 1;
                        Echo("Step 1 running");
                    }
                    else
                    {
                        WriteError("Projector not found => TorpProjector");
                    }

                    break;

                case 2: // Welder on
                    IMyShipWelder WelderObject = (IMyShipWelder)GridTerminalSystem.GetBlockWithName("TorpWelder");
                    IMyShipWelder WelderSmallObject = (IMyShipWelder)GridTerminalSystem.GetBlockWithName("TorpWelderSmall");
                    IMyShipWelder WelderSmall2Object = (IMyShipWelder)GridTerminalSystem.GetBlockWithName("TorpWelderSmall2");
                    if (WelderObject != null && WelderSmallObject != null)
                    {
                        WelderObject.ApplyAction("OnOff_On");
                        WelderSmallObject.ApplyAction("OnOff_On");
                        WelderSmall2Object.ApplyAction("OnOff_On");

                        StepCounter = 80;
                    }
                    else
                    {
                        WriteError("Welder not found => TorpWelder");
                    }

                    break;
                case 3: // Connector on

                    IMyShipConnector ConnectorObject3 = (IMyShipConnector)GridTerminalSystem.GetBlockWithName("Torpedo Connector #A#");
                    if (ConnectorObject3 != null)
                    {
                        ConnectorObject3.ApplyAction("OnOff_On");
                    }
                    else
                    {
                        WriteError("Not found => Torpedo Connector #A#");
                    }

                    break;

                case 4: // Connector activate & Tank stockpile & Welder off && Projector off

                    IMyShipConnector ConnectorObject = (IMyShipConnector)GridTerminalSystem.GetBlockWithName("Torpedo Connector #A#");
                    if (ConnectorObject != null)
                    {
                        ConnectorObject.ApplyAction("OnOff_On");
                        ConnectorObject.ApplyAction("Lock");
                    }
                    else
                    {
                        WriteError("Not found => Torpedo Connector #A#");
                    }

                    IMyGasTank HydroTankObject = (IMyGasTank)GridTerminalSystem.GetBlockWithName("Torpedo Tank #A#");
                    if (HydroTankObject != null)
                    {
                        HydroTankObject.ApplyAction("Stockpile_On");
                    }
                    else
                    {
                        WriteError("Not found => Torpedo Tank #A#");
                    }

                    IMyShipWelder WelderObject2 = (IMyShipWelder)GridTerminalSystem.GetBlockWithName("TorpWelder");
                    IMyShipWelder WelderSmallObject2 = (IMyShipWelder)GridTerminalSystem.GetBlockWithName("TorpWelderSmall");
                    IMyShipWelder WelderSmall2Object2 = (IMyShipWelder)GridTerminalSystem.GetBlockWithName("TorpWelderSmall2");
                    if (WelderObject2 != null && WelderSmallObject2 != null)
                    {
                        WelderObject2.ApplyAction("OnOff_Off");
                        WelderSmallObject2.ApplyAction("OnOff_Off");
                        WelderSmall2Object2.ApplyAction("OnOff_Off");
                    }
                    else
                    {
                        WriteError("Welder not found => TorpProjector");
                    }

                    IMyProjector ProjectorObject2 = (IMyProjector)GridTerminalSystem.GetBlockWithName("TorpProjector");
                    if (ProjectorObject2 != null)
                    {
                        ProjectorObject2.ApplyAction("OnOff_Off");
                    }
                    else
                    {
                        WriteError("Projector not found => TorpProjector");
                    }

                    StepCounter = 10;
                    break;

                case 5: // Connector deactivate & Tank stockpile off

                    IMyShipConnector ConnectorObject2 = (IMyShipConnector)GridTerminalSystem.GetBlockWithName("Torpedo Connector #A#");
                    if (ConnectorObject2 != null)
                    {
                        ConnectorObject2.ApplyAction("OnOff_Off");
                    }
                    else
                    {
                        WriteError("Not found => Torpedo Connector #A#");
                    }

                    IMyGasTank HydroTankObject2 = (IMyGasTank)GridTerminalSystem.GetBlockWithName("Torpedo Tank #A#");
                    if (HydroTankObject2 != null)
                    {
                        HydroTankObject2.ApplyAction("Stockpile_Off");
                    }
                    else
                    {
                        WriteError("Not found => Torpedo Tank #A#");
                    }

                    Echo("Step 4 finished");

                    _step = 0;
                    toggle = true;
                    StepCounter = 0;

                    Runtime.UpdateFrequency = UpdateFrequency.None;
                    break;
            }
        }

        private void WriteError(string errorText)
        {
            Echo(errorText);
            Runtime.UpdateFrequency = UpdateFrequency.None;
        }

    }
}
