﻿using ProtoBuf;
using Sandbox.ModAPI;
using VRage.Utils;

namespace DSC.NET
{
    // An example packet extending another packet.
    // Note that it must be ProtoIncluded in PacketBase for it to work.
    [ProtoContract]
    public class PacketSimple : PacketBase
    {
        public PacketSimple() { } // Empty constructor required for deserialization

        // tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(1)]
        public string Text;

        [ProtoMember(2)]
        public int Number;

        public PacketSimple(string text, int number)
        {
            Text = text;
            Number = number;
        }

        public override bool Received()
        {
            var msg = $"PacketSimple received: Text='{Text}'; Number={Number}";
            MyLog.Default.WriteLineAndConsole(msg);
            MyAPIGateway.Utilities.ShowNotification(msg, Number);

            return true; // relay packet to other clients (only works if server receives it)
        }
    }
}