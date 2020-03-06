using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;

//Sandbox.ModAPI.Ingame.IMyTerminalBlock or Sandbox.ModAPI.IMyTerminalBlock ?

namespace DSC
{
    // This object is always present, from the world load to world unload.
    // The MyUpdateOrder arg determines what update overrides are actually called.
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class DeepSpaceCombat : MySessionComponentBase
    {
        public static DeepSpaceCombat Instance; // the only way to access session comp from other classes and the only accepted static.

        private bool _isInitialized; // Is this instance is initialized
        private bool _isClientRegistered;
        private bool _isServerRegistered;

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

        public long NPCId { get; private set; } // NPC to create contracts

        //Use a single command char to avoid unneccesary loops/code. 
        //private char[] _commandStartChars = { '#' }; // Array of strings, with what the commands starts
        private char _commandStart = '#';

        public Networking Networking = new Networking(DSC_Config.ConnectionId);
        public CommandHandler CMDHandler = new CommandHandler();
        public DSC_Blocks BlockRef = new DSC_Blocks();
        public DSC_Grids GridRef = new DSC_Grids();
        public DSC_Players Players = new DSC_Players();
        public DSC_Factions Factions = new DSC_Factions();
        public DSC_TechTree Techtree = new DSC_TechTree();

        public TextLogger ServerLogger = new TextLogger(); // This is a dummy logger until Init() is called.
        public TextLogger ClientLogger = new TextLogger(); // This is a dummy logger until Init() is called.

        private ulong counter = 0;

        #region ingame overrides

        /*
         * Ingame Init
         * 
         * Main ingame initialization override
         */
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            base.Init(sessionComponent);

            MyVisualScriptLogicProvider.SendChatMessage("Deep Space Combat initialized", "[Server]");

            // TODO Do we need this? used in ship speed, keep it? :P
            if (MyAPIGateway.Utilities == null) { MyAPIGateway.Utilities = MyAPIUtilities.Static; }
            //Repeated in UpdateBeforeSimulation()
            //if (Instance == null) { Instance = this; }

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
            catch (Exception ex)
            {
                // Main init failure
                VRage.Utils.MyLog.Default.WriteLine("##Mod## ERROR " + ex.Message);
            }
        }

        /*
         * BeforeStart
         * 
         * Init networking
         */
        public override void BeforeStart()
        {
            base.BeforeStart();



            Networking.Register();
            MyVisualScriptLogicProvider.PlayerConnected += Players.PlayerConnected;
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
            base.UpdateAfterSimulation();
            // example for testing ingame, press L at any point when in a world with this mod loaded
            // then the server player/console/log will have the message you sent

            //if (MyAPIGateway.Input.IsNewKeyPressed(MyKeys.L))
            //{
            //    MyVisualScriptLogicProvider.SendChatMessage("L pressed, New player connected ", "[Server]");
            //    Players.PlayerConnected(MyAPIGateway.Session.Player.IdentityId);
            //}


            if (counter++ % (60 * 20) == 0)
            {
                if (IsServerRegistered && _isInitialized && NPCId > 0)
                {
                    try
                    {
                        long blockID = BlockRef.AddBlockWithName("DSC_Start");
                        long gridID = GridRef.AddGridWithName("DSC_End");
                        DSC_SearchContractBase searchContract = new DSC_SearchContractBase("Test", 1000, blockID, 0, 60 * 10, gridID, 10, "Find the Target!", NPCId);
                        MyAddContractResultWrapper result = searchContract.StartContract();

                        if (result.Success)
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Auto Contract added: " + result.ContractId.ToString(), "[Server]", 0);
                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Auto Contract failed: ", "[Server]", 0, "RED");
                        }
                    }
                    catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("ERROR: " + ex.Message, "[Server]", 0); }
                }
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
                Factions.Save();
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
                // Logger
                ServerLogger.WriteStop("Log Closed");
                ServerLogger.Terminate();

                // Factions
                Factions.unload();
                Factions = null;
            }

            // Unregister networking
            Networking?.Unregister();
            Networking = null;

            // Unregister player connected
            MyVisualScriptLogicProvider.PlayerConnected -= Players.PlayerConnected;

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

            // Get neutral npc
            GetNPC();

            ServerLogger.Flush();

            // Load faction data
            Factions.load();
        }

        public void GetNPC()
        {
            List<long> members = Util.GetNPCs();

            if (members != null && members.Count > 0)
            {
                NPCId = members[0];
                MyVisualScriptLogicProvider.SendChatMessage($"INFO: NPC faction found", "[Server]");
            }
            else
            {
                MyVisualScriptLogicProvider.SendChatMessage($"ERROR: Please add at least one NPC to the faction 'DSC' and write '#Get_NPC'", "[Server]");
                ServerLogger.WriteInfo("Please add at least one NPC to the faction 'DSC' and write '#get npc'");
            }
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
            //DZ changed command-start to a single char. delete commented lines when checked
            //foreach (char c in _commandStartChars)
            //{
            //    if (messageText.StartsWith(c.ToString()))
            if (messageText.StartsWith(_commandStart.ToString()))
            {
                Networking.SendToServer(new PacketCommand(messageText.TrimStart(_commandStart), MyAPIGateway.Session.Player.IdentityId));
                sendToOthers = false;
            }
            //}
            else { sendToOthers = true; }
        }
        #endregion

    }
}
