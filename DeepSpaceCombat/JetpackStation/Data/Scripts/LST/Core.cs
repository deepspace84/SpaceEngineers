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
using VRage.ModAPI;
using Sandbox.Game.Entities;
using Sandbox.Common.ObjectBuilders;
using VRageMath;

namespace LST
{
    // This object is always present, from the world load to world unload.
    // The MyUpdateOrder arg determines what update overrides are actually called.
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class LifeSupportStation : MySessionComponentBase
    {
        public static LifeSupportStation Instance; // the only way to access session comp from other classes and the only accepted static.

        public readonly double ModVersion = 0.42;
        public readonly int ModCommunicationVersion = 1;

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

        public bool isDebug = true;

        public LST_Config_Main Config;
        public TextLogger ServerLogger = new TextLogger(); // This is a dummy logger until Init() is called.


        private List<long> Stations = new List<long>();
        private Vector3D TerminalPos;
        private List<IMyPlayer> AllPlayers = new List<IMyPlayer>();
        private int CheckRange = 0;
        private Dictionary<long,List<long>> InPlayers = new Dictionary<long, List<long>>();
        private Dictionary<long, List<long>> InCachePlayers = new Dictionary<long, List<long>>();
        private List<long> InPlayersMsg = new List<long>();


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

            // Eeach 5 seconds
            if(TickCounter % 300 == 0 && IsServerRegistered)
            {
                FindLifeSupportStations();
            }

            // Each 1 seconds
            if (TickCounter % 60 == 0 && IsServerRegistered)
            {
                CheckStations();
            }
        }

        /*
         * UnloadData
         * 
         * UnloadData override
         */
        protected override void UnloadData()
        {
            ServerLogger.WriteStop("Shutting down");
            // Unregister Server
            if (_isServerRegistered)
            {

                // Logger
                ServerLogger.WriteStop("Log Closed");
                ServerLogger.Terminate();
            }

            base.UnloadData();
        }
        #endregion


        #region init functions

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
                ServerLogger.Init("LifeSupportStation.Log", false, 0); // comment this out if logging is not required for the Server.
                ServerLogger.WriteStart("LST MOD Server Log Started");
                ServerLogger.WriteInfo("LST MOD Server Version {0}", ModVersion.ToString());
                ServerLogger.WriteInfo("LST MOD Communiction Server Version {0}", ModCommunicationVersion);
                if (ServerLogger.IsActive)
                    VRage.Utils.MyLog.Default.WriteLine(string.Format("##Mod## LST Server Logging File: {0}", ServerLogger.LogFile));

                ServerLogger.Flush();

                // Load core config
                LoadCoreConfig();
                LifeSupportStation.Instance.ServerLogger.WriteInfo("InitServer::Config loaded");

                var def = MyDefinitionManager.Static.GetDefinition(MyVisualScriptLogicProvider.GetDefinitionId("GasContainerObject", "HydrogenBottle"));
                def.Public = false;
                LifeSupportStation.Instance.ServerLogger.WriteInfo("InitServer::Bottle removed");
            }
            catch (Exception e)
            {
                LifeSupportStation.Instance.ServerLogger.WriteException(e, "Core::InitServer");
            }
        }

        public void LoadCoreConfig()
        {
            // Load config xml
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("LST_Config_Main", typeof(LST_Config_Main)))
            {
                try
                {
                    System.IO.TextReader reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("LST_Config_Main", typeof(LST_Config_Main));
                    var xmlData = reader.ReadToEnd();
                    Config = MyAPIGateway.Utilities.SerializeFromXML<LST_Config_Main>(xmlData);
                    reader.Dispose();
                    LifeSupportStation.Instance.ServerLogger.WriteInfo("LST_Config_Main found and loaded");
                }
                catch (Exception e)
                {
                    LifeSupportStation.Instance.ServerLogger.WriteException(e, "LST_Config_Main loading failed");
                }
            }else
            {
                LifeSupportStation.Instance.ServerLogger.WriteInfo("No LST_Config_Main found, create default");
                // Create default values
                Config = new LST_Config_Main
                {
                    StationRange = 1000,
                    ShipRange = 500
                };

                // Write file
                var xmlData = MyAPIGateway.Utilities.SerializeToXML<LST_Config_Main>(Config);
                System.IO.TextWriter writerConfig = MyAPIGateway.Utilities.WriteFileInWorldStorage("LST_Config_Main", typeof(LST_Config_Main));
                writerConfig.Write(xmlData);
                writerConfig.Flush();
                writerConfig.Close();
            }
        }
        #endregion




        #region lst functions

        private void CheckStations()
        {
            LifeSupportStation.Instance.ServerLogger.WriteInfo("CheckStations::Start");

            // Update player positions
            AllPlayers.Clear();
            MyAPIGateway.Players.GetPlayers(AllPlayers);
            if (AllPlayers.Count == 0) return; // Return when no players online

            // Resert player list
            InPlayers.Clear();

            // Loop through all stations
            foreach (long stationId in Stations)
            {
                // Get block as terminal
                IMyTerminalBlock stationBlock = MyAPIGateway.Entities.GetEntityById(stationId) as IMyTerminalBlock;
                if (stationBlock != null)
                {
                    // Check if its running
                    if (stationBlock.IsWorking)
                    {
                        // Get position
                        TerminalPos = stationBlock.CubeGrid.GetPosition();

                        // Add to dictionary
                        InPlayers.Add(stationId, new List<long>());
                        // Check cache dictionary for easy calls
                        if(!InCachePlayers.ContainsKey(stationId))
                            InCachePlayers.Add(stationId, new List<long>());

                        // Check always range so we don't need to catch events
                        if (stationBlock.CubeGrid.IsStatic)
                        {
                            CheckRange = Config.StationRange;
                        }
                        else CheckRange = Config.ShipRange;

                        // Loop through all players
                        foreach (IMyPlayer player in AllPlayers)
                        {
                            // Check relation
                            if (stationBlock.GetUserRelationToOwner(player.IdentityId) != MyRelationsBetweenPlayerAndBlock.Enemies)
                            {
                                // Check distance
                                if (Vector3D.Distance(player.GetPosition(), TerminalPos) <= CheckRange) // Player is in range
                                {
                                    // Check if its a new one
                                    if (!InCachePlayers[stationId].Contains(player.IdentityId) && !InPlayersMsg.Contains(player.IdentityId))
                                    {
                                        // Player entered area -> send message
                                        MyVisualScriptLogicProvider.SendChatMessageColored("LS Area entered", Color.Red, "Server", player.IdentityId);
                                        InPlayersMsg.Add(player.IdentityId);
                                    }
                                    else
                                    {
                                        // Player still in here so delete from cache
                                        InCachePlayers[stationId].Remove(player.IdentityId);
                                    }

                                    // Add player to list
                                    InPlayers[stationId].Add(player.IdentityId);

                                    MyVisualScriptLogicProvider.SetPlayersHydrogenLevel(player.IdentityId, 1);
                                    MyVisualScriptLogicProvider.SetPlayersEnergyLevel(player.IdentityId, 1);
                                    MyVisualScriptLogicProvider.SetPlayersOxygenLevel(player.IdentityId, 1);

                                    
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Station does not exists any longer
                    LifeSupportStation.Instance.ServerLogger.WriteInfo("CheckStations::Station disappeared");
                }
            }

            // Check for players in cache -> player left area
            foreach (long stationId in InCachePlayers.Keys)
            {
                foreach(long playerId in InCachePlayers[stationId])
                {
                    foreach (long stationCheckId in Stations)
                    {
                        if (InPlayers.ContainsKey(stationCheckId))// Check because can missing from shutdown/destroy
                        {
                            if (InPlayers[stationCheckId].Contains(playerId)) return;
                        }
                    }
                    MyVisualScriptLogicProvider.SendChatMessageColored("LS Area left", Color.Red, "Server", playerId);
                    InPlayersMsg.Remove(playerId);
                }
            }

            // Rewrite cache
            InCachePlayers.Clear();
            InCachePlayers = new Dictionary<long, List<long>>(InPlayers);
        }

        // Read all Stations to reference
        private void FindLifeSupportStations()
        {
            LifeSupportStation.Instance.ServerLogger.WriteInfo("FindLifeSupportStations::Start");

            // Clear all old
            Stations.Clear();

            // Get all grid entities
            HashSet<IMyEntity> entList = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entList, e => e is IMyCubeGrid);
            if (entList.Count == 0)
                return;

            // Loop through all Grids
            foreach (IMyEntity ent in entList)
            {
                MyCubeGrid grid = ent as MyCubeGrid;
                if (grid == null) continue;
                long gridId = grid.EntityId;

                foreach (MyCubeBlock fb in grid.GetFatBlocks())
                {
                    if (fb.BlockDefinition.Id.TypeId == typeof(MyObjectBuilder_HydrogenEngine) && fb.BlockDefinition.Id.SubtypeName.Equals("LifeSupportStation"))
                    {
                        Stations.Add(fb.EntityId);
                        LifeSupportStation.Instance.ServerLogger.WriteInfo("FindLifeSupportStations::Station added=>"+fb.EntityId.ToString());
                    }
                }
            }
            LifeSupportStation.Instance.ServerLogger.WriteInfo("FindLifeSupportStations::Finish");
        }
        #endregion
    }
}
