using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace DSC
{
    
    [ProtoContract]
    [Serializable]
    public class DSC_Config_Trade
    {

        [ProtoMember(1)]
        public List<Station> Stations = new List<Station>();
        [ProtoMember(2)]
        public List<TradeType> Types = new List<TradeType>();

        internal DSC_Config_Trade Clone()
        {
            return new DSC_Config_Trade
            {
                Stations = Stations,
                Types = Types,
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
                [ProtoMember(2)]
                public int MaxAmount;

                public TradeItem() { }
                public TradeItem(string itemName, int price, int maxAmount)
                {
                    ItemName = itemName;
                    Price = price;
                    MaxAmount = maxAmount;
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

        internal DSC_Config_TechTree Clone()
        {
            return new DSC_Config_TechTree
            {
                Levels = Levels,
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
            public int TechArea;

            public TechLevel() { }

            public TechLevel(string techLevelName, string dependsOn, int researchPoints, int techArea, List<string> blocks)
            {
                TechLevelName = techLevelName;
                DependsOn = dependsOn;
                ResearchPoints = researchPoints;
                Blocks = blocks;
                TechArea = techArea;
            }
        }

    }








    }
