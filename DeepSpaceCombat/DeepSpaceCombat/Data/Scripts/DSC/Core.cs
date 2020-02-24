using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Input;
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

        private bool _isInitialized; // Is this instance is initialized
        private bool _isClientRegistered; // Is this instance a client
        public bool IsClientRegistered
        {
            get
            {
                return _isClientRegistered;
            }
        }

        private bool _isServerRegistered; // Is this instance a server

        public bool IsServerRegistered
        {
            get
            {
                return _isServerRegistered;
            }
        }

        public Networking Networking = new Networking(DSC_Config.ConnectionId);

        public TextLogger ServerLogger = new TextLogger(); // This is a dummy logger until Init() is called.
        public TextLogger ClientLogger = new TextLogger(); // This is a dummy logger until Init() is called.

        public Dictionary<string, long> BlockReference = new Dictionary<string, long>();
        public Dictionary<long, byte> PlayerLanguages = new Dictionary<long, byte>(); // PlayerId, Language as byte (German = 3)


        #region ingame overrides

        /*
         * Ingame Init
         * 
         * Main ingame initialization override
         */
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {

            // TODO Do we need this?
            if (MyAPIGateway.Utilities == null)
            {
                MyAPIGateway.Utilities = MyAPIUtilities.Static;
            }

            // TODO used in ship speed, keep it? :P
            if (Instance == null)
                Instance = this;

            try
            {
                //Define Speedes and Missile Range #TODO Check if its on the right location on init? Check back with original script
                MyDefinitionManager.Static.EnvironmentDefinition.LargeShipMaxSpeed = DSC_Config.largeShipSpeed;
                MyDefinitionManager.Static.EnvironmentDefinition.SmallShipMaxSpeed = DSC_Config.smallShipSpeed;
                MyDefinitionId missileId = new MyDefinitionId(typeof(MyObjectBuilder_AmmoDefinition), "Missile");
                MyMissileAmmoDefinition ammoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(missileId) as MyMissileAmmoDefinition;
                ammoDefinition.MaxTrajectory = DSC_Config.missileExplosionRange;
                ammoDefinition.MissileInitialSpeed = DSC_Config.missileMinSpeed;
                ammoDefinition.DesiredSpeed = DSC_Config.missileMaxSpeed;
            }
            catch(Exception ex)
            {
                // Main init failure
                VRage.Utils.MyLog.Default.WriteLine("##Mod## ERROR " + ex.Message);
            }

            MyVisualScriptLogicProvider.PlayerConnected += PlayerConnected;
        }

        /*
         * BeforeStart
         * 
         * Init networking
         */
        public override void BeforeStart()
        {
            Networking.Register();
        }

        /*
         * UpdateBeforeSimulation
         * 
         * UpdateBeforeSimulation override
         */
        public override void UpdateBeforeSimulation()
        {
            try
            {
                // Check if Instance exists
                if (Instance == null)
                    Instance = this;

                // This needs to wait until the MyAPIGateway.Session.Player is created, as running on a Dedicated server can cause issues.
                // It would be nicer to just read a property that indicates this is a dedicated server, and simply return.
                if (!_isInitialized && MyAPIGateway.Session != null && MyAPIGateway.Session.Player != null)
                {
                    if (MyAPIGateway.Session.OnlineMode.Equals(MyOnlineModeEnum.OFFLINE)) // pretend single player instance is also server.
                    {
                         InitServer();
                    }

                    if (!MyAPIGateway.Session.OnlineMode.Equals(MyOnlineModeEnum.OFFLINE) && MyAPIGateway.Multiplayer.IsServer && !MyAPIGateway.Utilities.IsDedicated)
                    {
                         InitServer();
                    }

                     InitClient();
                }

                // Dedicated Server.
                if (!_isInitialized && MyAPIGateway.Utilities != null && MyAPIGateway.Multiplayer != null
                    && MyAPIGateway.Session != null && MyAPIGateway.Utilities.IsDedicated && MyAPIGateway.Multiplayer.IsServer)
                {
                     InitServer();
                    return;
                }

                base.UpdateBeforeSimulation();
            }
            catch (Exception ex)
            {
                ClientLogger.WriteException(ex);
                ServerLogger.WriteException(ex);
            }
        }


        /*
         * UpdateAfterSimulation
         * 
         * UpdateAfterSimulation override
         */
        public override void UpdateAfterSimulation()
        {
            // example for testing ingame, press L at any point when in a world with this mod loaded
            // then the server player/console/log will have the message you sent
            if (MyAPIGateway.Input.IsNewKeyPressed(MyKeys.L))
            {
                Networking.SendToServer(new PacketSimple("testcommand", 5000));
            }
        }

        /*
         * UnloadData
         * 
         * UnloadData override
         */
        protected override void UnloadData()
        {
            ClientLogger.WriteStop("Shutting down");
            ServerLogger.WriteStop("Shutting down");

            // Unregister Client log
            if (_isClientRegistered)
            {
                // Unregister client message handler
                if (MyAPIGateway.Utilities != null)
                {
                    MyAPIGateway.Utilities.MessageEntered -= GotMessage;
                }

                ClientLogger.WriteStop("Log Closed");
                ClientLogger.Terminate();
            }

            // Unregister Server log
            if (_isServerRegistered)
            {
                ServerLogger.WriteStop("Log Closed");
                ServerLogger.Terminate();
            }

            // Unregister networking
            Networking?.Unregister();
            Networking = null;


            base.UnloadData();
        }

        #endregion


        #region init functions

        /*
         * Client init
         * - Init of the ClientLogger
         * - _messageHandler registration
         * - MessageEntered registration
         */
        private void InitClient()
        {
            _isInitialized = true; // Set this first to block any other calls from UpdateAfterSimulation().
            _isClientRegistered = true;
            ClientLogger.Init("DSC_Client.Log", false, 0); // comment this out if logging is not required for the Client. "AppData\Roaming\SpaceEngineers\Storage"
            ClientLogger.WriteStart("DSC MOD Client Log Started");

            // Register client message handler
            MyAPIGateway.Utilities.MessageEntered += GotMessage;

            ClientLogger.Flush();
        }

        /*
         * Server init
         * - Init of the ServerLogger
         * - _messageHandler registration
         * 
         */
        private void InitServer()
        {
            _isInitialized = true; // Set this first to block any other calls from UpdateAfterSimulation().
            _isServerRegistered = true;
            ServerLogger.Init("DSC_Server.Log", false, 0); // comment this out if logging is not required for the Server.
            ServerLogger.WriteStart("DSC MOD Server Log Started");
            ServerLogger.WriteInfo("DSC MOD Server Version {0}", DSC_Config.ModCommunicationVersion);
            if (ServerLogger.IsActive)
                VRage.Utils.MyLog.Default.WriteLine(string.Format("##Mod## DSC Server Logging File: {0}", ServerLogger.LogFile));

            ServerLogger.Flush();
        }


        #endregion
        

        #region message handlers

        /*
         * GotMessage
         * Local message handler function TODO
         * 
         */
        private void GotMessage(string messageText, ref bool sendToOthers)
        {

        }

        #endregion


        #region handler

        private void PlayerConnected(long playerId)
        {
            if (PlayerLanguages.Keys.Contains<long>(playerId))
            {
                ClientLogger.WriteInfo($"Player {playerId} already in list.");
                return;
            }

            List<IMyPlayer> allPlayers = new List<IMyPlayer>();
            MyAPIGateway.Multiplayer.Players.GetPlayers(allPlayers);
            IMyPlayer player = allPlayers.Find(p => p.IdentityId == playerId);

            if (player != null)
            {
                VRage.MyLanguagesEnum lang = MyAPIGateway.Session.Config.Language;
                ClientLogger.WriteInfo($"Player {playerId} found :).");
                Networking.SendToServer(new PacketSimple(lang.ToString(), (int)lang));
            }
            else
            {
                ClientLogger.WriteInfo($"Player {playerId} not found.");
            }
        }

        #endregion


    }
}
