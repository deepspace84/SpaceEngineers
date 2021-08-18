using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpace
{

    [ProtoContract]
    [Serializable]
    public class COMStorage
    {
        [ProtoMember(1)]
        public long nextChannelId;

        [ProtoMember(2)]
        public Dictionary<long, Channel> Channels = new Dictionary<long, Channel>();

        [ProtoMember(3)]
        public Dictionary<long, PlayerConfig> PlayerConfigs = new Dictionary<long, PlayerConfig>();

        internal COMStorage Clone()
        {
            return new COMStorage
            {
                nextChannelId = nextChannelId,
                Channels = Channels
            };
        }

        [ProtoContract]
        [Serializable]
        public class Channel
        {

            [ProtoMember(1)]
            public long Id;
            [ProtoMember(2)]
            public string Name;
            [ProtoMember(3)]
            public string Password;
            [ProtoMember(4)]
            public bool IsStatic;
            [ProtoMember(5)]
            public Dictionary<long, string> Admins;
            [ProtoMember(6)]
            public Dictionary<long, string> Members;
            [ProtoMember(7)]
            public long Creator;

            internal Channel() { }

            internal Channel(long id, string name, string password, bool isStatic, Dictionary<long, string> admins, Dictionary<long, string> members, long creator)
            {
                Id = id;
                Name = name;
                Password = password;
                IsStatic = isStatic;
                Admins = admins;
                Members = members;
                Creator = creator;
            }
        }


        [ProtoContract]
        [Serializable]
        public class PlayerConfig
        {

            [ProtoMember(1)]
            public long Id;
            [ProtoMember(2)]
            public string Name;
            [ProtoMember(3)]
            public bool ShowHints;
            [ProtoMember(4)]
            public bool ShowPlayers;


            internal PlayerConfig() { }

            internal PlayerConfig(long id, string name, bool showHints, bool showPlayers)
            {
                Id = id;
                Name = name;
                ShowHints = showHints;
                ShowPlayers = showPlayers;
            }
        }
    }
}
