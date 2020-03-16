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
        public Dictionary<string, int> FactionPCU = new Dictionary<string, int>(); // factionID -  PCU-Level


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
                PlayersToPCU = PlayersToPCU,
            };
        }

    }
    /*
    [ProtoContract]
    [Serializable]
    public class DSC_Storage_Reference
    {

        [ProtoMember(1)]
        public Dictionary<string, long> Blocks = new Dictionary<string, long>(); // blockName - blockId
        [ProtoMember(2)]
        public Dictionary<string, long> Grids = new Dictionary<string, long>(); // gridName - gridId



        internal DSC_Storage_Reference Clone()
        {
            return new DSC_Storage_Reference
            {
                Blocks = Blocks,
                Grids = Grids
            };
        }

    }
    */
}
