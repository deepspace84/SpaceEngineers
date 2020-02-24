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
    public class PackagePlayerLanguage : PacketBase
    {
        public PackagePlayerLanguage() { } // Empty constructor required for deserialization

        // tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(1)]
        public long User;

        [ProtoMember(2)]
        public byte Lang;

        public PackagePlayerLanguage(long user, byte lang)
        {
            User = user;
            Lang = lang;
        }

        public override bool Received()
        {
            DeepSpaceCombat.Instance.PlayerLanguages.Add(User, Lang);

            return false; // relay packet to other clients (only works if server receives it)
        }
    }
}