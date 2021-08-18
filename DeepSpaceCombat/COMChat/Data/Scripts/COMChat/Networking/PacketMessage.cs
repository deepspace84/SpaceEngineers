using ProtoBuf;
using Sandbox.ModAPI;
using VRage.Utils;



namespace DeepSpace
{
    // An example packet extending another packet.
    // Note that it must be ProtoIncluded in PacketBase for it to work.
    [ProtoContract]
    public class PacketMessage : PacketBase
    {
        public PacketMessage() { } // Empty constructor required for deserialization

        // tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(1)]
        public string Text;

        [ProtoMember(2)]
        public long PlayerId;

        public PacketMessage(string text, long playerId)
        {
            Text = text;
            PlayerId = playerId;
        }

        public override bool Received()
        {
            if (MyAPIGateway.Session.IsServer)
            {
                // Check mode
                COMChat.Instance.COMMaster.IncomingChat(PlayerId, Text);
            }

            return false;
        }
    }
}