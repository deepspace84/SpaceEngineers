using System.Collections.Generic;
using System.Linq;
using AwwScrap.Helpers;
using AwwScrap.Utilities;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace AwwScrap
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	// ReSharper disable once ClassNeverInstantiated.Global
	public class AwwScrapSession : MySessionComponentBase
	{
		public static bool LogSetupComplete;
		public static Log GeneralLog;
		
		public override void BeforeStart()
		{
			base.BeforeStart();
			Initialize();
		}

		protected override void UnloadData()
		{
			CloseLogs();
		}

		private static void InitLogs()
		{
			if (Constants.EnableGeneralLog) GeneralLog = new Log(Constants.GeneralLogName);
			LogSetupComplete = true;
		}

		private static void CloseLogs()
		{
			if (Constants.EnableGeneralLog) GeneralLog.Close();
		}


		private static void Initialize()
		{
			InitLogs();
			MyAPIGateway.Parallel.StartBackground(ScrubCubes);
		}

		private static readonly Dictionary<MyStringHash, string> ComponentDictionary = new Dictionary<MyStringHash, string>
		{
			{ MyStringHash.GetOrCompute("BulletproofGlass"), "BulletproofGlassScrap" },
			{ MyStringHash.GetOrCompute("Computer"), "ComputerScrap" },
			{ MyStringHash.GetOrCompute("Construction"), "ConstructionScrap" },
			{ MyStringHash.GetOrCompute("Detector"), "DetectorScrap" },
			{ MyStringHash.GetOrCompute("Display"), "DisplayScrap" },
			{ MyStringHash.GetOrCompute("Explosives"), "ExplosivesScrap" },
			{ MyStringHash.GetOrCompute("Girder"), "GirderScrap" },
			{ MyStringHash.GetOrCompute("GravityGenerator"), "GravityGeneratorScrap" },
			{ MyStringHash.GetOrCompute("InteriorPlate"), "InteriorPlateScrap" },
			{ MyStringHash.GetOrCompute("LargeTube"), "LargeTubeScrap" },
			{ MyStringHash.GetOrCompute("Medical"), "MedicalScrap" },
			{ MyStringHash.GetOrCompute("MetalGrid"), "MetalGridScrap" },
			{ MyStringHash.GetOrCompute("Motor"), "MotorScrap" },
			{ MyStringHash.GetOrCompute("PowerCell"), "PowerCellScrap" },
			{ MyStringHash.GetOrCompute("RadioCommunication"), "RadioCommunicationScrap" },
			{ MyStringHash.GetOrCompute("Reactor"), "ReactorScrap" },
			{ MyStringHash.GetOrCompute("SmallTube"), "SmallTubeScrap" },
			{ MyStringHash.GetOrCompute("SolarCell"), "SolarCellScrap" },
			{ MyStringHash.GetOrCompute("SteelPlate"), "SteelPlateScrap" },
			{ MyStringHash.GetOrCompute("Superconductor"), "SuperconductorScrap" },
			{ MyStringHash.GetOrCompute("Thrust"), "ThrustScrap" },
			{ MyStringHash.GetOrCompute("ShieldComponent"), "FieldEmitterScrap" }
		};

		private static readonly List<MyStringHash> AwwScrapSubTypeIds = new List<MyStringHash>
		{
			MyStringHash.GetOrCompute("BulletproofGlassToIngot"),
			MyStringHash.GetOrCompute("ComputerToIngot"),
			MyStringHash.GetOrCompute("ConstructionToIngot"),
			MyStringHash.GetOrCompute("DetectorToIngot"),
			MyStringHash.GetOrCompute("DisplayToIngot"),
			MyStringHash.GetOrCompute("ExplosivesToIngot"),
			MyStringHash.GetOrCompute("GirderToIngot"),
			MyStringHash.GetOrCompute("GravityGeneratorToIngot"),
			MyStringHash.GetOrCompute("InteriorPlateToIngot"),
			MyStringHash.GetOrCompute("LargeTubeToIngot"),
			MyStringHash.GetOrCompute("MedicalToIngot"),
			MyStringHash.GetOrCompute("MetalGridToIngot"),
			MyStringHash.GetOrCompute("MotorToIngot"),
			MyStringHash.GetOrCompute("PowerCellToIngot"),
			MyStringHash.GetOrCompute("RadioCommunicationToIngot"),
			MyStringHash.GetOrCompute("ReactorToIngot"),
			MyStringHash.GetOrCompute("SmallTubeToIngot"),
			MyStringHash.GetOrCompute("SolarCellToIngot"),
			MyStringHash.GetOrCompute("SteelPlateToIngot"),
			MyStringHash.GetOrCompute("SuperconductorToIngot"),
			MyStringHash.GetOrCompute("ThrustToIngot")
		};

		private static void ScrubCubes()
		{
			// This loop accounts for World Settings for the Assembler Efficiency Modifier (x1, x3, x10)
			foreach (MyBlueprintDefinitionBase myBlueprintDefinitionBase in AwwScrapSubTypeIds.Select(
				subtype => MyDefinitionManager.Static.GetBlueprintDefinition(
					new MyDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), subtype))))
			{
				for (int index = 0; index < myBlueprintDefinitionBase.Results.Length; index++)
				{	// MyFixedPoint can't do /= operations, so have to do a work around
					float f = (float) myBlueprintDefinitionBase.Results[index].Amount;
					f /= MyAPIGateway.Session.SessionSettings.AssemblerEfficiencyMultiplier;
					myBlueprintDefinitionBase.Results[index].Amount = (MyFixedPoint) f;
				}
			}
			
			MyPhysicalItemDefinition scrapDef = MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Scrap"));
			
			foreach (MyCubeBlockDefinition myCubeBlockDefinition in MyDefinitionManager.Static.GetAllDefinitions().Select(myDefinitionBase => myDefinitionBase as MyCubeBlockDefinition).Where(myCubeBlockDefinition => myCubeBlockDefinition?.Components != null))
			{
				foreach (MyCubeBlockDefinition.Component component in myCubeBlockDefinition.Components)
				{
					GeneralLog.WriteToLog("ScrubCubes", $"{component.Definition.Id.SubtypeId}");

					if (!component.Definition.Public)
						continue;

					string subtypeName;
					if (ComponentDictionary.TryGetValue(component.Definition.Id.SubtypeId, out subtypeName))
					{
						component.DeconstructItem = MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Ore), subtypeName));
						continue;
					}
					component.DeconstructItem = scrapDef;
				}
			}
		}
	}
}