using System;
using System.Collections.Generic;
using NLog;
using Torch;
using Torch.API;
using Sandbox.ModAPI;

using System.Net;
using System.Collections.Specialized;
using System.Text;
using ProtoBuf;
using System.Net.Http;
using System.IO;
using VRage.FileSystem;
using Sandbox.Game.World;
using Newtonsoft.Json;


namespace DSCPlugin
{
    public class DSCPlugin : TorchPluginBase
    {


        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private DSC_Storage_Factions Storage;
        private DSC_Config_TechTree Techtree;

        private static readonly HttpClient RemoteClient = new HttpClient();
        private int tickTimer = 0;

        BinaryReader ReadStorage(string file, Type callingType)
        {
            if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                throw new FileNotFoundException();
            }
            Stream stream = MyFileSystem.OpenRead(Path.Combine("C:\\Servers\\se_epsilon_9\\Instance\\Saves\\Epsilon 9\\Storage\\2033950117.sbm_DSC", file));
            if (stream != null)
            {
                return new BinaryReader(stream);
            }
            throw new FileNotFoundException();
        }

        TextReader TextReader(string file, Type callingType)
        {
            if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                throw new FileNotFoundException();
            }
            Stream stream = MyFileSystem.OpenRead(Path.Combine("C:\\Servers\\se_epsilon_9\\Instance\\Saves\\Epsilon 9\\Storage\\2033950117.sbm_DSC", file));
            if (stream != null)
            {
                return new StreamReader(stream);
            }
            throw new FileNotFoundException();
        }


        /// <inheritdoc />

        public override void Init(ITorchBase torch)
        {

            base.Init(torch);

        }

        public override void Update()
        {

            if (tickTimer % 1200 == 0)
            {

                
                Log.Info("Current directory=>" + Directory.GetCurrentDirectory());

                // Faction Data
                try
                {
                    string curFile = @"C:\Servers\se_epsilon_9\Instance\Saves\Epsilon 9\Storage\2033950117.sbm_DSC\DSC_Storage_Factions";
                    if (File.Exists(curFile))
                    {
                        var reader = ReadStorage("DSC_Storage_Factions", typeof(DSC_Storage_Factions));
                        Storage = MyAPIGateway.Utilities.SerializeFromBinary<DSC_Storage_Factions>(reader.ReadBytes((int)reader.BaseStream.Length));
                        reader.Dispose();

                        // Send data to website

                        string output = JsonConvert.SerializeObject(Storage);
                        SendDataAsync("faction", output);
                    }
                }
                catch (Exception e)
                {
                    Log.Info("Update failed with exception=>" + e.ToString());
                }


                // Techtree
                try
                {
                    string curFile = @"C:\Servers\se_epsilon_9\Instance\Saves\Epsilon 9\Storage\2033950117.sbm_DSC\DSC_Config_TechTree";
                    if (File.Exists(curFile))
                    {
                        System.IO.TextReader reader = TextReader("DSC_Config_TechTree", typeof(DSC_Config_TechTree));
                        var xmlData = reader.ReadToEnd();
                        Techtree = MyAPIGateway.Utilities.SerializeFromXML<DSC_Config_TechTree>(xmlData);
                        reader.Dispose();

                        // Send data to website

                        string output = JsonConvert.SerializeObject(Techtree);
                        SendDataAsync("techtree", output);
                    }
                }
                catch (Exception e)
                {
                    Log.Info("Update failed with exception=>" + e.ToString());
                }



                Log.Info("Update");


            }


            tickTimer++;
        }

        #region debug data


        public async System.Threading.Tasks.Task SendDataAsync(string tag, string Information)
        {
            // Read in data from savegame



            var values = new Dictionary<string, string>
                {
                { "tag", tag },
                { "data",  Information}
                };

            var content = new FormUrlEncodedContent(values);

            var response = await RemoteClient.PostAsync("https://deep-space-combat.de/lib/remote.pl", content);

            var responseString = await response.Content.ReadAsStringAsync();

            Log.Info("Response" + responseString);
        }

        #endregion
    }

    [ProtoContract]
    [Serializable]
    public class DSC_Storage_Factions
    {
        /*
         * Faction data 
         */
        [ProtoMember(1)]
        public Dictionary<long, string> PlayerFactions = new Dictionary<long, string>(); // factionTag - factionID
        [ProtoMember(2)]
        public Dictionary<long, string> NPCFactions = new Dictionary<long, string>(); // factionTag - factionID
        [ProtoMember(3)]
        public Dictionary<long, List<long>> FactionPlayers = new Dictionary<long, List<long>>(); // factionID - LIST->playerID
        [ProtoMember(4)]
        public Dictionary<long, long> PlayersToFaction = new Dictionary<long, long>(); // playerID - factionID
        [ProtoMember(5)]
        public Dictionary<long, List<string>> FactionTechs = new Dictionary<long, List<string>>(); // factionID - TechLevelName
        [ProtoMember(6)]
        public Dictionary<long, List<string>> FactionBlocks = new Dictionary<long, List<string>>(); // factionID - LIST->BlockTechName
        [ProtoMember(7)]
        public Dictionary<long, List<long>> PlayersToPCU = new Dictionary<long, List<long>>(); // playerID - LIST->blockIDs
        [ProtoMember(8)]
        public Dictionary<long, int> PlayerDamage = new Dictionary<long, int>(); // playerID - TotalDamage
        [ProtoMember(9)]
        public Dictionary<long, int> FactionDamage = new Dictionary<long, int>(); // factionID - TotalDamage

        internal DSC_Storage_Factions Clone()
        {
            return new DSC_Storage_Factions
            {
                PlayerFactions = PlayerFactions,
                NPCFactions = NPCFactions,
                FactionPlayers = FactionPlayers,
                PlayersToFaction = PlayersToFaction,
                FactionTechs = FactionTechs,
                FactionBlocks = FactionBlocks,
                PlayerDamage = PlayerDamage,
                FactionDamage = FactionDamage
            };
        }
    }


    [ProtoContract]
    [Serializable]
    public class DSC_Config_TechTree
    {

        [ProtoMember(1)]
        public List<TechLevel> Levels = new List<TechLevel>();
        [ProtoMember(2)]
        public List<TechStation> Stations = new List<TechStation>();
        [ProtoMember(3)]
        public List<string> TechAreas = new List<string>();

        internal DSC_Config_TechTree Clone()
        {
            return new DSC_Config_TechTree
            {
                Levels = Levels,
                Stations = Stations,
            };
        }

        [ProtoContract]
        [Serializable]
        public class TechLevel
        {
            [ProtoMember(1)]
            public string TechLevelName;
            [ProtoMember(2)]
            public string DependsOn;
            [ProtoMember(3)]
            public int ResearchPoints;
            [ProtoMember(4)]
            public List<string> Blocks = new List<string>();
            [ProtoMember(5)]
            public string TechArea;

            public TechLevel() { }

            public TechLevel(string techLevelName, string dependsOn, int researchPoints, string techArea, List<string> blocks)
            {
                TechLevelName = techLevelName;
                DependsOn = dependsOn;
                ResearchPoints = researchPoints;
                Blocks = blocks;
                TechArea = techArea;
            }
        }


        [ProtoContract]
        [Serializable]
        public class TechStation
        {
            [ProtoMember(1)]
            public string Name;
            [ProtoMember(5)]
            public List<string> TechAreas;

            public TechStation() { }

            public TechStation(string name, List<string> techAreas)
            {
                Name = name;
                TechAreas = techAreas;
            }
        }

    }
}
