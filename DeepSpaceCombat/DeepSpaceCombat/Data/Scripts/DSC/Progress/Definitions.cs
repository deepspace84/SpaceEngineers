using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using Sandbox.Game;
using Sandbox.Game.EntityComponents;


namespace DSC
{ 
    
    public class DSC_Definitions
    {

        public static Dictionary<string, DSC_BlockDef> Blocks = new Dictionary<string, DSC_BlockDef>() {
            
        };

    }

    public class DSC_BlockDef
    {
        public readonly string blockDefId; // DefinitionId as String
        public readonly string blockText; // Display Text
        public readonly string buildComponent; // Initial build component
        public readonly string extraComponent; // Extra component
        public readonly int extraCompCount; // Component count

        public DSC_BlockDef() { }

        public DSC_BlockDef(string BlockDefId, string BlockText, string BuildComponent, string ExtraComponent, int ExtraCompCount){
            blockDefId = BlockDefId;
            blockText = BlockText;
            buildComponent = BuildComponent;
            extraComponent = ExtraComponent;
            extraCompCount = ExtraCompCount;
        }
    }

    public class DSC_ComponentDef
    {
        public static string Construction = MyVisualScriptLogicProvider.GetDefinitionId("Component", "Construction").ToString();
        public static string InteriorPlate = MyVisualScriptLogicProvider.GetDefinitionId("Component", "InteriorPlate").ToString();
        public static string SteelPlate = MyVisualScriptLogicProvider.GetDefinitionId("Component", "SteelPlate").ToString();
        public static string Girder = MyVisualScriptLogicProvider.GetDefinitionId("Component", "Girder").ToString();
    }
}
