﻿using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSC
{
    public class DSC_TechTree
    {
        public Dictionary<string, DSC_TechLevel> TechLevels = new Dictionary<string, DSC_TechLevel>();


        public DSC_TechTree(){}

        public void load()
        {
            try
            {
            TechLevels.Add("Basic", new DSC_TechLevel("LBasic", "", 1, new List<string>() {
                // Light Armor
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorBlock"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorSlope"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorCorner"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorCornerInv"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeHalfArmorBlock"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeHalfSlopeArmorBlock"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorRoundSlope"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCornerInv"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Base"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Tip"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Base"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Tip"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Base"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Tip"].blockDefId,


                DSC_Definitions.Blocks["MyObjectBuilder_Assembler/BasicAssembler"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_SurvivalKit/SurvivalKitLarge"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_SolarPanel/LargeBlockSolarPanel"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_Refinery/Blast Furnace"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CargoContainer/LargeBlockLockers"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CargoContainer/LargeBlockSmallContainer"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_InteriorLight/SmallLight"].blockDefId,
            }));

            TechLevels.Add("LA1", new DSC_TechLevel("LA1", "LBasic", 1, new List<string>() {
                // Assemblers
                DSC_Definitions.Blocks["MyObjectBuilder_Assembler/LargeAssembler"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_Assembler/LargeAssembler2x"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_Assembler/LargeAssembler4x"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_Assembler/LargeAssembler8x"].blockDefId,

                // Refineries
                DSC_Definitions.Blocks["MyObjectBuilder_Refinery/LargeRefinery"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_Refinery/LargeRefinery2x"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_Refinery/LargeRefinery4x"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_Refinery/LargeRefinery8x"].blockDefId,
            }));

                TechLevels.Add("LA2", new DSC_TechLevel("LA2", "LA1", 1, new List<string>() {
                // Modules
                DSC_Definitions.Blocks["MyObjectBuilder_UpgradeModule/LargeProductivityModule"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_UpgradeModule/LargeEffectivenessModule"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_UpgradeModule/LargeEnergyModule"].blockDefId,
            }));

            TechLevels.Add("SBasic", new DSC_TechLevel("SBasic", "", 1, new List<string>() {
                // Light Armor
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorBlock"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCornerInv"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundSlope"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCorner"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCornerInv"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Base"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Tip"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Base"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Tip"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Base"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Tip"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/HeavyHalfArmorBlock"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CubeBlock/HeavyHalfSlopeArmorBlock"].blockDefId,

                DSC_Definitions.Blocks["MyObjectBuilder_SurvivalKit/SurvivalKit"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_SolarPanel/SmallBlockSolarPanel"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_CargoContainer/SmallBlockSmallContainer"].blockDefId,
                DSC_Definitions.Blocks["MyObjectBuilder_InteriorLight/SmallBlockSmallLight"].blockDefId,
            }));
            
            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Error Loading Techtree");
            }


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