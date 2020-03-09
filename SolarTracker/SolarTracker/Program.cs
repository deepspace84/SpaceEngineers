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
        int maxoutputdiff = 10;  //max output difference in percent, if it lower rotor stopps moving  

        void Main()
        {
            AlignSolarArray("SA01");
            Echo("----------------");
            //AlignSolarArray("SA02");  

        }

        public void AlignSolarArray(string array)
        {
            IMySolarPanel panel_lb = GridTerminalSystem.GetBlockWithName(array + " Solar Panel lb") as IMySolarPanel;
            IMySolarPanel panel_rb = GridTerminalSystem.GetBlockWithName(array + " Solar Panel rb") as IMySolarPanel;
            IMySolarPanel panel_lt = GridTerminalSystem.GetBlockWithName(array + " Solar Panel lt") as IMySolarPanel;
            IMySolarPanel panel_rt = GridTerminalSystem.GetBlockWithName(array + " Solar Panel rt") as IMySolarPanel;


            IMyMotorStator motorH = GridTerminalSystem.GetBlockWithName(array + " RotorH") as IMyMotorStator;
            IMyMotorStator motorV = GridTerminalSystem.GetBlockWithName(array + " RotorV") as IMyMotorStator;

            float powerOutput_lb = -1;
            float powerOutput_rb = -1;
            float powerOutput_lt = -1;
            float powerOutput_rt = -1;



            if (panel_lb != null && panel_rb != null && panel_lt != null && panel_rt != null)
            {
                motorH.SetValue("Velocity", 0f);
                motorV.SetValue("Velocity", 0f);

                powerOutput_lb = panel_lb.MaxOutput;
                powerOutput_lt = panel_lt.MaxOutput;
                powerOutput_rb = panel_rb.MaxOutput;
                powerOutput_rt = panel_rt.MaxOutput;

                float powerOutput_la = powerOutput_lb + powerOutput_lt;
                float powerOutput_ra = powerOutput_rb + powerOutput_rt;

                float powerOutput_at = powerOutput_lt + powerOutput_rt;
                float powerOutput_ab = powerOutput_rb + powerOutput_lb;


                Echo(array + " left both:         " + powerOutput_la + "\n" + array + " right both:       " + powerOutput_ra);
                Echo(array + " top both:         " + powerOutput_at + "\n" + array + " bottom both:  " + powerOutput_ab);

                //		Echo(array+" leftbottom: "+powerOutput_lb+"\n"+array+" rightbottom: "+powerOutput_rb);   



                float rot_h = ((powerOutput_ra - powerOutput_la) / (powerOutput_la + powerOutput_ra)) / 2;


                if (powerOutput_la < powerOutput_ra * (100 - maxoutputdiff) / 100 || powerOutput_la * (100 - maxoutputdiff) / 100 > powerOutput_ra)
                {
                    motorH.SetValue("Velocity", rot_h);
                    Echo(array + " roth_h: " + rot_h);
                }
                else
                {
                    Echo(array + " roth_h: 0 (max limit reached)");
                }


                //		Echo(array+" lefttop: "+powerOutput_lt+"\n"+array+" righttop: "+powerOutput_rt);   



                float rot_v = ((powerOutput_at - powerOutput_ab) / (powerOutput_at + powerOutput_ab)) / 2;



                if (powerOutput_at < powerOutput_ab * (100 - maxoutputdiff) / 100 || powerOutput_at * (100 - maxoutputdiff) / 100 > powerOutput_ab)
                {
                    motorV.SetValue("Velocity", rot_v);
                    Echo(array + " roth_v: " + rot_v);

                }
                else
                {
                    Echo(array + " roth_v: 0 (max limit reached)");
                }
            }
        }





    }
}
