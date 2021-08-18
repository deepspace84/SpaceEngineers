using ProtoBuf;
using Sandbox.ModAPI;
using VRage.Utils;



namespace DeepSpace
{
    // An example packet extending another packet.
    // Note that it must be ProtoIncluded in PacketBase for it to work.
    [ProtoContract]
    public class PacketPlayerCfg : PacketBase
    {
        public PacketPlayerCfg() { } // Empty constructor required for deserialization

        // tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(1)]
        public COMStorage.PlayerConfig PlayerCfg;

        [ProtoMember(2)]
        public long PlayerId;

        public PacketPlayerCfg(COMStorage.PlayerConfig playerCfg, long playerId)
        {
            PlayerCfg = playerCfg;
            PlayerId = playerId;
        }

        public override bool Received()
        {
            COMChat.Instance.ClientLogger.WriteInfo("PacketPlayerCfg::Received called");
            if (COMChat.Instance.IsClientRegistered)
            {
                // Proceed player config
                COMChat.Instance.COMClient.PlayerConfig = PlayerCfg;
                COMChat.Instance.ClientLogger.WriteInfo("PacketPlayerCfg::Received Client config received and wrote to COMClient");
            }

            return false;
        }
    }
}