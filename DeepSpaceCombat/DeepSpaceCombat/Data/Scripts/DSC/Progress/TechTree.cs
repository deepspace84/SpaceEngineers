using System;
using System.Collections.Generic;
using System.Text;

namespace DSC
{
    public class DSC_TechTree
    {
        public readonly Dictionary<string, DSC_TechLevel> TechLevels = new Dictionary<string, DSC_TechLevel>();


        public DSC_TechTree()
        {
            TechLevels.Add("L0",new DSC_TechLevel("L0", "", new List<string>() {
                DSC_BlockDefinitions.lightArmor,
                DSC_BlockDefinitions.lightArmorCorner,
                DSC_BlockDefinitions.lightArmorCorner2,
                DSC_BlockDefinitions.lightArmorRoundSlope,
            }));
        }

    }


    public class DSC_TechLevel
    {

        public readonly string TechLevelName;
        public readonly string DependsOn;
        public readonly List<string> Blocks = new List<string>();

        public DSC_TechLevel() { }

        public DSC_TechLevel(string techLevelName, string dependsOn, List<string> blocks)
        {
            TechLevelName = techLevelName;
            DependsOn = dependsOn;
            Blocks = blocks;
        }
    }
}
