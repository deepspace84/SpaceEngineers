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


namespace DeepSpace
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

            // Only proceed serverside
            if (COMChat.Instance.IsServerRegistered)
            {
                if (Text.TrimStart('#') == "") return false;

                string[] scommand = Text.TrimStart('#').Split(' ');
                if (null == scommand) { scommand = new string[1]; scommand[0] = Text.TrimStart('#'); }
                List<string> lcommand = new List<string>();
                foreach (string s in scommand)
                {
                    if ((null == s) || "".Equals(s))
                        continue;
                    lcommand.Add(s);
                }

                switch (lcommand[0])
                {
                    case "p_config":
                        COMChat.Instance.COMMaster.SendConfig(PlayerId);
                        break;
                    case "on":
                        COMChat.Instance.COMMaster.Switch(PlayerId, true);
                        break;
                    case "off":
                        COMChat.Instance.COMMaster.Switch(PlayerId, false);
                        break;
                    case "g":
                        // Switch channel to global
                        COMChat.Instance.COMMaster.SetMode(0, PlayerId);
                        break;
                    case "f":
                        // Switch channel to global
                        COMChat.Instance.COMMaster.SetMode(1, PlayerId);
                        break;
                    case "r": // Set radius
                        // Check for possible values
                        long radius = 0;
                        if (lcommand.Count == 2)
                        {
                            if (!long.TryParse(lcommand[1], out radius))
                            {
                                MyVisualScriptLogicProvider.SendChatMessage("Invalid radius! Example: #r 1000", "COMChat", PlayerId);
                                return false;
                            }
                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Provide radius! Example: #r 1000", "COMChat", PlayerId);
                            return false;
                        }

                        // Switch channel to radius
                        COMChat.Instance.COMMaster.SetRange(radius, PlayerId);
                        break;
                    case "w": // Wisper player
                        // Check for player
                        if (lcommand.Count >= 2)
                        {
                            if(lcommand.Count > 2)
                            {
                                lcommand = CombineCommand(lcommand);
                            }

                            COMChat.Instance.COMMaster.SetWisper(PlayerId, lcommand[1]);
                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Invalid playername! Example: #w Playername", "COMChat", PlayerId);
                            return false;
                        }
                        break;
                    case "c": // Activate channel
                        // Check for channel
                        if (lcommand.Count == 2)
                        {
                            COMChat.Instance.COMMaster.SetChannel(lcommand[1], PlayerId);
                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Invalid playername! Example: #w Playername", "COMChat", PlayerId);
                            return false;
                        }
                        break;
                    case "cc": // Create channel
                        if (lcommand.Count == 3)
                        {
                            COMChat.Instance.COMMaster.CreateChannel(lcommand[1], lcommand[2], PlayerId);
                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Invalid command! Example: #cc Channelname Password", "COMChat", PlayerId);
                            return false;
                        }
                        break;
                    case "rc": // Remove channel
                        if (lcommand.Count == 2)
                        {
                            long rchId = 0;

                            if (!long.TryParse(lcommand[1], out rchId))
                            {
                                MyVisualScriptLogicProvider.SendChatMessage("Invalid command! Example: #rc ChannelId", "COMChat", PlayerId);
                                return false;
                            }

                            COMChat.Instance.COMMaster.RemoveChannel(rchId, PlayerId);
                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Invalid command! Example: #rc ChannelId", "COMChat", PlayerId);
                            return false;
                        }
                        break;
                    case "jc": // Join channel
                        if (lcommand.Count == 3)
                        {
                            long jcId = 0;
                            if (!long.TryParse(lcommand[1], out jcId))
                            {
                                MyVisualScriptLogicProvider.SendChatMessage("Invalid command! Example: #jc ChannelId Password", "COMChat", PlayerId);
                                return false;
                            }

                            COMChat.Instance.COMMaster.JoinChannel(jcId, lcommand[2], PlayerId);
                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Invalid command! Example: #jc ChannelId Password", "COMChat", PlayerId);
                            return false;
                        }
                        break;
                    case "rp": // Remove player
                        if (lcommand.Count == 3)
                        {
                            long rpCHid = 0;
                            if (!long.TryParse(lcommand[1], out rpCHid))
                            {
                                MyVisualScriptLogicProvider.SendChatMessage("Invalid command! Example: #rp ChannelId Playername", "COMChat", PlayerId);
                                return false;
                            }

                            if (lcommand.Count > 2)
                            {
                                lcommand = CombineCommand(lcommand);
                            }

                            COMChat.Instance.COMMaster.RemovePlayer(rpCHid, lcommand[2], PlayerId);
                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Invalid command! Example: #rp ChannelId Playername", "COMChat", PlayerId);
                            return false;
                        }
                        break;
                }

            }
            return false;
        }

        private List<string> CombineCommand(List<string> inCommand)
        {
            List<string> outCommand = new List<string>();

            // Add command 1 and 2
            outCommand.Add(inCommand[0]);
            outCommand.Add(inCommand[1]);
            // Remove from incoming
            inCommand.Remove(outCommand[0]);
            inCommand.Remove(outCommand[1]);

            string lcommand2 = "";
            // Loop through remaining elements
            foreach (string el in inCommand)
            {
                lcommand2 += " " + el;
            }
            // Add to index 2
            outCommand.Add(lcommand2.Trim());

            return outCommand;
        }
    }
}