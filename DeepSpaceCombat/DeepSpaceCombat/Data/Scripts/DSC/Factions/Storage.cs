using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace DSC
{
    [ProtoContract]
    [Serializable]
    public class DSC_Storage_Factions
    {
        /*
         * Faction data 
         */
        [ProtoMember(1)]
        public Dictionary<string, long> FactionsPlayer = new Dictionary<string, long>(); // factionTag - factionID
        [ProtoMember(2)]
        public Dictionary<string, long> FactionsNPC = new Dictionary<string, long>(); // factionTag - factionID
        [ProtoMember(3)]
        public Dictionary<long, List<long>> FactionPlayers = new Dictionary<long, List<long>>(); // factionID - LIST->playerID
        [ProtoMember(4)]
        public Dictionary<long, List<string>> FactionTechs = new Dictionary<long, List<string>>(); // factionID - TechLevelName
        [ProtoMember(5)]
        public Dictionary<long, List<string>> FactionBlocks = new Dictionary<long, List<string>>(); // factionID - LIST->BlockTechName


        internal DSC_Storage_Factions Clone()
        {
            return new DSC_Storage_Factions
            {
                FactionsPlayer = FactionsPlayer,
                FactionsNPC = FactionsNPC,
                FactionPlayers = FactionPlayers,
                FactionTechs = FactionTechs,
                FactionBlocks = FactionBlocks,
            };
        }

    }
}
