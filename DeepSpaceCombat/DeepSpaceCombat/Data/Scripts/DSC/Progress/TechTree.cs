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
            TechLevels.Add("L0", new DSC_TechLevel("L0", "", new List<string>() {
                DSC_BlockDefinitions.CubeBlock_LargeBlockArmorBlock[0],
            }));

            TechLevels.Add("L1", new DSC_TechLevel("L1", "L0", new List<string>() {
                DSC_BlockDefinitions.CubeBlock_LargeBlockArmorSlope[3],
                DSC_BlockDefinitions.CubeBlock_LargeBlockArmorCorner[3],
                DSC_BlockDefinitions.CubeBlock_LargeBlockArmorCornerInv[3],
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
