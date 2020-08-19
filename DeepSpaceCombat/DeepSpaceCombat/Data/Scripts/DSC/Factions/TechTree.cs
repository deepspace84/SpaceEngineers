using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSC
{
    public class DSC_TechTree
    {
        
        public DSC_TechTree(){}

        public Dictionary<string, DSC_TechLevel> TechLevels { get; } = new Dictionary<string, DSC_TechLevel>();
        public DSC_Config_TechTree Config;

        public void Load()
        {

            // Load config xml
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_Config_TechTree", typeof(DSC_Config_TechTree)))
            {
                try
                {
                    System.IO.TextReader reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("DSC_Config_TechTree", typeof(DSC_Config_TechTree));
                    var xmlData = reader.ReadToEnd();
                    Config = MyAPIGateway.Utilities.SerializeFromXML<DSC_Config_TechTree>(xmlData);
                    reader.Dispose();
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DSC_Config_TechTree found and loaded");
                }
                catch (Exception e)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_Config_TechTree loading failed");
                }
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("No DSC_Config_TechTree found, create default");
                // Create default values
                Config = new DSC_Config_TechTree
                {
                    Levels = new List<DSC_Config_TechTree.TechLevel>(),
                };
            }

            try
            {
            TechLevels.Add("LBasic", new DSC_TechLevel("LBasic", "", 1, 0, new List<string>() {
                // Light Armor
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorBlock"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorSlope"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorCorner"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorCornerInv"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeHalfArmorBlock"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeHalfSlopeArmorBlock"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorRoundSlope"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCornerInv"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Base"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Tip"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Base"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Tip"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Base"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Tip"].blockDefId,


                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Assembler/BasicAssembler"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_SurvivalKit/SurvivalKitLarge"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_SolarPanel/LargeBlockSolarPanel"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Refinery/Blast Furnace"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CargoContainer/LargeBlockLockers"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CargoContainer/LargeBlockSmallContainer"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_InteriorLight/SmallLight"].blockDefId,
            }));

            TechLevels.Add("LA1", new DSC_TechLevel("LA1", "LBasic", 100, 0, new List<string>() {
                // Assemblers
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Assembler/LargeAssembler"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Assembler/LargeAssembler2x"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Assembler/LargeAssembler4x"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Assembler/LargeAssembler8x"].blockDefId,

                // Refineries
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Refinery/LargeRefinery"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Refinery/LargeRefinery2x"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Refinery/LargeRefinery4x"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_Refinery/LargeRefinery8x"].blockDefId,
            }));

                TechLevels.Add("LA2", new DSC_TechLevel("LA2", "LA1", 200, 0, new List<string>() {
                // Modules
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_UpgradeModule/LargeProductivityModule"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_UpgradeModule/LargeEffectivenessModule"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_UpgradeModule/LargeEnergyModule"].blockDefId,
            }));

            TechLevels.Add("SBasic", new DSC_TechLevel("SBasic", "", 1, 0, new List<string>() {
                // Light Armor
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorBlock"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCornerInv"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundSlope"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCorner"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCornerInv"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Base"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Tip"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Base"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Tip"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Base"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Tip"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/HeavyHalfArmorBlock"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CubeBlock/HeavyHalfSlopeArmorBlock"].blockDefId,

                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_SurvivalKit/SurvivalKit"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_SolarPanel/SmallBlockSolarPanel"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_CargoContainer/SmallBlockSmallContainer"].blockDefId,
                DeepSpaceCombat.Instance.Definitions.Blocks["MyObjectBuilder_InteriorLight/SmallBlockSmallLight"].blockDefId,
            }));
            
            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Error Loading Techtree");
            }

        }

        public void Save()
        {

            foreach(string name in TechLevels.Keys)
            {
                DSC_TechLevel level = TechLevels[name];

                Config.Levels.Add(new DSC_Config_TechTree.TechLevel(name, level.DependsOn, level.ResearchPoints, level.TechArea, new List<string>(level.Blocks)));
            }

            // Save Config
            var xmlData = MyAPIGateway.Utilities.SerializeToXML<DSC_Config_TechTree>(Config);
            System.IO.TextWriter writerConfig = MyAPIGateway.Utilities.WriteFileInWorldStorage("DSC_Config_TechTree", typeof(DSC_Config_TechTree));
            writerConfig.Write(xmlData);
            writerConfig.Flush();
            writerConfig.Close();
        }

    }


    public class DSC_TechLevel
    {

        public readonly string TechLevelName;
        public readonly string DependsOn;
        public readonly int ResearchPoints;
        public readonly List<string> Blocks = new List<string>();
        public readonly int TechArea;

        public DSC_TechLevel() { }

        public DSC_TechLevel(string techLevelName, string dependsOn, int researchPoints, int techArea, List<string> blocks)
        {
            TechLevelName = techLevelName;
            DependsOn = dependsOn;
            ResearchPoints = researchPoints;
            Blocks = blocks;
            TechArea = techArea;
        }
    }
}
