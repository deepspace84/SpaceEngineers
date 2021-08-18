using ProtoBuf;
using Sandbox.ModAPI;
using VRage.Utils;



namespace DeepSpace
{
    // An example packet extending another packet.
    // Note that it must be ProtoIncluded in PacketBase for it to work.
    [ProtoContract]
    public class PacketChatData : PacketBase
    {
        public PacketChatData() { } // Empty constructor required for deserialization

        // tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(1)]
        public ChatData Data;

        [ProtoMember(2)]
        public long PlayerId;

        public PacketChatData(ChatData data, long playerId)
        {
            Data = data;
            PlayerId = playerId;
        }

        public override bool Received()
        {
            if (COMChat.Instance.isDebug) COMChat.Instance.ClientLogger.WriteInfo("PacketChatData:Received called");
            if (COMChat.Instance.IsClientRegistered)
            {
                COMChat.Instance.COMClient.ChatConfig = Data;
                if (COMChat.Instance.isDebug) COMChat.Instance.ClientLogger.WriteInfo("PacketChatData:Received Chat data set");

                // Update Status
                COMChat.Instance.HudModule.SetStatus(Data.Active);

                // Update hud type
                COMChat.Instance.HudModule.SetInfo();

            }

            return false;
        }
    }
}