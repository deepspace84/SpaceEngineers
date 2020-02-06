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

        HashSet<string> distinctSet = new HashSet<string>();
        Dictionary<string, long> adminBlocks = new Dictionary<string, long>();

        // Main Initialisation
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            // executed before the world starts updating
            if (MyAPIGateway.Utilities == null)
            {
                MyAPIGateway.Utilities = MyAPIUtilities.Static;
            }

            if (MyAPIGateway.Session != null)
            {
                if (MyAPIGateway.Session.IsServer)
                {
                    // Register Events
                    MyVisualScriptLogicProvider.PlayerDied += Event_Player_Died;
                    MyVisualScriptLogicProvider.AreaTrigger_Entered += Event_Area_Entered;
                    //MyVisualScriptLogicProvider.BlockBuilt += Event_Block_Built;
                    MyAPIGateway.Entities.OnEntityAdd += Entities_OnEntityAdd;


                    MyVisualScriptLogicProvider.PlayerResearchClearAll();
                    MyVisualScriptLogicProvider.ResearchListClear();
                    MyVisualScriptLogicProvider.ResearchListWhitelist(true);
                }


            }
            //Player needs to be killed before character speeds works
            MyDefinitionManager.Static.EnvironmentDefinition.LargeShipMaxSpeed = largeShipSpeed;
            MyDefinitionManager.Static.EnvironmentDefinition.SmallShipMaxSpeed = smallShipSpeed;
            MyDefinitionId missileId = new MyDefinitionId(typeof(MyObjectBuilder_AmmoDefinition), "Missile");
            MyMissileAmmoDefinition ammoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(missileId) as MyMissileAmmoDefinition;
            ammoDefinition.MaxTrajectory = missileExplosionRange;
            ammoDefinition.MissileInitialSpeed = missileMinSpeed;
            ammoDefinition.DesiredSpeed = missileMaxSpeed;
        }

        private void Entities_OnEntityAdd(IMyEntity obj)
        {
            string s = obj.GetType().ToString();
            if (!(distinctSet.Contains(s)))
            {
                distinctSet.Add(s);
                MyVisualScriptLogicProvider.SendChatMessage(s, "", 0, "Red");
            }
            if (obj is IMyCubeGrid)
            {
                MyVisualScriptLogicProvider.SendChatMessage("Entity added: " + obj.Name + " EntityId =>" + obj.EntityId.ToString() + "| Display Name=>" + obj.DisplayName + " | FriendlyName => " + obj.GetFriendlyName(), "SYSTEM", 0, "Red");
            }
            //obj.GetFriendlyName
        }

        public override void LoadData()
        {
            // amogst the earliest execution points, but not everything is available at this point. 
            Instance = this;
        }

        public override void BeforeStart()
        {
            //MyVisualScriptLogicProvider.ResearchListClear();
            //MyVisualScriptLogicProvider.ResearchListWhitelist(true);
            // Main entry point: MyAPIGateway
            // Entry point for reading/editing definitions: MyDefinitionManager.Static
        }

        protected override void UnloadData()
        {
            // executed when world is exited to unregister events and stuff
            if (MyAPIGateway.Session.IsServer)
            {
                MyVisualScriptLogicProvider.PlayerDied -= Event_Player_Died;
                MyVisualScriptLogicProvider.AreaTrigger_Entered -= Event_Area_Entered;
            }

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
            // executed when game is paused
        }

        public void Event_Player_Died(long playerId)
        {
            msgDead = "Player died: " + MyVisualScriptLogicProvider.GetPlayersName(playerId);
            MyVisualScriptLogicProvider.SendChatMessage(msgDead, "SYSTEM", 0, "Red");
            //MyAPIGateway.Utilities.ShowNotification("Player died: " + MyVisualScriptLogicProvider.GetPlayersName(playerId), 60000);
        }

        public void Event_Area_Entered(string name, long playerId)
        {
            MyVisualScriptLogicProvider.SendChatMessage("Player entered area: " + MyVisualScriptLogicProvider.GetPlayersName(playerId) + " Name of area =>" + name, "SYSTEM", 0, "Red");
        }

        public void Event_Block_Built(IMyEntity obj)
        {

            /*
            finalblock_string = final_block.FatBlock.EntityId.ToString();
            final_block.FatBlock.Name = finalblock_string;
            MyEntities.SetEntityName((MyEntity)final_block.FatBlock, true);
            You can use this, or SetName(). Not sure if SetName() works in all situations
            */

            //test.OnEntityAdd;

            //IMyEntity test2;


            //MyEntities.SetEntityName()



        }

        public void Event_Message_Typed(string messageText, ref bool sendToOthers)
        {
            sendToOthers = false;//Test
            MyVisualScriptLogicProvider.SendChatMessage("Message received.", "SYSTEM", 0, "Red");
            DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> defset = MyDefinitionManager.Static.GetAllDefinitions();
            IMyPlayer p = MyAPIGateway.Session.Player;
            if (messageText == "!TEST")
            {

            }
            if (messageText.StartsWith("!MEMORIZE"))
            {
                MyVisualScriptLogicProvider.SendChatMessage("Memorize.", "SYSTEM", 0, "Red");
                string[] names = messageText.Split(' ');
                if (names.Length > 1)
                {
                    MyConcurrentHashSet<MyEntity> all = MyEntities.GetEntities();
                    int i = 0;

                    foreach (IMyEntity entity in all)
                    {
                        i++;
                        if (entity is IMyTerminalBlock)
                        {
                            IMyTerminalBlock block = (IMyTerminalBlock)entity;
                            if (block.CustomName.Contains(names[1]))
                            {
                                adminBlocks[block.CustomName] = block.EntityId;
                                MyVisualScriptLogicProvider.SendChatMessage("Added Entry: " + block.CustomName + " -> " + block.EntityId);
                            }
                        }
                    }
                    MyVisualScriptLogicProvider.SendChatMessage("Entities: " + i, "SYSTEM", 0, "Red");
                }
                else
                {
                    MyVisualScriptLogicProvider.SendChatMessage("Error: Usage: !MEMORIZE TAG", "SYSTEM", 0, "Red");
                }
            }
            if (messageText.StartsWith("!SHOW_MEMORY"))
            {
                foreach (KeyValuePair<string, long> m in adminBlocks)
                {
                    MyVisualScriptLogicProvider.SendChatMessage(m.Key + " -> " + m.Value);
                }
            }
            if (messageText == "!LIST")
            {
                MyAPIGateway.Utilities.ShowNotification("LIST MESSAGE detected", 5000);
                HashSet<string> types = new HashSet<string>();
                try
                {
                    Dictionary<MyDefinitionId, MyDefinitionBase>.ValueCollection.Enumerator enumerator = defset.GetEnumerator();
                    int limiter = 0;
                    while (enumerator.MoveNext() && limiter < 100)
                    {
                        if (!(enumerator.Current.Id.ToString().Contains("Block")))
                            continue;
                        if (!types.Contains(enumerator.Current.Id.ToString()))
                        {
                            limiter++;
                            MyVisualScriptLogicProvider.SendChatMessage(enumerator.Current.Id.ToString());
                            //MyVisualScriptLogicProvider.SendChatMessage(enumerator.Current.Id.ToString(), "SYSTEM", 0, "Red");
                            types.Add(enumerator.Current.Id.ToString());
                        }
                    }
                    MyAPIGateway.Utilities.ShowNotification("LIMIT: " + limiter, 5000);
                    enumerator.Dispose();
                }
                catch (Exception ex) { MyAPIGateway.Utilities.ShowNotification("Exception: " + ex.Message, 5000); }
            }
            else if (messageText == "!RESEARCH")
            {
                try
                {
                    Dictionary<MyDefinitionId, MyDefinitionBase>.ValueCollection.Enumerator enumerator = defset.GetEnumerator();
                    MyVisualScriptLogicProvider.SendChatMessage("Research test: " + p.DisplayName, "SYSTEM", 0, "Red");
                    MyVisualScriptLogicProvider.SendChatMessage("PlayerID: " + p.PlayerID + " Identity: " + p.IdentityId, "SYSTEM", 0, "Red");
                    while (enumerator.MoveNext())
                    {
                        try { MyVisualScriptLogicProvider.PlayerResearchUnlock(p.IdentityId, enumerator.Current.Id); }
                        catch (Exception ex2) { MyVisualScriptLogicProvider.SendChatMessage("Skip:" + enumerator.Current.Id.ToString()); }
                    }
                    enumerator.Dispose();
                }
                catch (Exception ex) { MyAPIGateway.Utilities.ShowNotification("Exception: " + ex.Message, 5000); }
            }
            else if (messageText == "!CLEAR")
            {
                Dictionary<MyDefinitionId, MyDefinitionBase>.ValueCollection.Enumerator enumerator = defset.GetEnumerator();

                //MyVisualScriptLogicProvider.PlayerResearchClearAll();
                while (enumerator.MoveNext())
                {
                    try { MyVisualScriptLogicProvider.PlayerResearchLock(p.IdentityId, enumerator.Current.Id); }
                    catch (Exception ex2) { MyVisualScriptLogicProvider.SendChatMessage("Skip:" + enumerator.Current.Id.ToString()); }
                }
                enumerator.Dispose();
            }
            else if (messageText == "!THRUST")
            {
                MyVisualScriptLogicProvider.PlayerResearchUnlock(p.IdentityId, MyVisualScriptLogicProvider.GetDefinitionId("Thrust", "SmallBlockSmallThrust"));
            }
            else if (messageText == "!addarea")
            {
                MyVisualScriptLogicProvider.RemoveTrigger("Testarea");

                Vector3D test = new Vector3D(17634.62, 55360.81, 22019.81);
                MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(test, 50, "Testarea");

            }
            else if (messageText == "!addquest")
            {
                //finalblock_string = final_block.FatBlock.EntityId.ToString();
                //final_block.FatBlock.Name = finalblock_string;
                //MyEntities.SetEntityName((MyEntity)final_block.FatBlock, true);

                long test;
                //MyVisualScriptLogicProvider.AddSearchContract(MyEntities.GetEntityByName("DSC_Contracts").EntityId, 1000, 0, 5000, MyVisualScriptLogicProvider.GetGridIdOfBlock("DSC_Target_Battery"), 50, out test);
                //MyVisualScriptLogicProvider.AddSearchContract(MyVisualScriptLogicProvider.GetEntityIdFromName("DSC_Contracts"), 1000, 0, 5000, MyVisualScriptLogicProvider.GetEntityIdFromName("Target"), 50, out test);

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
