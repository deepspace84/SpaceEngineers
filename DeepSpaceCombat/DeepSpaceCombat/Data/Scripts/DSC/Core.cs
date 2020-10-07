using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Scripting;

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

        public DSC_Storage_Core CoreStorage;

        public Networking Networking = new Networking(DSC_Config.ConnectionId);
        public CommandHandler CMDHandler = new CommandHandler();
        public DSC_Reference DSCReference = new DSC_Reference();
        public DSC_Factions Factions = new DSC_Factions();
        public DSC_Definitions Definitions = new DSC_Definitions();
        public DSC_TechTree Techtree = new DSC_TechTree();
        public DSC_SpawnManager SpawnManager = new DSC_SpawnManager();
        public DSC_RespawnManager RespawnManager = new DSC_RespawnManager();
        public DSC_TradeManager TradeManager = new DSC_TradeManager();

        public long NPCPlayerID;
        public long NPCFactionID;

        public TextLogger ServerLogger = new TextLogger(); // This is a dummy logger until Init() is called.
        public TextLogger ClientLogger = new TextLogger(); // This is a dummy logger until Init() is called.

        

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

            if (TickCounter % 60 == 0)
            {
                if (IsServerRegistered)
                {
                    SpawnManager.Check();
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

                // Save reference data to savegame
                DSCReference.Save();

                // Save core storage
                SaveCoreStorage();

                // Save TradeManager
                TradeManager.Save();

                // Techtree
                Techtree.Save();
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
                // TradeManager
                TradeManager.Unload();

                // Respawn Manager
                RespawnManager.Unload();

                // Factions
                Factions.Unload();
                Factions = null;

                // Reference
                DSCReference.Unload();
                DSCReference = null;

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
            ServerLogger.WriteInfo("DSC MOD Server Version {0}", DSC_Config.modVersion.ToString());
            ServerLogger.WriteInfo("DSC MOD Communiction Server Version {0}", DSC_Config.ModCommunicationVersion);
            if (ServerLogger.IsActive)
                VRage.Utils.MyLog.Default.WriteLine(string.Format("##Mod## DSC Server Logging File: {0}", ServerLogger.LogFile));

            ServerLogger.Flush();

            // Load core storage
            LoadCoreStorage();

            // Check for default faction / npc data
            CheckDefaultFactionNPC();

            // Load Reference data
            DSCReference.Load();

            // Load tech tree
            Techtree.Load();

        }

        private void InitServerLate()
        {
            // Load faction data
            Factions.Load();

            // Load RespawnManager
            RespawnManager.Load();

            // Load TradeManager
            TradeManager.Load();
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
            if (messageText.StartsWith(_commandStart.ToString()))
            {
                Networking.SendToServer(new PacketCommand(messageText.TrimStart(_commandStart), MyAPIGateway.Session.Player.IdentityId));
                sendToOthers = false;
            }
            else { sendToOthers = true; }
        }
        #endregion

        #region core functions

        private void LoadCoreStorage()
        {
            // Check if file exists
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_Storage_Core", typeof(DSC_Storage_Core)))
            {
                try
                {
                    var reader = MyAPIGateway.Utilities.ReadBinaryFileInWorldStorage("DSC_Storage_Core", typeof(DSC_Storage_Core));
                    CoreStorage = MyAPIGateway.Utilities.SerializeFromBinary<DSC_Storage_Core>(reader.ReadBytes((int)reader.BaseStream.Length));
                    reader.Dispose();
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DSC_Storage_Core found and loaded");
                }
                catch (Exception e)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_Storage_Core loading failed");
                }
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("No DSC_Storage_Core found, create default");
                // Create default values
                CoreStorage = new DSC_Storage_Core
                {
                    Respawns = new Dictionary<long, DateTime>(),
                };
            }
        }

        private void SaveCoreStorage()
        {
            // Save Storage
            byte[] serialized = MyAPIGateway.Utilities.SerializeToBinary<DSC_Storage_Core>(CoreStorage);
            System.IO.BinaryWriter writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage("DSC_Storage_Core", typeof(DSC_Storage_Core));
            writer.Write(serialized);
            writer.Flush();
            writer.Dispose();
        }


        private void CheckDefaultFactionNPC()
        {
            // Check if faction exists NPC
            if (!MyAPIGateway.Session.Factions.FactionTagExists(DSC_Config.MainFactionTag)) {

                // Create faction
                MyAPIGateway.Session.Factions.CreateNPCFaction(DSC_Config.MainFactionTag, DSC_Config.MainFactionName, "", "");
            }
                
            
            // Get faction
            IMyFaction factionObj = MyAPIGateway.Session.Factions.TryGetFactionByTag(DSC_Config.MainFactionTag);

            // Check for npc player
            bool check = false;
            foreach (long playerId in factionObj.Members.Keys)
            {
                if (MyVisualScriptLogicProvider.GetPlayersName(playerId) == DSC_Config.MainFactionNPC)
                {
                    NPCPlayerID = playerId;
                    NPCFactionID = factionObj.FactionId;
                    check = true;
                    if(isDebug) ServerLogger.WriteInfo("NPC Player found");
                }
            }

            // Player not found
            if (!check)
            {
                MyAPIGateway.Session.Factions.AddNewNPCToFaction(factionObj.FactionId, DSC_Config.MainFactionNPC);

                foreach (long playerId in factionObj.Members.Keys)
                {
                    if (MyVisualScriptLogicProvider.GetPlayersName(playerId) == DSC_Config.MainFactionNPC)
                    {
                        if (isDebug) ServerLogger.WriteInfo("NPC Player was not found, so added");
                        NPCPlayerID = playerId;
                        NPCFactionID = factionObj.FactionId;
                    }
                }
            }
        }

        #endregion
    }
}
