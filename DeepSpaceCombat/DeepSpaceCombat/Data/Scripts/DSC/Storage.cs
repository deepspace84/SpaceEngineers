﻿using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace DSC
{

    /*
     * MAIN Faction Storage
     * 
     */

    [ProtoContract]
    [Serializable]
    public class DSC_Storage_Factions
    {
        /*
         * Faction data 
         */
        [ProtoMember(1)]
        public Dictionary<long, string> PlayerFactions = new Dictionary<long, string>(); // factionId - factionTag
        [ProtoMember(2)]
        public Dictionary<long, string> NPCFactions = new Dictionary<long, string>(); // factionId - factionTag
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
        public Dictionary<long, ulong> PlayerDamage = new Dictionary<long, ulong>(); // playerID - TotalDamage
        [ProtoMember(9)]
        public Dictionary<long, ulong> FactionDamage = new Dictionary<long, ulong>(); // factionID - TotalDamage

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
                FactionDamage = FactionDamage,
            };
        }
    }




    /*
     * Trade Storage
     * 
     */


    [ProtoContract]
    [Serializable]
    public class DSC_Storage_Trade
    {

        [ProtoMember(1)]
        public Dictionary<long, List<Trade>> Trades = new Dictionary<long, List<Trade>>(); // factionId - "dateTimeString_amount"
        [ProtoMember(2)]
        public Dictionary<long, float> TradeMalus = new Dictionary<long, float>(); // factionID - TradeMalus

        internal DSC_Storage_Trade Clone()
        {
            return new DSC_Storage_Trade
            {
                Trades = Trades,
                TradeMalus = TradeMalus
            };
        }


        [ProtoContract]
        [Serializable]
        public class Trade
        {

            [ProtoMember(1)]
            public string ItemName;
            [ProtoMember(2)]
            public long Amount;
            [ProtoMember(3)]
            public long TotalPrice;
            [ProtoMember(4)]
            public int Utime;

            internal Trade(string itemName, int amount, long totalPrice, int utime)
            {
                ItemName = itemName;
                Amount = amount;
                TotalPrice = totalPrice;
                Utime = utime;
            }
        }

    }

    


    /*
     * Core Storage
     * 
     */
    [ProtoContract]
    [Serializable]
    public class DSC_Storage_Core
    {

        [ProtoMember(1)]
        public Dictionary<long, DateTime> Respawns = new Dictionary<long, DateTime>(); // playerId - lastSpawn


        internal DSC_Storage_Core Clone()
        {
            return new DSC_Storage_Core
            {
                Respawns = Respawns,

            };
        }

    }

    /*
     * SpawnMager Store
     */

    [ProtoContract]
    [Serializable]
    public class DSC_Storage_SpawnManager
    {

        [ProtoMember(1)]
        public Dictionary<ulong, DSC_SpawnShip> SpawnedData = new Dictionary<ulong, DSC_SpawnShip>(); // spawnId - SpawnShip
        [ProtoMember(2)]
        public ulong SpawnId; // SpawnID counter

        internal DSC_Storage_SpawnManager Clone()
        {
            return new DSC_Storage_SpawnManager
            {
                SpawnedData = SpawnedData,
                SpawnId = SpawnId

            };
        }

    }



    /*
     * Reference Store
     */

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
   














}
