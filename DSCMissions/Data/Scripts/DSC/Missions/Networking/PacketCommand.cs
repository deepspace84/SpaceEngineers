using ProtoBuf;
using Sandbox.ModAPI;



namespace DSC.Missions
{
    // An example packet extending another packet.
    // Note that it must be ProtoIncluded in PacketBase for it to work.
    [ProtoContract]
    public class PacketCommand : PacketBase
    {
        public PacketCommand() { } // Empty constructor required for deserialization

        // tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(1)]
        public string Text;

        [ProtoMember(2)]
        public long PlayerId;

        [ProtoMember(3)]
        public bool IsAdmin;

        public PacketCommand(string text, long playerId, bool isAdmin)
        {
            Text = text;
            PlayerId = playerId;
            IsAdmin = isAdmin;
        }

        public override bool Received()
        {
            if (MyAPIGateway.Session.IsServer)
            {
                DeepSpaceCombat.Instance.CMDHandler.HandleCommand(Text, PlayerId, IsAdmin);
            }

            return false;
        }
    }
}