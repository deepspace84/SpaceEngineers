using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace DSC
{
    [ProtoContract]
    [Serializable]
    public class DSC_Config_Main
    {

        [ProtoMember(1)]
        public List<string> NeutralFactions = new List<string>();
        [ProtoMember(2)]
        public string BlockKey;
        [ProtoMember(3)]
        public List<Respawn> Respawns = new List<Respawn>();


        [ProtoContract]
        [Serializable]
        public class Respawn
        {

            [ProtoMember(1)]
            public string ButtonName;
            [ProtoMember(2)]
            public string PrefabName;
            [ProtoMember(3)]
            public double SVector_X;
            [ProtoMember(4)]
            public double SVector_Y;
            [ProtoMember(5)]
            public double SVector_Z;
            [ProtoMember(6)]
            public double OVector_X;
            [ProtoMember(7)]
            public double OVector_Y;
            [ProtoMember(8)]
            public double OVector_Z;

            public Respawn() { }
            public Respawn(string buttonName, string prefabName, double sVector_X, double sVector_Y, double sVector_Z, double oVector_X, double oVector_Y, double oVector_Z)
            {
                ButtonName = buttonName;
                PrefabName = prefabName;
                SVector_X = sVector_X;
                SVector_Y = sVector_Y;
                SVector_Z = sVector_Z;
                OVector_X = oVector_X;
                OVector_Y = oVector_Y;
                OVector_Z = oVector_Z;
            }
        }

        internal DSC_Config_Main Clone()
        {
            return new DSC_Config_Main
            {
                NeutralFactions = NeutralFactions,
                BlockKey = BlockKey,
                Respawns = Respawns
            };
        }
    }


    [ProtoContract]
    [Serializable]
    public class DSC_Config_Trade
    {

        [ProtoMember(1)]
        public List<Station> Stations = new List<Station>();
        [ProtoMember(2)]
        public List<TradeType> Types = new List<TradeType>();
        [ProtoMember(3)]
        public int Treshold = 21600;
        [ProtoMember(4)]
        public float Malus = 0.2f;

        internal DSC_Config_Trade Clone()
        {
            return new DSC_Config_Trade
            {
                Stations = Stations,
                Types = Types,
                Treshold = Treshold,
                Malus = Malus,
            };
        }

        [ProtoContract]
        [Serializable]
        public class Station
        {

            [ProtoMember(1)]
            public string Name;
            [ProtoMember(2)]
            public string Type;

            public Station() { }
            public Station(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }

        [ProtoContract]
        [Serializable]
        public class TradeType
        {

            [ProtoMember(1)]
            public string Name;
            [ProtoMember(2)]
            public List<TradeItem> Items;

            public TradeType() { }
            public TradeType(string name, List<TradeItem> items)
            {
                Name = name;
                Items = items;
            }



            [ProtoContract]
            [Serializable]
            public class TradeItem
            {

                [ProtoMember(1)]
                public string ItemName;
                [ProtoMember(2)]
                public int Price;
                [ProtoMember(3)]
                public int MaxAmount;
                [ProtoMember(4)]
                public int Multiplier;

                public TradeItem() { }
                public TradeItem(string itemName, int price, int maxAmount, int multiplier)
                {
                    ItemName = itemName;
                    Price = price;
                    MaxAmount = maxAmount;
                    Multiplier = multiplier;
                }
            }

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
