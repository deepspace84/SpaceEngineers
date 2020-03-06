using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using Sandbox.Game;
using Sandbox.Game.EntityComponents;


namespace DSC
{ 

    class DSC_BlockDefinitions
    {
        #region Teir0r Definitions
        //Armors, Passages, Pistons, Motors, Suspension & Wheels
        public static string lightArmor = "MyObjectBuilder_CubeBlock/LargeBlockArmorBlock";
        public static string lightArmorRoundSlope = "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundSlope";
        public static string lightArmorRoundSlope2 = "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Base";
        public static string lightArmorCorner = "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Base";
        public static string lightArmorCorner2 = "MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Base";
        public static string passage = "MyObjectBuilder_Passage/(null)";
        public static string door = "MyObjectBuilder_Door/(null)";
        public static string stairsAndRamp = "MyObjectBuilder_CubeBlock/LargeStairs";
        public static string catwalk = "MyObjectBuilder_CubeBlock/LargeSteelCatwalk";
        public static string covers = "MyObjectBuilder_CubeBlock/LargeCoverWall";
        public static string interiorWall = "MyObjectBuilder_CubeBlock/LargeBlockInteriorWall";
        public static string pillar = "MyObjectBuilder_CubeBlock/LargeInteriorPillar";
        public static string piston = "MyObjectBuilder_ExtendedPistonBase/LargePistonBase";
        public static string pistonTop = "MyObjectBuilder_PistonTop/LargePistonTop";
        public static string motor = "MyObjectBuilder_MotorStator/LargeStator";
        public static string rotor = "MyObjectBuilder_MotorRotor/LargeRotor";
        public static string suspension1x1 = "MyObjectBuilder_MotorSuspension/Suspension1x1";
        public static string suspension3x3 = "MyObjectBuilder_MotorSuspension/Suspension3x3";
        public static string suspension5x5 = "MyObjectBuilder_MotorSuspension/Suspension5x5";
        public static string virtualMass = "MyObjectBuilder_VirtualMass/VirtualMassLarge";
        public static string spaceBall = "MyObjectBuilder_SpaceBall/SpaceBallLarge";
        public static string smallRealwheel1x1 = "MyObjectBuilder_Wheel/SmallRealWheel1x1";
        public static string smallWheel = "MyObjectBuilder_Wheel/SmallRealWheel";
        public static string smallRealWheel5x5 = "MyObjectBuilder_Wheel/SmallRealWheel5x5";
        public static string realWheel1x1 = "MyObjectBuilder_Wheel/RealWheel1x1";
        public static string realWheel = "MyObjectBuilder_Wheel/RealWheel";
        public static string realWheel5x5 = "MyObjectBuilder_Wheel/RealWheel5x5";
        public static string wheel = "MyObjectBuilder_Wheel/Wheel1x1";
        public static string ladder = "MyObjectBuilder_Ladder2/(null)";

        //Mining, Refining, Storage & Production
        public static string blastFurnace = "MyObjectBuilder_Refinery/Blast Furnace";
        public static string basicAssembler = "MyObjectBuilder_Assembler/BasicAssembler";
        public static string assembler = "MyObjectBuilder_Assembler/LargeAssembler";
        public static string oreDetector = "MyObjectBuilder_OreDetector/LargeOreDetector";
        public static string drill = "MyObjectBuilder_Drill/LargeBlockDrill";
        public static string smallCargo = "MyObjectBuilder_CargoContainer/SmallBlockSmallContainer";
        public static string mediumCargo = "MyObjectBuilder_CargoContainer/SmallBlockMediumContainer";
        public static string conveyor = "MyObjectBuilder_Conveyor/LargeBlockConveyor";
        public static string collector = "MyObjectBuilder_Collector/Collector";
        public static string connector = "MyObjectBuilder_ShipConnector/Connector";
        public static string smallConveyor = "MyObjectBuilder_ConveyorConnector/ConveyorTubeSmall";
        public static string conveyorFrame = "MyObjectBuilder_ConveyorConnector/ConveyorFrameMedium";
        public static string conveyorSorter = "MyObjectBuilder_ConveyorSorter/LargeBlockConveyorSorter";
        public static string smallConveyorSorter = "MyObjectBuilder_ConveyorSorter/SmallBlockConveyorSorter";
        public static string grinder = "MyObjectBuilder_ShipGrinder/LargeShipGrinder";
        public static string welder = "MyObjectBuilder_ShipWelder/LargeShipWelder";
        public static string mergeBlock = "MyObjectBuilder_MergeBlock/LargeShipMergeBlock";
        public static string ejector = "MyObjectBuilder_ShipConnector/ConnectorSmall";
        public static string smallBARS = "MyObjectBuilder_ShipWelder/SELtdSmallNanobotBuildAndRepairSystem";
        public static string largeBARS = "MyObjectBuilder_ShipWelder/SELtdLargeNanobotBuildAndRepairSystem";
        public static string soilTray = "MyObjectBuilder_Assembler/CropGrower";
        public static string waterRecycler = "MyObjectBuilder_Refinery/WRS";
        public static string openHydroponics = "MyObjectBuilder_Refinery/Hydroponics2";
        public static string kitchen = "MyObjectBuilder_Assembler/Kitchen";

        //Communications & Sensors
        public static string radioAntennaLarge = "MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna";
        public static string radioAntennaSmall = "MyObjectBuilder_RadioAntenna/SmallBlockRadioAntenna";
        public static string beacon = "MyObjectBuilder_Beacon/LargeBlockBeacon";
        public static string sensor = "MyObjectBuilder_SensorBlock/LargeBlockSensor";
        public static string soundBlock = "MyObjectBuilder_SoundBlock/LargeBlockSoundBlock";
        public static string cornerLCD = "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_1";
        public static string textPanel = "MyObjectBuilder_TextPanel/LargeTextPanel";
        public static string remoteControl = "MyObjectBuilder_RemoteControl/LargeBlockRemoteControl";
        public static string camera = "MyObjectBuilder_CameraBlock/LargeCameraBlock";
        public static string buttonPanel = "MyObjectBuilder_ButtonPanel/ButtonPanelLarge";
        public static string timerBlock = "MyObjectBuilder_TimerBlock/TimerBlockLarge";

        //Control Panels, Stations, Landing Gear & Medical
        public static string controlPanel = "MyObjectBuilder_TerminalBlock/ControlPanel";
        public static string landingGear = "MyObjectBuilder_LandingGear/LargeBlockLandingGear";
        public static string survivalKit = "MyObjectBuilder_SurvivalKit/SurvivalKitLarge";
        public static string medicalRoom = "MyObjectBuilder_MedicalRoom/LargeMedicalRoom";
        public static string stationControl = "MyObjectBuilder_Cockpit/LargeBlockCockpit";
        public static string cockpit = "MyObjectBuilder_Cockpit/LargeBlockCockpitSeat";
        public static string fighterCockpit = "MyObjectBuilder_Cockpit/DBSmallBlockFighterCockpit";
        public static string flightSeat = "MyObjectBuilder_Cockpit/CockpitOpen";
        public static string passengerSeat = "MyObjectBuilder_Cockpit/PassengerSeatLarge";
        public static string cryoPod = "MyObjectBuilder_CryoChamber/LargeBlockCryoChamber";
        public static string gyroscope = "MyObjectBuilder_Gyro/LargeBlockGyro";
        public static string progBlock = "MyObjectBuilder_MyProgrammableBlock/LargeProgrammableBlock";
        public static string parachute = "MyObjectBuilder_Parachute/LgParachute";

        //Lights
        public static string spotLight = "MyObjectBuilder_ReflectorLight/LargeBlockFrontLight";
        public static string interiorLight = "MyObjectBuilder_InteriorLight/SmallLight";
        public static string cornerLight = "MyObjectBuilder_InteriorLight/LargeBlockLight_1corner";

        //Oxygen, Hydrogen & Power
        public static string oxygenFarm = "MyObjectBuilder_OxygenFarm/LargeBlockOxygenFarm";
        public static string oxygenGeneratorLarge = "MyObjectBuilder_OxygenGenerator/(null)";
        public static string oxygenGeneratorSmall = "MyObjectBuilder_OxygenGenerator/OxygenGeneratorSmall";
        public static string oxygenTankLarge = "MyObjectBuilder_OxygenTank/(null)";
        public static string oxygenTankSmall = "MyObjectBuilder_OxygenTank/OxygenTankSmall";
        public static string hydrogenTankLarge = "MyObjectBuilder_OxygenTank/LargeHydrogenTank";
        public static string hydrogenTankSmall = "MyObjectBuilder_OxygenTank/SmallHydrogenTank";
        public static string airVent = "MyObjectBuilder_AirVent/(null)";
        public static string solarPanel = "MyObjectBuilder_SolarPanel/LargeBlockSolarPanel";
        public static string battery = "MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock";
        public static string smallBattery = "MyObjectBuilder_BatteryBlock/SmallBlockSmallBatteryBlock";
        public static string windTurbine = "MyObjectBuilder_WindTurbine/LargeBlockWindTurbine";


        //Weapons
        public static string smallGatlingGun = "MyObjectBuilder_SmallGatlingGun/(null)";
        public static string smallReloadableRocketLauncher = "MyObjectBuilder_SmallMissileLauncherReload/SmallRocketLauncherReload";
        public static string interiorTurret = "MyObjectBuilder_InteriorTurret/LargeInteriorTurret";
        public static string decoy = "MyObjectBuilder_Decoy/LargeDecoy";
        public static string warhead = "MyObjectBuilder_Warhead/LargeWarhead";
        public static string gatlingTurret = "MyObjectBuilder_LargeGatlingTurret/(null)";
        public static string missileTurret = "MyObjectBuilder_LargeMissileTurret/(null)";
        public static string missileLauncher = "MyObjectBuilder_SmallMissileLauncher/LargeMissileLauncher";

        //Windows
        public static string steelWindow = "MyObjectBuilder_CubeBlock/LargeWindowSquare";
        public static string window1x2Slope = "MyObjectBuilder_CubeBlock/Window1x2Slope";
        public static string window1x2Inv = "MyObjectBuilder_CubeBlock/Window1x2Inv";
        public static string window1x2SideLeft = "MyObjectBuilder_CubeBlock/Window1x2SideLeft";
        public static string window1x2SideLeftInv = "MyObjectBuilder_CubeBlock/Window1x2SideLeftInv";
        public static string window1x2SideRight = "MyObjectBuilder_CubeBlock/Window1x2SideRight";
        public static string window1x2SideRightInv = "MyObjectBuilder_CubeBlock/Window1x2SideRightInv";
        public static string window1x1Face = "MyObjectBuilder_CubeBlock/Window1x1Face";
        public static string window1x2Flat = "MyObjectBuilder_CubeBlock/Window1x2Flat";
        public static string window1x1Flat = "MyObjectBuilder_CubeBlock/Window1x1Flat";
        public static string window3x3Flat = "MyObjectBuilder_CubeBlock/Window3x3Flat";
        public static string window2x3Flat = "MyObjectBuilder_CubeBlock/Window2x3Flat";
        #endregion

        #region tier1 Definitions
        //Heavy Armor
        public static string heavyArmor = "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorBlock";
        public static string heavyArmorRoundSlope = "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundSlope";
        public static string heavyArmorRoundCorner = "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundCorner";
        public static string heavyArmorSlope = "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope2Base";
        public static string heavyArmorCorner = "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner2Base";

        //Atmospheric Thrusters
        private static string atmoThrusterSmallShipSmall = "MyObjectBuilder_Thrust/SmallBlockSmallAtmosphericThrust";
        private static string atmoThrusterSmallShipLarge = "MyObjectBuilder_Thrust/SmallBlockLargeAtmosphericThrust";
        private static string atmoThrusterLargeShipSmall = "MyObjectBuilder_Thrust/LargeBlockSmallAtmosphericThrust";
        private static string atmoThrusterLargeShipLarge = "MyObjectBuilder_Thrust/LargeBlockLargeAtmosphericThrust";

        //Mining, Refining, Storage & Production
        public static string refinery = "MyObjectBuilder_Refinery/LargeRefinery";
        public static string powerModule = "MyObjectBuilder_UpgradeModule/LargeEnergyModule";
        public static string productivityModule = "MyObjectBuilder_UpgradeModule/LargeProductivityModule";
        public static string effectivenessModule = "MyObjectBuilder_UpgradeModule/LargeEffectivenessModule";
        public static string largeCargo = "MyObjectBuilder_CargoContainer/LargeBlockLargeContainer";
        #endregion

        #region tier2 Definitions
        //Hydrogen Thrusters
        private static string hydroThrusterLargeShipSmall = "MyObjectBuilder_Thrust/LargeBlockSmallHydrogenThrust";
        private static string hydroThrusterLargeShipLarge = "MyObjectBuilder_Thrust/LargeBlockLargeHydrogenThrust";
        #endregion

        #region tier3 Definitions
        //Ion Thrusters
        private static string ionThrusterLargeShipSmall = "MyObjectBuilder_Thrust/LargeBlockSmallThrust";
        private static string ionThrusterLargeShipLarge = "MyObjectBuilder_Thrust/LargeBlockLargeThrust";

        //Gravity & Jump Drive
        public static string gravityGenerator = "MyObjectBuilder_GravityGenerator/(null)";
        public static string gravityGeneratorSpherical = "MyObjectBuilder_GravityGeneratorSphere/(null)";
        public static string jumpDrive = "MyObjectBuilder_JumpDrive/LargeJumpDrive";
        #endregion

        #region tier4 Definitions
        //Projectors
        public static string projectorLarge = "MyObjectBuilder_MyObjectBuilder_Projector/LargeProjector";
        public static string projectorSmall = "MyObjectBuilder_MyObjectBuilder_Projector/SmallProjector";
        #endregion
    }
}
