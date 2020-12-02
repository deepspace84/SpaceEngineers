using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.ModAPI;
using VRage.Game;



namespace DSC
{
    // An example packet extending another packet.
    // Note that it must be ProtoIncluded in PacketBase for it to work.
    [ProtoContract]
    public class PacketScreen : PacketBase
    {
        public PacketScreen() { } // Empty constructor required for deserialization

        // tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(1)]
        public long PlayerId;

        [ProtoMember(2)]
        public string Title;

        [ProtoMember(3)]
        public string Message;

        [ProtoMember(4)]
        public string ButtonText;

        public PacketScreen(long playerId, string title, string message, string buttonText="Close")
        {
            PlayerId = playerId;
            Title = title;
            Message = message;
            ButtonText = buttonText;
        }

        public override bool Received()
        {
            if (DeepSpaceCombat.Instance._isClientRegistered)
            {
                MyAPIGateway.Utilities.ShowMissionScreen(Title, "", "", Message.Replace("|", "\n\r"), null, "Close");
            }

            return false;
        }
    }
}