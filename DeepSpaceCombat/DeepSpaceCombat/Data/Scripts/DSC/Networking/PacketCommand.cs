﻿using ProtoBuf;
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