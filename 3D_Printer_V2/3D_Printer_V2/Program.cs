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

        private int d3_Status = 0; // 0 = Off / 1=Printing / 2 = Moving
        private int d3_activeAxis = 0; // 1 = X / 2=Y / 3=Z
        private bool d3_direction = true; // 0=Forward / 1=Backward

        private int d3_zIndex = 0; // Groundlevel of the Blueprint

        MyIni _ini = new MyIni();

        // Piston variables
        private float d3_pos_x = 0;
        private float d3_pos_y = 0;
        private float d3_pos_z = 0;

        private float d3_posMax_x = 35;
        private float d3_posMax_y = 105;
        private float d3_posMax_z = 35;

        // true => zero to max | false => max to zero
        private bool d3_direction_x = true;
        private bool d3_direction_y = false;
        private bool d3_direction_z = true;

        //Block-Object variables
        private IMyPistonBase pistonObject;


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

        public class piston
        {
            public piston(int id, string name, string axis, bool direction, float maxlength, IMyPistonBase grid)
            {
                sName = name;
                iId = id;
                oGrid = grid;
                saxis = axis;
                bDirection = direction;
                fMaxLength = maxlength;
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
        }

        private IDictionary<int, piston> dPiston = new Dictionary<int, piston>();


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
                if (init_tablePiston() == false) {
                    return;
                }

                // Check piston positons
                if (!pistZeroCheck())
                {
                    pistZero();
                }

                // Read custom data and check maximums
                MyIniParseResult result;
                if (!_ini.TryParse(Me.CustomData, out result))
                    throw new Exception(result.ToString());

                // Check maxsize
                _ini.Get("Configuration", "MaxPositon_X").ToInt32();


                // Set ini flag
                d3_init = true;
            }

            // Wait until pistons are at zero
            if (!pistZeroCheck())
            {
                return;
            }

            // 
            


        }


        /*
            Piston Initialization 
        */
        private bool init_tablePiston()
        {

            int iCount = 0;

            // X-Axis
            float maxLength = (d3_posMax_x / d3_pist_x.Count);

            foreach (string name in d3_pist_x)
            {
                pistonObject = (IMyPistonBase)GridTerminalSystem.GetBlockWithName(name);
                if (null != pistonObject)
                {
                    dPiston.Add(iCount, new piston(iCount, name, "x", d3_direction_x, maxLength, pistonObject));
                    iCount++;
                }
                else
                {
                    fatal_error("Piston not found =>"+ name);
                    return false;
                }
                
            }

            // Y-Axis
            maxLength = (d3_posMax_y / d3_pist_x.Count);

            foreach (string name in d3_pist_y)
            {
                pistonObject = (IMyPistonBase)GridTerminalSystem.GetBlockWithName(name);
                if (null != pistonObject)
                {
                    dPiston.Add(iCount, new piston(iCount, name, "y", d3_direction_y, maxLength, pistonObject));
                    iCount++;
                }
                else
                {
                    fatal_error("Piston not found =>" + name);
                    return false;
                }
            }

            // Y-Axis
            maxLength = (d3_posMax_z / d3_pist_x.Count);

            foreach (string name in d3_pist_z)
            {
                pistonObject = (IMyPistonBase)GridTerminalSystem.GetBlockWithName(name);
                if (null != pistonObject)
                {
                    dPiston.Add(iCount, new piston(iCount, name, "z", d3_direction_z, maxLength, pistonObject));
                    iCount++;
                }
                else
                {
                    fatal_error("Piston not found =>" + name);
                    return false;
                }
            }

            return true;
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
                else {
                    if (entry.Value.grid.CurrentPosition < entry.Value.maxLength)
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
                    if (entry.Value.grid.CurrentPosition < entry.Value.maxLength)
                    {
                        zero = false;
                    }
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


        /*
         * Fatal error
         *  ---------------------------------
         */
         private void fatal_error(string msg)
        {
            Echo("Fatal Error: "+msg);

            throw;
            
        }

    }
}
