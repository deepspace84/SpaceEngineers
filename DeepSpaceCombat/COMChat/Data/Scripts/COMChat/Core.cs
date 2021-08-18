using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Scripting;

namespace DeepSpace
{
    // This object is always present, from the world load to world unload.
    // The MyUpdateOrder arg determines what update overrides are actually called.
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class COMChat : MySessionComponentBase
    {
        public static COMChat Instance; // the only way to access session comp from other classes and the only accepted static.

        private bool _isInitialized; // Is this instance is initialized
        public bool _isClientRegistered;
        public bool _isServerRegistered;
        public ulong TickCounter { get; private set; } // Big enough for years

        public bool IsClientRegistered // Is this instance a client
        {
            get
            {
                return _isClientRegistered;
            }
        }
        public bool IsServerRegistered // Is this instance a server
        {
            get
            {
                return _isServerRegistered;
            }
        }

        public bool isDebug = true; // Switch debug mode

        public TextLogger ServerLogger = new TextLogger(); // This is a dummy logger until Init() is called.
        public TextLogger ClientLogger = new TextLogger(); // This is a dummy logger until Init() is called.
        public Networking Networking = new Networking(Mod_Config.ConnectionId);
        public Communicator COMMaster = new Communicator();
        public CommunicatorClient COMClient = new CommunicatorClient();
        public TextHudModule HudModule = new TextHudModule();

        #region ingame overrides

        /*
         * Ingame Init
         * 
         * Main ingame initialization override
         */
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            base.Init(sessionComponent);

            // Set TickCounter always to zero at startup
            TickCounter = 0;

            // TODO Do we need this? used in ship speed, keep it? :P
            if (MyAPIGateway.Utilities == null) { MyAPIGateway.Utilities = MyAPIUtilities.Static; }

        }

        /*
         * BeforeStart
         * 
         * Init networking
         */
        public override void BeforeStart()
        {
            base.BeforeStart();

            // Register network handling
            Networking.Register();

            // Initiate Text Hud Api
            HudModule.Init();

            
        }

        /*
         * UpdateBeforeSimulation
         * 
         * UpdateBeforeSimulation override
         */
        public override void UpdateBeforeSimulation()
        {
            // Init Block
            try
            {
                // Check if Instance exists
                if (Instance == null) { Instance = this; }

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

                //TODO: Why not before everything else?
                base.UpdateBeforeSimulation();
            }
            catch (Exception ex)
            {
                ServerLogger.WriteException(ex);
                ClientLogger.WriteException(ex);
            }
        }


        /*
         * UpdateAfterSimulation
         * 
         * UpdateAfterSimulation override
         */
        public override void UpdateAfterSimulation()
        {
            TickCounter += 1;

            if(TickCounter % 20 == 0 && IsClientRegistered)
            {
                COMClient.CheckChat();
            }
        }


        /*
         * SaveData
         * 
         * SaveData override
         */
        public override void SaveData()
        {
            if (_isServerRegistered)
            {
                // Save Factions data to savegame
                COMMaster.Save();
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

            // Unregister Server
            if (_isServerRegistered)
            {
                // Factions
                COMMaster.Unload();
                COMMaster = null;

                // Logger
                ServerLogger.WriteStop("Log Closed");
                ServerLogger.Terminate();
            }

            // Unregister Client
            if (_isClientRegistered)
            {
                COMClient.Unload();

                ClientLogger.WriteStop("Log Closed");
                ClientLogger.Terminate();
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
            ClientLogger.Init("COMChat_Client.Log", false, 0); // comment this out if logging is not required for the Client. "AppData\Roaming\SpaceEngineers\Storage"
            ClientLogger.WriteStart("COMChat Client Log Started");

            COMClient.Init();

            ClientLogger.Flush();
        }

        /*
         * Server init
         * - Init of the ServerLogger
         */
        private void InitServer()
        {
            try
            {
                _isInitialized = true; // Set this first to block any other calls from UpdateAfterSimulation().
                _isServerRegistered = true;
                ServerLogger.Init("COMChat.Log", false, 0); // comment this out if logging is not required for the Server.
                ServerLogger.WriteStart("COMChat Server Log Started");
                ServerLogger.WriteInfo("COMChat Server Version {0}", Mod_Config.modVersion.ToString());
                ServerLogger.WriteInfo("COMChat Communiction Server Version {0}", Mod_Config.ModCommunicationVersion);
                if (ServerLogger.IsActive)
                    VRage.Utils.MyLog.Default.WriteLine(string.Format("##Mod## LST Server Logging File: {0}", ServerLogger.LogFile));

                ServerLogger.Flush();

                // Start COMMAster
                COMMaster.Init();
            }
            catch (Exception e)
            {
                COMChat.Instance.ServerLogger.WriteException(e, "Core::InitServer");
            }
        }

        #endregion


        
    }
}
