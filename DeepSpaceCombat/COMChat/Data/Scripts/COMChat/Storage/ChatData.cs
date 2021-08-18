
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
    public class ChatData
    {

        [ProtoMember(1)]
        public bool Active;
        [ProtoMember(2)]
        public long Mode; // 0=>Global 1=>Faction 2=>Radius 3=>Channel 4=>Wisper
        [ProtoMember(3)]
        public long Channel;
        [ProtoMember(4)]
        public long Radius;
        [ProtoMember(5)]
        public long WPlayer;
        [ProtoMember(6)]
        public string WPlayerName;
        [ProtoMember(7)]
        public ulong WPlayerSteamId;
        [ProtoMember(8)]
        public ulong SteamId;
        [ProtoMember(9)]
        public Dictionary<long, string> AvailChannels;

        internal ChatData() { }

        internal ChatData(bool active, long mode, long channel, long radius, long wPlayer, string wPlayerName, ulong wPlayerSteamId, ulong steamId, Dictionary<long, string> availChannels)
        {
            Active = active;
            Mode = mode;
            Channel = channel;
            Radius = radius;
            WPlayer = wPlayer;
            WPlayerName = wPlayerName;
            SteamId = steamId;
            WPlayerSteamId = wPlayerSteamId;
            AvailChannels = availChannels;
        }
    }
}
 
