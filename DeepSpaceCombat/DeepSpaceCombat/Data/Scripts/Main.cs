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

        HashSet<string> areas = new HashSet<string>();
        string defaultArea = "DEFAULT_AREA";

        Dictionary<string, string> adminBlocks = new Dictionary<string, string>();

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
                    MyVisualScriptLogicProvider.AreaTrigger_Left += Event_Area_Left;
                    MyVisualScriptLogicProvider.PlayerResearchClearAll();
                    MyVisualScriptLogicProvider.ResearchListWhitelist(true);
                    //MyVisualScriptLogicProvider.BlockBuilt += Event_Block_Built;
                    //MyAPIGateway.Entities.OnEntityAdd += Event_OnEntityAdd;     
                }
                else
                {
                    MyVisualScriptLogicProvider.PlayerResearchClearAll();
                }
                MyAPIGateway.Utilities.MessageEntered += Event_Message_Typed;
            }

            //Define Speedes and Missile Range
            //Player needs to be killed before character speeds works
            MyDefinitionManager.Static.EnvironmentDefinition.LargeShipMaxSpeed = largeShipSpeed;
            MyDefinitionManager.Static.EnvironmentDefinition.SmallShipMaxSpeed = smallShipSpeed;
            MyDefinitionId missileId = new MyDefinitionId(typeof(MyObjectBuilder_AmmoDefinition), "Missile");
            MyMissileAmmoDefinition ammoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(missileId) as MyMissileAmmoDefinition;
            ammoDefinition.MaxTrajectory = missileExplosionRange;
            ammoDefinition.MissileInitialSpeed = missileMinSpeed;
            ammoDefinition.DesiredSpeed = missileMaxSpeed;
        }

        public override void LoadData()
        {
            // amogst the earliest execution points, but not everything is available at this point. 
            Instance = this;
        }

        public override void BeforeStart()
        {
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
                MyVisualScriptLogicProvider.AreaTrigger_Left -= Event_Area_Left;
            }

            foreach (string areaName in areas)
            {
                MyVisualScriptLogicProvider.RemoveTrigger(areaName);
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
            MyVisualScriptLogicProvider.SendChatMessage("Player died: " + MyVisualScriptLogicProvider.GetPlayersName(playerId), "SYSTEM", 0, "Red");
            //MyAPIGateway.Utilities.ShowNotification("Player died: " + MyVisualScriptLogicProvider.GetPlayersName(playerId), 60000);
        }

        public void Event_Area_Entered(string name, long playerId)
        {
            MyVisualScriptLogicProvider.SendChatMessage("Player entered area: " + MyVisualScriptLogicProvider.GetPlayersName(playerId) + " Name of area =>" + name, "SYSTEM");
        }
        public void Event_Area_Left(string name, long playerId)
        {
            MyVisualScriptLogicProvider.SendChatMessage("Player left area: " + MyVisualScriptLogicProvider.GetPlayersName(playerId) + " Name of area =>" + name, "SYSTEM");
        }


        public void Event_Message_Typed(string messageText, ref bool sendToOthers)
        {
            sendToOthers = false;//Test

            IMyPlayer p = MyAPIGateway.Session.Player;
            MyVisualScriptLogicProvider.SendChatMessage("Message received.", "SYSTEM", 0, "Red");

            //Map Strings to EntityID
            if (messageText.StartsWith("#MEMORIZE"))
            {
                //!MEMORIZE [TAG]
                MyVisualScriptLogicProvider.SendChatMessage("Memorize called.", "SYSTEM", 0, "White");
                string[] names = messageText.Split(' ');
                if ((null != names) && (names.Length > 1))
                {
                    MyVisualScriptLogicProvider.SendChatMessage("TAG=" + names[1], "SYSTEM", 0, "White");
                    DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> defset = MyDefinitionManager.Static.GetAllDefinitions();

                    //Get all Entities
                    MyConcurrentHashSet<MyEntity> allEntities = MyEntities.GetEntities();
                    foreach (IMyEntity entity in allEntities)
                    {
                        //Get All grid-entities
                        if (entity is IMyCubeGrid)
                        {
                            IMyCubeGrid grid = (IMyCubeGrid)entity;

                            //Possible Null-Pointer-Exception
                            try
                            {
                                //Get Terminal Blocks. (Use FatBlocks instead?)
                                List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks = new List<Sandbox.ModAPI.Ingame.IMyTerminalBlock>();
                                Sandbox.ModAPI.Ingame.IMyGridTerminalSystem gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(grid);
                                gts.GetBlocks(blocks);

                                foreach (Sandbox.ModAPI.Ingame.IMyTerminalBlock block in blocks)
                                {
                                    //Look for tagged Terminal blocks
                                    if (block.CustomName.Contains(names[1]))
                                    {
                                        adminBlocks[block.CustomName] = block.EntityId.ToString();
                                        MyVisualScriptLogicProvider.SendChatMessage("Added Entry: " + block.CustomName + " -> " + block.EntityId);
                                    }
                                }
                            }
                            catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message, "SYSTEM", 0, "Red"); }
                        }
                    }
                }
                else
                {
                    MyVisualScriptLogicProvider.SendChatMessage("Error: Usage: !MEMORIZE TAG", "SYSTEM", 0, "Red");
                }
            }

            //Show String-ID Map in Chat
            else if (messageText.StartsWith("#SHOW_MEMORY"))
            {
                MyVisualScriptLogicProvider.SendChatMessage("Show memory called.", "SYSTEM", 0, "Red");
                foreach (KeyValuePair<string, string> m in adminBlocks)
                {
                    MyVisualScriptLogicProvider.SendChatMessage(m.Key + " -> " + m.Value);
                }
            }

            //Research all available blocks
            else if (messageText.StartsWith("#RESEARCH"))
            {
                //!RESEARCH <Substring>
                MyVisualScriptLogicProvider.SendChatMessage("Research called.", "SYSTEM", 0, "Red");

                string[] names = messageText.Split(' ');
                if ((null != names) && (names.Length > 1))
                {
                    if (names.Length > 2)
                    {
                        try
                        {
                            MyVisualScriptLogicProvider.PlayerResearchUnlock(p.IdentityId, MyVisualScriptLogicProvider.GetDefinitionId(names[1],names[2]));
                        }
                        catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message); }
                    }
                    else
                    {
                        try
                        {
                            MyVisualScriptLogicProvider.PlayerResearchUnlock(p.IdentityId, MyVisualScriptLogicProvider.GetDefinitionId(names[1],null));
                        }
                        catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message); }
                    }
                }
                else if (null != names)
                {
                    try
                    {
                        DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> defset = MyDefinitionManager.Static.GetAllDefinitions();
                        Dictionary<MyDefinitionId, MyDefinitionBase>.ValueCollection.Enumerator enumerator = defset.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            try { MyVisualScriptLogicProvider.PlayerResearchUnlock(p.IdentityId, enumerator.Current.Id); }
                            catch (Exception exin) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + exin.Message + "ID: " + enumerator.Current.Id.ToString()); }
                        }
                        enumerator.Dispose();
                    }
                    catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message); }
                }
            }

            else if (messageText.StartsWith("#CLEAR_RESEARCH"))
            {
                //!CLEAR_RESEARCH <Substring>
                MyVisualScriptLogicProvider.SendChatMessage("Clear research called.", "SYSTEM", 0, "Red");

                string[] names = messageText.Split(' ');
                if ((null != names) && (names.Length > 1))
                {
                    try
                    {
                        DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> defset = MyDefinitionManager.Static.GetAllDefinitions();
                        Dictionary<MyDefinitionId, MyDefinitionBase>.ValueCollection.Enumerator enumerator = defset.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.Id.ToString().Contains(names[1]))
                            {
                                try { MyVisualScriptLogicProvider.PlayerResearchLock(p.IdentityId, enumerator.Current.Id); }
                                catch (Exception exin) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + exin.Message + "ID: " + enumerator.Current.Id.ToString()); }
                            }
                        }
                        enumerator.Dispose();
                    }
                    catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message); }
                }
                else if (null != names)
                {
                    try
                    {
                        DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> defset = MyDefinitionManager.Static.GetAllDefinitions();
                        Dictionary<MyDefinitionId, MyDefinitionBase>.ValueCollection.Enumerator enumerator = defset.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            try { MyVisualScriptLogicProvider.PlayerResearchLock(p.IdentityId, enumerator.Current.Id); }
                            catch (Exception exin) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + exin.Message + "ID: " + enumerator.Current.Id.ToString()); }
                        }
                        enumerator.Dispose();
                    }
                    catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message); }
                }
            }

            else if (messageText.StartsWith("#ADD_AREA"))
            {
                //Vector3D test = new Vector3D(17634.62, 55360.81, 22019.81);

                //!ADD_AREA <Substring>
                MyVisualScriptLogicProvider.SendChatMessage("Adding area...", "SYSTEM", 0, "Red");

                float radius = 10f;
                string[] names = messageText.Split(' ');
                if ((null != names) && (names.Length > 1))
                {
                    try
                    {
                        if ((names.Length > 2) && (!(float.TryParse(names[2], out radius))))
                        { radius = 10f; }

                        if (areas.Contains(names[1]))
                        { MyVisualScriptLogicProvider.RemoveTrigger(names[1]); }
                        MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(p.GetPosition(), radius, names[1]);
                        areas.Add(names[1]);
                    }
                    catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message); }
                }
                else if (null != names)
                {
                    try
                    {
                        if (areas.Contains(defaultArea))
                        { MyVisualScriptLogicProvider.RemoveTrigger(defaultArea); }
                        MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(p.GetPosition(), radius, defaultArea);
                        areas.Add(defaultArea);
                    }
                    catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message); }
                }
            }

            else if (messageText == "#REMOVE_AREA")
            {
                //!ADD_AREA <Substring>
                MyVisualScriptLogicProvider.SendChatMessage("Removing all areas...", "SYSTEM", 0, "Red");

                try
                {
                    foreach (string areaName in areas)
                    {
                        MyVisualScriptLogicProvider.RemoveTrigger(areaName);
                        areas.Clear();
                    }
                }
                catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + ex.Message); }

            }

            else if (messageText.StartsWith("#faction"))
            {

                // Check that only owner can set faction data
                if (p.PromoteLevel.ToString() == "Owner")
                {
                    // Split up command
                    string[] names = messageText.Split(' ');

                    // Check if we have at least one parameter
                    if ((null != names) && (names.Length >= 3))
                    {
                        if (names[2] == "add") // Add Faction --------------------------------------------------------------
                        {
                            // Check faction name
                            IMyFaction faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(names[3]);
                            if(null != faction)
                            {
                                // Check if allready added
                                MyVisualScriptLogicProvider.SendChatMessage("Faction was found", "SYSTEM", 0, "Red");
                            }
                            else
                            {
                                MyVisualScriptLogicProvider.SendChatMessage("Faction not found", "SYSTEM", 0, "Red");
                            }

                        }
                        else if (names[2] == "remove") // Remove Faction -------------------------------------------------
                        {

                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Unknown action", "SYSTEM", 0, "Red");
                        }
                    }
                    else
                    {
                        MyVisualScriptLogicProvider.SendChatMessage("Wrong parameters. /faction [action] [Factionname]", "SYSTEM", 0, "Red");
                    }
                }
            }

            if (messageText == "!LIST")
            {
                MyAPIGateway.Utilities.ShowNotification("LIST MESSAGE detected", 5000);
                DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> defset = MyDefinitionManager.Static.GetAllDefinitions();
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

            //public void Event_Block_Built(IMyEntity obj)
            //{
            /*
            finalblock_string = final_block.FatBlock.EntityId.ToString();
            final_block.FatBlock.Name = finalblock_string;
            MyEntities.SetEntityName((MyEntity)final_block.FatBlock, true);
            You can use this, or SetName(). Not sure if SetName() works in all situations
            */

            //test.OnEntityAdd;
            //IMyEntity test2;
            //MyEntities.SetEntityName()
            //}

            //private void Event_OnEntityAdd(IMyEntity obj)
            //{
            //    string s = obj.GetType().ToString();
            //    if (!(distinctSet.Contains(s)))
            //    {
            //        distinctSet.Add(s);
            //        MyVisualScriptLogicProvider.SendChatMessage(s, "", 0, "Red");
            //    }
            //    if (obj is IMyCubeGrid)
            //    {
            //       MyVisualScriptLogicProvider.SendChatMessage("Entity added: " + obj.Name + " EntityId =>" + obj.EntityId.ToString() + "| Display Name=>" + obj.DisplayName + " | FriendlyName => " + obj.GetFriendlyName(), "SYSTEM", 0, "Red");
            //    }
            //}


            //List<MyDefinitionId> deflist = new List<MyDefinitionId>();
            //MyConveyor x = new MyConveyor();
            //if ("!Research" == messageText)
            //    MyVisualScriptLogicProvider.ResearchListAddItem(x.BlockDefinition.Id);//MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
            //else if ("!PCU" == messageText)
            //    x.BlockDefinition.PCU = 666;
            //MyAPIGateway.Utilities.ShowNotification("Conveyor PCU: " + x.BlockDefinition.PCU.ToString(),60000);
        }

        class DSCArea
        {
            public string name;
            public Vector3D center;
            public float radius;
            public bool active;

            public DSCArea(string pName) { this.name = pName; center = new Vector3D(); radius = 10.0f; active = false; }
            public DSCArea(string pName, Vector3D pCenter) { this.name = pName; center = pCenter; radius = 10.0f; active = false; }
            public DSCArea(string pName, Vector3D pCenter, float pRadius) { this.name = pName; center = pCenter; radius = pRadius; active = false; }

            public void activate()
            {
                if (!active)
                {
                    try
                    {
                        MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(center, radius, name);
                        active = true;
                    }
                    catch (Exception ex) { }
                }
            }
            public void deactivate()
            {
                if (active)
                {
                    try
                    {
                        MyVisualScriptLogicProvider.RemoveTrigger(name);
                        active = false;
                    }
                    catch (Exception ex) { }
                }
            }
            public void toggle()
            {
                if (active)
                {
                    try
                    {
                        MyVisualScriptLogicProvider.RemoveTrigger(name);
                        active = false;
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    try
                    {
                        MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(center, radius, name);
                        active = true;
                    }
                    catch (Exception ex) { }
                }
            }

            public override int GetHashCode()
            {
                return name.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (null == obj) return false;
                if (obj.GetType() != this.GetType()) return false;
                DSCArea cmp = (DSCArea)obj;
                return ((name == cmp.name) && (center == cmp.center) && (radius == cmp.radius));
            }
        }
    }
}
