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

        private static readonly HttpClient RemoteClient = new HttpClient();
        private int tickTimer = 0;


        BinaryReader ReadStorage(string file, Type callingType)
        {
            if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                throw new FileNotFoundException();
            }
            Stream stream = MyFileSystem.OpenRead(Path.Combine("C:\\Servers\\se_server_e8_alpha\\Instance\\Saves\\Epsilon 8 Alpha\\Storage\\2033950117.sbm_DSC", file));
            if (stream != null)
            {
                return new BinaryReader(stream);
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

                // Check if file exists
                //if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_Storage_Factions", typeof(DSC_Storage_Factions)))
                //{
                try
                {
                    var reader = ReadStorage("DSC_Storage_Factions", typeof(DSC_Storage_Factions));
                    Storage = MyAPIGateway.Utilities.SerializeFromBinary<DSC_Storage_Factions>(reader.ReadBytes((int)reader.BaseStream.Length));
                    reader.Dispose();

                    // Send data to website

                    string output = JsonConvert.SerializeObject(Storage);
                    SendDataAsync(output);

                }
                catch (Exception e)
                {
                    Log.Info("Update failed with exception=>" + e.ToString());
                }
                /*}
                else
                {
                    SendDataAsync("failed");
                }*/


                Log.Info("Update");


            }


            tickTimer++;
        }

        #region debug data


        public async System.Threading.Tasks.Task SendDataAsync(string Information)
        {


            // Read in data from savegame



            var values = new Dictionary<string, string>
                {
                { "login", "1" },
                { "thing2", "world" },
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
}
