using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceEngineersXMLParser
{
    class Utils
    {
        public static void Main(String[] args)
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filename = $"{path}\\Definitions.xml";
            string researchBlocks = $"{path}\\ResearchBlocks.sbc";
            string researchGroups = $"{path}\\ResearchGroups.sbc";


            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found");
                Console.Read();
                return;
            }

            try
            {
                string blocks = "";
                string groups = "";

                using (StreamReader sr = new StreamReader(filename))
                {
                    //< BlockId Type = "MyObjectBuilder_AirtightHangarDoor" Subtype = "" />
                    string line;
                    while (!String.IsNullOrEmpty(line =sr.ReadLine()))
                    {
                        string value = line.Replace("<BlockId", "").Replace("/>", "").Replace("Type=", "").Replace("Subtype", "").Trim();
                        string[] values = value.Split('=');
                        string type = values[0].Replace("\"", "").Trim();
                        string subtype = values[1].Replace("\"", "").Trim();

                        blocks += GetResearchBlock(type, subtype);
                        groups += GetResearchGroup(type, subtype);
                    }
                }

                WriteBlocks(researchBlocks, blocks);
                WriteGroups(researchGroups, groups);
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }

        private static void WriteBlocks(string fileName, string blocks)
        {
            using (StreamWriter swBlocks = new StreamWriter(fileName))
            {
                swBlocks.WriteLine("<?xml version=\"1.0\"?>");
                swBlocks.WriteLine("<Definitions xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                swBlocks.WriteLine("\t<ResearchBlocks>");
                swBlocks.WriteLine(blocks);
                swBlocks.WriteLine("\t</ResearchBlocks>");
                swBlocks.WriteLine("</Definitions>");
            }
        }

        private static void WriteGroups(string fileName, string groups)
        {
            using (StreamWriter swGroups = new StreamWriter(fileName))
            {
                swGroups.WriteLine("<?xml version=\"1.0\"?>");
                swGroups.WriteLine("<Definitions xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                swGroups.WriteLine("\t<ResearchGroups>");
                swGroups.WriteLine(groups);
                swGroups.WriteLine("\t</ResearchGroups>");
                swGroups.WriteLine("</Definitions>");
            }
        }


        private static string GetResearchBlock(string type, string subtype)
        {
            return
                    $"\t\t<ResearchBlock xsi:type=\"ResearchBlock\">\n" +
                    $"\t\t\t<Id Type=\"{type}\" Subtype=\"{subtype}\" />\n" +
                    $"\t\t\t<UnlockedByGroups>\n" +
                    $"\t\t\t\t<GroupSubtype>TODO</GroupSubtype>\n" +
                    $"\t\t\t</UnlockedByGroups>\n" +
                    $"\t\t</ResearchBlock>\n";
        }
        private static string GetResearchGroup(string type, string subtype)
        {
            return
                    $"\t\t<ResearchGroup xsi:type=\"ResearchGroup\">\n" +
                    $"\t\t\t<Id Type=\"MyObjectBuilder_ResearchGroupDefinition\" Subtype=\"TODO\" />\n" +
                    $"\t\t\t<Members>\n" +
                    $"\t\t\t\t<BlockId Type=\"{type}\" Subtype=\"{subtype}\" />\n" +
                    $"\t\t\t</Members>\n" +
                    $"\t\t</ResearchGroup>\n";
        }

    }
}
