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

namespace DSCPlugin
{
    public class DSCPlugin : TorchPluginBase
    {


        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private DSC_Storage_Factions Storage;

        /// <inheritdoc />

        public override void Init(ITorchBase torch)
        {

            base.Init(torch);

        }

        public override void Save()
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
                    MyAPIGateway.Utilities.InvokeOnGameThread(() => {

                        using (var wb = new WebClient())
                        {
                            var data = new NameValueCollection();
                            data["username"] = "myUser";
                            data["password"] = "myPassword";

                            var response = wb.UploadValues("", "POST", data);
                            string responseInString = Encoding.UTF8.GetString(response);
                        }
                    });

                }
                catch (Exception e)
                {

                }
            }
            else
            {
                    
            }


            Log.Info("Update");

        }

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
