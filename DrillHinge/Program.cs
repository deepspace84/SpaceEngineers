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
        public float HingeSpeedDown = 0.1f;
        public float HingeSpeedUp = -0.1f;
        private IMyMotorAdvancedStator HingeObject;


        public Program()
        {
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
            HingeObject = (IMyMotorAdvancedStator)GridTerminalSystem.GetBlockWithName("DrillHinge");
            
            if(HingeObject == null)
            {
                return;
            }

            // Check argument
            if (argument.Equals("UP"))
            {
                HingeObject.LowerLimitDeg = (HingeObject.Angle * (180 / (float)Math.PI))-1;
                HingeObject.UpperLimitDeg = (HingeObject.Angle * (180 / (float)Math.PI)) - 1;
                HingeObject.TargetVelocityRPM = HingeSpeedDown;
            }

            if (argument.Equals("DOWN"))
            {
                HingeObject.UpperLimitDeg = (HingeObject.Angle * (180 / (float)Math.PI)) + 1;
                HingeObject.LowerLimitDeg = (HingeObject.Angle * (180 / (float)Math.PI)) + 1;
                HingeObject.TargetVelocityRPM = HingeSpeedUp;
            }

        }

    }
}
