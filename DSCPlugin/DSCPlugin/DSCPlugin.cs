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
//using Newtonsoft.Json;


namespace DSCPlugin
{
    public class DSCPlugin : TorchPluginBase
    {


        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private DSC_Storage_Factions Storage;

        private static readonly HttpClient RemoteClient = new HttpClient();
        private int tickTimer = 0;

        /// <inheritdoc />

        public override void Init(ITorchBase torch)
        {

            base.Init(torch);

        }

        public override void Update()
        {

            if(tickTimer % 1200 == 0)
            {



                // Check if file exists
                if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_Storage_Factions", typeof(DSC_Storage_Factions)))
                {
                    try
                    {
                        var reader = MyAPIGateway.Utilities.ReadBinaryFileInWorldStorage("DSC_Storage_Factions", typeof(DSC_Storage_Factions));
                        Storage = MyAPIGateway.Utilities.SerializeFromBinary<DSC_Storage_Factions>(reader.ReadBytes((int)reader.BaseStream.Length));
                        reader.Dispose();

                        // Send data to website
                        
                        SendDataAsync(Storage.ToString());

                    }
                    catch (Exception e)
                    {

                    }
                }
                else
                {
                    SendDataAsync("failed");
                }

                
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

            Log.Info("Response"+responseString);
            
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
            };
        }

    }
}
