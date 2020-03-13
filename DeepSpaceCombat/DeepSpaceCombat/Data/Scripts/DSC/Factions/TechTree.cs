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
            TechLevels.Add("L0", new DSC_TechLevel("L0", "", 0, new List<string>() {
                DSC_Definitions.Blocks["CubeBlock_LargeBlockArmorBlock"].blockDefId,
            }));

            TechLevels.Add("L1", new DSC_TechLevel("L1", "L0", 5000, new List<string>() {
                DSC_Definitions.Blocks["CubeBlock_LargeBlockArmorSlope"].blockDefId,
                DSC_Definitions.Blocks["CubeBlock_LargeBlockArmorCorner"].blockDefId,
                DSC_Definitions.Blocks["CubeBlock_LargeBlockArmorCornerInv"].blockDefId,
            }));
        }

    }


    public class DSC_TechLevel
    {

        public readonly string TechLevelName;
        public readonly string DependsOn;
        public readonly int SpaceCredits;
        public readonly List<string> Blocks = new List<string>();

        public DSC_TechLevel() { }

        public DSC_TechLevel(string techLevelName, string dependsOn, int spaceCredits, List<string> blocks)
        {
            TechLevelName = techLevelName;
            DependsOn = dependsOn;
            SpaceCredits = spaceCredits;
            Blocks = blocks;
        }
    }
}
