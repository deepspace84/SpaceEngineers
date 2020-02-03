using System;
using System.Collections.Generic;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using VRage.ObjectBuilders;

namespace DeepSpaceCombat
{
    // This object is always present, from the world load to world unload.
    // NOTE: all clients and server run mod scripts, keep that in mind.
    // The MyUpdateOrder arg determines what update overrides are actually called.
    // Remove any method that you don't need, none of them are required, they're only there to show what you can use.
    // Also remove all comments you've read to avoid the overload of comments that is this file.
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class DeepSpaceCombat : MySessionComponentBase
    {
        public static DeepSpaceCombat Instance; // the only way to access session comp from other classes and the only accepted static.

        int tick = 0;
        int frequency = 120;
        int selectedCol = 0;
        List<MyFontEnum> cols = new List<MyFontEnum>();


        // Found in another script
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            if (MyAPIGateway.Session != null && MyAPIGateway.Session.IsServer)
            {
                MyVisualScriptLogicProvider.PlayerDied += Event_Player_Died;
            }

            Storage store = new Storage("BASIC");
            if (!store.Load())
            {
                MyAPIGateway.Utilities.ShowNotification("First time init Mod Version 0.42", 60000);
                store.Set("Mod_Version", "0.42");
                store.Save();
            }
            else
            {
                MyAPIGateway.Utilities.ShowNotification("Loaded Mod Version " + store.Get("Mod_Version"), 60000);
            }

        }

        public override void LoadData()
        {
            // amogst the earliest execution points, but not everything is available at this point. 
            Instance = this;
        }

        public override void BeforeStart()
        {
            cols.Add(MyFontEnum.Red);
            cols.Add(MyFontEnum.White);
            cols.Add(MyFontEnum.Green);
            // executed before the world starts updating

            // Main entry point: MyAPIGateway
            // Entry point for reading/editing definitions: MyDefinitionManager.Static
        }

        protected override void UnloadData()
        {
            // executed when world is exited to unregister events and stuff

            Instance = null; // important for avoiding this object to remain allocated in memory
        }

        public override void HandleInput()
        {
            // gets called 60 times a second before all other update methods, regardless of framerate, game pause or MyUpdateOrder.
        }

        public override void UpdateBeforeSimulation()
        {
            // executed every tick, 60 times a second, before physics simulation and only if game is not paused.
        }

        public override void Simulate()
        {
            // executed every tick, 60 times a second, during physics simulation and only if game is not paused.
            // NOTE in this example this won't actually be called because of the lack of MyUpdateOrder.Simulation argument in MySessionComponentDescriptor
        }

        public override void UpdateAfterSimulation()
        {
            // executed every tick, 60 times a second, after physics simulation and only if game is not paused.

            if (MyAPIGateway.Session != null && MyAPIGateway.Session.IsServer)
            {
                try // example try-catch for catching errors and notifying player, use only for non-critical code!
                {
                    if (tick % frequency == 0)
                    {
                        MyAPIGateway.Utilities.ShowNotification("Hello World", 1000, cols[selectedCol]);
                    }
                    tick++;
                }
                catch (Exception e) // NOTE: never use try-catch for code flow or to ignore errors! catching has a noticeable performance impact.
                {
                    if (MyAPIGateway.Session?.Player != null)
                        MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} | Send SpaceEngineers.Log to mod author ]", 10000, MyFontEnum.Red);
                }
            }
        }

        public override void Draw()
        {
            // gets called 60 times a second after all other update methods, regardless of framerate, game pause or MyUpdateOrder.
            // NOTE: this is the only place where the camera matrix (MyAPIGateway.Session.Camera.WorldMatrix) is accurate, everywhere else it's 1 frame behind.
        }

        public override void SaveData()
        {
            // executed AFTER world was saved
        }

        public override void UpdatingStopped()
        {
            selectedCol = (selectedCol + 1) % 3;
            // executed when game is paused
        }



        public void Event_Player_Died(long playerId)
        {
            MyAPIGateway.Utilities.ShowNotification("Player died: " + MyVisualScriptLogicProvider.GetPlayersName(playerId), 60000);
        }
    }
}
