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
        private bool d3_pistonZeroMove = false;
        private bool d3_Running = false;

        private int d3_Status = 0; // 0 = Off / 1=Printing / 2 = Moving
        private int d3_activeAxis = 0; // 1 = X / 2=Y / 3=Z
        private bool d3_direction = true; // 0=Forward / 1=Backward

        private int d3_zIndex = 0; // Groundlevel of the Blueprint

        // Piston variables
        private int d3_pos_x = 0;
        private int d3_pos_y = 0;
        private int d3_pos_z = 0;

        private int d3_posMax_x = 35;
        private int d3_posMax_y = 35;
        private int d3_posMax_z = 35;

        //Block-Object variables
        private IMyPistonBase pistonObject;


        /* Piston declaration
         */

        // X
        List<string> d3_pist_x = new List<string>()
        {"3d_Pist_Forward_0","3d_Pist_Forward_1","3d_Pist_Forward_2","3d_Pist_Forward_3"};

        // Y
        List<string> d3_pist_y = new List<string>()
        {"3d_Pist_Side_0","3d_Pist_Side_1","3d_Pist_Side_3"};

        // Z
        List<string> d3_pist_z = new List<string>()
        {"3d_Pist_Vert_0","3d_Pist_Vert_1","3d_Pist_Vert_2","3d_Pist_Vert_3"};

        private DataTable tablePiston = new DataTable();





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
                // Initialize pistons
                init_tablePiston();

                // Check piston positons
                if (!pistZero())
                {
                    d3_pistonZeroMove = true;
                }

                // Set ini flag
                d3_init = true;
            }

            // Check for piston zero moving
            if (d3_pistonZeroMove)
            {
                if (pistZero())
                {
                    d3_pistonZeroMove = false;
                }
                else
                {
                    return;
                }
            }
        }


        /*
            Piston Initialization 
        */
        private void init_tablePiston()
        {
            // Define columns
            tablePiston.Columns.Add("name", typeof(string));
            tablePiston.Columns.Add("directon", typeof(string));
            tablePiston.Columns.Add("order", typeof(int));
            tablePiston.Columns.Add("active", typeof(bool));

            int oCount = 0;
            foreach (string name in d3_pist_x)
            {
                tablePiston.Rows.Add(name, "x", oCount, false);
                oCount++;
            }
            foreach (string name in d3_pist_y)
            {
                tablePiston.Rows.Add(name, "y", oCount, false);
                oCount++;
            }
            foreach (string name in d3_pist_z)
            {
                tablePiston.Rows.Add(name, "z", oCount, false);
                oCount++;
            }
        }

        /*
            Piston zero check
        */
        private bool pistZero()
        {
            bool zero = true;
            foreach (DataRow row in tablePiston.Rows)
            {
                pistonObject = (IMyPistonBase)GridTerminalSystem.GetBlockWithName(row["name"].ToString());

                if (pistonObject.CurrentPosition > 0.01f)
                {
                    // Set piston to zero
                    setPistons(parseNames(row["name"].ToString()), 0, 0.1f, false);
                    zero = false;
                }
            }

            return zero;
        }


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
    }
