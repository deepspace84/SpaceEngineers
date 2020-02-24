using ProtoBuf;
using Sandbox.ModAPI;
using VRage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Input;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using VRage.ObjectBuilders;
using VRage.Collections;
using Sandbox.Game.SessionComponents;


namespace DSC
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

            // if(DeepSpaceCombat.Instance._isClientRegistered)

            if( Text == "testcommand"){
                if (MyAPIGateway.Session.IsServer)
                {
                    MyVisualScriptLogicProvider.SendChatMessage("Test command received on Session.IsServer");
                }
                if(MyAPIGateway.Utilities.IsDedicated)
                {
                    MyVisualScriptLogicProvider.SendChatMessage("Test command received on Utilities.IsDedicated");
                }
                if (MyAPIGateway.Session != null && MyAPIGateway.Session.Player != null)
                {
                    MyVisualScriptLogicProvider.SendChatMessage("Test command received on Player Side name=>"+MyAPIGateway.Session.Player.DisplayName);
                }
                

                return false;
            }

            return true; // relay packet to other clients (only works if server receives it)
        }
    }
}