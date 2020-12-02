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

namespace DSC.NPCCleanup
{
    // This object is always present, from the world load to world unload.
    // The MyUpdateOrder arg determines what update overrides are actually called.
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class DeepSpaceCombat : MySessionComponentBase
    {
        public static DeepSpaceCombat Instance; // the only way to access session comp from other classes and the only accepted static.

        private bool _isInitialized; // Is this instance is initialized
        public bool _isClientRegistered;
        public bool _isServerRegistered;
        public ulong TickCounter { get; private set; }

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

        //Use a single command char to avoid unneccesary loops/code. 
        private char _commandStart = '#';

        public bool isDebug = true;

        public TextLogger ServerLogger = new TextLogger(); // This is a dummy logger until Init() is called.
        public TextLogger ClientLogger = new TextLogger(); // This is a dummy logger until Init() is called.

        public CommandHandler CMDHandler = new CommandHandler();
        public Networking Networking = new Networking(DSC_Config.ConnectionId);



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

        }

        /*
         * BeforeStart
         * 
         * Init networking
         */
        public override void BeforeStart()
        {
            // Register network handling
            Networking.Register();

            base.BeforeStart();

        }

        /*
         * UpdateBeforeSimulation
         * 
         * UpdateBeforeSimulation override
         */

        public override void LoadData()
        {
            base.LoadData();
        }



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
            TickCounter += 1;

            // Server late start
            // We had problems that we can't delete areas and so on
            if (TickCounter == 100 && IsServerRegistered)
            {
                InitServerLate();
            }

            if (IsServerRegistered)
            {

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

            // Unregister Client
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

            // Unregister Server
            if (_isServerRegistered)
            {
                // Logger
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
            ClientLogger.Init("DSC_NPCCleanup_Client.Log", false, 0); // comment this out if logging is not required for the Client. "AppData\Roaming\SpaceEngineers\Storage"
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
            try
            {
                _isInitialized = true; // Set this first to block any other calls from UpdateAfterSimulation().
                _isServerRegistered = true;
                ServerLogger.Init("DSC_NPCCleanup_Server.Log", false, 0); // comment this out if logging is not required for the Server.
                ServerLogger.WriteStart("DSC MOD Server Log Started");
                ServerLogger.WriteInfo("DSC MOD Server Version {0}", DSC_Config.modVersion.ToString());
                ServerLogger.WriteInfo("DSC MOD Communiction Server Version {0}", DSC_Config.ModCommunicationVersion);
                if (ServerLogger.IsActive)
                    VRage.Utils.MyLog.Default.WriteLine(string.Format("##Mod## DSC Server Logging File: {0}", ServerLogger.LogFile));

                ServerLogger.Flush();

            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Core::InitServer");
            }
        }

        private void InitServerLate()
        {

        }

        #endregion


        #region message handlers

        /*
         * GotMessage
         * Local message handler function
         * 
         */
        private void GotMessage(string messageText, ref bool sendToOthers)
        {
            if (messageText.StartsWith(_commandStart.ToString()))
            {

                bool isAdmin = false;
                if ((MyAPIGateway.Session.Player.PromoteLevel.Equals(MyPromoteLevel.Admin) || MyAPIGateway.Session.Player.PromoteLevel.Equals(MyPromoteLevel.Owner)))
                {
                    isAdmin = true;
                }

                Networking.SendToServer(new PacketCommand(messageText.TrimStart(_commandStart), MyAPIGateway.Session.Player.IdentityId, isAdmin));
                sendToOthers = false;
            }
        }
        #endregion

        #region core functions


        #endregion
    }
}
