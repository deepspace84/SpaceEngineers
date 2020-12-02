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
using System.Runtime.CompilerServices;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        

        public float minAngle = 0;
        public float maxAngle = 180;
        public float defspeed = 0.2f;

        private IMyMotorAdvancedStator RotorObj;
        private IMyMotorAdvancedStator PistForwardObj;
        private IMyMotorAdvancedStator PistSideObj;

        private string RotorName = "DrillRotor";
        private string PistForward = "";
        private string PistSide = "";

        public bool Initialized = false;
        private bool CurrentDirection = false; // false = negative | true = positive

        private bool PistForwardDirection = false;
        private bool PistSideDirection = false;


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

            if (!Initialized)
            {
                RotorObj = (IMyMotorAdvancedStator)GridTerminalSystem.GetBlockWithName(RotorName);
                if (null == RotorObj)
                {
                    fatal_error("Rotor Name not found");
                }

                // Set limits
                RotorObj.LowerLimitDeg = minAngle;
                RotorObj.UpperLimitDeg = maxAngle;

                RotorObj.TargetVelocityRPM = 0;

                Initialized = true;
                return;
            }

            float CurrentPosition = MathHelper.ToDegrees(RotorObj.Angle);

            // Check state
            if (CurrentDirection)
            {
                // Check position
                if(Math.Abs(CurrentPosition-maxAngle) < 1)
                {
                    CurrentDirection = false;
                    return;
                }

                // Positive run
                RotorObj.TargetVelocityRPM = defspeed;

            }
            else
            {
                // Negative run

                // Check position
                if (Math.Abs(CurrentPosition - minAngle) < 1)
                {
                    CurrentDirection = true;
                    return;
                }

                // Positive run
                RotorObj.TargetVelocityRPM = -defspeed;
            }


            //MathHelper.ToDegrees(rotor.Angle)

            // The main entry point of the script, invoked every time
            // one of the programmable block's Run actions are invoked,
            // or the script updates itself. The updateSource argument
            // describes where the update came from. Be aware that the
            // updateSource is a  bitfield  and might contain more than 
            // one update type.
            // 
            // The method itself is required, but the arguments above
            // can be removed if not needed.
        }

        private void fatal_error(string msg)
        {
            Echo("Fatal Error: " + msg);

            throw new Exception("Fatal Error: " + msg);
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
}
