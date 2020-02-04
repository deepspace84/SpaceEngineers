using System;
using System.Collections.Generic;
using Sandbox.Definitions;
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
using VRage.Collections;

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
        public string msgDead = "Hello World";
        public static DeepSpaceCombat Instance; // the only way to access session comp from other classes and the only accepted static.

        float largeShipSpeed = 150;
        float smallShipSpeed = 225;
        float missileMinSpeed = 240;
        float missileMaxSpeed = 360;
        float missileExplosionRange = 2500;
        int selectedCol = 0;
        List<MyFontEnum> cols = new List<MyFontEnum>();


        // Found in another script
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            if (MyAPIGateway.Utilities == null)
            {
                MyAPIGateway.Utilities = MyAPIUtilities.Static;
            }
            if (MyAPIGateway.Session != null)
            {
                if (MyAPIGateway.Session.IsServer)
                    MyVisualScriptLogicProvider.PlayerDied += Event_Player_Died;
                MyAPIGateway.Utilities.MessageEntered += Event_Message_Typed;
            }

            //Storage store = new Storage("BASIC");
            //if (!store.Load())
            //{
            //    MyAPIGateway.Utilities.ShowNotification("First time init Mod Version 0.42", 60000);
            //    store.Set("Mod_Version", "0.42");
            //    store.Save();
            //}
            //else
            //{
            //    MyAPIGateway.Utilities.ShowNotification("Loaded Mod Version " + store.Get("Mod_Version"), 60000);
            //}
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

            //Player needs to be killed before character speeds works
            MyDefinitionManager.Static.EnvironmentDefinition.LargeShipMaxSpeed = largeShipSpeed;
            MyDefinitionManager.Static.EnvironmentDefinition.SmallShipMaxSpeed = smallShipSpeed;
            MyDefinitionId missileId = new MyDefinitionId(typeof(MyObjectBuilder_AmmoDefinition), "Missile");
            MyMissileAmmoDefinition ammoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(missileId) as MyMissileAmmoDefinition;
            ammoDefinition.MaxTrajectory = missileExplosionRange;
            ammoDefinition.MissileInitialSpeed = missileMinSpeed;
            ammoDefinition.DesiredSpeed = missileMaxSpeed;
            //            MyDefinitionManager.Static.

            MyVisualScriptLogicProvider.ResearchListClear();
            MyVisualScriptLogicProvider.ResearchListWhitelist(true);
            // Main entry point: MyAPIGateway
            // Entry point for reading/editing definitions: MyDefinitionManager.Static
        }

        protected override void UnloadData()
        {
            // executed when world is exited to unregister events and stuff
            if (MyAPIGateway.Session.IsServer)
                MyVisualScriptLogicProvider.PlayerDied -= Event_Player_Died;
            MyAPIGateway.Utilities.MessageEntered -= Event_Message_Typed;
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
                //try // example try-catch for catching errors and notifying player, use only for non-critical code!
                //{
                //   if (tick % frequency == 0)
                //  {
                //      MyAPIGateway.Utilities.ShowNotification(msgDead, 1000, cols[selectedCol]);
                // }
                //tick++;
                //}
                //catch (Exception e) // NOTE: never use try-catch for code flow or to ignore errors! catching has a noticeable performance impact.
                //{
                //    if (MyAPIGateway.Session?.Player != null)
                //        MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} | Send SpaceEngineers.Log to mod author ]", 10000, MyFontEnum.Red);
                //}
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
            msgDead = "Player died: " + MyVisualScriptLogicProvider.GetPlayersName(playerId);
            MyVisualScriptLogicProvider.SendChatMessage(msgDead, "SYSTEM", 0, "Red");
            //MyAPIGateway.Utilities.ShowNotification("Player died: " + MyVisualScriptLogicProvider.GetPlayersName(playerId), 60000);
        }

        public void Event_Message_Typed(string messageText, ref bool sendToOthers)
        {
            sendToOthers = false;//Test
            MyVisualScriptLogicProvider.SendChatMessage("Message received.", "SYSTEM", 0, "Red");
            if (messageText == "!LIST")
            {
                MyAPIGateway.Utilities.ShowNotification("LIST MESSAGE detected", 60000);
                //    DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> defset = MyDefinitionManager.Static.GetAllDefinitions();
                //    var enumerator = defset.GetEnumerator();
                //    int limiter = 10;
                //    do
                //    {
                //        MyVisualScriptLogicProvider.SendChatMessage("L: "+enumerator.Current.ToString(), "SYSTEM", 0, "Red");
                //        limiter--;
                //    } while (enumerator.MoveNext() && limiter > 0);
                //	enumerator.Dispose();
            }
            //List<MyDefinitionId> deflist = new List<MyDefinitionId>();
            //MyConveyor x = new MyConveyor();
            //if ("!Research" == messageText)
            //    MyVisualScriptLogicProvider.ResearchListAddItem(x.BlockDefinition.Id);//MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
            //else if ("!PCU" == messageText)
            //    x.BlockDefinition.PCU = 666;
            //MyAPIGateway.Utilities.ShowNotification("Conveyor PCU: " + x.BlockDefinition.PCU.ToString(),60000);
        }
    }
}
