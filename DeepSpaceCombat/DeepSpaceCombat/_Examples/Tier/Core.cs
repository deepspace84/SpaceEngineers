using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.ModAPI;
using VRage.Game.ModAPI;
using VRageMath;
using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using VRage.Game.Entity;
using VRage.Utils;
using VRage;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;

/*  
  MASSIVE THANKS to everyone that helped me with coding this from Discord and to Phoera for letting me use his Grind To Learn code
  and to MeridiusIX/Lucas for pieces of code from his various mods as well - without which this mod would not be possible! 
 */
namespace Stollie.Progression
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class Core : CoreBase
    {
        #region Teir0 Definitions
        //Armors, Passages, Pistons, Motors, Suspension & Wheels
        public readonly MyDefinitionId lightArmor = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeBlockArmorBlock");
        public readonly MyDefinitionId lightArmorRoundSlope = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeBlockArmorRoundSlope");
        public readonly MyDefinitionId lightArmorRoundSlope2 = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeBlockArmorSlope2Base");
        public readonly MyDefinitionId lightArmorCorner = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeBlockArmorCorner2Base");
        public readonly MyDefinitionId lightArmorCorner2 = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeBlockArmorInvCorner2Base");
        public readonly MyDefinitionId passage = MyVisualScriptLogicProvider.GetDefinitionId("Passage", null);
        public readonly MyDefinitionId door = MyVisualScriptLogicProvider.GetDefinitionId("Door", null);
        public readonly MyDefinitionId stairsAndRamp = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeStairs");
        public readonly MyDefinitionId catwalk = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeSteelCatwalk");
        public readonly MyDefinitionId covers = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeCoverWall");
        public readonly MyDefinitionId interiorWall = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeBlockInteriorWall");
        public readonly MyDefinitionId pillar = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeInteriorPillar");
        public readonly MyDefinitionId piston = MyVisualScriptLogicProvider.GetDefinitionId("ExtendedPistonBase", "LargePistonBase");
        public readonly MyDefinitionId pistonTop = MyVisualScriptLogicProvider.GetDefinitionId("PistonTop", "LargePistonTop");
        public readonly MyDefinitionId motor = MyVisualScriptLogicProvider.GetDefinitionId("MotorStator", "LargeStator");
        public readonly MyDefinitionId rotor = MyVisualScriptLogicProvider.GetDefinitionId("MotorRotor", "LargeRotor");
        public readonly MyDefinitionId suspension1x1 = MyVisualScriptLogicProvider.GetDefinitionId("MotorSuspension", "Suspension1x1");
        public readonly MyDefinitionId suspension3x3 = MyVisualScriptLogicProvider.GetDefinitionId("MotorSuspension", "Suspension3x3");
        public readonly MyDefinitionId suspension5x5 = MyVisualScriptLogicProvider.GetDefinitionId("MotorSuspension", "Suspension5x5");
        public readonly MyDefinitionId virtualMass = MyVisualScriptLogicProvider.GetDefinitionId("VirtualMass", "VirtualMassLarge");
        public readonly MyDefinitionId spaceBall = MyVisualScriptLogicProvider.GetDefinitionId("SpaceBall", "SpaceBallLarge");
        public readonly MyDefinitionId smallRealwheel1x1 = MyVisualScriptLogicProvider.GetDefinitionId("Wheel", "SmallRealWheel1x1");
        public readonly MyDefinitionId smallWheel = MyVisualScriptLogicProvider.GetDefinitionId("Wheel", "SmallRealWheel");
        public readonly MyDefinitionId smallRealWheel5x5 = MyVisualScriptLogicProvider.GetDefinitionId("Wheel", "SmallRealWheel5x5");
        public readonly MyDefinitionId realWheel1x1 = MyVisualScriptLogicProvider.GetDefinitionId("Wheel", "RealWheel1x1");
        public readonly MyDefinitionId realWheel = MyVisualScriptLogicProvider.GetDefinitionId("Wheel", "RealWheel");
        public readonly MyDefinitionId realWheel5x5 = MyVisualScriptLogicProvider.GetDefinitionId("Wheel", "RealWheel5x5");
        public readonly MyDefinitionId wheel = MyVisualScriptLogicProvider.GetDefinitionId("Wheel", "Wheel1x1");
        public readonly MyDefinitionId ladder = MyVisualScriptLogicProvider.GetDefinitionId("Ladder2", null);

        //Mining, Refining, Storage & Production
        public readonly MyDefinitionId blastFurnace = MyVisualScriptLogicProvider.GetDefinitionId("Refinery", "Blast Furnace");
        public readonly MyDefinitionId basicAssembler = MyVisualScriptLogicProvider.GetDefinitionId("Assembler", "BasicAssembler");
        public readonly MyDefinitionId assembler = MyVisualScriptLogicProvider.GetDefinitionId("Assembler", "LargeAssembler");
        public readonly MyDefinitionId oreDetector = MyVisualScriptLogicProvider.GetDefinitionId("OreDetector", "LargeOreDetector");
        public readonly MyDefinitionId drill = MyVisualScriptLogicProvider.GetDefinitionId("Drill", "LargeBlockDrill");
        public readonly MyDefinitionId smallCargo = MyVisualScriptLogicProvider.GetDefinitionId("CargoContainer", "SmallBlockSmallContainer");
        public readonly MyDefinitionId mediumCargo = MyVisualScriptLogicProvider.GetDefinitionId("CargoContainer", "SmallBlockMediumContainer");
        public readonly MyDefinitionId conveyor = MyVisualScriptLogicProvider.GetDefinitionId("Conveyor", "LargeBlockConveyor");
        public readonly MyDefinitionId collector = MyVisualScriptLogicProvider.GetDefinitionId("Collector", "Collector");
        public readonly MyDefinitionId connector = MyVisualScriptLogicProvider.GetDefinitionId("ShipConnector", "Connector");
        public readonly MyDefinitionId smallConveyor = MyVisualScriptLogicProvider.GetDefinitionId("ConveyorConnector", "ConveyorTubeSmall");
        public readonly MyDefinitionId conveyorFrame = MyVisualScriptLogicProvider.GetDefinitionId("ConveyorConnector", "ConveyorFrameMedium");
        public readonly MyDefinitionId conveyorSorter = MyVisualScriptLogicProvider.GetDefinitionId("ConveyorSorter", "LargeBlockConveyorSorter");
        public readonly MyDefinitionId smallConveyorSorter = MyVisualScriptLogicProvider.GetDefinitionId("ConveyorSorter", "SmallBlockConveyorSorter");
        public readonly MyDefinitionId grinder = MyVisualScriptLogicProvider.GetDefinitionId("ShipGrinder", "LargeShipGrinder");
        public readonly MyDefinitionId welder = MyVisualScriptLogicProvider.GetDefinitionId("ShipWelder", "LargeShipWelder");
        public readonly MyDefinitionId mergeBlock = MyVisualScriptLogicProvider.GetDefinitionId("MergeBlock", "LargeShipMergeBlock");
        public readonly MyDefinitionId ejector = MyVisualScriptLogicProvider.GetDefinitionId("ShipConnector", "ConnectorSmall");
        public readonly MyDefinitionId smallBARS = MyVisualScriptLogicProvider.GetDefinitionId("ShipWelder", "SELtdSmallNanobotBuildAndRepairSystem");
        public readonly MyDefinitionId largeBARS = MyVisualScriptLogicProvider.GetDefinitionId("ShipWelder", "SELtdLargeNanobotBuildAndRepairSystem");
        public readonly MyDefinitionId soilTray = MyVisualScriptLogicProvider.GetDefinitionId("Assembler", "CropGrower");
        public readonly MyDefinitionId waterRecycler = MyVisualScriptLogicProvider.GetDefinitionId("Refinery", "WRS");
        public readonly MyDefinitionId openHydroponics = MyVisualScriptLogicProvider.GetDefinitionId("Refinery", "Hydroponics2");
        public readonly MyDefinitionId kitchen = MyVisualScriptLogicProvider.GetDefinitionId("Assembler", "Kitchen");

        //Communications & Sensors
        public readonly MyDefinitionId radioAntennaLarge = MyVisualScriptLogicProvider.GetDefinitionId("RadioAntenna", "LargeBlockRadioAntenna");
        public readonly MyDefinitionId radioAntennaSmall = MyVisualScriptLogicProvider.GetDefinitionId("RadioAntenna", "SmallBlockRadioAntenna");
        public readonly MyDefinitionId beacon = MyVisualScriptLogicProvider.GetDefinitionId("Beacon", "LargeBlockBeacon");
        public readonly MyDefinitionId sensor = MyVisualScriptLogicProvider.GetDefinitionId("SensorBlock", "LargeBlockSensor");
        public readonly MyDefinitionId soundBlock = MyVisualScriptLogicProvider.GetDefinitionId("SoundBlock", "LargeBlockSoundBlock");
        public readonly MyDefinitionId cornerLCD = MyVisualScriptLogicProvider.GetDefinitionId("TextPanel", "LargeBlockCorner_LCD_1");
        public readonly MyDefinitionId textPanel = MyVisualScriptLogicProvider.GetDefinitionId("TextPanel", "LargeTextPanel");
        public readonly MyDefinitionId remoteControl = MyVisualScriptLogicProvider.GetDefinitionId("RemoteControl", "LargeBlockRemoteControl");
        public readonly MyDefinitionId camera = MyVisualScriptLogicProvider.GetDefinitionId("CameraBlock", "LargeCameraBlock");
        public readonly MyDefinitionId buttonPanel = MyVisualScriptLogicProvider.GetDefinitionId("ButtonPanel", "ButtonPanelLarge");
        public readonly MyDefinitionId timerBlock = MyVisualScriptLogicProvider.GetDefinitionId("TimerBlock", "TimerBlockLarge");

        //Control Panels, Stations, Landing Gear & Medical
        public readonly MyDefinitionId controlPanel = MyVisualScriptLogicProvider.GetDefinitionId("TerminalBlock", "ControlPanel");
        public readonly MyDefinitionId landingGear = MyVisualScriptLogicProvider.GetDefinitionId("LandingGear", "LargeBlockLandingGear");
        public readonly MyDefinitionId survivalKit = MyVisualScriptLogicProvider.GetDefinitionId("SurvivalKit", "SurvivalKitLarge");
        public readonly MyDefinitionId medicalRoom = MyVisualScriptLogicProvider.GetDefinitionId("MedicalRoom", "LargeMedicalRoom");
        public readonly MyDefinitionId stationControl = MyVisualScriptLogicProvider.GetDefinitionId("Cockpit", "LargeBlockCockpit");
        public readonly MyDefinitionId cockpit = MyVisualScriptLogicProvider.GetDefinitionId("Cockpit", "LargeBlockCockpitSeat");
        public readonly MyDefinitionId fighterCockpit = MyVisualScriptLogicProvider.GetDefinitionId("Cockpit", "DBSmallBlockFighterCockpit");
        public readonly MyDefinitionId flightSeat = MyVisualScriptLogicProvider.GetDefinitionId("Cockpit", "CockpitOpen");
        public readonly MyDefinitionId passengerSeat = MyVisualScriptLogicProvider.GetDefinitionId("Cockpit", "PassengerSeatLarge");
        public readonly MyDefinitionId cryoPod = MyVisualScriptLogicProvider.GetDefinitionId("CryoChamber", "LargeBlockCryoChamber");
        public readonly MyDefinitionId gyroscope = MyVisualScriptLogicProvider.GetDefinitionId("Gyro", "LargeBlockGyro");
        public readonly MyDefinitionId progBlock = MyVisualScriptLogicProvider.GetDefinitionId("MyProgrammableBlock", "LargeProgrammableBlock");
        public readonly MyDefinitionId parachute = MyVisualScriptLogicProvider.GetDefinitionId("Parachute", "LgParachute");

        //Lights
        public readonly MyDefinitionId spotLight = MyVisualScriptLogicProvider.GetDefinitionId("ReflectorLight", "LargeBlockFrontLight");
        public readonly MyDefinitionId interiorLight = MyVisualScriptLogicProvider.GetDefinitionId("InteriorLight", "SmallLight");
        public readonly MyDefinitionId cornerLight = MyVisualScriptLogicProvider.GetDefinitionId("InteriorLight", "LargeBlockLight_1corner");

        //Oxygen, Hydrogen & Power
        public readonly MyDefinitionId oxygenFarm = MyVisualScriptLogicProvider.GetDefinitionId("OxygenFarm", "LargeBlockOxygenFarm");
        public readonly MyDefinitionId oxygenGeneratorLarge = MyVisualScriptLogicProvider.GetDefinitionId("OxygenGenerator", null);
        public readonly MyDefinitionId oxygenGeneratorSmall = MyVisualScriptLogicProvider.GetDefinitionId("OxygenGenerator", "OxygenGeneratorSmall");
        public readonly MyDefinitionId oxygenTankLarge = MyVisualScriptLogicProvider.GetDefinitionId("OxygenTank", null);
        public readonly MyDefinitionId oxygenTankSmall = MyVisualScriptLogicProvider.GetDefinitionId("OxygenTank", "OxygenTankSmall");
        public readonly MyDefinitionId hydrogenTankLarge = MyVisualScriptLogicProvider.GetDefinitionId("OxygenTank", "LargeHydrogenTank");
        public readonly MyDefinitionId hydrogenTankSmall = MyVisualScriptLogicProvider.GetDefinitionId("OxygenTank", "SmallHydrogenTank");
        public readonly MyDefinitionId airVent = MyVisualScriptLogicProvider.GetDefinitionId("AirVent", null);
        public readonly MyDefinitionId solarPanel = MyVisualScriptLogicProvider.GetDefinitionId("SolarPanel", "LargeBlockSolarPanel");
        public readonly MyDefinitionId battery = MyVisualScriptLogicProvider.GetDefinitionId("BatteryBlock", "LargeBlockBatteryBlock");
        public readonly MyDefinitionId smallBattery = MyVisualScriptLogicProvider.GetDefinitionId("BatteryBlock", "SmallBlockSmallBatteryBlock");
        public readonly MyDefinitionId windTurbine = MyVisualScriptLogicProvider.GetDefinitionId("WindTurbine", "LargeBlockWindTurbine");


        //Weapons
        public readonly MyDefinitionId smallGatlingGun = MyVisualScriptLogicProvider.GetDefinitionId("SmallGatlingGun", null);
        public readonly MyDefinitionId smallReloadableRocketLauncher = MyVisualScriptLogicProvider.GetDefinitionId("SmallMissileLauncherReload", "SmallRocketLauncherReload");
        public readonly MyDefinitionId interiorTurret = MyVisualScriptLogicProvider.GetDefinitionId("InteriorTurret", "LargeInteriorTurret");
        public readonly MyDefinitionId decoy = MyVisualScriptLogicProvider.GetDefinitionId("Decoy", "LargeDecoy");
        public readonly MyDefinitionId warhead = MyVisualScriptLogicProvider.GetDefinitionId("Warhead", "LargeWarhead");
        public readonly MyDefinitionId gatlingTurret = MyVisualScriptLogicProvider.GetDefinitionId("LargeGatlingTurret", null);
        public readonly MyDefinitionId missileTurret = MyVisualScriptLogicProvider.GetDefinitionId("LargeMissileTurret", null);
        public readonly MyDefinitionId missileLauncher = MyVisualScriptLogicProvider.GetDefinitionId("SmallMissileLauncher", "LargeMissileLauncher");

        //Windows
        public readonly MyDefinitionId steelWindow = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeWindowSquare");
        public readonly MyDefinitionId window1x2Slope = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window1x2Slope");
        public readonly MyDefinitionId window1x2Inv = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window1x2Inv");
        public readonly MyDefinitionId window1x2SideLeft = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window1x2SideLeft");
        public readonly MyDefinitionId window1x2SideLeftInv = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window1x2SideLeftInv");
        public readonly MyDefinitionId window1x2SideRight = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window1x2SideRight");
        public readonly MyDefinitionId window1x2SideRightInv = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window1x2SideRightInv");
        public readonly MyDefinitionId window1x1Face = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window1x1Face");
        public readonly MyDefinitionId window1x2Flat = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window1x2Flat");
        public readonly MyDefinitionId window1x1Flat = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window1x1Flat");
        public readonly MyDefinitionId window3x3Flat = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window3x3Flat");
        public readonly MyDefinitionId window2x3Flat = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "Window2x3Flat");
        #endregion

        #region tier1 Definitions
        //Heavy Armor
        public readonly MyDefinitionId heavyArmor = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeHeavyBlockArmorBlock");
        public readonly MyDefinitionId heavyArmorRoundSlope = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeHeavyBlockArmorRoundSlope");
        public readonly MyDefinitionId heavyArmorRoundCorner = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeHeavyBlockArmorRoundCorner");
        public readonly MyDefinitionId heavyArmorSlope = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeHeavyBlockArmorSlope2Base");
        public readonly MyDefinitionId heavyArmorCorner = MyVisualScriptLogicProvider.GetDefinitionId("CubeBlock", "LargeHeavyBlockArmorCorner2Base");

        //Atmospheric Thrusters
        private readonly MyDefinitionId atmoThrusterSmallShipSmall = MyVisualScriptLogicProvider.GetDefinitionId("Thrust", "SmallBlockSmallAtmosphericThrust");
        private readonly MyDefinitionId atmoThrusterSmallShipLarge = MyVisualScriptLogicProvider.GetDefinitionId("Thrust", "SmallBlockLargeAtmosphericThrust");
        private readonly MyDefinitionId atmoThrusterLargeShipSmall = MyVisualScriptLogicProvider.GetDefinitionId("Thrust", "LargeBlockSmallAtmosphericThrust");
        private readonly MyDefinitionId atmoThrusterLargeShipLarge = MyVisualScriptLogicProvider.GetDefinitionId("Thrust", "LargeBlockLargeAtmosphericThrust");

        //Mining, Refining, Storage & Production
        public readonly MyDefinitionId refinery = MyVisualScriptLogicProvider.GetDefinitionId("Refinery", "LargeRefinery");
        public readonly MyDefinitionId powerModule = MyVisualScriptLogicProvider.GetDefinitionId("UpgradeModule", "LargeEnergyModule");
        public readonly MyDefinitionId productivityModule = MyVisualScriptLogicProvider.GetDefinitionId("UpgradeModule", "LargeProductivityModule");
        public readonly MyDefinitionId effectivenessModule = MyVisualScriptLogicProvider.GetDefinitionId("UpgradeModule", "LargeEffectivenessModule");
        public readonly MyDefinitionId largeCargo = MyVisualScriptLogicProvider.GetDefinitionId("CargoContainer", "LargeBlockLargeContainer");
        #endregion

        #region tier2 Definitions
        //Hydrogen Thrusters
        private readonly MyDefinitionId hydroThrusterLargeShipSmall = MyVisualScriptLogicProvider.GetDefinitionId("Thrust", "LargeBlockSmallHydrogenThrust");
        private readonly MyDefinitionId hydroThrusterLargeShipLarge = MyVisualScriptLogicProvider.GetDefinitionId("Thrust", "LargeBlockLargeHydrogenThrust");
        #endregion

        #region tier3 Definitions
        //Ion Thrusters
        private readonly MyDefinitionId ionThrusterLargeShipSmall = MyVisualScriptLogicProvider.GetDefinitionId("Thrust", "LargeBlockSmallThrust");
        private readonly MyDefinitionId ionThrusterLargeShipLarge = MyVisualScriptLogicProvider.GetDefinitionId("Thrust", "LargeBlockLargeThrust");

        //Gravity & Jump Drive
        public readonly MyDefinitionId gravityGenerator = MyVisualScriptLogicProvider.GetDefinitionId("GravityGenerator", null);
        public readonly MyDefinitionId gravityGeneratorSpherical = MyVisualScriptLogicProvider.GetDefinitionId("GravityGeneratorSphere", null);
        public readonly MyDefinitionId jumpDrive = MyVisualScriptLogicProvider.GetDefinitionId("JumpDrive", "LargeJumpDrive");
        #endregion

        #region tier4 Definitions
        //Projectors
        public readonly MyDefinitionId projectorLarge = MyVisualScriptLogicProvider.GetDefinitionId("MyObjectBuilder_Projector", "LargeProjector");
        public readonly MyDefinitionId projectorSmall = MyVisualScriptLogicProvider.GetDefinitionId("MyObjectBuilder_Projector", "SmallProjector");
        #endregion

        //NPC Faction and Founder Data
        public static List<long> NPCFactionFounders = new List<long>();
        public static List<IMyPlayer> PlayerList = new List<IMyPlayer>();
        public static HashSet<IMyEntity> EntityList = new HashSet<IMyEntity>();

        NetworkHandlerSystem nhs = new NetworkHandlerSystem(3);
        Settings settings = new Settings();
        Dictionary<long, HashSet<MyDefinitionId>> playersData = new Dictionary<long, HashSet<MyDefinitionId>>();
        Dictionary<MyDefinitionId, HashSet<MyDefinitionId>> variantGroups = new Dictionary<MyDefinitionId, HashSet<MyDefinitionId>>(MyDefinitionId.Comparer);
        Dictionary<MyCubeBlockDefinition, HashSet<MyCubeBlockDefinition>> blockDefintions = new Dictionary<MyCubeBlockDefinition, HashSet<MyCubeBlockDefinition>>();
        HashSet<MyDefinitionId> tier0Unlocks = new HashSet<MyDefinitionId>();
        HashSet<MyDefinitionId> tier0BlockIds = new HashSet<MyDefinitionId>();
        HashSet<MyDefinitionId> tier1BlockIds = new HashSet<MyDefinitionId>();
        HashSet<MyDefinitionId> tier2BlockIds = new HashSet<MyDefinitionId>();
        HashSet<MyDefinitionId> tier3BlockIds = new HashSet<MyDefinitionId>();
        HashSet<MyDefinitionId> tier4BlockIds = new HashSet<MyDefinitionId>();
        HashSet<MyDefinitionId> tier5BlockIds = new HashSet<MyDefinitionId>();
        HashSet<GpsLocation> gpsList = new HashSet<GpsLocation>();
        HashSet<IMyCubeGrid> NPCList = new HashSet<IMyCubeGrid>();

        Dictionary<long, ulong> userIds = new Dictionary<long, ulong>();

        const string configFile = "config.xml";
        const string playerFile = "{0}.xml";
        private MessageEventCaller<long> PlayerInit;
        bool networkCommsInit = false;
        Guid gridStorageKey = new Guid("0622A557-9D9B-4FE2-A589-ADAD9FF933FC");

        public MessageEventCaller<MyDefinitionId> SendUnlockNotification { get; private set; }

        public override void Deinitialize()
        {
            MyAPIGateway.Session.Player.Controller.ControlledEntityChanged -= Controller_ControlledEntityChanged;
            nhs.Unload();
        }
        
        public override bool Initialize(out MyUpdateOrder order)
        {
            Logging.Instance.WriteLine(nameof(Initialize));
            order = MyUpdateOrder.NoUpdate;
            if (networkCommsInit == false)
            {
                networkCommsInit = true;
            }
            EntityList.Clear();
            NPCList.Clear();
            NPCFactionFounders.Clear();
            PlayerList.Clear();
            InitFactionData();

            MyAPIGateway.Entities.GetEntities(EntityList);
            foreach (var entity in EntityList)
            {
                var cubeGrid = entity as IMyCubeGrid;
                if (cubeGrid == null)
                {
                    continue;
                }
                try
                {
                    List<IMySlimBlock> slimBlocks = new List<IMySlimBlock>();
                    cubeGrid.GetBlocks(slimBlocks, x => x.FatBlock is IMyTerminalBlock);
                    var cubeGridOwnerList = new List<long>();
                    bool foundNpcOwner = false;
                    foreach (var block in slimBlocks)
                    {
                        if (NPCFactionFounders.Contains(block.OwnerId))
                        {
                            foundNpcOwner = true;
                        }
                        if (foundNpcOwner)
                        {
                            var sblock = block.FatBlock as IMyEntity;
                            if (sblock.Storage == null)
                                sblock.Storage = new MyModStorageComponent();
                            sblock.Storage.Add(gridStorageKey, "Blah");
                            //Logging.Instance.WriteLine(cubeGrid.DisplayName + "has NPC Owner");
                            //NPCList.Add(cubeGrid);
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    Logging.Instance.WriteLine("Error" + e);
                }
            }
            if (NetworkHandlerSystem.IsServer)
            {
                try
                {
                    using (var sw = MyAPIGateway.Utilities.ReadFileInWorldStorage(configFile, typeof(Core)))
                        settings = MyAPIGateway.Utilities.SerializeFromXML<Settings>(sw.ReadToEnd());
                }
                catch (Exception e)
                {
                    Logging.Instance.WriteLine("Exception Error: " + e.ToString());
                }
                if (settings.AlwaysUnlocked == null)
                    settings.AlwaysUnlocked = new HashSet<SerializableDefinitionId>();
                MyAPIGateway.Session.DamageSystem.RegisterDestroyHandler(0, DestroyHandler);
                MyVisualScriptLogicProvider.PlayerResearchClearAll();
                PrepareCache();
                //MyAPIGateway.Parallel.StartBackground();
            }
            PlayerInit = nhs.Create<long>(null, PlayerJoined, EventOptions.OnlyToServer);
            SendUnlockNotification = nhs.Create<MyDefinitionId>(LearnedById, null, EventOptions.OnlyToTarget);
            MyAPIGateway.Entities.OnEntityAdd += NewEntityDetected;
            if (NetworkHandlerSystem.IsClient)
            {
                if (MyAPIGateway.Session.Player == null)
                    return false;
                MyVisualScriptLogicProvider.ResearchListWhitelist(true);
                MyVisualScriptLogicProvider.PlayerResearchClear();
                if (MyAPIGateway.Session.Player.Controller.ControlledEntity != null)
                {
                    //MyVisualScriptLogicProvider.ShowNotificationToAll("running", 10000, "Green");
                    MyAPIGateway.Utilities.InvokeOnGameThread(() => PlayerInit(MyAPIGateway.Session.Player.IdentityId));
                }
                else
                {
                    MyAPIGateway.Session.Player.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged;
                }
            }
            return true;
        }

        public static void InitFactionData()
        {
            //Get NPC Faction Data
            var defaultFactionList = MyDefinitionManager.Static.GetDefaultFactions();
            foreach (var faction in defaultFactionList)
            {
                //Get Default Factions and Add Them
                var defaultFaction = MyAPIGateway.Session.Factions.TryGetFactionByTag(faction.Tag);
                if (defaultFaction != null)
                {
                    if (defaultFaction.IsEveryoneNpc())
                    {
                        NPCFactionFounders.Add(defaultFaction.FounderId);
                    }
                }
            }

            //Get Existing / Remaining NPC Faction Data
            var allFactions = MyAPIGateway.Session.Factions.Factions;
            foreach (var faction in allFactions.Keys)
            {

                var thisFaction = allFactions[faction];
                if (thisFaction.IsEveryoneNpc())
                {
                    NPCFactionFounders.Add(thisFaction.FounderId);
                }
            }
        }

        public void NewEntityDetected(IMyEntity entity)
        {
            IMyCubeGrid newCubeGrid = entity as IMyCubeGrid;
            try
            {
                if (newCubeGrid == null) return;
                List<IMySlimBlock> slimBlocks = new List<IMySlimBlock>();
                newCubeGrid.GetBlocks(slimBlocks, x => x.FatBlock is IMyTerminalBlock);
                var cubeGridOwnerList = new List<long>();
                bool foundNpcOwner = false;
                foreach (var block in slimBlocks)
                {
                    var sblock = block.FatBlock as IMyEntity;
                    if (sblock.Storage == null)
                        sblock.Storage = new MyModStorageComponent();
                    sblock.Storage.Add(gridStorageKey, "Blah");
                    if (NPCFactionFounders.Contains(block.OwnerId))
                    {
                        foundNpcOwner = true;
                    }
                }
                if (foundNpcOwner)
                {
                    //Logging.Instance.WriteLine(newCubeGrid.DisplayName + "has NPC Owner");
                    NPCList.Add(newCubeGrid);
                }
            }
            catch (Exception e)
            {
                Logging.Instance.WriteLine("Error" + e);
            }
        }

        void LearnedById(MyDefinitionId id, ulong sender)
        {
            try
            {
                Logging.Instance.WriteLine($"You can now build {MyDefinitionManager.Static.GetCubeBlockDefinition(id).DisplayNameText}.");
                if (MyDefinitionManager.Static.GetCubeBlockDefinition(id).DisplayNameText.Contains("Heavy"))
                {
                    MyAPIGateway.Utilities.ShowNotification("Target Destroyed: You can now build Heavy Armor", 4000, "Green");
                    return;
                }
                MyAPIGateway.Utilities.ShowNotification($"Target Destroyed: You can now build {MyDefinitionManager.Static.GetCubeBlockDefinition(id).DisplayNameText}.", 15000, "Green");
            }
            catch (Exception e)
            {
                Logging.Instance.WriteLine("Exception Error: " + e.ToString());
            }
        }

        private void Controller_ControlledEntityChanged(VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg1, VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg2)
        {
            if (arg2 != null && arg2.Entity is IMyCharacter)
            {
                MyAPIGateway.Utilities.InvokeOnGameThread(() =>
                {
                    if (MyAPIGateway.Session != null && MyAPIGateway.Session.Player != null)
                        PlayerInit(MyAPIGateway.Session.Player.IdentityId);
                });
                MyAPIGateway.Session.Player.Controller.ControlledEntityChanged -= Controller_ControlledEntityChanged;
            }
        }

        void DestroyHandler(object target, MyDamageInformation damage)
        {

            if (damage.Type == MyDamageType.Bullet || damage.Type == MyDamageType.Explosion || damage.Type == MyDamageType.Weapon || damage.Type == MyDamageType.Grind ||
                damage.Type == MyDamageType.Rocket || damage.Type == MyDamageType.Bolt)
            {
                if (target is IMySlimBlock && target != null)
                {
                    var slimBlock = target as IMySlimBlock;
                    var attachedGrid = slimBlock.CubeGrid;
                    var slimBlockAsEntity = slimBlock.FatBlock as IMyEntity;
                    if ((slimBlock.BlockDefinition.Id.TypeId == typeof(MyObjectBuilder_MyProgrammableBlock) ||
                        slimBlock.BlockDefinition.Id.TypeId == typeof(MyObjectBuilder_RemoteControl) ||
                        slimBlock.BlockDefinition.Id.TypeId == typeof(MyObjectBuilder_Cockpit)) &&
                        !slimBlock.BlockDefinition.Id.SubtypeId.ToString().Contains("Passenger"))
                        
                    {
                        try
                        {
                            if (slimBlockAsEntity.Storage != null)
                            {
                                if (slimBlockAsEntity.Storage.ContainsKey(gridStorageKey))
                                {
                                    //MyVisualScriptLogicProvider.ClearNotifications();
                                    var players = new List<IMyPlayer>();
                                    var player = MyAPIGateway.Session.LocalHumanPlayer;
                                    int blkGpsCount = 1;
                                    List<IMySlimBlock> slimBkList = new List<IMySlimBlock>();
                                    MyAPIGateway.Players.GetPlayers(players);
                                    Random randomizer = new Random();
                                    attachedGrid.GetBlocks(slimBkList, b => b.FatBlock is IMyTerminalBlock);
                                   
                                    foreach (var block in slimBkList)
                                    {
                                        
                                        var blockAsSlimblk = block as IMySlimBlock;
                                        
                                        if (block.BlockDefinition.Id.TypeId == typeof(MyObjectBuilder_MyProgrammableBlock) ||
                                            block.BlockDefinition.Id.TypeId == typeof(MyObjectBuilder_RemoteControl) ||
                                            block.BlockDefinition.Id.TypeId == typeof(MyObjectBuilder_Cockpit) &&
                                            !blockAsSlimblk.BlockDefinition.Id.SubtypeId.ToString().Contains("Passenger"))
                                        {
                                            var blockAsEntity = block.FatBlock as IMyEntity;
                                            if (blockAsEntity.Name == null || blockAsEntity.Name == "" || blockAsEntity.Name != attachedGrid.EntityId.ToString() && blockAsEntity != null)
                                            {
                                                var blkGpsLocation = new GpsLocation();
                                                var blockName = "AP"+blkGpsCount.ToString();
                                                MyVisualScriptLogicProvider.SetName(blockAsEntity.EntityId, blockName);
                                                MyVisualScriptLogicProvider.AddGPSToEntity(blockName, blockName, blockName, Color.MediumPurple, player.IdentityId);
                                                blkGpsLocation.EntityId = blockAsEntity.EntityId;
                                                blkGpsLocation.EntityName = blockName;
                                                blkGpsLocation.GpsName = blockName;
                                                blkGpsLocation.GpsDescription = blockName;
                                                if (!gpsList.Contains(blkGpsLocation))
                                                {
                                                    gpsList.Add(blkGpsLocation);
                                                }
                                                blkGpsCount++;
                                            }
                                        }
                                    }
                                    attachedGrid.OnBlockRemoved += RemoveGps;
                                    
                                    if (tier1BlockIds != null)
                                    {
                                        foreach (var pl in players)
                                        {
                                            MyVisualScriptLogicProvider.ShowNotificationToAll(tier1BlockIds.Count.ToString(), 3000, "Green");
                                            var randomBlockId = tier1BlockIds.ElementAt(randomizer.Next(tier1BlockIds.Count));
                                            
                                            var playerData = playersData[pl.IdentityId];
                                            var blockId = randomBlockId;
                                            int count = 0;
                                            //Logging.Instance.WriteLine("1st pass: " + blockId.ToString());
                                            
                                            while ((playerData.Contains(blockId) && count < tier1BlockIds.Count) || (blockId.ToString().Contains("Nurf") && count < tier1BlockIds.Count))
                                            {
                                                randomBlockId = tier1BlockIds.ElementAt(randomizer.Next(tier1BlockIds.Count));
                                                blockId = randomBlockId;
                                                var blockTierId = MyDefinitionManager.Static.GetCubeBlockDefinition(blockId);
                                                //Logging.Instance.WriteLine("2nd pass = " + blockId.ToString());
                                                count++;
                                            }
                                            count = 0;
                                            if (tier2BlockIds != null)
                                            {
                                                while (playerData.Contains(blockId) && count < tier2BlockIds.Count)
                                                {
                                                    randomBlockId = tier2BlockIds.ElementAt(randomizer.Next(tier2BlockIds.Count));
                                                    blockId = randomBlockId;
                                                    var blockTierId = MyDefinitionManager.Static.GetCubeBlockDefinition(blockId);
                                                    //Logging.Instance.WriteLine("3rd pass = " + blockId.ToString());
                                                    count++;
                                                }
                                            }
                                            count = 0;
                                            if (tier3BlockIds != null)
                                            {
                                                while (playerData.Contains(blockId) && count < tier3BlockIds.Count)
                                                {
                                                    randomBlockId = tier3BlockIds.ElementAt(randomizer.Next(tier3BlockIds.Count));
                                                    blockId = randomBlockId;
                                                    var blockTierId = MyDefinitionManager.Static.GetCubeBlockDefinition(blockId);
                                                    //Logging.Instance.WriteLine("4th pass = " + blockId.ToString());
                                                    count++;
                                                }
                                            }
                                            count = 0;
                                            if (tier4BlockIds != null)
                                            {
                                                while (playerData.Contains(blockId) && count < tier4BlockIds.Count)
                                                {
                                                    randomBlockId = tier4BlockIds.ElementAt(randomizer.Next(tier4BlockIds.Count));
                                                    blockId = randomBlockId;
                                                    var blockTierId = MyDefinitionManager.Static.GetCubeBlockDefinition(blockId);
                                                    //Logging.Instance.WriteLine("5th pass = " + blockId.ToString());
                                                    count++;
                                                }
                                            }
                                            count = 0;
                                            if (tier5BlockIds != null)
                                            {
                                                while (playerData.Contains(blockId) && count < tier5BlockIds.Count)
                                                {
                                                    randomBlockId = tier5BlockIds.ElementAt(randomizer.Next(tier5BlockIds.Count));
                                                    blockId = randomBlockId;
                                                    var blockTierId = MyDefinitionManager.Static.GetCubeBlockDefinition(blockId);
                                                    //Logging.Instance.WriteLine("6th pass = " + blockId.ToString());
                                                    count++;
                                                }
                                            }
                                            if (pl != null)
                                            {
                                                if (target is IMySlimBlock)
                                                {
                                                    var slim = target as IMySlimBlock;
                                                    CalculatePlayers (blockId, slim.CubeGrid.GridIntegerToWorld(slim.Position), damage.AttackerId);
                                                }
                                                else if (target is IMyCubeBlock)//just in cause
                                                {
                                                    var fat = target as IMyCubeBlock;
                                                    CalculatePlayers(blockId, fat.GetPosition(), damage.AttackerId);
                                                }
                                                //UnlockById(blockId, target. pl.IdentityId);

                                            }
                                            Vector3D explodeCoords = attachedGrid.GetPosition();
                                            if (damage.Type != MyDamageType.Grind)
                                            {
                                                MyVisualScriptLogicProvider.CreateExplosion(explodeCoords, 14, 1000);
                                            }
                                            
                                            attachedGrid = null;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MyVisualScriptLogicProvider.ShowNotificationToAll("Destroy Handler Error", 10000, "Red");
                            Logging.Instance.WriteLine("Destroy Handler Error:- " + e);
                        }
                    }
                }
            }
        }

        void CalculatePlayers(MyDefinitionId blockId, Vector3D pos, long attackerId)
        {
            IMyEntity ent;
            if (settings.UseLearnRadius && settings.LearnRadius > 0)
            {
                if (MyAPIGateway.Entities.TryGetEntityById(attackerId, out ent))
                {
                    {
                        var sphere = new BoundingSphereD(ent.GetPosition(), settings.LearnRadius);
                        var players = new List<IMyPlayer>();
                        MyAPIGateway.Players.GetPlayers(players, p =>
                        {
                            return sphere.Contains(p.GetPosition()) != ContainmentType.Disjoint;
                        });
                        foreach (var player in players)
                        {
                            UnlockById(blockId, player.IdentityId);
                        }
                    }
                }
            }
            if (MyAPIGateway.Entities.TryGetEntityById(attackerId, out ent))
            {
                {
                    var pos2 = ent.GetPosition();
                    var players = new List<IMyPlayer>();
                    MyAPIGateway.Players.GetPlayers(players);
                    var pl = players.OrderBy(p => (pos2 - p.GetPosition()).LengthSquared()).FirstOrDefault();
                    if (pl != null)
                    {
                        UnlockById(blockId, pl.IdentityId);
                    }
                }
            }
        }

        public void RemoveGps(IMySlimBlock slimblk)
        {
            try
            {
                var player = MyAPIGateway.Session.LocalHumanPlayer;
                var slimBlkAsEntity = slimblk.FatBlock as IMyEntity;
                if (slimBlkAsEntity != null)
                {
                    foreach (var listing in gpsList)
                    {
                        if (listing.EntityId == slimBlkAsEntity.EntityId)
                        {
                            //Logging.Instance.WriteLine("Match");
                            if (listing.EntityName != null)
                            {
                                MyVisualScriptLogicProvider.RemoveGPS(listing.GpsName, player.IdentityId);
                                //MyVisualScriptLogicProvider.RemoveGPSFromEntity(listing.EntityName, listing.GpsName, listing.GpsDescription, player.IdentityId);
                                //Logging.Instance.WriteLine("Removing EName " + listing.EntityName.ToString() + "\n\r " + "GName " + listing.GpsName.ToString() + "\n\r " + "GDesc " + listing.GpsDescription.ToString() + "\n\r " + "PID " + player.IdentityId);
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                MyVisualScriptLogicProvider.ShowNotificationToAll("Error: - " + e, 10000, "Red");
            } 
        }

        // This method prepares an array of 'block groups' so when a 'block' is unlocked the
        // UnlockById method can check for which 'group' it's part of and unlock associated blocks.
        // BlockStages actually represents block variants.
        private void PrepareCache() 
        {
            //Handles block variants for UnlockById method & adds modded blocks to a final tier.
            foreach (var cube in MyDefinitionManager.Static.GetAllDefinitions().OfType<MyCubeBlockDefinition>())
            {
                if (cube.BlockStages != null && cube.BlockStages.Length > 0)
                {
                    var ids = new HashSet<MyDefinitionId>(cube.BlockStages, MyDefinitionId.Comparer);
                    ids.Add(cube.Id);
                    foreach (var id in ids)
                    {
                        variantGroups[id] = ids;
                    }
                }
            }

            Dictionary<string, string> blueprintDictionary = new Dictionary<string, string>(); //Component Name (SubtypeId) & Blueprint Name (SubtypeId)
            List<MyBlueprintDefinitionBase> blueprintList = MyDefinitionManager.Static.GetBlueprintDefinitions().ToList();
            foreach (var blueprint in blueprintList)
            {
                if (blueprint.Results[0].Id.TypeId.ToString().Contains("MyObjectBuilder_Component"))
                {
                    if (!blueprintDictionary.ContainsKey(blueprint.Results[0].Id.SubtypeId.ToString()))
                    {
                        blueprintDictionary.Add(blueprint.Results[0].Id.SubtypeId.ToString(), blueprint.Id.SubtypeId.ToString());
                    }
                }
            }
            
            foreach (var cube in MyDefinitionManager.Static.GetAllDefinitions().OfType<MyCubeBlockDefinition>())
            {
                bool containsIronOnly = true;
                bool containsPlat = false;
                bool containsSilverOrGold = false;
                bool containsModdedOres = false;
                if (cube?.Components == null) continue;
                foreach (var component in cube.Components)
                {
                    if (blueprintDictionary.ContainsKey(component.Definition.Id.SubtypeId.ToString()))
                    {
                        MyDefinitionId blueprint = new MyDefinitionId();
                        if (MyDefinitionId.TryParse("MyObjectBuilder_BlueprintDefinition/" + blueprintDictionary[component.Definition.Id.SubtypeId.ToString()], out blueprint))
                        {
                            var bp = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
                            List<MyBlueprintDefinitionBase.Item> list = bp.Prerequisites.ToList();
                            foreach (var componentOreCost in bp.Prerequisites)
                            {
                                //Logging.Instance.WriteLine("CUBE: " + cube.Id.ToString());
                                //Logging.Instance.WriteLine("COMP: " + component.Definition.Id.SubtypeId.ToString());
                                //Logging.Instance.WriteLine("COST: " + componentOreCost.ToString());
                                if (componentOreCost.ToString().Contains("Nickel") || componentOreCost.ToString().Contains("Cobalt") ||
                                   componentOreCost.ToString().Contains("Magnesium") || componentOreCost.ToString().Contains("Silicon") || componentOreCost.ToString().Contains("Silver") ||
                                   componentOreCost.ToString().Contains("Gold") || componentOreCost.ToString().Contains("Platinum") || componentOreCost.ToString().Contains("Stone"))
                                {
                                    containsIronOnly = false;
                                }
                                if (componentOreCost.ToString().Contains("Platinum"))
                                {
                                    containsPlat = true;
                                    containsIronOnly = false;
                                }
                                if (componentOreCost.ToString().Contains("Gold") || componentOreCost.ToString().Contains("Silver"))
                                {
                                    containsSilverOrGold = true;
                                    containsIronOnly = false;
                                }
                                if (!componentOreCost.ToString().Contains("Iron") && !componentOreCost.ToString().Contains("Nickel") && !componentOreCost.ToString().Contains("Cobalt") &&
                                    !componentOreCost.ToString().Contains("Magnesium") && !componentOreCost.ToString().Contains("Silicon") && !componentOreCost.ToString().Contains("Silver") &&
                                    !componentOreCost.ToString().Contains("Gold") && !componentOreCost.ToString().Contains("Platinum") && !componentOreCost.ToString().Contains("Stone"))
                                {
                                    containsModdedOres = true;
                                    containsIronOnly = false;
                                }
                                
                            }
                        }
                    }
                }
                if (cube.Id != null)
                {
                    if (containsIronOnly == true)
                    {
                        tier0BlockIds.Add(cube.Id);
                        //Logging.Instance.WriteLine("Tier 0 Added  = " + cube.Id);
                        foreach (var cubeBlk in tier0BlockIds)
                        {
                            var cubeBlkDef = MyDefinitionManager.Static.GetDefinition(cubeBlk);
                            cubeBlkDef.Public = false;
                        }
                    }
                    if (containsPlat == false && containsSilverOrGold == false && containsModdedOres == false && cube.Public)
                    {
                        tier1BlockIds.Add(cube.Id);
                        foreach (var cubeBlk in tier1BlockIds)
                        {
                            var cubeBlkDef = MyDefinitionManager.Static.GetDefinition(cubeBlk);
                            cubeBlkDef.Public = false;
                        }
                        //Logging.Instance.WriteLine("Tier 1 Added  = " + cube.Id);
                    }
                    if (containsPlat == false && containsSilverOrGold && cube.Public && containsModdedOres == false)
                    {
                        tier2BlockIds.Add(cube.Id);
                        foreach (var cubeBlk in tier2BlockIds)
                        {
                            var cubeBlkDef = MyDefinitionManager.Static.GetDefinition(cubeBlk);
                            cubeBlkDef.Public = false;
                        }
                        //Logging.Instance.WriteLine("Tier 2 Added  = " + cube.Id);
                    }
                    if (containsPlat == true && containsModdedOres == false && cube.Public)
                    {
                        tier3BlockIds.Add(cube.Id);
                        foreach (var cubeBlk in tier3BlockIds)
                        {
                            var cubeBlkDef = MyDefinitionManager.Static.GetDefinition(cubeBlk);
                            cubeBlkDef.Public = false;
                        }
                        //Logging.Instance.WriteLine("Tier 3 Added  = " + cube.Id);
                    }
                    if (containsModdedOres == true && cube.Public)
                    {
                        tier4BlockIds.Add(cube.Id);
                        foreach (var cubeBlk in tier4BlockIds)
                        {
                            var cubeBlkDef = MyDefinitionManager.Static.GetDefinition(cubeBlk);
                            cubeBlkDef.Public = false;
                        }
                        //Logging.Instance.WriteLine("Tier 4 Added  = " + cube.Id);
                    }
                }
            }
        }

        public override void SaveData()
        {
            if (!Initialized)
                return;
            using (var sw = MyAPIGateway.Utilities.WriteFileInWorldStorage(configFile, typeof(Core)))
                sw.Write(MyAPIGateway.Utilities.SerializeToXML(settings));
            foreach (var player in playersData)
            {
                try
                {
                    using (var sw = MyAPIGateway.Utilities.WriteFileInWorldStorage(string.Format(playerFile, player.Key), typeof(Core)))
                        sw.Write(MyAPIGateway.Utilities.SerializeToXML(player.Value.Select(s => (SerializableDefinitionId)s).ToList()));
                }
                catch { }
            }
        }

        void PlayerJoined(long playerID, ulong sender)
        {
            if (sender == 0)
                sender = MyAPIGateway.Session?.Player?.SteamUserId ?? 0;
            userIds[playerID] = sender;
            var playerIds = (HashSet<MyDefinitionId>)null;
            try
            {
                using (var sw = MyAPIGateway.Utilities.ReadFileInWorldStorage(string.Format(playerFile, playerID), typeof(Core)))
                {
                    var ids = MyAPIGateway.Utilities.SerializeFromXML<List<SerializableDefinitionId>>(sw.ReadToEnd());
                    playerIds = new HashSet<MyDefinitionId>(ids.Select(s => (MyDefinitionId)s), MyDefinitionId.Comparer);
                }
            }
            catch
            {
            }
            if (playerIds == null)
                playerIds = new HashSet<MyDefinitionId>(MyDefinitionId.Comparer);
            if (playerIds.Count == 0)
            {
                MyVisualScriptLogicProvider.ClearAllToolbarSlots(playerID);
            }
            foreach (var id in settings.AlwaysUnlocked)
            {
                playerIds.Add(id);
            }
            playersData[playerID] = playerIds;
            foreach (var id in playerIds.ToList())
            {
                UnlockById(id, playerID, true);
            }
            if(tier0BlockIds != null){
                foreach (var id in tier0BlockIds)
                {
                    UnlockById(id, playerID, true);
                }
            }
            #region Tier0Unlocks
            //Mining, Refining, Storage & Production
            tier0Unlocks.Add(refinery);
            tier0Unlocks.Add(blastFurnace);
            tier0Unlocks.Add(basicAssembler);
            tier0Unlocks.Add(assembler);
            tier0Unlocks.Add(oreDetector);
            tier0Unlocks.Add(drill);
            tier0Unlocks.Add(smallCargo);
            tier0Unlocks.Add(mediumCargo);
            tier0Unlocks.Add(conveyor);
            tier0Unlocks.Add(collector);
            tier0Unlocks.Add(connector);
            tier0Unlocks.Add(smallConveyor);
            tier0Unlocks.Add(conveyorFrame);
            tier0Unlocks.Add(conveyorSorter);
            tier0Unlocks.Add(smallConveyorSorter);
            tier0Unlocks.Add(grinder);
            tier0Unlocks.Add(welder);
            tier0Unlocks.Add(mergeBlock);
            tier0Unlocks.Add(ejector);
            tier0Unlocks.Add(smallBARS);
            tier0Unlocks.Add(largeBARS);
            tier0Unlocks.Add(soilTray);
            tier0Unlocks.Add(waterRecycler);
            tier0Unlocks.Add(openHydroponics);
            tier0Unlocks.Add(kitchen);

            //Control Panels, Stations, Landing Gear & Medical
            tier0Unlocks.Add(controlPanel);
            tier0Unlocks.Add(landingGear);
            tier0Unlocks.Add(medicalRoom);
            tier0Unlocks.Add(stationControl);
            tier0Unlocks.Add(cockpit);
            tier0Unlocks.Add(fighterCockpit);
            tier0Unlocks.Add(flightSeat);
            tier0Unlocks.Add(passengerSeat);
            tier0Unlocks.Add(cryoPod);
            tier0Unlocks.Add(gyroscope);
            tier0Unlocks.Add(progBlock);
            tier0Unlocks.Add(parachute);
            tier0Unlocks.Add(survivalKit);

            //Communications & Sensors
            tier0Unlocks.Add(radioAntennaLarge);
            tier0Unlocks.Add(radioAntennaSmall);
            tier0Unlocks.Add(beacon);
            tier0Unlocks.Add(sensor);
            tier0Unlocks.Add(soundBlock);
            tier0Unlocks.Add(cornerLCD);
            tier0Unlocks.Add(textPanel);
            tier0Unlocks.Add(remoteControl);
            tier0Unlocks.Add(camera);
            tier0Unlocks.Add(buttonPanel);
            tier0Unlocks.Add(timerBlock);

            //Lights
            tier0Unlocks.Add(spotLight);
            tier0Unlocks.Add(interiorLight);
            tier0Unlocks.Add(cornerLight);

            //Weapons
            tier0Unlocks.Add(gatlingTurret);
            tier0Unlocks.Add(smallGatlingGun);
            tier0Unlocks.Add(smallReloadableRocketLauncher);
            tier0Unlocks.Add(interiorTurret);
            tier0Unlocks.Add(decoy);

            //Oxygen, Hydrogen & Power
            tier0Unlocks.Add(oxygenFarm);
            tier0Unlocks.Add(oxygenGeneratorLarge);
            tier0Unlocks.Add(oxygenGeneratorSmall);
            tier0Unlocks.Add(oxygenTankLarge);
            tier0Unlocks.Add(oxygenTankSmall);
            tier0Unlocks.Add(hydrogenTankLarge);
            tier0Unlocks.Add(hydrogenTankSmall);
            tier0Unlocks.Add(airVent);
            tier0Unlocks.Add(solarPanel);
            tier0Unlocks.Add(battery);
            tier0Unlocks.Add(smallBattery);
            tier0Unlocks.Add(windTurbine);
            
            //Armors, Passages, Pistons, Motors & Suspension
            tier0Unlocks.Add(lightArmor);
            tier0Unlocks.Add(lightArmorRoundSlope);
            tier0Unlocks.Add(lightArmorRoundSlope2);
            tier0Unlocks.Add(lightArmorCorner);
            tier0Unlocks.Add(lightArmorCorner2);
            tier0Unlocks.Add(passage);
            tier0Unlocks.Add(door);
            tier0Unlocks.Add(stairsAndRamp);
            tier0Unlocks.Add(catwalk);
            tier0Unlocks.Add(covers);
            tier0Unlocks.Add(interiorWall);
            tier0Unlocks.Add(pillar);
            tier0Unlocks.Add(piston);
            tier0Unlocks.Add(pistonTop);
            tier0Unlocks.Add(motor);
            tier0Unlocks.Add(rotor);
            tier0Unlocks.Add(suspension1x1);
            tier0Unlocks.Add(suspension3x3);
            tier0Unlocks.Add(suspension5x5);
            tier0Unlocks.Add(virtualMass);
            tier0Unlocks.Add(spaceBall);
            tier0Unlocks.Add(smallRealwheel1x1);
            tier0Unlocks.Add(smallWheel);
            tier0Unlocks.Add(smallRealWheel5x5);
            tier0Unlocks.Add(realWheel1x1);
            tier0Unlocks.Add(realWheel);
            tier0Unlocks.Add(realWheel5x5);
            tier0Unlocks.Add(wheel);
            
            //Windows
            tier0Unlocks.Add(steelWindow);
            tier0Unlocks.Add(window1x2Slope);
            tier0Unlocks.Add(window1x2Inv);
            tier0Unlocks.Add(window1x2SideLeft);
            tier0Unlocks.Add(window1x2SideLeftInv);
            tier0Unlocks.Add(window1x2SideRight);
            tier0Unlocks.Add(window1x2SideRightInv);
            tier0Unlocks.Add(window1x1Face);
            tier0Unlocks.Add(window1x2Flat);
            tier0Unlocks.Add(window1x1Flat);
            tier0Unlocks.Add(window3x3Flat);
            tier0Unlocks.Add(window2x3Flat);
            #endregion
            foreach (var cube in tier0Unlocks)
            {
                UnlockById(cube, playerID, true);
            }
        }

        public void UnlockById(MyDefinitionId blockId, long player, bool force = false)
        {
            try
            {
                var playerData = playersData[player];
                if (!force && playerData.Contains(blockId))
                {
                    return;
                }
                var ids = new HashSet<MyDefinitionId>();
                ids.Add(blockId);
                var cubeblock = MyDefinitionManager.Static.GetCubeBlockDefinition(blockId);
                cubeblock.Public = true;
                if (!cubeblock.Public)
                    return;
                ulong steamId;
                if (!force && userIds.TryGetValue(player, out steamId))
                {
                    SendUnlockNotification(blockId, steamId);
                }
                var defGrp = MyDefinitionManager.Static.TryGetDefinitionGroup(cubeblock.BlockPairName);
                if (blockId.ToString().Contains("Heavy"))
                {
                    ids.Add(heavyArmor);
                    ids.Add(heavyArmorCorner);
                    ids.Add(heavyArmorRoundCorner);
                    ids.Add(heavyArmorRoundSlope);
                    ids.Add(heavyArmorSlope);
                }
                if (defGrp != null)
                {
                    if (defGrp.Large != null)
                        ids.Add(defGrp.Large.Id);
                    if (defGrp.Small != null)
                        ids.Add(defGrp.Small.Id);
                }
                if (!cubeblock.GuiVisible || (cubeblock.BlockStages != null && cubeblock.BlockStages.Length > 0))
                    foreach (var blkId in ids.ToList())
                    {
                        HashSet<MyDefinitionId> blocks;
                        if (variantGroups.TryGetValue(blkId, out blocks))
                        {
                            if (blocks != null)
                                foreach (var block in blocks)
                                {
                                    ids.Add(block);
                                }
                        }
                    }
                    foreach (var id in ids)
                    {
                    var cubeBlkDef = MyDefinitionManager.Static.GetDefinition(id);
                    cubeBlkDef.Public = true;
                    playerData.Add(id);
                    MyVisualScriptLogicProvider.PlayerResearchUnlock(player, id);
                    }
            }
            catch
            {
            }
        }

        protected override void UnloadData()
        {
            MyAPIGateway.Entities.OnEntityAdd -= NewEntityDetected;
            Logging.Instance.Close();

        }
    }

    public class Settings
    {
        public float LearnRadius { get; set; } = 1500;
        public bool UseLearnRadius { get; set; } = false;
        public HashSet<SerializableDefinitionId> AlwaysUnlocked = new HashSet<SerializableDefinitionId>();

    }

    public class GpsLocation
    {
        //Create Properties
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public string GpsName { get; set; }
        public string GpsDescription { get; set; }

        public GpsLocation()
        {
            EntityId = 0;
            EntityName = "";
            GpsName = "";
            GpsDescription = "";
        }
    }
}
