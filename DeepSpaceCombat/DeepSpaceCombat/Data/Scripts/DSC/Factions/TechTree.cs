using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSC
{
    public class DSC_TechTree
    {

        public DSC_TechTree() { }

        public Dictionary<string, DSC_Config_TechTree.TechLevel> TechLevels { get; } = new Dictionary<string, DSC_Config_TechTree.TechLevel>();
        public Dictionary<string, DSC_Config_TechTree.TechStation> TechStations { get; } = new Dictionary<string, DSC_Config_TechTree.TechStation>();
        public List<string> TechAreas { get; } = new List<string>();

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


            // Parse techtree
            foreach(DSC_Config_TechTree.TechLevel level in Config.Levels)
            {
                TechLevels.Add(level.TechLevelName, level);
            }
            foreach (DSC_Config_TechTree.TechStation station in Config.Stations)
            {
                TechStations.Add(station.Name, station);
            }
            TechAreas.AddList(Config.TechAreas);
        }

        public void Save()
        {
            // Save Config
            /*
            var xmlData = MyAPIGateway.Utilities.SerializeToXML<DSC_Config_TechTree>(Config);
            System.IO.TextWriter writerConfig = MyAPIGateway.Utilities.WriteFileInWorldStorage("DSC_Config_TechTree", typeof(DSC_Config_TechTree));
            writerConfig.Write(xmlData);
            writerConfig.Flush();
            writerConfig.Close();
            */
        }

    }
}


