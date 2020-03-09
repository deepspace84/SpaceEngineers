using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using Sandbox.Game;
using Sandbox.Game.EntityComponents;


namespace DSC
{ 
    class DSC_ComponentDefinitions
    {
        public static string constructionComponent = MyVisualScriptLogicProvider.GetDefinitionId("Component", "Construction").ToString();
        public static string interiorPlate = MyVisualScriptLogicProvider.GetDefinitionId("Component", "InteriorPlate").ToString();
        public static string steelPlate = MyVisualScriptLogicProvider.GetDefinitionId("Component", "SteelPlate").ToString();
        public static string girder = MyVisualScriptLogicProvider.GetDefinitionId("Component", "Girder").ToString();

    }

    class DSC_BlockDefinitions
    {

        public static Dictionary<string, Dictionary<string, string>> blocks = new Dictionary<string, Dictionary<string, string>>(){
            {"test", new Dictionary<string, string>(){{"test","test"}}}
        };

        public static string [] CubeBlock_LargeRailStraight = { "LargeRailStraight", "MyObjectBuilder_CubeBlock", "LargeRailStraight", "MyObjectBuilder_CubeBlock/LargeRailStraight" };
        public static string [] CubeBlock_LargeBlockArmorBlock = { "Light Armor Block", "MyObjectBuilder_CubeBlock", "LargeBlockArmorBlock", "MyObjectBuilder_CubeBlock/LargeBlockArmorBlock" };
        public static string [] CubeBlock_LargeBlockArmorSlope = { "Light Armor Slope", "MyObjectBuilder_CubeBlock", "LargeBlockArmorSlope", "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope" };
        public static string [] CubeBlock_LargeBlockArmorCorner = { "Light Armor Corner", "MyObjectBuilder_CubeBlock", "LargeBlockArmorCorner", "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner" };
        public static string [] CubeBlock_LargeBlockArmorCornerInv = { "Light Armor Inv. Corner", "MyObjectBuilder_CubeBlock", "LargeBlockArmorCornerInv", "MyObjectBuilder_CubeBlock/LargeBlockArmorCornerInv" };
        public static string [] CubeBlock_LargeRoundArmor_Slope = { "Round Armor Slope", "MyObjectBuilder_CubeBlock", "LargeRoundArmor_Slope", "MyObjectBuilder_CubeBlock/LargeRoundArmor_Slope" };
        public static string [] CubeBlock_LargeRoundArmor_Corner = { "Round Armor Corner", "MyObjectBuilder_CubeBlock", "LargeRoundArmor_Corner", "MyObjectBuilder_CubeBlock/LargeRoundArmor_Corner" };
        public static string [] CubeBlock_LargeRoundArmor_CornerInv = { "LargeRoundArmor_CornerInv", "MyObjectBuilder_CubeBlock", "LargeRoundArmor_CornerInv", "MyObjectBuilder_CubeBlock/LargeRoundArmor_CornerInv" };
        public static string [] CubeBlock_LargeHeavyBlockArmorBlock = { "Heavy Armor Block", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorBlock", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorBlock" };
        public static string [] CubeBlock_LargeHeavyBlockArmorSlope = { "Heavy Armor Slope", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorSlope", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope" };
        public static string [] CubeBlock_LargeHeavyBlockArmorCorner = { "Heavy Armor Corner", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorCorner", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner" };
        public static string [] CubeBlock_LargeHeavyBlockArmorCornerInv = { "Heavy Armor Inv. Corner", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorCornerInv", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCornerInv" };
        public static string [] CubeBlock_SmallBlockArmorBlock = { "Light Armor Block", "MyObjectBuilder_CubeBlock", "SmallBlockArmorBlock", "MyObjectBuilder_CubeBlock/SmallBlockArmorBlock" };
        public static string [] CubeBlock_SmallBlockArmorSlope = { "Light Armor Slope", "MyObjectBuilder_CubeBlock", "SmallBlockArmorSlope", "MyObjectBuilder_CubeBlock/SmallBlockArmorSlope" };
        public static string [] CubeBlock_SmallBlockArmorCorner = { "Light Armor Corner", "MyObjectBuilder_CubeBlock", "SmallBlockArmorCorner", "MyObjectBuilder_CubeBlock/SmallBlockArmorCorner" };
        public static string [] CubeBlock_SmallBlockArmorCornerInv = { "Light Armor Inv. Corner", "MyObjectBuilder_CubeBlock", "SmallBlockArmorCornerInv", "MyObjectBuilder_CubeBlock/SmallBlockArmorCornerInv" };
        public static string [] CubeBlock_SmallHeavyBlockArmorBlock = { "Heavy Armor Block", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorBlock", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorBlock" };
        public static string [] CubeBlock_SmallHeavyBlockArmorSlope = { "Heavy Armor Slope", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorSlope", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope" };
        public static string [] CubeBlock_SmallHeavyBlockArmorCorner = { "Heavy Armor Corner", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorCorner", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner" };
        public static string [] CubeBlock_SmallHeavyBlockArmorCornerInv = { "Heavy Armor Inv. Corner", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorCornerInv", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCornerInv" };
        public static string [] CubeBlock_LargeHalfArmorBlock = { "Half Light Armor Block", "MyObjectBuilder_CubeBlock", "LargeHalfArmorBlock", "MyObjectBuilder_CubeBlock/LargeHalfArmorBlock" };
        public static string [] CubeBlock_LargeHeavyHalfArmorBlock = { "Half Heavy Armor Block", "MyObjectBuilder_CubeBlock", "LargeHeavyHalfArmorBlock", "MyObjectBuilder_CubeBlock/LargeHeavyHalfArmorBlock" };
        public static string [] CubeBlock_LargeHalfSlopeArmorBlock = { "Half Slope Light Armor Block", "MyObjectBuilder_CubeBlock", "LargeHalfSlopeArmorBlock", "MyObjectBuilder_CubeBlock/LargeHalfSlopeArmorBlock" };
        public static string [] CubeBlock_LargeHeavyHalfSlopeArmorBlock = { "Half Slope Heavy Armor Block", "MyObjectBuilder_CubeBlock", "LargeHeavyHalfSlopeArmorBlock", "MyObjectBuilder_CubeBlock/LargeHeavyHalfSlopeArmorBlock" };
        public static string [] CubeBlock_HalfArmorBlock = { "Half Light Armor Block", "MyObjectBuilder_CubeBlock", "HalfArmorBlock", "MyObjectBuilder_CubeBlock/HalfArmorBlock" };
        public static string [] CubeBlock_HeavyHalfArmorBlock = { "Half Heavy Armor Block", "MyObjectBuilder_CubeBlock", "HeavyHalfArmorBlock", "MyObjectBuilder_CubeBlock/HeavyHalfArmorBlock" };
        public static string [] CubeBlock_HalfSlopeArmorBlock = { "Half Slope Light Armor Block", "MyObjectBuilder_CubeBlock", "HalfSlopeArmorBlock", "MyObjectBuilder_CubeBlock/HalfSlopeArmorBlock" };
        public static string [] CubeBlock_HeavyHalfSlopeArmorBlock = { "Half Slope Heavy Armor Block", "MyObjectBuilder_CubeBlock", "HeavyHalfSlopeArmorBlock", "MyObjectBuilder_CubeBlock/HeavyHalfSlopeArmorBlock" };
        public static string [] CubeBlock_LargeBlockArmorRoundSlope = { "Round Armor Slope", "MyObjectBuilder_CubeBlock", "LargeBlockArmorRoundSlope", "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundSlope" };
        public static string [] CubeBlock_LargeBlockArmorRoundCorner = { "Round Armor Corner", "MyObjectBuilder_CubeBlock", "LargeBlockArmorRoundCorner", "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner" };
        public static string [] CubeBlock_LargeBlockArmorRoundCornerInv = { "Round Armor Inv. Corner", "MyObjectBuilder_CubeBlock", "LargeBlockArmorRoundCornerInv", "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCornerInv" };
        public static string [] CubeBlock_LargeHeavyBlockArmorRoundSlope = { "Heavy Armor Round Slope", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorRoundSlope", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundSlope" };
        public static string [] CubeBlock_LargeHeavyBlockArmorRoundCorner = { "Heavy Armor Round Corner", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorRoundCorner", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundCorner" };
        public static string [] CubeBlock_LargeHeavyBlockArmorRoundCornerInv = { "Heavy Armor Round Inv. Corner", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorRoundCornerInv", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundCornerInv" };
        public static string [] CubeBlock_SmallBlockArmorRoundSlope = { "Round Armor Slope", "MyObjectBuilder_CubeBlock", "SmallBlockArmorRoundSlope", "MyObjectBuilder_CubeBlock/SmallBlockArmorRoundSlope" };
        public static string [] CubeBlock_SmallBlockArmorRoundCorner = { "Round Armor Corner", "MyObjectBuilder_CubeBlock", "SmallBlockArmorRoundCorner", "MyObjectBuilder_CubeBlock/SmallBlockArmorRoundCorner" };
        public static string [] CubeBlock_SmallBlockArmorRoundCornerInv = { "Round Armor Inv. Corner", "MyObjectBuilder_CubeBlock", "SmallBlockArmorRoundCornerInv", "MyObjectBuilder_CubeBlock/SmallBlockArmorRoundCornerInv" };
        public static string [] CubeBlock_SmallHeavyBlockArmorRoundSlope = { "Heavy Armor Round Slope", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorRoundSlope", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundSlope" };
        public static string [] CubeBlock_SmallHeavyBlockArmorRoundCorner = { "Heavy Armor Round Corner", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorRoundCorner", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCorner" };
        public static string [] CubeBlock_SmallHeavyBlockArmorRoundCornerInv = { "Heavy Armor Round Inv. Corner", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorRoundCornerInv", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCornerInv" };
        public static string [] CubeBlock_LargeBlockArmorSlope2Base = { "Light Armor Slope 2x1x1 Base", "MyObjectBuilder_CubeBlock", "LargeBlockArmorSlope2Base", "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Base" };
        public static string [] CubeBlock_LargeBlockArmorSlope2Tip = { "Light Armor Slope 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "LargeBlockArmorSlope2Tip", "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Tip" };
        public static string [] CubeBlock_LargeBlockArmorCorner2Base = { "Light Armor Corner 2x1x1 Base", "MyObjectBuilder_CubeBlock", "LargeBlockArmorCorner2Base", "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Base" };
        public static string [] CubeBlock_LargeBlockArmorCorner2Tip = { "Light Armor Corner 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "LargeBlockArmorCorner2Tip", "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Tip" };
        public static string [] CubeBlock_LargeBlockArmorInvCorner2Base = { "Light Armor Inv. Corner 2x1x1 Base", "MyObjectBuilder_CubeBlock", "LargeBlockArmorInvCorner2Base", "MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Base" };
        public static string [] CubeBlock_LargeBlockArmorInvCorner2Tip = { "Light Armor Inv. Corner 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "LargeBlockArmorInvCorner2Tip", "MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Tip" };
        public static string [] CubeBlock_LargeHeavyBlockArmorSlope2Base = { "Heavy Armor Slope 2x1x1 Base", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorSlope2Base", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope2Base" };
        public static string [] CubeBlock_LargeHeavyBlockArmorSlope2Tip = { "Heavy Armor Slope 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorSlope2Tip", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope2Tip" };
        public static string [] CubeBlock_LargeHeavyBlockArmorCorner2Base = { "Heavy Armor Corner 2x1x1 Base", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorCorner2Base", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner2Base" };
        public static string [] CubeBlock_LargeHeavyBlockArmorCorner2Tip = { "Heavy Armor Corner 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorCorner2Tip", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner2Tip" };
        public static string [] CubeBlock_LargeHeavyBlockArmorInvCorner2Base = { "Heavy Armor Inv. Corner 2x1x1 Base", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorInvCorner2Base", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorInvCorner2Base" };
        public static string [] CubeBlock_LargeHeavyBlockArmorInvCorner2Tip = { "Heavy Armor Inv. Corner 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "LargeHeavyBlockArmorInvCorner2Tip", "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorInvCorner2Tip" };
        public static string [] CubeBlock_SmallBlockArmorSlope2Base = { "Light Armor Slope 2x1x1 Base", "MyObjectBuilder_CubeBlock", "SmallBlockArmorSlope2Base", "MyObjectBuilder_CubeBlock/SmallBlockArmorSlope2Base" };
        public static string [] CubeBlock_SmallBlockArmorSlope2Tip = { "Light Armor Slope 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "SmallBlockArmorSlope2Tip", "MyObjectBuilder_CubeBlock/SmallBlockArmorSlope2Tip" };
        public static string [] CubeBlock_SmallBlockArmorCorner2Base = { "Light Armor Corner 2x1x1 Base", "MyObjectBuilder_CubeBlock", "SmallBlockArmorCorner2Base", "MyObjectBuilder_CubeBlock/SmallBlockArmorCorner2Base" };
        public static string [] CubeBlock_SmallBlockArmorCorner2Tip = { "Light Armor Corner 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "SmallBlockArmorCorner2Tip", "MyObjectBuilder_CubeBlock/SmallBlockArmorCorner2Tip" };
        public static string [] CubeBlock_SmallBlockArmorInvCorner2Base = { "Light Armor Inv. Corner 2x1x1 Base", "MyObjectBuilder_CubeBlock", "SmallBlockArmorInvCorner2Base", "MyObjectBuilder_CubeBlock/SmallBlockArmorInvCorner2Base" };
        public static string [] CubeBlock_SmallBlockArmorInvCorner2Tip = { "Light Armor Inv. Corner 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "SmallBlockArmorInvCorner2Tip", "MyObjectBuilder_CubeBlock/SmallBlockArmorInvCorner2Tip" };
        public static string [] CubeBlock_SmallHeavyBlockArmorSlope2Base = { "Heavy Armor Slope 2x1x1 Base", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorSlope2Base", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Base" };
        public static string [] CubeBlock_SmallHeavyBlockArmorSlope2Tip = { "Heavy Armor Slope 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorSlope2Tip", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Tip" };
        public static string [] CubeBlock_SmallHeavyBlockArmorCorner2Base = { "Heavy Armor Corner 2x1x1 Base", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorCorner2Base", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Base" };
        public static string [] CubeBlock_SmallHeavyBlockArmorCorner2Tip = { "Heavy Armor Corner 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorCorner2Tip", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Tip" };
        public static string [] CubeBlock_SmallHeavyBlockArmorInvCorner2Base = { "Heavy Armor Inv. Corner 2x1x1 Base", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorInvCorner2Base", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Base" };
        public static string [] CubeBlock_SmallHeavyBlockArmorInvCorner2Tip = { "Heavy Armor Inv. Corner 2x1x1 Tip", "MyObjectBuilder_CubeBlock", "SmallHeavyBlockArmorInvCorner2Tip", "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Tip" };
        public static string [] MyProgrammableBlock_SmallProgrammableBlock = { "Programmable block", "MyObjectBuilder_MyProgrammableBlock", "SmallProgrammableBlock", "MyObjectBuilder_MyProgrammableBlock/SmallProgrammableBlock" };
        public static string [] Projector_LargeProjector = { "Projector", "MyObjectBuilder_Projector", "LargeProjector", "MyObjectBuilder_Projector/LargeProjector" };
        public static string [] Projector_SmallProjector = { "Projector", "MyObjectBuilder_Projector", "SmallProjector", "MyObjectBuilder_Projector/SmallProjector" };
        public static string [] SensorBlock_SmallBlockSensor = { "Sensor", "MyObjectBuilder_SensorBlock", "SmallBlockSensor", "MyObjectBuilder_SensorBlock/SmallBlockSensor" };
        public static string [] SensorBlock_LargeBlockSensor = { "Sensor", "MyObjectBuilder_SensorBlock", "LargeBlockSensor", "MyObjectBuilder_SensorBlock/LargeBlockSensor" };
        public static string [] SoundBlock_SmallBlockSoundBlock = { "Sound Block", "MyObjectBuilder_SoundBlock", "SmallBlockSoundBlock", "MyObjectBuilder_SoundBlock/SmallBlockSoundBlock" };
        public static string [] SoundBlock_LargeBlockSoundBlock = { "Sound Block", "MyObjectBuilder_SoundBlock", "LargeBlockSoundBlock", "MyObjectBuilder_SoundBlock/LargeBlockSoundBlock" };
        public static string [] ButtonPanel_ButtonPanelLarge = { "Button Panel", "MyObjectBuilder_ButtonPanel", "ButtonPanelLarge", "MyObjectBuilder_ButtonPanel/ButtonPanelLarge" };
        public static string [] ButtonPanel_ButtonPanelSmall = { "Button Panel", "MyObjectBuilder_ButtonPanel", "ButtonPanelSmall", "MyObjectBuilder_ButtonPanel/ButtonPanelSmall" };
        public static string [] TimerBlock_TimerBlockLarge = { "Timer Block", "MyObjectBuilder_TimerBlock", "TimerBlockLarge", "MyObjectBuilder_TimerBlock/TimerBlockLarge" };
        public static string [] TimerBlock_TimerBlockSmall = { "Timer Block", "MyObjectBuilder_TimerBlock", "TimerBlockSmall", "MyObjectBuilder_TimerBlock/TimerBlockSmall" };
        public static string [] MyProgrammableBlock_LargeProgrammableBlock = { "Programmable block", "MyObjectBuilder_MyProgrammableBlock", "LargeProgrammableBlock", "MyObjectBuilder_MyProgrammableBlock/LargeProgrammableBlock" };
        public static string [] RadioAntenna_LargeBlockRadioAntenna = { "Antenna", "MyObjectBuilder_RadioAntenna", "LargeBlockRadioAntenna", "MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna" };
        public static string [] Beacon_LargeBlockBeacon = { "Beacon", "MyObjectBuilder_Beacon", "LargeBlockBeacon", "MyObjectBuilder_Beacon/LargeBlockBeacon" };
        public static string [] Beacon_SmallBlockBeacon = { "Beacon", "MyObjectBuilder_Beacon", "SmallBlockBeacon", "MyObjectBuilder_Beacon/SmallBlockBeacon" };
        public static string [] RadioAntenna_SmallBlockRadioAntenna = { "Antenna", "MyObjectBuilder_RadioAntenna", "SmallBlockRadioAntenna", "MyObjectBuilder_RadioAntenna/SmallBlockRadioAntenna" };
        public static string [] RemoteControl_LargeBlockRemoteControl = { "Remote Control", "MyObjectBuilder_RemoteControl", "LargeBlockRemoteControl", "MyObjectBuilder_RemoteControl/LargeBlockRemoteControl" };
        public static string [] RemoteControl_SmallBlockRemoteControl = { "Remote Control", "MyObjectBuilder_RemoteControl", "SmallBlockRemoteControl", "MyObjectBuilder_RemoteControl/SmallBlockRemoteControl" };
        public static string [] LaserAntenna_LargeBlockLaserAntenna = { "Laser Antenna", "MyObjectBuilder_LaserAntenna", "LargeBlockLaserAntenna", "MyObjectBuilder_LaserAntenna/LargeBlockLaserAntenna" };
        public static string [] LaserAntenna_SmallBlockLaserAntenna = { "Laser Antenna", "MyObjectBuilder_LaserAntenna", "SmallBlockLaserAntenna", "MyObjectBuilder_LaserAntenna/SmallBlockLaserAntenna" };
        public static string [] TerminalBlock_ControlPanel = { "Control Panel", "MyObjectBuilder_TerminalBlock", "ControlPanel", "MyObjectBuilder_TerminalBlock/ControlPanel" };
        public static string [] TerminalBlock_SmallControlPanel = { "Control Panel", "MyObjectBuilder_TerminalBlock", "SmallControlPanel", "MyObjectBuilder_TerminalBlock/SmallControlPanel" };
        public static string [] Cockpit_LargeBlockCockpit = { "Control Stations", "MyObjectBuilder_Cockpit", "LargeBlockCockpit", "MyObjectBuilder_Cockpit/LargeBlockCockpit" };
        public static string [] Cockpit_LargeBlockCockpitSeat = { "Cockpit", "MyObjectBuilder_Cockpit", "LargeBlockCockpitSeat", "MyObjectBuilder_Cockpit/LargeBlockCockpitSeat" };
        public static string [] Cockpit_SmallBlockCockpit = { "Cockpit", "MyObjectBuilder_Cockpit", "SmallBlockCockpit", "MyObjectBuilder_Cockpit/SmallBlockCockpit" };
        public static string [] Cockpit_DBSmallBlockFighterCockpit = { "Fighter Cockpit", "MyObjectBuilder_Cockpit", "DBSmallBlockFighterCockpit", "MyObjectBuilder_Cockpit/DBSmallBlockFighterCockpit" };
        public static string [] Cockpit_CockpitOpen = { "Flight Seat", "MyObjectBuilder_Cockpit", "CockpitOpen", "MyObjectBuilder_Cockpit/CockpitOpen" };
        public static string [] Gyro_LargeBlockGyro = { "Gyroscope", "MyObjectBuilder_Gyro", "LargeBlockGyro", "MyObjectBuilder_Gyro/LargeBlockGyro" };
        public static string [] Gyro_SmallBlockGyro = { "Gyroscope", "MyObjectBuilder_Gyro", "SmallBlockGyro", "MyObjectBuilder_Gyro/SmallBlockGyro" };
        public static string [] Cockpit_OpenCockpitSmall = { "Control Seat", "MyObjectBuilder_Cockpit", "OpenCockpitSmall", "MyObjectBuilder_Cockpit/OpenCockpitSmall" };
        public static string [] Cockpit_OpenCockpitLarge = { "Control Seat", "MyObjectBuilder_Cockpit", "OpenCockpitLarge", "MyObjectBuilder_Cockpit/OpenCockpitLarge" };
        public static string [] Cockpit_LargeBlockDesk = { "Desk", "MyObjectBuilder_Cockpit", "LargeBlockDesk", "MyObjectBuilder_Cockpit/LargeBlockDesk" };
        public static string [] Cockpit_LargeBlockDeskCorner = { "Desk Corner", "MyObjectBuilder_Cockpit", "LargeBlockDeskCorner", "MyObjectBuilder_Cockpit/LargeBlockDeskCorner" };
        public static string [] CubeBlock_LargeBlockDeskChairless = { "Chairless Desk", "MyObjectBuilder_CubeBlock", "LargeBlockDeskChairless", "MyObjectBuilder_CubeBlock/LargeBlockDeskChairless" };
        public static string [] CubeBlock_LargeBlockDeskChairlessCorner = { "Chairless Desk Corner", "MyObjectBuilder_CubeBlock", "LargeBlockDeskChairlessCorner", "MyObjectBuilder_CubeBlock/LargeBlockDeskChairlessCorner" };
        public static string [] Kitchen_LargeBlockKitchen = { "Kitchen", "MyObjectBuilder_Kitchen", "LargeBlockKitchen", "MyObjectBuilder_Kitchen/LargeBlockKitchen" };
        public static string [] CryoChamber_LargeBlockBed = { "Bed", "MyObjectBuilder_CryoChamber", "LargeBlockBed", "MyObjectBuilder_CryoChamber/LargeBlockBed" };
        public static string [] CargoContainer_LargeBlockLockerRoom = { "Armory", "MyObjectBuilder_CargoContainer", "LargeBlockLockerRoom", "MyObjectBuilder_CargoContainer/LargeBlockLockerRoom" };
        public static string [] CargoContainer_LargeBlockLockerRoomCorner = { "Armory Lockers", "MyObjectBuilder_CargoContainer", "LargeBlockLockerRoomCorner", "MyObjectBuilder_CargoContainer/LargeBlockLockerRoomCorner" };
        public static string [] Planter_LargeBlockPlanters = { "Planters", "MyObjectBuilder_Planter", "LargeBlockPlanters", "MyObjectBuilder_Planter/LargeBlockPlanters" };
        public static string [] Cockpit_LargeBlockCouch = { "Couch", "MyObjectBuilder_Cockpit", "LargeBlockCouch", "MyObjectBuilder_Cockpit/LargeBlockCouch" };
        public static string [] Cockpit_LargeBlockCouchCorner = { "Corner Couch", "MyObjectBuilder_Cockpit", "LargeBlockCouchCorner", "MyObjectBuilder_Cockpit/LargeBlockCouchCorner" };
        public static string [] CargoContainer_LargeBlockLockers = { "Lockers", "MyObjectBuilder_CargoContainer", "LargeBlockLockers", "MyObjectBuilder_CargoContainer/LargeBlockLockers" };
        public static string [] Cockpit_LargeBlockBathroomOpen = { "Toilet", "MyObjectBuilder_Cockpit", "LargeBlockBathroomOpen", "MyObjectBuilder_Cockpit/LargeBlockBathroomOpen" };
        public static string [] Cockpit_LargeBlockBathroom = { "Bathroom", "MyObjectBuilder_Cockpit", "LargeBlockBathroom", "MyObjectBuilder_Cockpit/LargeBlockBathroom" };
        public static string [] Cockpit_LargeBlockToilet = { "Toilet Seat", "MyObjectBuilder_Cockpit", "LargeBlockToilet", "MyObjectBuilder_Cockpit/LargeBlockToilet" };
        public static string [] Projector_LargeBlockConsole = { "Console Block", "MyObjectBuilder_Projector", "LargeBlockConsole", "MyObjectBuilder_Projector/LargeBlockConsole" };
        public static string [] Cockpit_SmallBlockCockpitIndustrial = { "Industrial Cockpit", "MyObjectBuilder_Cockpit", "SmallBlockCockpitIndustrial", "MyObjectBuilder_Cockpit/SmallBlockCockpitIndustrial" };
        public static string [] Cockpit_LargeBlockCockpitIndustrial = { "Industrial Cockpit", "MyObjectBuilder_Cockpit", "LargeBlockCockpitIndustrial", "MyObjectBuilder_Cockpit/LargeBlockCockpitIndustrial" };
        public static string [] VendingMachine_FoodDispenser = { "Dispenser", "MyObjectBuilder_VendingMachine", "FoodDispenser", "MyObjectBuilder_VendingMachine/FoodDispenser" };
        public static string [] Jukebox_Jukebox = { "Jukebox", "MyObjectBuilder_Jukebox", "Jukebox", "MyObjectBuilder_Jukebox/Jukebox" };
        public static string [] LCDPanelsBlock_LabEquipment = { "Lab Equipment", "MyObjectBuilder_LCDPanelsBlock", "LabEquipment", "MyObjectBuilder_LCDPanelsBlock/LabEquipment" };
        public static string [] CubeBlock_Shower = { "Shower", "MyObjectBuilder_CubeBlock", "Shower", "MyObjectBuilder_CubeBlock/Shower" };
        public static string [] CubeBlock_WindowWall = { "Window Wall", "MyObjectBuilder_CubeBlock", "WindowWall", "MyObjectBuilder_CubeBlock/WindowWall" };
        public static string [] CubeBlock_WindowWallLeft = { "Window Wall Left", "MyObjectBuilder_CubeBlock", "WindowWallLeft", "MyObjectBuilder_CubeBlock/WindowWallLeft" };
        public static string [] CubeBlock_WindowWallRight = { "Window Wall Right", "MyObjectBuilder_CubeBlock", "WindowWallRight", "MyObjectBuilder_CubeBlock/WindowWallRight" };
        public static string [] LCDPanelsBlock_MedicalStation = { "Medical Station", "MyObjectBuilder_LCDPanelsBlock", "MedicalStation", "MyObjectBuilder_LCDPanelsBlock/MedicalStation" };
        public static string [] TextPanel_TransparentLCDLarge = { "Transparent LCD", "MyObjectBuilder_TextPanel", "TransparentLCDLarge", "MyObjectBuilder_TextPanel/TransparentLCDLarge" };
        public static string [] TextPanel_TransparentLCDSmall = { "Transparent LCD", "MyObjectBuilder_TextPanel", "TransparentLCDSmall", "MyObjectBuilder_TextPanel/TransparentLCDSmall" };
        public static string [] CubeBlock_Catwalk = { "Grated Catwalk", "MyObjectBuilder_CubeBlock", "Catwalk", "MyObjectBuilder_CubeBlock/Catwalk" };
        public static string [] CubeBlock_CatwalkCorner = { "Grated Catwalk Corner", "MyObjectBuilder_CubeBlock", "CatwalkCorner", "MyObjectBuilder_CubeBlock/CatwalkCorner" };
        public static string [] CubeBlock_CatwalkStraight = { "Grated Catwalk Straight", "MyObjectBuilder_CubeBlock", "CatwalkStraight", "MyObjectBuilder_CubeBlock/CatwalkStraight" };
        public static string [] CubeBlock_CatwalkWall = { "Grated Catwalk Wall", "MyObjectBuilder_CubeBlock", "CatwalkWall", "MyObjectBuilder_CubeBlock/CatwalkWall" };
        public static string [] CubeBlock_CatwalkRailingEnd = { "Grated Catwalk End", "MyObjectBuilder_CubeBlock", "CatwalkRailingEnd", "MyObjectBuilder_CubeBlock/CatwalkRailingEnd" };
        public static string [] CubeBlock_CatwalkRailingHalfRight = { "Grated Catwalk Half Right", "MyObjectBuilder_CubeBlock", "CatwalkRailingHalfRight", "MyObjectBuilder_CubeBlock/CatwalkRailingHalfRight" };
        public static string [] CubeBlock_CatwalkRailingHalfLeft = { "Grated Catwalk Half Left", "MyObjectBuilder_CubeBlock", "CatwalkRailingHalfLeft", "MyObjectBuilder_CubeBlock/CatwalkRailingHalfLeft" };
        public static string [] CubeBlock_GratedStairs = { "Grated stairs", "MyObjectBuilder_CubeBlock", "GratedStairs", "MyObjectBuilder_CubeBlock/GratedStairs" };
        public static string [] CubeBlock_GratedHalfStairs = { "Grated half stairs", "MyObjectBuilder_CubeBlock", "GratedHalfStairs", "MyObjectBuilder_CubeBlock/GratedHalfStairs" };
        public static string [] CubeBlock_GratedHalfStairsMirrored = { "Grated half stairs mirrored", "MyObjectBuilder_CubeBlock", "GratedHalfStairsMirrored", "MyObjectBuilder_CubeBlock/GratedHalfStairsMirrored" };
        public static string [] CubeBlock_RailingStraight = { "Railing Straight", "MyObjectBuilder_CubeBlock", "RailingStraight", "MyObjectBuilder_CubeBlock/RailingStraight" };
        public static string [] CubeBlock_RailingDouble = { "Railing Double", "MyObjectBuilder_CubeBlock", "RailingDouble", "MyObjectBuilder_CubeBlock/RailingDouble" };
        public static string [] CubeBlock_RailingCorner = { "Railing Corner", "MyObjectBuilder_CubeBlock", "RailingCorner", "MyObjectBuilder_CubeBlock/RailingCorner" };
        public static string [] CubeBlock_RailingDiagonal = { "Railing Diagonal", "MyObjectBuilder_CubeBlock", "RailingDiagonal", "MyObjectBuilder_CubeBlock/RailingDiagonal" };
        public static string [] CubeBlock_RailingHalfRight = { "Railing Half Right", "MyObjectBuilder_CubeBlock", "RailingHalfRight", "MyObjectBuilder_CubeBlock/RailingHalfRight" };
        public static string [] CubeBlock_RailingHalfLeft = { "Railing Half Left", "MyObjectBuilder_CubeBlock", "RailingHalfLeft", "MyObjectBuilder_CubeBlock/RailingHalfLeft" };
        public static string [] ReflectorLight_RotatingLightLarge = { "Rotating Light", "MyObjectBuilder_ReflectorLight", "RotatingLightLarge", "MyObjectBuilder_ReflectorLight/RotatingLightLarge" };
        public static string [] ReflectorLight_RotatingLightSmall = { "Rotating Light", "MyObjectBuilder_ReflectorLight", "RotatingLightSmall", "MyObjectBuilder_ReflectorLight/RotatingLightSmall" };
        public static string [] CubeBlock_Freight1 = { "Freight 1", "MyObjectBuilder_CubeBlock", "Freight1", "MyObjectBuilder_CubeBlock/Freight1" };
        public static string [] CubeBlock_Freight2 = { "Freight 2", "MyObjectBuilder_CubeBlock", "Freight2", "MyObjectBuilder_CubeBlock/Freight2" };
        public static string [] CubeBlock_Freight3 = { "Freight 3", "MyObjectBuilder_CubeBlock", "Freight3", "MyObjectBuilder_CubeBlock/Freight3" };
        public static string [] Door_null = {"Door","MyObjectBuilder_Door","","MyObjectBuilder_Door/(null)"};
        public static string [] AirtightHangarDoor_null = {"Airtight Hangar Door","MyObjectBuilder_AirtightHangarDoor","","MyObjectBuilder_AirtightHangarDoor/(null)"};
        public static string [] AirtightSlideDoor_LargeBlockSlideDoor = { "Sliding Door", "MyObjectBuilder_AirtightSlideDoor", "LargeBlockSlideDoor", "MyObjectBuilder_AirtightSlideDoor/LargeBlockSlideDoor" };
        public static string [] CubeBlock_ArmorCenter = { "Blast doors", "MyObjectBuilder_CubeBlock", "ArmorCenter", "MyObjectBuilder_CubeBlock/ArmorCenter" };
        public static string [] CubeBlock_ArmorCorner = { "Blast door corner", "MyObjectBuilder_CubeBlock", "ArmorCorner", "MyObjectBuilder_CubeBlock/ArmorCorner" };
        public static string [] CubeBlock_ArmorInvCorner = { "Blast door corner inverted", "MyObjectBuilder_CubeBlock", "ArmorInvCorner", "MyObjectBuilder_CubeBlock/ArmorInvCorner" };
        public static string [] CubeBlock_ArmorSide = { "Blast door edge", "MyObjectBuilder_CubeBlock", "ArmorSide", "MyObjectBuilder_CubeBlock/ArmorSide" };
        public static string [] CubeBlock_SmallArmorCenter = { "Blast doors", "MyObjectBuilder_CubeBlock", "SmallArmorCenter", "MyObjectBuilder_CubeBlock/SmallArmorCenter" };
        public static string [] CubeBlock_SmallArmorCorner = { "Blast door corner", "MyObjectBuilder_CubeBlock", "SmallArmorCorner", "MyObjectBuilder_CubeBlock/SmallArmorCorner" };
        public static string [] CubeBlock_SmallArmorInvCorner = { "Blast door corner inverted", "MyObjectBuilder_CubeBlock", "SmallArmorInvCorner", "MyObjectBuilder_CubeBlock/SmallArmorInvCorner" };
        public static string [] CubeBlock_SmallArmorSide = { "Blast door edge", "MyObjectBuilder_CubeBlock", "SmallArmorSide", "MyObjectBuilder_CubeBlock/SmallArmorSide" };
        public static string [] StoreBlock_StoreBlock = { "Store", "MyObjectBuilder_StoreBlock", "StoreBlock", "MyObjectBuilder_StoreBlock/StoreBlock" };
        public static string [] SafeZoneBlock_SafeZoneBlock = { "Safe Zone", "MyObjectBuilder_SafeZoneBlock", "SafeZoneBlock", "MyObjectBuilder_SafeZoneBlock/SafeZoneBlock" };
        public static string [] ContractBlock_ContractBlock = { "Contracts", "MyObjectBuilder_ContractBlock", "ContractBlock", "MyObjectBuilder_ContractBlock/ContractBlock" };
        public static string [] VendingMachine_VendingMachine = { "Vending Machine", "MyObjectBuilder_VendingMachine", "VendingMachine", "MyObjectBuilder_VendingMachine/VendingMachine" };
        public static string [] StoreBlock_AtmBlock = { "ATM", "MyObjectBuilder_StoreBlock", "AtmBlock", "MyObjectBuilder_StoreBlock/AtmBlock" };
        public static string [] BatteryBlock_LargeBlockBatteryBlock = { "Battery", "MyObjectBuilder_BatteryBlock", "LargeBlockBatteryBlock", "MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock" };
        public static string [] BatteryBlock_SmallBlockBatteryBlock = { "Battery", "MyObjectBuilder_BatteryBlock", "SmallBlockBatteryBlock", "MyObjectBuilder_BatteryBlock/SmallBlockBatteryBlock" };
        public static string [] BatteryBlock_SmallBlockSmallBatteryBlock = { "Small Battery", "MyObjectBuilder_BatteryBlock", "SmallBlockSmallBatteryBlock", "MyObjectBuilder_BatteryBlock/SmallBlockSmallBatteryBlock" };
        public static string [] Reactor_SmallBlockSmallGenerator = { "Small Reactor", "MyObjectBuilder_Reactor", "SmallBlockSmallGenerator", "MyObjectBuilder_Reactor/SmallBlockSmallGenerator" };
        public static string [] Reactor_SmallBlockLargeGenerator = { "Large Reactor", "MyObjectBuilder_Reactor", "SmallBlockLargeGenerator", "MyObjectBuilder_Reactor/SmallBlockLargeGenerator" };
        public static string [] Reactor_LargeBlockSmallGenerator = { "Small Reactor", "MyObjectBuilder_Reactor", "LargeBlockSmallGenerator", "MyObjectBuilder_Reactor/LargeBlockSmallGenerator" };
        public static string [] Reactor_LargeBlockLargeGenerator = { "Large Reactor", "MyObjectBuilder_Reactor", "LargeBlockLargeGenerator", "MyObjectBuilder_Reactor/LargeBlockLargeGenerator" };
        public static string [] HydrogenEngine_LargeHydrogenEngine = { "Hydrogen Engine", "MyObjectBuilder_HydrogenEngine", "LargeHydrogenEngine", "MyObjectBuilder_HydrogenEngine/LargeHydrogenEngine" };
        public static string [] HydrogenEngine_SmallHydrogenEngine = { "Hydrogen Engine", "MyObjectBuilder_HydrogenEngine", "SmallHydrogenEngine", "MyObjectBuilder_HydrogenEngine/SmallHydrogenEngine" };
        public static string [] WindTurbine_LargeBlockWindTurbine = { "Wind Turbine", "MyObjectBuilder_WindTurbine", "LargeBlockWindTurbine", "MyObjectBuilder_WindTurbine/LargeBlockWindTurbine" };
        public static string [] SolarPanel_LargeBlockSolarPanel = { "Solar Panel", "MyObjectBuilder_SolarPanel", "LargeBlockSolarPanel", "MyObjectBuilder_SolarPanel/LargeBlockSolarPanel" };
        public static string [] SolarPanel_SmallBlockSolarPanel = { "Solar Panel", "MyObjectBuilder_SolarPanel", "SmallBlockSolarPanel", "MyObjectBuilder_SolarPanel/SmallBlockSolarPanel" };
        public static string [] CubeBlock_Monolith = { "Monolith", "MyObjectBuilder_CubeBlock", "Monolith", "MyObjectBuilder_CubeBlock/Monolith" };
        public static string [] CubeBlock_Stereolith = { "Stereolith", "MyObjectBuilder_CubeBlock", "Stereolith", "MyObjectBuilder_CubeBlock/Stereolith" };
        public static string [] CubeBlock_DeadAstronaut = { "Dead Astronaut", "MyObjectBuilder_CubeBlock", "DeadAstronaut", "MyObjectBuilder_CubeBlock/DeadAstronaut" };
        public static string [] CubeBlock_LargeDeadAstronaut = { "Dead Astronaut", "MyObjectBuilder_CubeBlock", "LargeDeadAstronaut", "MyObjectBuilder_CubeBlock/LargeDeadAstronaut" };
        public static string [] GravityGenerator_null = {"Gravity Generator","MyObjectBuilder_GravityGenerator","","MyObjectBuilder_GravityGenerator/(null)"};
        public static string [] GravityGeneratorSphere_null = {"Spherical Gravity Generator","MyObjectBuilder_GravityGeneratorSphere","","MyObjectBuilder_GravityGeneratorSphere/(null)"};
        public static string [] VirtualMass_VirtualMassLarge = { "Artificial Mass", "MyObjectBuilder_VirtualMass", "VirtualMassLarge", "MyObjectBuilder_VirtualMass/VirtualMassLarge" };
        public static string [] VirtualMass_VirtualMassSmall = { "Artificial Mass", "MyObjectBuilder_VirtualMass", "VirtualMassSmall", "MyObjectBuilder_VirtualMass/VirtualMassSmall" };
        public static string [] SpaceBall_SpaceBallLarge = { "Space Ball", "MyObjectBuilder_SpaceBall", "SpaceBallLarge", "MyObjectBuilder_SpaceBall/SpaceBallLarge" };
        public static string [] SpaceBall_SpaceBallSmall = { "Space Ball", "MyObjectBuilder_SpaceBall", "SpaceBallSmall", "MyObjectBuilder_SpaceBall/SpaceBallSmall" };
        public static string [] Passage_null = {"Passage","MyObjectBuilder_Passage","","MyObjectBuilder_Passage/(null)"};
        public static string [] CubeBlock_LargeStairs = { "Stairs", "MyObjectBuilder_CubeBlock", "LargeStairs", "MyObjectBuilder_CubeBlock/LargeStairs" };
        public static string [] CubeBlock_LargeRamp = { "Ramp", "MyObjectBuilder_CubeBlock", "LargeRamp", "MyObjectBuilder_CubeBlock/LargeRamp" };
        public static string [] CubeBlock_LargeSteelCatwalk = { "Steel Catwalk", "MyObjectBuilder_CubeBlock", "LargeSteelCatwalk", "MyObjectBuilder_CubeBlock/LargeSteelCatwalk" };
        public static string [] CubeBlock_LargeSteelCatwalk2Sides = { "Steel Catwalk Two Sides", "MyObjectBuilder_CubeBlock", "LargeSteelCatwalk2Sides", "MyObjectBuilder_CubeBlock/LargeSteelCatwalk2Sides" };
        public static string [] CubeBlock_LargeSteelCatwalkCorner = { "Steel Catwalk Corner", "MyObjectBuilder_CubeBlock", "LargeSteelCatwalkCorner", "MyObjectBuilder_CubeBlock/LargeSteelCatwalkCorner" };
        public static string [] CubeBlock_LargeSteelCatwalkPlate = { "Steel Catwalk Plate", "MyObjectBuilder_CubeBlock", "LargeSteelCatwalkPlate", "MyObjectBuilder_CubeBlock/LargeSteelCatwalkPlate" };
        public static string [] CubeBlock_LargeCoverWall = { "Cover Walls", "MyObjectBuilder_CubeBlock", "LargeCoverWall", "MyObjectBuilder_CubeBlock/LargeCoverWall" };
        public static string [] CubeBlock_LargeCoverWallHalf = { "Half Cover Wall", "MyObjectBuilder_CubeBlock", "LargeCoverWallHalf", "MyObjectBuilder_CubeBlock/LargeCoverWallHalf" };
        public static string [] CubeBlock_LargeBlockInteriorWall = { "Interior Wall", "MyObjectBuilder_CubeBlock", "LargeBlockInteriorWall", "MyObjectBuilder_CubeBlock/LargeBlockInteriorWall" };
        public static string [] CubeBlock_LargeInteriorPillar = { "Interior Pillar", "MyObjectBuilder_CubeBlock", "LargeInteriorPillar", "MyObjectBuilder_CubeBlock/LargeInteriorPillar" };
        public static string [] Cockpit_PassengerSeatLarge = { "Passenger Seat", "MyObjectBuilder_Cockpit", "PassengerSeatLarge", "MyObjectBuilder_Cockpit/PassengerSeatLarge" };
        public static string [] Cockpit_PassengerSeatSmall = { "Passenger Seat", "MyObjectBuilder_Cockpit", "PassengerSeatSmall", "MyObjectBuilder_Cockpit/PassengerSeatSmall" };
        public static string [] Ladder2_null = {"Ladder","MyObjectBuilder_Ladder2","","MyObjectBuilder_Ladder2/(null)"};
        public static string [] Ladder2_LadderSmall = { "Ladder", "MyObjectBuilder_Ladder2", "LadderSmall", "MyObjectBuilder_Ladder2/LadderSmall" };
        public static string [] TextPanel_SmallTextPanel = { "Text panel", "MyObjectBuilder_TextPanel", "SmallTextPanel", "MyObjectBuilder_TextPanel/SmallTextPanel" };
        public static string [] TextPanel_SmallLCDPanelWide = { "Wide LCD panel", "MyObjectBuilder_TextPanel", "SmallLCDPanelWide", "MyObjectBuilder_TextPanel/SmallLCDPanelWide" };
        public static string [] TextPanel_SmallLCDPanel = { "LCD Panel", "MyObjectBuilder_TextPanel", "SmallLCDPanel", "MyObjectBuilder_TextPanel/SmallLCDPanel" };
        public static string [] TextPanel_LargeBlockCorner_LCD_1 = { "Corner LCD Top", "MyObjectBuilder_TextPanel", "LargeBlockCorner_LCD_1", "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_1" };
        public static string [] TextPanel_LargeBlockCorner_LCD_2 = { "Corner LCD Bottom", "MyObjectBuilder_TextPanel", "LargeBlockCorner_LCD_2", "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_2" };
        public static string [] TextPanel_LargeBlockCorner_LCD_Flat_1 = { "Corner LCD Flat Top", "MyObjectBuilder_TextPanel", "LargeBlockCorner_LCD_Flat_1", "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_Flat_1" };
        public static string [] TextPanel_LargeBlockCorner_LCD_Flat_2 = { "Corner LCD Flat Bottom", "MyObjectBuilder_TextPanel", "LargeBlockCorner_LCD_Flat_2", "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_Flat_2" };
        public static string [] TextPanel_SmallBlockCorner_LCD_1 = { "Corner LCD Top", "MyObjectBuilder_TextPanel", "SmallBlockCorner_LCD_1", "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_1" };
        public static string [] TextPanel_SmallBlockCorner_LCD_2 = { "Corner LCD Bottom", "MyObjectBuilder_TextPanel", "SmallBlockCorner_LCD_2", "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_2" };
        public static string [] TextPanel_SmallBlockCorner_LCD_Flat_1 = { "Corner LCD Flat Top", "MyObjectBuilder_TextPanel", "SmallBlockCorner_LCD_Flat_1", "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_Flat_1" };
        public static string [] TextPanel_SmallBlockCorner_LCD_Flat_2 = { "Corner LCD Flat Bottom", "MyObjectBuilder_TextPanel", "SmallBlockCorner_LCD_Flat_2", "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_Flat_2" };
        public static string [] TextPanel_LargeTextPanel = { "Text panel", "MyObjectBuilder_TextPanel", "LargeTextPanel", "MyObjectBuilder_TextPanel/LargeTextPanel" };
        public static string [] TextPanel_LargeLCDPanel = { "LCD Panel", "MyObjectBuilder_TextPanel", "LargeLCDPanel", "MyObjectBuilder_TextPanel/LargeLCDPanel" };
        public static string [] TextPanel_LargeLCDPanelWide = { "Wide LCD panel", "MyObjectBuilder_TextPanel", "LargeLCDPanelWide", "MyObjectBuilder_TextPanel/LargeLCDPanelWide" };
        public static string [] ReflectorLight_LargeBlockFrontLight = { "Spotlight", "MyObjectBuilder_ReflectorLight", "LargeBlockFrontLight", "MyObjectBuilder_ReflectorLight/LargeBlockFrontLight" };
        public static string [] ReflectorLight_SmallBlockFrontLight = { "Spotlight", "MyObjectBuilder_ReflectorLight", "SmallBlockFrontLight", "MyObjectBuilder_ReflectorLight/SmallBlockFrontLight" };
        public static string [] InteriorLight_SmallLight = { "Interior Light", "MyObjectBuilder_InteriorLight", "SmallLight", "MyObjectBuilder_InteriorLight/SmallLight" };
        public static string [] InteriorLight_SmallBlockSmallLight = { "Interior Light", "MyObjectBuilder_InteriorLight", "SmallBlockSmallLight", "MyObjectBuilder_InteriorLight/SmallBlockSmallLight" };
        public static string [] InteriorLight_LargeBlockLight_1corner = { "Corner Light", "MyObjectBuilder_InteriorLight", "LargeBlockLight_1corner", "MyObjectBuilder_InteriorLight/LargeBlockLight_1corner" };
        public static string [] InteriorLight_LargeBlockLight_2corner = { "Corner LightDouble", "MyObjectBuilder_InteriorLight", "LargeBlockLight_2corner", "MyObjectBuilder_InteriorLight/LargeBlockLight_2corner" };
        public static string [] InteriorLight_SmallBlockLight_1corner = { "Corner Light", "MyObjectBuilder_InteriorLight", "SmallBlockLight_1corner", "MyObjectBuilder_InteriorLight/SmallBlockLight_1corner" };
        public static string [] InteriorLight_SmallBlockLight_2corner = { "Corner LightDouble", "MyObjectBuilder_InteriorLight", "SmallBlockLight_2corner", "MyObjectBuilder_InteriorLight/SmallBlockLight_2corner" };
        public static string [] OxygenTank_OxygenTankSmall = { "Oxygen Tank", "MyObjectBuilder_OxygenTank", "OxygenTankSmall", "MyObjectBuilder_OxygenTank/OxygenTankSmall" };
        public static string [] OxygenTank_null = {"Oxygen Tank","MyObjectBuilder_OxygenTank","","MyObjectBuilder_OxygenTank/(null)"};
        public static string [] OxygenTank_LargeHydrogenTank = { "Hydrogen Tank", "MyObjectBuilder_OxygenTank", "LargeHydrogenTank", "MyObjectBuilder_OxygenTank/LargeHydrogenTank" };
        public static string [] OxygenTank_SmallHydrogenTank = { "Hydrogen Tank", "MyObjectBuilder_OxygenTank", "SmallHydrogenTank", "MyObjectBuilder_OxygenTank/SmallHydrogenTank" };
        public static string [] AirVent_null = {"Air Vent","MyObjectBuilder_AirVent","","MyObjectBuilder_AirVent/(null)"};
        public static string [] AirVent_SmallAirVent = { "Air Vent", "MyObjectBuilder_AirVent", "SmallAirVent", "MyObjectBuilder_AirVent/SmallAirVent" };
        public static string [] CargoContainer_SmallBlockSmallContainer = { "Small Cargo Container", "MyObjectBuilder_CargoContainer", "SmallBlockSmallContainer", "MyObjectBuilder_CargoContainer/SmallBlockSmallContainer" };
        public static string [] CargoContainer_SmallBlockMediumContainer = { "Medium Cargo Container", "MyObjectBuilder_CargoContainer", "SmallBlockMediumContainer", "MyObjectBuilder_CargoContainer/SmallBlockMediumContainer" };
        public static string [] CargoContainer_SmallBlockLargeContainer = { "Large Cargo Container", "MyObjectBuilder_CargoContainer", "SmallBlockLargeContainer", "MyObjectBuilder_CargoContainer/SmallBlockLargeContainer" };
        public static string [] CargoContainer_LargeBlockSmallContainer = { "Small Cargo Container", "MyObjectBuilder_CargoContainer", "LargeBlockSmallContainer", "MyObjectBuilder_CargoContainer/LargeBlockSmallContainer" };
        public static string [] CargoContainer_LargeBlockLargeContainer = { "Large Cargo Container", "MyObjectBuilder_CargoContainer", "LargeBlockLargeContainer", "MyObjectBuilder_CargoContainer/LargeBlockLargeContainer" };
        public static string [] Conveyor_SmallBlockConveyor = { "Small Conveyor", "MyObjectBuilder_Conveyor", "SmallBlockConveyor", "MyObjectBuilder_Conveyor/SmallBlockConveyor" };
        public static string [] Conveyor_LargeBlockConveyor = { "Conveyor Junction", "MyObjectBuilder_Conveyor", "LargeBlockConveyor", "MyObjectBuilder_Conveyor/LargeBlockConveyor" };
        public static string [] Collector_Collector = { "Collector", "MyObjectBuilder_Collector", "Collector", "MyObjectBuilder_Collector/Collector" };
        public static string [] Collector_CollectorSmall = { "Collector", "MyObjectBuilder_Collector", "CollectorSmall", "MyObjectBuilder_Collector/CollectorSmall" };
        public static string [] ShipConnector_Connector = { "Connector", "MyObjectBuilder_ShipConnector", "Connector", "MyObjectBuilder_ShipConnector/Connector" };
        public static string [] ShipConnector_ConnectorSmall = { "Ejector", "MyObjectBuilder_ShipConnector", "ConnectorSmall", "MyObjectBuilder_ShipConnector/ConnectorSmall" };
        public static string [] ShipConnector_ConnectorMedium = { "Connector", "MyObjectBuilder_ShipConnector", "ConnectorMedium", "MyObjectBuilder_ShipConnector/ConnectorMedium" };
        public static string [] ConveyorConnector_ConveyorTube = { "Conveyor Tube", "MyObjectBuilder_ConveyorConnector", "ConveyorTube", "MyObjectBuilder_ConveyorConnector/ConveyorTube" };
        public static string [] ConveyorConnector_ConveyorTubeSmall = { "Small Conveyor Tube", "MyObjectBuilder_ConveyorConnector", "ConveyorTubeSmall", "MyObjectBuilder_ConveyorConnector/ConveyorTubeSmall" };
        public static string [] ConveyorConnector_ConveyorTubeMedium = { "Medium Conveyor Tube", "MyObjectBuilder_ConveyorConnector", "ConveyorTubeMedium", "MyObjectBuilder_ConveyorConnector/ConveyorTubeMedium" };
        public static string [] ConveyorConnector_ConveyorFrameMedium = { "Conveyor Frame", "MyObjectBuilder_ConveyorConnector", "ConveyorFrameMedium", "MyObjectBuilder_ConveyorConnector/ConveyorFrameMedium" };
        public static string [] ConveyorConnector_ConveyorTubeCurved = { "Curved Conveyor Tube", "MyObjectBuilder_ConveyorConnector", "ConveyorTubeCurved", "MyObjectBuilder_ConveyorConnector/ConveyorTubeCurved" };
        public static string [] ConveyorConnector_ConveyorTubeSmallCurved = { "Small Curved Conveyor Tube", "MyObjectBuilder_ConveyorConnector", "ConveyorTubeSmallCurved", "MyObjectBuilder_ConveyorConnector/ConveyorTubeSmallCurved" };
        public static string [] ConveyorConnector_ConveyorTubeCurvedMedium = { "Curved Conveyor Tube", "MyObjectBuilder_ConveyorConnector", "ConveyorTubeCurvedMedium", "MyObjectBuilder_ConveyorConnector/ConveyorTubeCurvedMedium" };
        public static string [] Conveyor_SmallShipConveyorHub = { "Conveyor Junction", "MyObjectBuilder_Conveyor", "SmallShipConveyorHub", "MyObjectBuilder_Conveyor/SmallShipConveyorHub" };
        public static string [] ConveyorSorter_LargeBlockConveyorSorter = { "Conveyor Sorter", "MyObjectBuilder_ConveyorSorter", "LargeBlockConveyorSorter", "MyObjectBuilder_ConveyorSorter/LargeBlockConveyorSorter" };
        public static string [] ConveyorSorter_MediumBlockConveyorSorter = { "Conveyor Sorter", "MyObjectBuilder_ConveyorSorter", "MediumBlockConveyorSorter", "MyObjectBuilder_ConveyorSorter/MediumBlockConveyorSorter" };
        public static string [] ConveyorSorter_SmallBlockConveyorSorter = { "Small Conveyor Sorter", "MyObjectBuilder_ConveyorSorter", "SmallBlockConveyorSorter", "MyObjectBuilder_ConveyorSorter/SmallBlockConveyorSorter" };
        public static string [] PistonBase_LargePistonBase = { "Piston", "MyObjectBuilder_PistonBase", "LargePistonBase", "MyObjectBuilder_PistonBase/LargePistonBase" };
        public static string [] ExtendedPistonBase_LargePistonBase = { "Piston", "MyObjectBuilder_ExtendedPistonBase", "LargePistonBase", "MyObjectBuilder_ExtendedPistonBase/LargePistonBase" };
        public static string [] PistonTop_LargePistonTop = { "Top Piston Part", "MyObjectBuilder_PistonTop", "LargePistonTop", "MyObjectBuilder_PistonTop/LargePistonTop" };
        public static string [] PistonBase_SmallPistonBase = { "Piston", "MyObjectBuilder_PistonBase", "SmallPistonBase", "MyObjectBuilder_PistonBase/SmallPistonBase" };
        public static string [] ExtendedPistonBase_SmallPistonBase = { "Piston", "MyObjectBuilder_ExtendedPistonBase", "SmallPistonBase", "MyObjectBuilder_ExtendedPistonBase/SmallPistonBase" };
        public static string [] PistonTop_SmallPistonTop = { "Top Piston Part", "MyObjectBuilder_PistonTop", "SmallPistonTop", "MyObjectBuilder_PistonTop/SmallPistonTop" };
        public static string [] MotorStator_LargeStator = { "Rotor", "MyObjectBuilder_MotorStator", "LargeStator", "MyObjectBuilder_MotorStator/LargeStator" };
        public static string [] MotorRotor_LargeRotor = { "Rotor Part", "MyObjectBuilder_MotorRotor", "LargeRotor", "MyObjectBuilder_MotorRotor/LargeRotor" };
        public static string [] MotorStator_SmallStator = { "Rotor", "MyObjectBuilder_MotorStator", "SmallStator", "MyObjectBuilder_MotorStator/SmallStator" };
        public static string [] MotorRotor_SmallRotor = { "Rotor Part", "MyObjectBuilder_MotorRotor", "SmallRotor", "MyObjectBuilder_MotorRotor/SmallRotor" };
        public static string [] MotorAdvancedStator_LargeAdvancedStator = { "Advanced Rotor", "MyObjectBuilder_MotorAdvancedStator", "LargeAdvancedStator", "MyObjectBuilder_MotorAdvancedStator/LargeAdvancedStator" };
        public static string [] MotorAdvancedRotor_LargeAdvancedRotor = { "Advanced Rotor Part", "MyObjectBuilder_MotorAdvancedRotor", "LargeAdvancedRotor", "MyObjectBuilder_MotorAdvancedRotor/LargeAdvancedRotor" };
        public static string [] MotorAdvancedStator_SmallAdvancedStator = { "Advanced Rotor", "MyObjectBuilder_MotorAdvancedStator", "SmallAdvancedStator", "MyObjectBuilder_MotorAdvancedStator/SmallAdvancedStator" };
        public static string [] MotorAdvancedRotor_SmallAdvancedRotor = { "Rotor Part", "MyObjectBuilder_MotorAdvancedRotor", "SmallAdvancedRotor", "MyObjectBuilder_MotorAdvancedRotor/SmallAdvancedRotor" };
        public static string [] MedicalRoom_LargeMedicalRoom = { "Medical Room", "MyObjectBuilder_MedicalRoom", "LargeMedicalRoom", "MyObjectBuilder_MedicalRoom/LargeMedicalRoom" };
        public static string [] CryoChamber_LargeBlockCryoChamber = { "Cryo Chamber", "MyObjectBuilder_CryoChamber", "LargeBlockCryoChamber", "MyObjectBuilder_CryoChamber/LargeBlockCryoChamber" };
        public static string [] CryoChamber_SmallBlockCryoChamber = { "Cryo Chamber", "MyObjectBuilder_CryoChamber", "SmallBlockCryoChamber", "MyObjectBuilder_CryoChamber/SmallBlockCryoChamber" };
        public static string [] Refinery_LargeRefinery = { "Refinery", "MyObjectBuilder_Refinery", "LargeRefinery", "MyObjectBuilder_Refinery/LargeRefinery" };
        public static string [] Refinery_Blast_Furnace = {"Basic Refinery","MyObjectBuilder_Refinery","Blast Furnace","MyObjectBuilder_Refinery/Blast Furnace"};
        public static string [] OxygenGenerator_null = {"O2/H2 Generator","MyObjectBuilder_OxygenGenerator","","MyObjectBuilder_OxygenGenerator/(null)"};
        public static string [] OxygenGenerator_OxygenGeneratorSmall = { "O2/H2 Generator", "MyObjectBuilder_OxygenGenerator", "OxygenGeneratorSmall", "MyObjectBuilder_OxygenGenerator/OxygenGeneratorSmall" };
        public static string [] Assembler_LargeAssembler = { "Assembler", "MyObjectBuilder_Assembler", "LargeAssembler", "MyObjectBuilder_Assembler/LargeAssembler" };
        public static string [] Assembler_BasicAssembler = { "Basic Assembler", "MyObjectBuilder_Assembler", "BasicAssembler", "MyObjectBuilder_Assembler/BasicAssembler" };
        public static string [] SurvivalKit_SurvivalKitLarge = { "Survival kit", "MyObjectBuilder_SurvivalKit", "SurvivalKitLarge", "MyObjectBuilder_SurvivalKit/SurvivalKitLarge" };
        public static string [] SurvivalKit_SurvivalKit = { "Survival kit", "MyObjectBuilder_SurvivalKit", "SurvivalKit", "MyObjectBuilder_SurvivalKit/SurvivalKit" };
        public static string [] OxygenFarm_LargeBlockOxygenFarm = { "Oxygen Farm", "MyObjectBuilder_OxygenFarm", "LargeBlockOxygenFarm", "MyObjectBuilder_OxygenFarm/LargeBlockOxygenFarm" };
        public static string [] UpgradeModule_LargeProductivityModule = { "Speed Module", "MyObjectBuilder_UpgradeModule", "LargeProductivityModule", "MyObjectBuilder_UpgradeModule/LargeProductivityModule" };
        public static string [] UpgradeModule_LargeEffectivenessModule = { "Yield Module", "MyObjectBuilder_UpgradeModule", "LargeEffectivenessModule", "MyObjectBuilder_UpgradeModule/LargeEffectivenessModule" };
        public static string [] UpgradeModule_LargeEnergyModule = { "Power Efficiency Module", "MyObjectBuilder_UpgradeModule", "LargeEnergyModule", "MyObjectBuilder_UpgradeModule/LargeEnergyModule" };
        public static string [] Thrust_SmallBlockSmallThrust = { "Ion Thrusters", "MyObjectBuilder_Thrust", "SmallBlockSmallThrust", "MyObjectBuilder_Thrust/SmallBlockSmallThrust" };
        public static string [] Thrust_SmallBlockLargeThrust = { "Large Ion Thruster", "MyObjectBuilder_Thrust", "SmallBlockLargeThrust", "MyObjectBuilder_Thrust/SmallBlockLargeThrust" };
        public static string [] Thrust_LargeBlockSmallThrust = { "Ion Thrusters", "MyObjectBuilder_Thrust", "LargeBlockSmallThrust", "MyObjectBuilder_Thrust/LargeBlockSmallThrust" };
        public static string [] Thrust_LargeBlockLargeThrust = { "Large Ion Thruster", "MyObjectBuilder_Thrust", "LargeBlockLargeThrust", "MyObjectBuilder_Thrust/LargeBlockLargeThrust" };
        public static string [] Thrust_LargeBlockLargeHydrogenThrust = { "Large Hydrogen Thruster", "MyObjectBuilder_Thrust", "LargeBlockLargeHydrogenThrust", "MyObjectBuilder_Thrust/LargeBlockLargeHydrogenThrust" };
        public static string [] Thrust_LargeBlockSmallHydrogenThrust = { "Hydrogen Thrusters", "MyObjectBuilder_Thrust", "LargeBlockSmallHydrogenThrust", "MyObjectBuilder_Thrust/LargeBlockSmallHydrogenThrust" };
        public static string [] Thrust_SmallBlockLargeHydrogenThrust = { "Large Hydrogen Thruster", "MyObjectBuilder_Thrust", "SmallBlockLargeHydrogenThrust", "MyObjectBuilder_Thrust/SmallBlockLargeHydrogenThrust" };
        public static string [] Thrust_SmallBlockSmallHydrogenThrust = { "Hydrogen Thrusters", "MyObjectBuilder_Thrust", "SmallBlockSmallHydrogenThrust", "MyObjectBuilder_Thrust/SmallBlockSmallHydrogenThrust" };
        public static string [] Thrust_LargeBlockLargeAtmosphericThrust = { "Large Atmospheric Thruster", "MyObjectBuilder_Thrust", "LargeBlockLargeAtmosphericThrust", "MyObjectBuilder_Thrust/LargeBlockLargeAtmosphericThrust" };
        public static string [] Thrust_LargeBlockSmallAtmosphericThrust = { "Atmospheric Thrusters", "MyObjectBuilder_Thrust", "LargeBlockSmallAtmosphericThrust", "MyObjectBuilder_Thrust/LargeBlockSmallAtmosphericThrust" };
        public static string [] Thrust_SmallBlockLargeAtmosphericThrust = { "Large Atmospheric Thruster", "MyObjectBuilder_Thrust", "SmallBlockLargeAtmosphericThrust", "MyObjectBuilder_Thrust/SmallBlockLargeAtmosphericThrust" };
        public static string [] Thrust_SmallBlockSmallAtmosphericThrust = { "Atmospheric Thrusters", "MyObjectBuilder_Thrust", "SmallBlockSmallAtmosphericThrust", "MyObjectBuilder_Thrust/SmallBlockSmallAtmosphericThrust" };
        public static string [] Drill_SmallBlockDrill = { "Drill", "MyObjectBuilder_Drill", "SmallBlockDrill", "MyObjectBuilder_Drill/SmallBlockDrill" };
        public static string [] Drill_LargeBlockDrill = { "Drill", "MyObjectBuilder_Drill", "LargeBlockDrill", "MyObjectBuilder_Drill/LargeBlockDrill" };
        public static string [] ShipGrinder_LargeShipGrinder = { "Grinder", "MyObjectBuilder_ShipGrinder", "LargeShipGrinder", "MyObjectBuilder_ShipGrinder/LargeShipGrinder" };
        public static string [] ShipGrinder_SmallShipGrinder = { "Grinder", "MyObjectBuilder_ShipGrinder", "SmallShipGrinder", "MyObjectBuilder_ShipGrinder/SmallShipGrinder" };
        public static string [] ShipWelder_LargeShipWelder = { "Welder", "MyObjectBuilder_ShipWelder", "LargeShipWelder", "MyObjectBuilder_ShipWelder/LargeShipWelder" };
        public static string [] ShipWelder_SmallShipWelder = { "Welder", "MyObjectBuilder_ShipWelder", "SmallShipWelder", "MyObjectBuilder_ShipWelder/SmallShipWelder" };
        public static string [] OreDetector_LargeOreDetector = { "Ore Detector", "MyObjectBuilder_OreDetector", "LargeOreDetector", "MyObjectBuilder_OreDetector/LargeOreDetector" };
        public static string [] OreDetector_SmallBlockOreDetector = { "Ore Detector", "MyObjectBuilder_OreDetector", "SmallBlockOreDetector", "MyObjectBuilder_OreDetector/SmallBlockOreDetector" };
        public static string [] LandingGear_LargeBlockLandingGear = { "Landing Gear", "MyObjectBuilder_LandingGear", "LargeBlockLandingGear", "MyObjectBuilder_LandingGear/LargeBlockLandingGear" };
        public static string [] LandingGear_SmallBlockLandingGear = { "Landing Gear", "MyObjectBuilder_LandingGear", "SmallBlockLandingGear", "MyObjectBuilder_LandingGear/SmallBlockLandingGear" };
        public static string [] JumpDrive_LargeJumpDrive = { "Jump Drive", "MyObjectBuilder_JumpDrive", "LargeJumpDrive", "MyObjectBuilder_JumpDrive/LargeJumpDrive" };
        public static string [] CameraBlock_SmallCameraBlock = { "Camera", "MyObjectBuilder_CameraBlock", "SmallCameraBlock", "MyObjectBuilder_CameraBlock/SmallCameraBlock" };
        public static string [] CameraBlock_LargeCameraBlock = { "Camera", "MyObjectBuilder_CameraBlock", "LargeCameraBlock", "MyObjectBuilder_CameraBlock/LargeCameraBlock" };
        public static string [] MergeBlock_LargeShipMergeBlock = { "Merge Block", "MyObjectBuilder_MergeBlock", "LargeShipMergeBlock", "MyObjectBuilder_MergeBlock/LargeShipMergeBlock" };
        public static string [] MergeBlock_SmallShipMergeBlock = { "Merge Block", "MyObjectBuilder_MergeBlock", "SmallShipMergeBlock", "MyObjectBuilder_MergeBlock/SmallShipMergeBlock" };
        public static string [] Parachute_LgParachute = { "Parachute Hatch", "MyObjectBuilder_Parachute", "LgParachute", "MyObjectBuilder_Parachute/LgParachute" };
        public static string [] Parachute_SmParachute = { "Parachute Hatch", "MyObjectBuilder_Parachute", "SmParachute", "MyObjectBuilder_Parachute/SmParachute" };
        public static string [] Warhead_LargeWarhead = { "Warhead", "MyObjectBuilder_Warhead", "LargeWarhead", "MyObjectBuilder_Warhead/LargeWarhead" };
        public static string [] Warhead_SmallWarhead = { "Warhead", "MyObjectBuilder_Warhead", "SmallWarhead", "MyObjectBuilder_Warhead/SmallWarhead" };
        public static string [] Decoy_LargeDecoy = { "Decoy", "MyObjectBuilder_Decoy", "LargeDecoy", "MyObjectBuilder_Decoy/LargeDecoy" };
        public static string [] Decoy_SmallDecoy = { "Decoy", "MyObjectBuilder_Decoy", "SmallDecoy", "MyObjectBuilder_Decoy/SmallDecoy" };
        public static string [] LargeGatlingTurret_null = {"Gatling Turret","MyObjectBuilder_LargeGatlingTurret","","MyObjectBuilder_LargeGatlingTurret/(null)"};
        public static string [] LargeGatlingTurret_SmallGatlingTurret = { "Gatling Turret", "MyObjectBuilder_LargeGatlingTurret", "SmallGatlingTurret", "MyObjectBuilder_LargeGatlingTurret/SmallGatlingTurret" };
        public static string [] LargeMissileTurret_null = {"Missile Turret","MyObjectBuilder_LargeMissileTurret","","MyObjectBuilder_LargeMissileTurret/(null)"};
        public static string [] LargeMissileTurret_SmallMissileTurret = { "Missile Turret", "MyObjectBuilder_LargeMissileTurret", "SmallMissileTurret", "MyObjectBuilder_LargeMissileTurret/SmallMissileTurret" };
        public static string [] InteriorTurret_LargeInteriorTurret = { "Interior Turret", "MyObjectBuilder_InteriorTurret", "LargeInteriorTurret", "MyObjectBuilder_InteriorTurret/LargeInteriorTurret" };
        public static string [] SmallMissileLauncher_null = {"Rocket Launcher","MyObjectBuilder_SmallMissileLauncher","","MyObjectBuilder_SmallMissileLauncher/(null)"};
        public static string [] SmallMissileLauncher_LargeMissileLauncher = { "Rocket Launcher", "MyObjectBuilder_SmallMissileLauncher", "LargeMissileLauncher", "MyObjectBuilder_SmallMissileLauncher/LargeMissileLauncher" };
        public static string [] SmallMissileLauncherReload_SmallRocketLauncherReload = { "Reloadable Rocket Launcher", "MyObjectBuilder_SmallMissileLauncherReload", "SmallRocketLauncherReload", "MyObjectBuilder_SmallMissileLauncherReload/SmallRocketLauncherReload" };
        public static string [] SmallGatlingGun_null = {"Gatling Gun","MyObjectBuilder_SmallGatlingGun","","MyObjectBuilder_SmallGatlingGun/(null)"};
        public static string [] MotorSuspension_Suspension3x3 = { "Wheel Suspension 3x3 Right", "MyObjectBuilder_MotorSuspension", "Suspension3x3", "MyObjectBuilder_MotorSuspension/Suspension3x3" };
        public static string [] MotorSuspension_Suspension5x5 = { "Wheel Suspension 5x5 Right", "MyObjectBuilder_MotorSuspension", "Suspension5x5", "MyObjectBuilder_MotorSuspension/Suspension5x5" };
        public static string [] MotorSuspension_Suspension1x1 = { "Wheel Suspension 1x1 Right", "MyObjectBuilder_MotorSuspension", "Suspension1x1", "MyObjectBuilder_MotorSuspension/Suspension1x1" };
        public static string [] MotorSuspension_SmallSuspension3x3 = { "Wheel Suspension 3x3 Right", "MyObjectBuilder_MotorSuspension", "SmallSuspension3x3", "MyObjectBuilder_MotorSuspension/SmallSuspension3x3" };
        public static string [] MotorSuspension_SmallSuspension5x5 = { "Wheel Suspension 5x5 Right", "MyObjectBuilder_MotorSuspension", "SmallSuspension5x5", "MyObjectBuilder_MotorSuspension/SmallSuspension5x5" };
        public static string [] MotorSuspension_SmallSuspension1x1 = { "Wheel Suspension 1x1 Right", "MyObjectBuilder_MotorSuspension", "SmallSuspension1x1", "MyObjectBuilder_MotorSuspension/SmallSuspension1x1" };
        public static string [] MotorSuspension_Suspension3x3mirrored = { "Wheel Suspension 3x3 Left", "MyObjectBuilder_MotorSuspension", "Suspension3x3mirrored", "MyObjectBuilder_MotorSuspension/Suspension3x3mirrored" };
        public static string [] MotorSuspension_Suspension5x5mirrored = { "Wheel Suspension 5x5 Left", "MyObjectBuilder_MotorSuspension", "Suspension5x5mirrored", "MyObjectBuilder_MotorSuspension/Suspension5x5mirrored" };
        public static string [] MotorSuspension_Suspension1x1mirrored = { "Wheel Suspension 1x1 Left", "MyObjectBuilder_MotorSuspension", "Suspension1x1mirrored", "MyObjectBuilder_MotorSuspension/Suspension1x1mirrored" };
        public static string [] MotorSuspension_SmallSuspension3x3mirrored = { "Wheel Suspension 3x3 Left", "MyObjectBuilder_MotorSuspension", "SmallSuspension3x3mirrored", "MyObjectBuilder_MotorSuspension/SmallSuspension3x3mirrored" };
        public static string [] MotorSuspension_SmallSuspension5x5mirrored = { "Wheel Suspension 5x5 Left", "MyObjectBuilder_MotorSuspension", "SmallSuspension5x5mirrored", "MyObjectBuilder_MotorSuspension/SmallSuspension5x5mirrored" };
        public static string [] MotorSuspension_SmallSuspension1x1mirrored = { "Wheel Suspension 1x1 Left", "MyObjectBuilder_MotorSuspension", "SmallSuspension1x1mirrored", "MyObjectBuilder_MotorSuspension/SmallSuspension1x1mirrored" };
        public static string [] Wheel_SmallRealWheel1x1 = { "Wheel 1x1", "MyObjectBuilder_Wheel", "SmallRealWheel1x1", "MyObjectBuilder_Wheel/SmallRealWheel1x1" };
        public static string [] Wheel_SmallRealWheel = { "Wheel 3x3", "MyObjectBuilder_Wheel", "SmallRealWheel", "MyObjectBuilder_Wheel/SmallRealWheel" };
        public static string [] Wheel_SmallRealWheel5x5 = { "Wheel 5x5", "MyObjectBuilder_Wheel", "SmallRealWheel5x5", "MyObjectBuilder_Wheel/SmallRealWheel5x5" };
        public static string [] Wheel_RealWheel1x1 = { "Wheel 1x1", "MyObjectBuilder_Wheel", "RealWheel1x1", "MyObjectBuilder_Wheel/RealWheel1x1" };
        public static string [] Wheel_RealWheel = { "Wheel 3x3", "MyObjectBuilder_Wheel", "RealWheel", "MyObjectBuilder_Wheel/RealWheel" };
        public static string [] Wheel_RealWheel5x5 = { "Wheel 5x5", "MyObjectBuilder_Wheel", "RealWheel5x5", "MyObjectBuilder_Wheel/RealWheel5x5" };
        public static string [] Wheel_SmallRealWheel1x1mirrored = { "Wheel 1x1", "MyObjectBuilder_Wheel", "SmallRealWheel1x1mirrored", "MyObjectBuilder_Wheel/SmallRealWheel1x1mirrored" };
        public static string [] Wheel_SmallRealWheelmirrored = { "Wheel 3x3", "MyObjectBuilder_Wheel", "SmallRealWheelmirrored", "MyObjectBuilder_Wheel/SmallRealWheelmirrored" };
        public static string [] Wheel_SmallRealWheel5x5mirrored = { "Wheel 5x5", "MyObjectBuilder_Wheel", "SmallRealWheel5x5mirrored", "MyObjectBuilder_Wheel/SmallRealWheel5x5mirrored" };
        public static string [] Wheel_RealWheel1x1mirrored = { "Wheel 1x1", "MyObjectBuilder_Wheel", "RealWheel1x1mirrored", "MyObjectBuilder_Wheel/RealWheel1x1mirrored" };
        public static string [] Wheel_RealWheelmirrored = { "Wheel 3x3", "MyObjectBuilder_Wheel", "RealWheelmirrored", "MyObjectBuilder_Wheel/RealWheelmirrored" };
        public static string [] Wheel_RealWheel5x5mirrored = { "Wheel 5x5", "MyObjectBuilder_Wheel", "RealWheel5x5mirrored", "MyObjectBuilder_Wheel/RealWheel5x5mirrored" };
        public static string [] Wheel_Wheel1x1 = { "Wheel 1x1", "MyObjectBuilder_Wheel", "Wheel1x1", "MyObjectBuilder_Wheel/Wheel1x1" };
        public static string [] Wheel_SmallWheel1x1 = { "Wheel 1x1", "MyObjectBuilder_Wheel", "SmallWheel1x1", "MyObjectBuilder_Wheel/SmallWheel1x1" };
        public static string [] Wheel_Wheel3x3 = { "Wheel 3x3", "MyObjectBuilder_Wheel", "Wheel3x3", "MyObjectBuilder_Wheel/Wheel3x3" };
        public static string [] Wheel_SmallWheel3x3 = { "Wheel 3x3", "MyObjectBuilder_Wheel", "SmallWheel3x3", "MyObjectBuilder_Wheel/SmallWheel3x3" };
        public static string [] Wheel_Wheel5x5 = { "Wheel 5x5", "MyObjectBuilder_Wheel", "Wheel5x5", "MyObjectBuilder_Wheel/Wheel5x5" };
        public static string [] Wheel_SmallWheel5x5 = { "Wheel 5x5", "MyObjectBuilder_Wheel", "SmallWheel5x5", "MyObjectBuilder_Wheel/SmallWheel5x5" };
        public static string [] CubeBlock_LargeWindowSquare = { "Vertical Window", "MyObjectBuilder_CubeBlock", "LargeWindowSquare", "MyObjectBuilder_CubeBlock/LargeWindowSquare" };
        public static string [] CubeBlock_LargeWindowEdge = { "Diagonal Window", "MyObjectBuilder_CubeBlock", "LargeWindowEdge", "MyObjectBuilder_CubeBlock/LargeWindowEdge" };
        public static string [] CubeBlock_Window1x2Slope = { "Window 1x2 Slope", "MyObjectBuilder_CubeBlock", "Window1x2Slope", "MyObjectBuilder_CubeBlock/Window1x2Slope" };
        public static string [] CubeBlock_Window1x2Inv = { "Window 1x2 Face Inv.", "MyObjectBuilder_CubeBlock", "Window1x2Inv", "MyObjectBuilder_CubeBlock/Window1x2Inv" };
        public static string [] CubeBlock_Window1x2Face = { "Window 1x2 Face", "MyObjectBuilder_CubeBlock", "Window1x2Face", "MyObjectBuilder_CubeBlock/Window1x2Face" };
        public static string [] CubeBlock_Window1x2SideLeft = { "Window 1x2 Side Left", "MyObjectBuilder_CubeBlock", "Window1x2SideLeft", "MyObjectBuilder_CubeBlock/Window1x2SideLeft" };
        public static string [] CubeBlock_Window1x2SideLeftInv = { "Window 1x2 Side Left Inv", "MyObjectBuilder_CubeBlock", "Window1x2SideLeftInv", "MyObjectBuilder_CubeBlock/Window1x2SideLeftInv" };
        public static string [] CubeBlock_Window1x2SideRight = { "Window 1x2 Side Right", "MyObjectBuilder_CubeBlock", "Window1x2SideRight", "MyObjectBuilder_CubeBlock/Window1x2SideRight" };
        public static string [] CubeBlock_Window1x2SideRightInv = { "Window 1x2 Side Right Inv", "MyObjectBuilder_CubeBlock", "Window1x2SideRightInv", "MyObjectBuilder_CubeBlock/Window1x2SideRightInv" };
        public static string [] CubeBlock_Window1x1Slope = { "Window 1x1 Slope", "MyObjectBuilder_CubeBlock", "Window1x1Slope", "MyObjectBuilder_CubeBlock/Window1x1Slope" };
        public static string [] CubeBlock_Window1x1Face = { "Window 1x1 Face", "MyObjectBuilder_CubeBlock", "Window1x1Face", "MyObjectBuilder_CubeBlock/Window1x1Face" };
        public static string [] CubeBlock_Window1x1Side = { "Window 1x1 Side", "MyObjectBuilder_CubeBlock", "Window1x1Side", "MyObjectBuilder_CubeBlock/Window1x1Side" };
        public static string [] CubeBlock_Window1x1SideInv = { "Window 1x1 Side Inv", "MyObjectBuilder_CubeBlock", "Window1x1SideInv", "MyObjectBuilder_CubeBlock/Window1x1SideInv" };
        public static string [] CubeBlock_Window1x1Inv = { "Window 1x1 Face Inv.", "MyObjectBuilder_CubeBlock", "Window1x1Inv", "MyObjectBuilder_CubeBlock/Window1x1Inv" };
        public static string [] CubeBlock_Window1x2Flat = { "Window 1x2 Flat", "MyObjectBuilder_CubeBlock", "Window1x2Flat", "MyObjectBuilder_CubeBlock/Window1x2Flat" };
        public static string [] CubeBlock_Window1x2FlatInv = { "Window 1x2 Flat Inv.", "MyObjectBuilder_CubeBlock", "Window1x2FlatInv", "MyObjectBuilder_CubeBlock/Window1x2FlatInv" };
        public static string [] CubeBlock_Window1x1Flat = { "Window 1x1 Flat", "MyObjectBuilder_CubeBlock", "Window1x1Flat", "MyObjectBuilder_CubeBlock/Window1x1Flat" };
        public static string [] CubeBlock_Window1x1FlatInv = { "Window 1x1 Flat Inv.", "MyObjectBuilder_CubeBlock", "Window1x1FlatInv", "MyObjectBuilder_CubeBlock/Window1x1FlatInv" };
        public static string [] CubeBlock_Window3x3Flat = { "Window 3x3 Flat", "MyObjectBuilder_CubeBlock", "Window3x3Flat", "MyObjectBuilder_CubeBlock/Window3x3Flat" };
        public static string [] CubeBlock_Window3x3FlatInv = { "Window 3x3 Flat Inv.", "MyObjectBuilder_CubeBlock", "Window3x3FlatInv", "MyObjectBuilder_CubeBlock/Window3x3FlatInv" };
        public static string [] CubeBlock_Window2x3Flat = { "Window 2x3 Flat", "MyObjectBuilder_CubeBlock", "Window2x3Flat", "MyObjectBuilder_CubeBlock/Window2x3Flat" };
        public static string [] CubeBlock_Window2x3FlatInv = { "Window 2x3 Flat Inv.", "MyObjectBuilder_CubeBlock", "Window2x3FlatInv", "MyObjectBuilder_CubeBlock/Window2x3FlatInv" };
        public static string [] CubeBlock_SmallWindow1x2Slope = { "Window 1x2 Slope", "MyObjectBuilder_CubeBlock", "SmallWindow1x2Slope", "MyObjectBuilder_CubeBlock/SmallWindow1x2Slope" };
        public static string [] CubeBlock_SmallWindow1x2Inv = { "Window 1x2 Face Inv.", "MyObjectBuilder_CubeBlock", "SmallWindow1x2Inv", "MyObjectBuilder_CubeBlock/SmallWindow1x2Inv" };
        public static string [] CubeBlock_SmallWindow1x2Face = { "Window 1x2 Face", "MyObjectBuilder_CubeBlock", "SmallWindow1x2Face", "MyObjectBuilder_CubeBlock/SmallWindow1x2Face" };
        public static string [] CubeBlock_SmallWindow1x2SideLeft = { "Window 1x2 Side Left", "MyObjectBuilder_CubeBlock", "SmallWindow1x2SideLeft", "MyObjectBuilder_CubeBlock/SmallWindow1x2SideLeft" };
        public static string [] CubeBlock_SmallWindow1x2SideLeftInv = { "Window 1x2 Side Left Inv", "MyObjectBuilder_CubeBlock", "SmallWindow1x2SideLeftInv", "MyObjectBuilder_CubeBlock/SmallWindow1x2SideLeftInv" };
        public static string [] CubeBlock_SmallWindow1x2SideRight = { "Window 1x2 Side Right", "MyObjectBuilder_CubeBlock", "SmallWindow1x2SideRight", "MyObjectBuilder_CubeBlock/SmallWindow1x2SideRight" };
        public static string [] CubeBlock_SmallWindow1x2SideRightInv = { "Window 1x2 Side Right Inv", "MyObjectBuilder_CubeBlock", "SmallWindow1x2SideRightInv", "MyObjectBuilder_CubeBlock/SmallWindow1x2SideRightInv" };
        public static string [] CubeBlock_SmallWindow1x1Slope = { "Window 1x1 Slope", "MyObjectBuilder_CubeBlock", "SmallWindow1x1Slope", "MyObjectBuilder_CubeBlock/SmallWindow1x1Slope" };
        public static string [] CubeBlock_SmallWindow1x1Face = { "Window 1x1 Face", "MyObjectBuilder_CubeBlock", "SmallWindow1x1Face", "MyObjectBuilder_CubeBlock/SmallWindow1x1Face" };
        public static string [] CubeBlock_SmallWindow1x1Side = { "Window 1x1 Side", "MyObjectBuilder_CubeBlock", "SmallWindow1x1Side", "MyObjectBuilder_CubeBlock/SmallWindow1x1Side" };
        public static string [] CubeBlock_SmallWindow1x1SideInv = { "Window 1x1 Side Inv", "MyObjectBuilder_CubeBlock", "SmallWindow1x1SideInv", "MyObjectBuilder_CubeBlock/SmallWindow1x1SideInv" };
        public static string [] CubeBlock_SmallWindow1x1Inv = { "Window 1x1 Face Inv.", "MyObjectBuilder_CubeBlock", "SmallWindow1x1Inv", "MyObjectBuilder_CubeBlock/SmallWindow1x1Inv" };
        public static string [] CubeBlock_SmallWindow1x2Flat = { "Window 1x2 Flat", "MyObjectBuilder_CubeBlock", "SmallWindow1x2Flat", "MyObjectBuilder_CubeBlock/SmallWindow1x2Flat" };
        public static string [] CubeBlock_SmallWindow1x2FlatInv = { "Window 1x2 Flat Inv.", "MyObjectBuilder_CubeBlock", "SmallWindow1x2FlatInv", "MyObjectBuilder_CubeBlock/SmallWindow1x2FlatInv" };
        public static string [] CubeBlock_SmallWindow1x1Flat = { "Window 1x1 Flat", "MyObjectBuilder_CubeBlock", "SmallWindow1x1Flat", "MyObjectBuilder_CubeBlock/SmallWindow1x1Flat" };
        public static string [] CubeBlock_SmallWindow1x1FlatInv = { "Window 1x1 Flat Inv.", "MyObjectBuilder_CubeBlock", "SmallWindow1x1FlatInv", "MyObjectBuilder_CubeBlock/SmallWindow1x1FlatInv" };
        public static string [] CubeBlock_SmallWindow3x3Flat = { "Window 3x3 Flat", "MyObjectBuilder_CubeBlock", "SmallWindow3x3Flat", "MyObjectBuilder_CubeBlock/SmallWindow3x3Flat" };
        public static string [] CubeBlock_SmallWindow3x3FlatInv = { "Window 3x3 Flat Inv.", "MyObjectBuilder_CubeBlock", "SmallWindow3x3FlatInv", "MyObjectBuilder_CubeBlock/SmallWindow3x3FlatInv" };
        public static string [] CubeBlock_SmallWindow2x3Flat = { "Window 2x3 Flat", "MyObjectBuilder_CubeBlock", "SmallWindow2x3Flat", "MyObjectBuilder_CubeBlock/SmallWindow2x3Flat" };
        public static string [] CubeBlock_SmallWindow2x3FlatInv = { "Window 2x3 Flat Inv.", "MyObjectBuilder_CubeBlock", "SmallWindow2x3FlatInv", "MyObjectBuilder_CubeBlock/SmallWindow2x3FlatInv" };

        public static List<string> AllBlocks = new List<string>() {
            "MyObjectBuilder_CubeBlock/LargeRailStraight",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorBlock",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorCornerInv",
            "MyObjectBuilder_CubeBlock/LargeRoundArmor_Slope",
            "MyObjectBuilder_CubeBlock/LargeRoundArmor_Corner",
            "MyObjectBuilder_CubeBlock/LargeRoundArmor_CornerInv",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorBlock",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCornerInv",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorBlock",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorSlope",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorCorner",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorCornerInv",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorBlock",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCornerInv",
            "MyObjectBuilder_CubeBlock/LargeHalfArmorBlock",
            "MyObjectBuilder_CubeBlock/LargeHeavyHalfArmorBlock",
            "MyObjectBuilder_CubeBlock/LargeHalfSlopeArmorBlock",
            "MyObjectBuilder_CubeBlock/LargeHeavyHalfSlopeArmorBlock",
            "MyObjectBuilder_CubeBlock/HalfArmorBlock",
            "MyObjectBuilder_CubeBlock/HeavyHalfArmorBlock",
            "MyObjectBuilder_CubeBlock/HalfSlopeArmorBlock",
            "MyObjectBuilder_CubeBlock/HeavyHalfSlopeArmorBlock",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundSlope",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCornerInv",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundSlope",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundCorner",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundCornerInv",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorRoundSlope",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorRoundCorner",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorRoundCornerInv",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundSlope",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCorner",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCornerInv",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Base",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Tip",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Base",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Tip",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Base",
            "MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Tip",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope2Base",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope2Tip",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner2Base",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner2Tip",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorInvCorner2Base",
            "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorInvCorner2Tip",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorSlope2Base",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorSlope2Tip",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorCorner2Base",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorCorner2Tip",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorInvCorner2Base",
            "MyObjectBuilder_CubeBlock/SmallBlockArmorInvCorner2Tip",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Base",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Tip",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Base",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Tip",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Base",
            "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Tip",
            "MyObjectBuilder_MyProgrammableBlock/SmallProgrammableBlock",
            "MyObjectBuilder_Projector/LargeProjector",
            "MyObjectBuilder_Projector/SmallProjector",
            "MyObjectBuilder_SensorBlock/SmallBlockSensor",
            "MyObjectBuilder_SensorBlock/LargeBlockSensor",
            "MyObjectBuilder_SoundBlock/SmallBlockSoundBlock",
            "MyObjectBuilder_SoundBlock/LargeBlockSoundBlock",
            "MyObjectBuilder_ButtonPanel/ButtonPanelLarge",
            "MyObjectBuilder_ButtonPanel/ButtonPanelSmall",
            "MyObjectBuilder_TimerBlock/TimerBlockLarge",
            "MyObjectBuilder_TimerBlock/TimerBlockSmall",
            "MyObjectBuilder_MyProgrammableBlock/LargeProgrammableBlock",
            "MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna",
            "MyObjectBuilder_Beacon/LargeBlockBeacon",
            "MyObjectBuilder_Beacon/SmallBlockBeacon",
            "MyObjectBuilder_RadioAntenna/SmallBlockRadioAntenna",
            "MyObjectBuilder_RemoteControl/LargeBlockRemoteControl",
            "MyObjectBuilder_RemoteControl/SmallBlockRemoteControl",
            "MyObjectBuilder_LaserAntenna/LargeBlockLaserAntenna",
            "MyObjectBuilder_LaserAntenna/SmallBlockLaserAntenna",
            "MyObjectBuilder_TerminalBlock/ControlPanel",
            "MyObjectBuilder_TerminalBlock/SmallControlPanel",
            "MyObjectBuilder_Cockpit/LargeBlockCockpit",
            "MyObjectBuilder_Cockpit/LargeBlockCockpitSeat",
            "MyObjectBuilder_Cockpit/SmallBlockCockpit",
            "MyObjectBuilder_Cockpit/DBSmallBlockFighterCockpit",
            "MyObjectBuilder_Cockpit/CockpitOpen",
            "MyObjectBuilder_Gyro/LargeBlockGyro",
            "MyObjectBuilder_Gyro/SmallBlockGyro",
            "MyObjectBuilder_Cockpit/OpenCockpitSmall",
            "MyObjectBuilder_Cockpit/OpenCockpitLarge",
            "MyObjectBuilder_Cockpit/LargeBlockDesk",
            "MyObjectBuilder_Cockpit/LargeBlockDeskCorner",
            "MyObjectBuilder_CubeBlock/LargeBlockDeskChairless",
            "MyObjectBuilder_CubeBlock/LargeBlockDeskChairlessCorner",
            "MyObjectBuilder_Kitchen/LargeBlockKitchen",
            "MyObjectBuilder_CryoChamber/LargeBlockBed",
            "MyObjectBuilder_CargoContainer/LargeBlockLockerRoom",
            "MyObjectBuilder_CargoContainer/LargeBlockLockerRoomCorner",
            "MyObjectBuilder_Planter/LargeBlockPlanters",
            "MyObjectBuilder_Cockpit/LargeBlockCouch",
            "MyObjectBuilder_Cockpit/LargeBlockCouchCorner",
            "MyObjectBuilder_CargoContainer/LargeBlockLockers",
            "MyObjectBuilder_Cockpit/LargeBlockBathroomOpen",
            "MyObjectBuilder_Cockpit/LargeBlockBathroom",
            "MyObjectBuilder_Cockpit/LargeBlockToilet",
            "MyObjectBuilder_Projector/LargeBlockConsole",
            "MyObjectBuilder_Cockpit/SmallBlockCockpitIndustrial",
            "MyObjectBuilder_Cockpit/LargeBlockCockpitIndustrial",
            "MyObjectBuilder_VendingMachine/FoodDispenser",
            "MyObjectBuilder_Jukebox/Jukebox",
            "MyObjectBuilder_LCDPanelsBlock/LabEquipment",
            "MyObjectBuilder_CubeBlock/Shower",
            "MyObjectBuilder_CubeBlock/WindowWall",
            "MyObjectBuilder_CubeBlock/WindowWallLeft",
            "MyObjectBuilder_CubeBlock/WindowWallRight",
            "MyObjectBuilder_LCDPanelsBlock/MedicalStation",
            "MyObjectBuilder_TextPanel/TransparentLCDLarge",
            "MyObjectBuilder_TextPanel/TransparentLCDSmall",
            "MyObjectBuilder_CubeBlock/Catwalk",
            "MyObjectBuilder_CubeBlock/CatwalkCorner",
            "MyObjectBuilder_CubeBlock/CatwalkStraight",
            "MyObjectBuilder_CubeBlock/CatwalkWall",
            "MyObjectBuilder_CubeBlock/CatwalkRailingEnd",
            "MyObjectBuilder_CubeBlock/CatwalkRailingHalfRight",
            "MyObjectBuilder_CubeBlock/CatwalkRailingHalfLeft",
            "MyObjectBuilder_CubeBlock/GratedStairs",
            "MyObjectBuilder_CubeBlock/GratedHalfStairs",
            "MyObjectBuilder_CubeBlock/GratedHalfStairsMirrored",
            "MyObjectBuilder_CubeBlock/RailingStraight",
            "MyObjectBuilder_CubeBlock/RailingDouble",
            "MyObjectBuilder_CubeBlock/RailingCorner",
            "MyObjectBuilder_CubeBlock/RailingDiagonal",
            "MyObjectBuilder_CubeBlock/RailingHalfRight",
            "MyObjectBuilder_CubeBlock/RailingHalfLeft",
            "MyObjectBuilder_ReflectorLight/RotatingLightLarge",
            "MyObjectBuilder_ReflectorLight/RotatingLightSmall",
            "MyObjectBuilder_CubeBlock/Freight1",
            "MyObjectBuilder_CubeBlock/Freight2",
            "MyObjectBuilder_CubeBlock/Freight3",
            "MyObjectBuilder_Door/(null)",
            "MyObjectBuilder_AirtightHangarDoor/(null)",
            "MyObjectBuilder_AirtightSlideDoor/LargeBlockSlideDoor",
            "MyObjectBuilder_CubeBlock/ArmorCenter",
            "MyObjectBuilder_CubeBlock/ArmorCorner",
            "MyObjectBuilder_CubeBlock/ArmorInvCorner",
            "MyObjectBuilder_CubeBlock/ArmorSide",
            "MyObjectBuilder_CubeBlock/SmallArmorCenter",
            "MyObjectBuilder_CubeBlock/SmallArmorCorner",
            "MyObjectBuilder_CubeBlock/SmallArmorInvCorner",
            "MyObjectBuilder_CubeBlock/SmallArmorSide",
            "MyObjectBuilder_StoreBlock/StoreBlock",
            "MyObjectBuilder_SafeZoneBlock/SafeZoneBlock",
            "MyObjectBuilder_ContractBlock/ContractBlock",
            "MyObjectBuilder_VendingMachine/VendingMachine",
            "MyObjectBuilder_StoreBlock/AtmBlock",
            "MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock",
            "MyObjectBuilder_BatteryBlock/SmallBlockBatteryBlock",
            "MyObjectBuilder_BatteryBlock/SmallBlockSmallBatteryBlock",
            "MyObjectBuilder_Reactor/SmallBlockSmallGenerator",
            "MyObjectBuilder_Reactor/SmallBlockLargeGenerator",
            "MyObjectBuilder_Reactor/LargeBlockSmallGenerator",
            "MyObjectBuilder_Reactor/LargeBlockLargeGenerator",
            "MyObjectBuilder_HydrogenEngine/LargeHydrogenEngine",
            "MyObjectBuilder_HydrogenEngine/SmallHydrogenEngine",
            "MyObjectBuilder_WindTurbine/LargeBlockWindTurbine",
            "MyObjectBuilder_SolarPanel/LargeBlockSolarPanel",
            "MyObjectBuilder_SolarPanel/SmallBlockSolarPanel",
            "MyObjectBuilder_CubeBlock/Monolith",
            "MyObjectBuilder_CubeBlock/Stereolith",
            "MyObjectBuilder_CubeBlock/DeadAstronaut",
            "MyObjectBuilder_CubeBlock/LargeDeadAstronaut",
            "MyObjectBuilder_GravityGenerator/(null)",
            "MyObjectBuilder_GravityGeneratorSphere/(null)",
            "MyObjectBuilder_VirtualMass/VirtualMassLarge",
            "MyObjectBuilder_VirtualMass/VirtualMassSmall",
            "MyObjectBuilder_SpaceBall/SpaceBallLarge",
            "MyObjectBuilder_SpaceBall/SpaceBallSmall",
            "MyObjectBuilder_Passage/(null)",
            "MyObjectBuilder_CubeBlock/LargeStairs",
            "MyObjectBuilder_CubeBlock/LargeRamp",
            "MyObjectBuilder_CubeBlock/LargeSteelCatwalk",
            "MyObjectBuilder_CubeBlock/LargeSteelCatwalk2Sides",
            "MyObjectBuilder_CubeBlock/LargeSteelCatwalkCorner",
            "MyObjectBuilder_CubeBlock/LargeSteelCatwalkPlate",
            "MyObjectBuilder_CubeBlock/LargeCoverWall",
            "MyObjectBuilder_CubeBlock/LargeCoverWallHalf",
            "MyObjectBuilder_CubeBlock/LargeBlockInteriorWall",
            "MyObjectBuilder_CubeBlock/LargeInteriorPillar",
            "MyObjectBuilder_Cockpit/PassengerSeatLarge",
            "MyObjectBuilder_Cockpit/PassengerSeatSmall",
            "MyObjectBuilder_Ladder2/(null)",
            "MyObjectBuilder_Ladder2/LadderSmall",
            "MyObjectBuilder_TextPanel/SmallTextPanel",
            "MyObjectBuilder_TextPanel/SmallLCDPanelWide",
            "MyObjectBuilder_TextPanel/SmallLCDPanel",
            "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_1",
            "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_2",
            "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_Flat_1",
            "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_Flat_2",
            "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_1",
            "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_2",
            "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_Flat_1",
            "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_Flat_2",
            "MyObjectBuilder_TextPanel/LargeTextPanel",
            "MyObjectBuilder_TextPanel/LargeLCDPanel",
            "MyObjectBuilder_TextPanel/LargeLCDPanelWide",
            "MyObjectBuilder_ReflectorLight/LargeBlockFrontLight",
            "MyObjectBuilder_ReflectorLight/SmallBlockFrontLight",
            "MyObjectBuilder_InteriorLight/SmallLight",
            "MyObjectBuilder_InteriorLight/SmallBlockSmallLight",
            "MyObjectBuilder_InteriorLight/LargeBlockLight_1corner",
            "MyObjectBuilder_InteriorLight/LargeBlockLight_2corner",
            "MyObjectBuilder_InteriorLight/SmallBlockLight_1corner",
            "MyObjectBuilder_InteriorLight/SmallBlockLight_2corner",
            "MyObjectBuilder_OxygenTank/OxygenTankSmall",
            "MyObjectBuilder_OxygenTank/(null)",
            "MyObjectBuilder_OxygenTank/LargeHydrogenTank",
            "MyObjectBuilder_OxygenTank/SmallHydrogenTank",
            "MyObjectBuilder_AirVent/(null)",
            "MyObjectBuilder_AirVent/SmallAirVent",
            "MyObjectBuilder_CargoContainer/SmallBlockSmallContainer",
            "MyObjectBuilder_CargoContainer/SmallBlockMediumContainer",
            "MyObjectBuilder_CargoContainer/SmallBlockLargeContainer",
            "MyObjectBuilder_CargoContainer/LargeBlockSmallContainer",
            "MyObjectBuilder_CargoContainer/LargeBlockLargeContainer",
            "MyObjectBuilder_Conveyor/SmallBlockConveyor",
            "MyObjectBuilder_Conveyor/LargeBlockConveyor",
            "MyObjectBuilder_Collector/Collector",
            "MyObjectBuilder_Collector/CollectorSmall",
            "MyObjectBuilder_ShipConnector/Connector",
            "MyObjectBuilder_ShipConnector/ConnectorSmall",
            "MyObjectBuilder_ShipConnector/ConnectorMedium",
            "MyObjectBuilder_ConveyorConnector/ConveyorTube",
            "MyObjectBuilder_ConveyorConnector/ConveyorTubeSmall",
            "MyObjectBuilder_ConveyorConnector/ConveyorTubeMedium",
            "MyObjectBuilder_ConveyorConnector/ConveyorFrameMedium",
            "MyObjectBuilder_ConveyorConnector/ConveyorTubeCurved",
            "MyObjectBuilder_ConveyorConnector/ConveyorTubeSmallCurved",
            "MyObjectBuilder_ConveyorConnector/ConveyorTubeCurvedMedium",
            "MyObjectBuilder_Conveyor/SmallShipConveyorHub",
            "MyObjectBuilder_ConveyorSorter/LargeBlockConveyorSorter",
            "MyObjectBuilder_ConveyorSorter/MediumBlockConveyorSorter",
            "MyObjectBuilder_ConveyorSorter/SmallBlockConveyorSorter",
            "MyObjectBuilder_PistonBase/LargePistonBase",
            "MyObjectBuilder_ExtendedPistonBase/LargePistonBase",
            "MyObjectBuilder_PistonTop/LargePistonTop",
            "MyObjectBuilder_PistonBase/SmallPistonBase",
            "MyObjectBuilder_ExtendedPistonBase/SmallPistonBase",
            "MyObjectBuilder_PistonTop/SmallPistonTop",
            "MyObjectBuilder_MotorStator/LargeStator",
            "MyObjectBuilder_MotorRotor/LargeRotor",
            "MyObjectBuilder_MotorStator/SmallStator",
            "MyObjectBuilder_MotorRotor/SmallRotor",
            "MyObjectBuilder_MotorAdvancedStator/LargeAdvancedStator",
            "MyObjectBuilder_MotorAdvancedRotor/LargeAdvancedRotor",
            "MyObjectBuilder_MotorAdvancedStator/SmallAdvancedStator",
            "MyObjectBuilder_MotorAdvancedRotor/SmallAdvancedRotor",
            "MyObjectBuilder_MedicalRoom/LargeMedicalRoom",
            "MyObjectBuilder_CryoChamber/LargeBlockCryoChamber",
            "MyObjectBuilder_CryoChamber/SmallBlockCryoChamber",
            "MyObjectBuilder_Refinery/LargeRefinery",
            "MyObjectBuilder_Refinery/Blast Furnace",
            "MyObjectBuilder_OxygenGenerator/(null)",
            "MyObjectBuilder_OxygenGenerator/OxygenGeneratorSmall",
            "MyObjectBuilder_Assembler/LargeAssembler",
            "MyObjectBuilder_Assembler/BasicAssembler",
            "MyObjectBuilder_SurvivalKit/SurvivalKitLarge",
            "MyObjectBuilder_SurvivalKit/SurvivalKit",
            "MyObjectBuilder_OxygenFarm/LargeBlockOxygenFarm",
            "MyObjectBuilder_UpgradeModule/LargeProductivityModule",
            "MyObjectBuilder_UpgradeModule/LargeEffectivenessModule",
            "MyObjectBuilder_UpgradeModule/LargeEnergyModule",
            "MyObjectBuilder_Thrust/SmallBlockSmallThrust",
            "MyObjectBuilder_Thrust/SmallBlockLargeThrust",
            "MyObjectBuilder_Thrust/LargeBlockSmallThrust",
            "MyObjectBuilder_Thrust/LargeBlockLargeThrust",
            "MyObjectBuilder_Thrust/LargeBlockLargeHydrogenThrust",
            "MyObjectBuilder_Thrust/LargeBlockSmallHydrogenThrust",
            "MyObjectBuilder_Thrust/SmallBlockLargeHydrogenThrust",
            "MyObjectBuilder_Thrust/SmallBlockSmallHydrogenThrust",
            "MyObjectBuilder_Thrust/LargeBlockLargeAtmosphericThrust",
            "MyObjectBuilder_Thrust/LargeBlockSmallAtmosphericThrust",
            "MyObjectBuilder_Thrust/SmallBlockLargeAtmosphericThrust",
            "MyObjectBuilder_Thrust/SmallBlockSmallAtmosphericThrust",
            "MyObjectBuilder_Drill/SmallBlockDrill",
            "MyObjectBuilder_Drill/LargeBlockDrill",
            "MyObjectBuilder_ShipGrinder/LargeShipGrinder",
            "MyObjectBuilder_ShipGrinder/SmallShipGrinder",
            "MyObjectBuilder_ShipWelder/LargeShipWelder",
            "MyObjectBuilder_ShipWelder/SmallShipWelder",
            "MyObjectBuilder_OreDetector/LargeOreDetector",
            "MyObjectBuilder_OreDetector/SmallBlockOreDetector",
            "MyObjectBuilder_LandingGear/LargeBlockLandingGear",
            "MyObjectBuilder_LandingGear/SmallBlockLandingGear",
            "MyObjectBuilder_JumpDrive/LargeJumpDrive",
            "MyObjectBuilder_CameraBlock/SmallCameraBlock",
            "MyObjectBuilder_CameraBlock/LargeCameraBlock",
            "MyObjectBuilder_MergeBlock/LargeShipMergeBlock",
            "MyObjectBuilder_MergeBlock/SmallShipMergeBlock",
            "MyObjectBuilder_Parachute/LgParachute",
            "MyObjectBuilder_Parachute/SmParachute",
            "MyObjectBuilder_Warhead/LargeWarhead",
            "MyObjectBuilder_Warhead/SmallWarhead",
            "MyObjectBuilder_Decoy/LargeDecoy",
            "MyObjectBuilder_Decoy/SmallDecoy",
            "MyObjectBuilder_LargeGatlingTurret/(null)",
            "MyObjectBuilder_LargeGatlingTurret/SmallGatlingTurret",
            "MyObjectBuilder_LargeMissileTurret/(null)",
            "MyObjectBuilder_LargeMissileTurret/SmallMissileTurret",
            "MyObjectBuilder_InteriorTurret/LargeInteriorTurret",
            "MyObjectBuilder_SmallMissileLauncher/(null)",
            "MyObjectBuilder_SmallMissileLauncher/LargeMissileLauncher",
            "MyObjectBuilder_SmallMissileLauncherReload/SmallRocketLauncherReload",
            "MyObjectBuilder_SmallGatlingGun/(null)",
            "MyObjectBuilder_MotorSuspension/Suspension3x3",
            "MyObjectBuilder_MotorSuspension/Suspension5x5",
            "MyObjectBuilder_MotorSuspension/Suspension1x1",
            "MyObjectBuilder_MotorSuspension/SmallSuspension3x3",
            "MyObjectBuilder_MotorSuspension/SmallSuspension5x5",
            "MyObjectBuilder_MotorSuspension/SmallSuspension1x1",
            "MyObjectBuilder_MotorSuspension/Suspension3x3mirrored",
            "MyObjectBuilder_MotorSuspension/Suspension5x5mirrored",
            "MyObjectBuilder_MotorSuspension/Suspension1x1mirrored",
            "MyObjectBuilder_MotorSuspension/SmallSuspension3x3mirrored",
            "MyObjectBuilder_MotorSuspension/SmallSuspension5x5mirrored",
            "MyObjectBuilder_MotorSuspension/SmallSuspension1x1mirrored",
            "MyObjectBuilder_Wheel/SmallRealWheel1x1",
            "MyObjectBuilder_Wheel/SmallRealWheel",
            "MyObjectBuilder_Wheel/SmallRealWheel5x5",
            "MyObjectBuilder_Wheel/RealWheel1x1",
            "MyObjectBuilder_Wheel/RealWheel",
            "MyObjectBuilder_Wheel/RealWheel5x5",
            "MyObjectBuilder_Wheel/SmallRealWheel1x1mirrored",
            "MyObjectBuilder_Wheel/SmallRealWheelmirrored",
            "MyObjectBuilder_Wheel/SmallRealWheel5x5mirrored",
            "MyObjectBuilder_Wheel/RealWheel1x1mirrored",
            "MyObjectBuilder_Wheel/RealWheelmirrored",
            "MyObjectBuilder_Wheel/RealWheel5x5mirrored",
            "MyObjectBuilder_Wheel/Wheel1x1",
            "MyObjectBuilder_Wheel/SmallWheel1x1",
            "MyObjectBuilder_Wheel/Wheel3x3",
            "MyObjectBuilder_Wheel/SmallWheel3x3",
            "MyObjectBuilder_Wheel/Wheel5x5",
            "MyObjectBuilder_Wheel/SmallWheel5x5",
            "MyObjectBuilder_CubeBlock/LargeWindowSquare",
            "MyObjectBuilder_CubeBlock/LargeWindowEdge",
            "MyObjectBuilder_CubeBlock/Window1x2Slope",
            "MyObjectBuilder_CubeBlock/Window1x2Inv",
            "MyObjectBuilder_CubeBlock/Window1x2Face",
            "MyObjectBuilder_CubeBlock/Window1x2SideLeft",
            "MyObjectBuilder_CubeBlock/Window1x2SideLeftInv",
            "MyObjectBuilder_CubeBlock/Window1x2SideRight",
            "MyObjectBuilder_CubeBlock/Window1x2SideRightInv",
            "MyObjectBuilder_CubeBlock/Window1x1Slope",
            "MyObjectBuilder_CubeBlock/Window1x1Face",
            "MyObjectBuilder_CubeBlock/Window1x1Side",
            "MyObjectBuilder_CubeBlock/Window1x1SideInv",
            "MyObjectBuilder_CubeBlock/Window1x1Inv",
            "MyObjectBuilder_CubeBlock/Window1x2Flat",
            "MyObjectBuilder_CubeBlock/Window1x2FlatInv",
            "MyObjectBuilder_CubeBlock/Window1x1Flat",
            "MyObjectBuilder_CubeBlock/Window1x1FlatInv",
            "MyObjectBuilder_CubeBlock/Window3x3Flat",
            "MyObjectBuilder_CubeBlock/Window3x3FlatInv",
            "MyObjectBuilder_CubeBlock/Window2x3Flat",
            "MyObjectBuilder_CubeBlock/Window2x3FlatInv",
            "MyObjectBuilder_CubeBlock/SmallWindow1x2Slope",
            "MyObjectBuilder_CubeBlock/SmallWindow1x2Inv",
            "MyObjectBuilder_CubeBlock/SmallWindow1x2Face",
            "MyObjectBuilder_CubeBlock/SmallWindow1x2SideLeft",
            "MyObjectBuilder_CubeBlock/SmallWindow1x2SideLeftInv",
            "MyObjectBuilder_CubeBlock/SmallWindow1x2SideRight",
            "MyObjectBuilder_CubeBlock/SmallWindow1x2SideRightInv",
            "MyObjectBuilder_CubeBlock/SmallWindow1x1Slope",
            "MyObjectBuilder_CubeBlock/SmallWindow1x1Face",
            "MyObjectBuilder_CubeBlock/SmallWindow1x1Side",
            "MyObjectBuilder_CubeBlock/SmallWindow1x1SideInv",
            "MyObjectBuilder_CubeBlock/SmallWindow1x1Inv",
            "MyObjectBuilder_CubeBlock/SmallWindow1x2Flat",
            "MyObjectBuilder_CubeBlock/SmallWindow1x2FlatInv",
            "MyObjectBuilder_CubeBlock/SmallWindow1x1Flat",
            "MyObjectBuilder_CubeBlock/SmallWindow1x1FlatInv",
            "MyObjectBuilder_CubeBlock/SmallWindow3x3Flat",
            "MyObjectBuilder_CubeBlock/SmallWindow3x3FlatInv",
            "MyObjectBuilder_CubeBlock/SmallWindow2x3Flat",
            "MyObjectBuilder_CubeBlock/SmallWindow2x3FlatInv",
        };
    }
}
