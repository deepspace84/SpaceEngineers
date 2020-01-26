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


        // Global definitions
        public float pistonDefaultSpeed = 0.3f;

        // Programm variables
        private bool d3_init = false; // Initialization flag
        private bool d3_Running = false;

        private bool d3_Status = false; // 0 = Off / 1=Printing
        private string d3_activeAxis = "y";
        private bool d3_pdirection_x = false;
        private bool d3_pdirection_y = false;
        private bool d3_pdirection_z = false;

        MyIni _ini = new MyIni();

        // Ini variables
        private float d3_ini_posMax_x = 0;
        private float d3_ini_posMax_y = 0;
        private float d3_ini_posMax_z = 0;
        private int d3_ini_zeroLevel = 0;

        // Piston variables
        private float d3_posMax_x = 140;
        private float d3_posMax_y = 70;
        private float d3_posMax_z = 140;

        // true => zero to max | false => max to zero
        private bool d3_direction_x = true;
        private bool d3_direction_y = false;
        private bool d3_direction_z = false;

        //Block-Object variables
        private IMyPistonBase pistonObject;
        private IMyShipWelder welderObject;
        private IMyProjector projectorObject;

        // Prjector counter
        private int proj_checks = 0;
        private int proj_count = 0;

        // Welder name
        private string welderName = "3D_Welder";
        private string projectorName = "3D_Projector";

        private int debug_counter = 0;

        /* Piston declaration
         */

        // X
        List<string> d3_pist_x = new List<string>()
        {"3D_Pist_Forward_0","3D_Pist_Forward_1","3D_Pist_Forward_2","3D_Pist_Forward_3"};

        // Y
        List<string> d3_pist_y = new List<string>()
        {"3D_Pist_Side_0","3D_Pist_Side_1","3D_Pist_Side_2"};

        // Z
        List<string> d3_pist_z = new List<string>()
        {"3D_Pist_Down_0","3D_Pist_Down_1","3D_Pist_Down_2","3D_Pist_Down_3"};

        // Piston dictoinary
        private IDictionary<int, piston> dPiston = new Dictionary<int, piston>();

        /*
         *  Classes
         * -------------------------------------------------
         */

        public class piston
        {
            public piston(int id, string name, string axis, bool direction, float maxlength, float printLength, IMyPistonBase grid)
            {
                sName = name;
                iId = id;
                oGrid = grid;
                saxis = axis;
                bDirection = direction;
                fMaxLength = maxlength;
                fPrintLength = printLength;
            }

            private int iId;

            public int id
            {
                get { return iId; }
                set { iId = value; }
            }

            private string sName;

            public string name
            {
                get { return sName; }
                set { sName = value; }
            }

            private IMyPistonBase oGrid;

            public IMyPistonBase grid
            {
                get { return oGrid; }
                set { oGrid = value; }
            }

            private string saxis;

            public string axis
            {
                get { return saxis; }
                set { saxis = value; }
            }

            private bool bDirection;

            public bool direction
            {
                get { return bDirection; }
                set { bDirection = value; }
            }

            private float fMaxLength;

            public float maxLength
            {
                get { return fMaxLength; }
                set { fMaxLength = value; }
            }

            private float fPrintLength;

            public float printLength
            {
                get { return fPrintLength; }
                set { fPrintLength = value; }
            }
        }


        /*
         * Init function 
         * -----------------------------------------------
         */
        public Program()
        {

            Runtime.UpdateFrequency = UpdateFrequency.Update100; // Set Tick update to 100

        }

        /*
         * Main function (called every 100 Ticks)
         * ------------------------------------------------
         */
        public void Main(string argument)
        {
            // Check init for first run
            if (!d3_init)
            {

                // Read ini file
                readini();

                // Calculate z zero level
                switch (d3_ini_zeroLevel)
                {
                    case 1:
                        d3_posMax_z = 35;
                        break;
                    case 2:
                        d3_posMax_z = 70;
                        break;
                    case 3:
                        d3_posMax_z = 105;
                        break;
                    case 4:
                        d3_posMax_z = 140;
                        break;
                    default:
                        fatal_error("Zero level is out of range");
                        break;
                }
                Echo("d3_posMax_z => " + d3_posMax_z.ToString());

                // Init welder
                welderObject = (IMyShipWelder) GridTerminalSystem.GetBlockWithName(welderName);
                if (null == welderObject)
                {
                    fatal_error("Welder Name not found");
                }
                welderObject.ApplyAction("OnOff_Off");

                // Init projector
                projectorObject = (IMyProjector)GridTerminalSystem.GetBlockWithName(projectorName);
                if (null == projectorObject)
                {
                    fatal_error("Projector Name not found");
                }

                // Initialize pistons
                init_tablePiston();

                // Set pistons to zero
                if (!pistZeroCheck())
                {
                    pistZero();
                }
                
                // Set ini flag
                d3_init = true;
                
            }

            // Wait until pistons are at zero
            if (!pistZeroCheck() && d3_Status == false)
            {
                Echo("Pist ZeroCheck");
                return;
            }
            else if(!d3_Status)
            {
                // Save projection count
                proj_count = projectorObject.RemainingBlocks;

                // Zero check finished, so start welder 
                welderObject.ApplyAction("OnOff_On");

                d3_Status = true;
            }


            // Check projector count
            if(proj_checks == 12)
            {
                // Printer Finished
                if (pistLimitCheck("y", true) && pistLimitCheck("x", true) && pistLimitCheck("z", true))
                {
                    fatal_error("Print finished");
                }

                // Check next step
                if (d3_activeAxis == "y")
                {

                    if(pistLimitCheck("y", d3_pdirection_y) && pistLimitCheck("x", d3_pdirection_x)) // Plate finished, go step up
                    {
                        // Change direction
                        if (d3_pdirection_y)
                        {
                            d3_pdirection_y = false;
                        }
                        else
                        {
                            d3_pdirection_y = true;
                        }
                        d3_activeAxis = "z";
                    }else if (pistLimitCheck("y", d3_pdirection_y)) // Limit reached move to next row
                    {
                        // Change direction
                        if (d3_pdirection_y)
                        {
                            d3_pdirection_y = false;
                        }
                        else
                        {
                            d3_pdirection_y = true;
                        }
                        d3_activeAxis = "x";
                    }
                    else // Move forward
                    {
                        foreach (KeyValuePair<int, piston> entry in dPiston)
                        {
                            if (entry.Value.axis != "y") continue;

                            int step = 1;
                            if (!d3_direction_y && !d3_pdirection_y) step = -1;

                            setPistons(parseNames(entry.Value.name), step, 0.1f, true);

                            Echo("Next Piston Set => " + debug_counter + " | Piston => " + entry.Value.name);
                            debug_counter++;
                        }
                    }


                }else if(d3_activeAxis == "x")
                {
                    // Check if we reached x axis limit
                    if(pistLimitCheck("x", d3_pdirection_x))
                    {
                        // Change direction
                        if (d3_pdirection_x)
                        {
                            d3_pdirection_x = false;
                        }
                        else
                        {
                            d3_pdirection_x = true;
                        }
                    }
                    else // Move one step and switch bach to y
                    {
                        foreach (KeyValuePair<int, piston> entry in dPiston)
                        {
                            if (entry.Value.axis != "x") continue;

                            int step = 1;
                            if (d3_direction_x && d3_pdirection_x) step = -1;

                            // Set piston 
                            setPistons(parseNames(entry.Value.name), step, 0.1f, true);

                            Echo("Next Piston Set => " + debug_counter + " | Piston => " + entry.Value.name);
                            debug_counter++;
                        }
                    }

                    // Always send back to y
                    d3_activeAxis = "y";
                }
                else if(d3_activeAxis == "z")
                {
                    
                    fatal_error("Plate finished");

                    // Check if we reached z axis limit
                    if (pistLimitCheck("z", d3_direction_z))
                    { 
                        foreach (KeyValuePair<int, piston> entry in dPiston)
                        {
                            if (entry.Value.axis != "z") continue;

                            // Set piston 
                            setPistons(parseNames(entry.Value.name), -1, 0.1f, true);

                            Echo("Next Piston Set => " + debug_counter + " | Piston => " + entry.Value.name);
                            debug_counter++;
                        }
                    }
                    
                    // Always send back to y
                    d3_activeAxis = "y";
                }
                

                proj_checks = 0;
            }
            else
            {
                // Check if smth changed
                if(projectorObject.RemainingBlocks < proj_count)
                {
                    // Set new count
                    proj_count = projectorObject.RemainingBlocks;

                    // Reset checks
                    proj_checks = 0;
                }
                else
                {
                    // Count didn't changed so go on with checking
                    proj_checks++;
                }
            }


        }



        /*
         * Init customData ini 
         */
         private void readini()
        {

            // Read custom data and check maximums
            MyIniParseResult result;
            if (!_ini.TryParse(Me.CustomData, out result))
            {
                fatal_error("Could not parse customData information");
            }

            // Read ini values
            d3_ini_posMax_x = _ini.Get("Configuration", "MaxPositon_X").ToInt32();
            d3_ini_posMax_y = _ini.Get("Configuration", "MaxPositon_Y").ToInt32();
            d3_ini_posMax_z = _ini.Get("Configuration", "MaxPositon_Z").ToInt32();
            d3_ini_zeroLevel = _ini.Get("Configuration", "ZeroLevel").ToInt32();
            Echo("d3_ini_posMax_x => " + d3_ini_posMax_x.ToString());
            Echo("d3_ini_posMax_y => " + d3_ini_posMax_y.ToString());
            Echo("d3_ini_posMax_z => " + d3_ini_posMax_z.ToString());
            Echo("d3_ini_zeroLevel => " + d3_ini_zeroLevel.ToString());
            Echo("------------------------");
        }


        /*
         * Piston Initialization 
        */
        private void init_tablePiston()
        {

            int iCount = 0;

            // X-Axis
            float printLength;

            foreach (string name in d3_pist_x)
            {
                pistonObject = (IMyPistonBase)GridTerminalSystem.GetBlockWithName(name);
                if (null != pistonObject)
                {
                    // Calculate piston max
                    if(d3_ini_posMax_x < d3_posMax_x)
                    {
                        printLength = (d3_ini_posMax_x / d3_pist_x.Count);
                    }
                    else
                    {
                        printLength = (d3_posMax_x / d3_pist_x.Count);
                    }

                    // Add piston object to dictonary
                    //Echo("Piston X => " + name + " | maxLength => " + (d3_posMax_x / d3_pist_x.Count).ToString() + " | printLength=> " + printLength.ToString() + "\n");
                    dPiston.Add(iCount, new piston(iCount, name, "x", d3_direction_x, (d3_posMax_x / d3_pist_x.Count), printLength, pistonObject));
                    iCount++;
                }
                else
                {
                    fatal_error("Piston not found =>"+ name);
                }
                
            }

            // Y-Axis
            
            foreach (string name in d3_pist_y)
            {
                // Calculate piston max
                if (d3_ini_posMax_y < d3_posMax_y)
                {
                    printLength = (d3_ini_posMax_y / d3_pist_y.Count);
                }
                else
                {
                    printLength = (d3_posMax_y / d3_pist_y.Count);
                }

                // Add piston object to dictonary
                pistonObject = (IMyPistonBase)GridTerminalSystem.GetBlockWithName(name);
                if (null != pistonObject)
                {
                    //Echo("Piston Y => " + name + " | maxLength => " + (d3_posMax_y / d3_pist_y.Count).ToString() + " | printLength=> " + printLength.ToString()+"\n");
                    dPiston.Add(iCount, new piston(iCount, name, "y", d3_direction_y, (d3_posMax_y / d3_pist_y.Count), printLength, pistonObject));
                    iCount++;
                }
                else
                {
                    fatal_error("Piston not found =>" + name);
                }
            }

            // Y-Axis
            
            foreach (string name in d3_pist_z)
            {
                // Calculate piston max
                if (d3_ini_posMax_z < d3_posMax_z)
                {
                    printLength = (d3_ini_posMax_z / d3_pist_z.Count);
                }
                else
                {
                    printLength = (d3_posMax_z / d3_pist_z.Count);
                }

                // Add piston object to dictonary
                pistonObject = (IMyPistonBase)GridTerminalSystem.GetBlockWithName(name);
                if (null != pistonObject)
                {
                    //Echo("Piston Z => " + name + " | maxLength => " + (d3_posMax_z / d3_pist_z.Count).ToString() + " | printLength=> " + printLength.ToString() + "\n");
                    dPiston.Add(iCount, new piston(iCount, name, "z", d3_direction_z, (d3_posMax_z / d3_pist_z.Count), printLength, pistonObject));
                    iCount++;
                }
                else
                {
                    fatal_error("Piston not found =>" + name);
                }
            }
        }

        /*
            Piston zero 
        */
        private void pistZero()
        {
            foreach (KeyValuePair<int, piston> entry in dPiston)
            {

                // Check direction
                if (entry.Value.direction)
                {
                    if (entry.Value.grid.CurrentPosition > 0.01f)
                    {
                        // Set piston to zero
                        setPistons(parseNames(entry.Value.name), 0, 0.1f, false);
                    }
                }
                else
                {
                    if (entry.Value.grid.CurrentPosition < entry.Value.maxLength || entry.Value.grid.CurrentPosition > entry.Value.maxLength)
                    {
                        // Set piston to zero
                        setPistons(parseNames(entry.Value.name), entry.Value.maxLength, 0.1f, false);
                    }
                }
            }
        }

        /*
            Piston zero check
        */
        private bool pistZeroCheck()
        {
            bool zero = true;

            foreach (KeyValuePair<int, piston> entry in dPiston)
            {
                // Check direction
                if (entry.Value.direction)
                {
                    if (entry.Value.grid.CurrentPosition > 0.01f)
                    {
                        zero = false;
                    }
                }
                else
                {
                    if (entry.Value.grid.CurrentPosition < entry.Value.maxLength || entry.Value.grid.CurrentPosition > entry.Value.maxLength)
                    {
                        zero = false;
                    }
                }
                    
            }

            return zero;
        }

        /*
         *  Piston build_limit_check
         */
         private bool pistLimitCheck(string axis, bool pdirect)
        {
            bool check = true;

            foreach (KeyValuePair<int, piston> entry in dPiston)
            {
                // Skipp non needed axis
                if (entry.Value.axis != axis) continue;

                // Check direction
                if (!entry.Value.direction && !pdirect)
                {
                    // Check if piston is on or over its print length
                    if (entry.Value.grid.CurrentPosition < entry.Value.printLength)
                    {
                        // Print length not reached
                        check = false;
                    }
                }
                else
                {
                    // Check if piston is on or over its print length
                    if (entry.Value.grid.CurrentPosition > (entry.Value.maxLength-entry.Value.printLength))
                    {
                        // Print length not reached
                        check = false;
                    }
                }
            }

            return check;
        }

        // Parse Blocks into List
        public List<IMyTerminalBlock> parseNames(string s)
        {
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            List<IMyTerminalBlock> acc = new List<IMyTerminalBlock>();

            string[] names = s.Split(',');
            if (null == names) { names = new string[1]; names[0] = s; }

            foreach (string name in names)
            {
                if (name.StartsWith("G:"))
                {
                    IMyBlockGroup g = GridTerminalSystem.GetBlockGroupWithName(name);
                    g.GetBlocks(acc);
                }
                else
                    GridTerminalSystem.SearchBlocksOfName(name, acc);
                foreach (IMyTerminalBlock tb in acc)
                {
                    if (!blocks.Contains(tb))
                        blocks.Add(tb);
                }
                acc.Clear();
            }

            if (blocks.Count == 0)
            {
                Echo("No Blocks found!");
                return null;
            }
            return blocks;
        }


        //Set Position&Velocity for Pistons
        private void setPistons(List<IMyTerminalBlock> blocks, float posv, float v, bool delta)
        {
            float pos = posv;
            foreach (IMyTerminalBlock block in blocks)
            {
                if (block is IMyPistonBase)
                {
                    IMyPistonBase piston = (IMyPistonBase)block;
                    float cpos = piston.CurrentPosition;
                    if (delta)
                    {
                        pos = posv + cpos;
                        if (pos > 35.0f)
                            pos = 35.0f;
                        else if (pos < 0.0f)
                            pos = 0.0f;
                    }
                    if (pos >= 0.0f && pos <= 35.0f)
                    {
                        float vabs = v;
                        if (v == 0.0f)
                            vabs = pistonDefaultSpeed;
                        if (pos > cpos)
                        {
                            piston.MinLimit = 0.0f;
                            piston.MaxLimit = pos;
                            if (vabs < 0)
                                piston.Velocity = -vabs;
                            else
                                piston.Velocity = vabs;
                        }
                        else if (pos < cpos)
                        {
                            piston.MinLimit = pos;
                            piston.MaxLimit = 35.0f;
                            if (vabs < 0)
                                piston.Velocity = vabs;
                            else
                                piston.Velocity = -vabs;
                        }
                        else
                        {
                            piston.Velocity = 0.0f;
                            piston.MinLimit = 0.0f;
                            piston.MaxLimit = 35.0f;
                        }
                    }
                    else
                    {
                        piston.MinLimit = 0.0f;
                        piston.MaxLimit = 0.0f;
                        piston.Velocity = v;
                    }
                }
            }
        }


        /*
         * Fatal error
         *  ---------------------------------
         */
         private void fatal_error(string msg)
        {
            Echo("Fatal Error: "+msg);

            throw new Exception("Fatal Error: " + msg);
        }


    }
}
