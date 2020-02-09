using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using VRage.ObjectBuilders;
using VRage.Collections;
using Sandbox.Game.SessionComponents;

//Sandbox.ModAPI.Ingame.IMyTerminalBlock or Sandbox.ModAPI.IMyTerminalBlock ?

namespace DSC
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


        float largeShipSpeed = 150;
        float smallShipSpeed = 225;
        float missileMinSpeed = 240;
        float missileMaxSpeed = 360;
        float missileExplosionRange = 2500;



        // Main Initialisation
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            // executed before the world starts updating
            if (MyAPIGateway.Utilities == null)
            {
                MyAPIGateway.Utilities = MyAPIUtilities.Static;
            }


            //Define Speedes and Missile Range #TODO Check if its on the right location on init? Check back with original script
            MyDefinitionManager.Static.EnvironmentDefinition.LargeShipMaxSpeed = largeShipSpeed;
            MyDefinitionManager.Static.EnvironmentDefinition.SmallShipMaxSpeed = smallShipSpeed;
            MyDefinitionId missileId = new MyDefinitionId(typeof(MyObjectBuilder_AmmoDefinition), "Missile");
            MyMissileAmmoDefinition ammoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(missileId) as MyMissileAmmoDefinition;
            ammoDefinition.MaxTrajectory = missileExplosionRange;
            ammoDefinition.MissileInitialSpeed = missileMinSpeed;
            ammoDefinition.DesiredSpeed = missileMaxSpeed;
        }

        
    }
}
