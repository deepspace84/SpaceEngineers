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

        private bool Initialized = false;
        MyIni IniStorage = new MyIni();
        MyIni IniCustomData = new MyIni();


        private IMyShipWelder WelderObject;
        private IMyProjector ProjectorObject;
        private List<D3Piston> Pistons = new List<D3Piston>();

        // Block counting
        private int BlocksMissing = 0;
        private int BlocksLastCheck = 0;
        private int BlocksCheckRuns = 20;

        private enum States
        {
            Zero = 0,
            XAxis = 1,
            YAxis = 2,
            ZAxis = 3,
        }

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100; // Set Tick update to 100
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (UpdateType.Terminal == updateSource)
            {
                // Check for arguments
                if (argument != "")
                {
                    // Check arguments
                    switch(argument)
                    {
                        case "test":

                            break;
                    }


                }
                else
                {
                    // First start after compilation


                    // Parse CustomData
                    int cdResult = ParseCustomData();
                    if(cdResult == 0)
                    {
                        // First call
                        WriteError("CustomData filled. Please provide data");
                        return;
                    }else if(cdResult == 1)
                    {
                        WriteError("CustomData is incorrect. Please provide correct data or delete all for new configuration");
                        return;
                    }

                    // Check for projector
                    ProjectorObject = (IMyProjector)GridTerminalSystem.GetBlockWithName("D3_Projector");
                    if (null == ProjectorObject)
                    {
                        WriteError("Projector Name not found");
                        return;
                    }
                    else
                    {
                        // Projector found, check for projection
                        if(ProjectorObject.IsProjecting && ProjectorObject.Enabled)
                        {
                            // Get missing count
                            BlocksMissing = ProjectorObject.RemainingBlocks;
                        }
                        else
                        {
                            WriteError("Projector not active or no projection");
                            return;
                        }
                    }

                    // Check for Welder
                    WelderObject = (IMyShipWelder)GridTerminalSystem.GetBlockWithName("D3_Welder");
                    if (null == WelderObject)
                    {
                        WriteError("Welder Name not found");
                        return;
                    }
                    WelderObject.ApplyAction("OnOff_Off");

                    // Check for piston groups

                    // X


                    // Y


                    // Z


                    // Check limits


                    // Zero all


                }
            }

            
        }






        private int ParseCustomData()
        {
            // Check for empty CustomData
            if(Me.CustomData == "")
            {
                // Write default ini

            }
            else
            {
                // Read ini
                MyIniParseResult result;
                if (!IniCustomData.TryParse(Me.CustomData, out result))
                {
                    WriteError("Could not parse CustomData");
                    return 0;
                }

                /* Section => Min/Max Levels
                 *
                 */
                IMyPistonBase piston;
            }

            


        }



        private void WriteError(string errorText)
        {
            Runtime.UpdateFrequency = UpdateFrequency.None;

        }

        private class D3Settings
        {
            public readonly float X_MinLength;
            public readonly float X_MaxLength;
            public readonly float Y_MinLength;
            public readonly float Y_MaxLength;
            public readonly float Z_MinLength;
            public readonly float Z_MaxLength;

            public readonly float X_Blueprint_MaxLength;
            public readonly float Y_Blueprint_MaxLength;
            public readonly float Z_Blueprint_MaxLength;

            public readonly float WelderLength;
            public readonly float WelderPrintLength;

            public D3Settings() { }
        }

        private class D3Piston
        {
            public int pos;
            public D3Piston() { }
        }

        
    }
}
