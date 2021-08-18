using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace DeepSpace
{
    public class Communicator
    {

        public Communicator() { }

        public COMStorage Storage;


        // Cache data
        public Dictionary<long, List<long>> ChannelPlayers = new Dictionary<long, List<long>>();
        public Dictionary<long, ChatData> ActivePlayers = new Dictionary<long, ChatData>();



        /*
         * Load all data from savegame and register handlers 
         */
        public void Init()
        {
            // Check if file exists
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("COMStorage", typeof(COMStorage)))
            {
                try
                {
                    var reader = MyAPIGateway.Utilities.ReadBinaryFileInWorldStorage("COMStorage", typeof(COMStorage));
                    Storage = MyAPIGateway.Utilities.SerializeFromBinary<COMStorage>(reader.ReadBytes((int)reader.BaseStream.Length));
                    reader.Dispose();
                    COMChat.Instance.ServerLogger.WriteInfo("COMStorage found and loaded");
                }
                catch (Exception e)
                {
                    COMChat.Instance.ServerLogger.WriteException(e, "COMStorage loading failed");
                }
            }
            else
            {
                COMChat.Instance.ServerLogger.WriteInfo("No COMStorage found, create default");
                // Create default values
                Storage = new COMStorage
                {
                    nextChannelId = 1,
                    Channels = new Dictionary<long, COMStorage.Channel>()
                };
            }

            // Check for players, for example singleplay instance
            List<IMyPlayer> players = new List<IMyPlayer>();
            MyAPIGateway.Players.GetPlayers(players);
            foreach (IMyPlayer player in players)
            {
                PlayerConnected(player.IdentityId);
            }

            // Register player connect event
            MyVisualScriptLogicProvider.PlayerConnected += PlayerConnected;


        }

        /*
         * Save all data to savegame 
         */
        public void Save()
        {
            // Save Storage
            byte[] serialized = MyAPIGateway.Utilities.SerializeToBinary<COMStorage>(Storage);
            System.IO.BinaryWriter writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage("COMStorage", typeof(COMStorage));
            writer.Write(serialized);
            writer.Flush();
            writer.Dispose();
        }

        /*
         * Unregister handlers 
         */
        public void Unload()
        {

            MyVisualScriptLogicProvider.PlayerConnected -= PlayerConnected;
        }


        /*
         * Command functions
         */
        public void SetMode(long mode, long playerId)
        {
            ActivePlayers[playerId].Mode = mode;

            // Send data to player
            COMChat.Instance.Networking.SendToPlayer(new PacketChatData(ActivePlayers[playerId], playerId), ActivePlayers[playerId].SteamId);
        }

        public void SetRange(long radius, long playerId)
        {
            // Set chat data
            ActivePlayers[playerId].Mode = 2;
            ActivePlayers[playerId].Radius = radius;

            // Send data to player
            COMChat.Instance.Networking.SendToPlayer(new PacketChatData(ActivePlayers[playerId], playerId), ActivePlayers[playerId].SteamId);
        }

        public void SetChannel(string channel, long playerId)
        {
            long chId = 0;
            // Try to parse channel as id
            if (!long.TryParse(channel, out chId))
            {
                // Not an id so try to find it by name
                foreach (KeyValuePair<long, string> entry in ActivePlayers[playerId].AvailChannels)
                {
                    if (entry.Value.Contains(channel))
                    {
                        chId = entry.Key;
                        break;
                    }
                }

            }

            if(chId > 0)
            {
                // Channel found, so update config
                ActivePlayers[playerId].Mode = 3;
                ActivePlayers[playerId].Channel = chId;

                // Send data to player
                COMChat.Instance.Networking.SendToPlayer(new PacketChatData(ActivePlayers[playerId], playerId), ActivePlayers[playerId].SteamId);
            }
            else
            {
                MyVisualScriptLogicProvider.SendChatMessage("No channel found! Example: #c Channelname OR #c ChannelId", "COMChat", playerId);
            }
        }

        public void Switch(long playerId, bool status)
        {
            // Get steam id for caching
            if (status)
            {
                ulong steamId = MyAPIGateway.Players.TryGetSteamId(playerId);
                if (steamId != 0 && Storage.PlayerConfigs.ContainsKey(playerId))
                {
                    // Add player to config if not allready added
                    if (ActivePlayers.ContainsKey(playerId))
                    {
                        ActivePlayers[playerId].Active = true;
                    }else ActivePlayers.Add(playerId, new ChatData(true, 0, 0, 0, 0, "", 0, steamId, new Dictionary<long, string>()));

                    // Check for available Channels TODO

                    // Send data to player
                    COMChat.Instance.Networking.SendToPlayer(new PacketChatData(ActivePlayers[playerId], playerId), steamId);

                    if (COMChat.Instance.isDebug) COMChat.Instance.ServerLogger.WriteInfo("Communicator::Switch Added player =>" + playerId.ToString());
                }
                else
                {
                    if (COMChat.Instance.isDebug) COMChat.Instance.ServerLogger.WriteInfo("Communicator::Switch Could not find player steam id or config");
                }
            }
            else
            {
                // Deactivate in player config if available
                if (ActivePlayers.ContainsKey(playerId))
                {
                    ActivePlayers[playerId].Active = false;

                    // Send config to player
                    COMChat.Instance.Networking.SendToPlayer(new PacketChatData(ActivePlayers[playerId], playerId), ActivePlayers[playerId].SteamId);
                    if (COMChat.Instance.isDebug) COMChat.Instance.ServerLogger.WriteInfo("Communicator::Switch Removed player =>" + playerId.ToString());
                }
            }
        }

        public void SetWisper(long playerId, string name)
        {
            // Try to find target
            List<IMyPlayer> targetPlayer = new List<IMyPlayer>();
            List<IMyPlayer> players = new List<IMyPlayer>();
            MyAPIGateway.Players.GetPlayers(players);
            foreach (IMyPlayer player in players)
            {
                if (player.DisplayName.Contains(name))
                {
                    targetPlayer.Add(player);
                    break;
                }
            }

            // Check for zero result
            if(targetPlayer.Count == 0)
            {
                MyVisualScriptLogicProvider.SendChatMessage("Could not find a player with that name!", "COMChat", playerId);
                return;
            }

            // Check if we found more than one
            if (targetPlayer.Count > 1)
            {
                MyVisualScriptLogicProvider.SendChatMessage("More than one player found:", "COMChat", playerId);
                foreach(IMyPlayer player in targetPlayer)
                {
                    MyVisualScriptLogicProvider.SendChatMessage(player.DisplayName, "COMChat", playerId);
                }
                return;
            }

            // Set this player as target
            COMChat.Instance.COMMaster.ActivePlayers[playerId].Mode = 4;
            COMChat.Instance.COMMaster.ActivePlayers[playerId].WPlayer = targetPlayer[1].IdentityId;
            COMChat.Instance.COMMaster.ActivePlayers[playerId].WPlayerName = targetPlayer[1].DisplayName;

            // Send data to player
            COMChat.Instance.Networking.SendToPlayer(new PacketChatData(ActivePlayers[playerId], playerId), ActivePlayers[playerId].SteamId);
        }

        public void CreateChannel(string name, string password, long playerId)
        {
            // Check name length
            if(name.Length < 3 || name.Length > 10)
            {
                MyVisualScriptLogicProvider.SendChatMessage("Channel name length must be between 3 and 10 characters", "COMChat", playerId);
                return;
            }

            // Check password length
            if (password.Length < 3 || password.Length > 10)
            {
                MyVisualScriptLogicProvider.SendChatMessage("Password length must be between 3 and 10 characters", "COMChat", playerId);
                return;
            }

            long chCount = 0;
            bool nameCheck = false;
            foreach (COMStorage.Channel channel in Storage.Channels.Values)
            {
                // Check for existing name
                if (channel.Name.Equals(name))
                {
                    nameCheck = true;
                }

                if(channel.Creator == playerId)
                {
                    chCount++;
                }
            }

            if (chCount == Mod_Config.MaxChannelsPerPlayer)
            {
                MyVisualScriptLogicProvider.SendChatMessage("You have the maximum of channels: "+Mod_Config.MaxChannelsPerPlayer.ToString(), "COMChat", playerId);
                return;
            }

            if (nameCheck)
            {
                MyVisualScriptLogicProvider.SendChatMessage("Channel name allready exists", "COMChat", playerId);
                return;
            }

            // Add channel
            Storage.nextChannelId++;

            Storage.Channels.Add(Storage.nextChannelId, new COMStorage.Channel(Storage.nextChannelId, name, password, true, new Dictionary<long, string>() { { playerId, MyVisualScriptLogicProvider.GetPlayersName(playerId) } }, new Dictionary<long, string>(), playerId));

            // Add to data of that player
            ActivePlayers[playerId].AvailChannels.Add(Storage.nextChannelId, Storage.Channels[Storage.nextChannelId].Name);

            // Send new data to player
            COMChat.Instance.Networking.SendToPlayer(new PacketChatData(ActivePlayers[playerId], playerId), ActivePlayers[playerId].SteamId);

            MyVisualScriptLogicProvider.SendChatMessage("Channel created with id:"+Storage.nextChannelId, "COMChat", playerId);
        }

        public void RemoveChannel(long channelId, long playerId)
        {
            // Check if Channel exists
            if (Storage.Channels.ContainsKey(channelId))
            {
                bool isAdmin = false;
                if ((MyAPIGateway.Session.Player.PromoteLevel.Equals(MyPromoteLevel.Admin) || MyAPIGateway.Session.Player.PromoteLevel.Equals(MyPromoteLevel.Owner)))
                {
                    isAdmin = true;
                }

                // Check if player is Creator or Admin
                if (Storage.Channels[channelId].Creator == playerId || isAdmin)
                {
                    // Send all players a chan remove and set chan to global if they are in that channel active
                    foreach(KeyValuePair<long, ChatData> entry in ActivePlayers)
                    {
                        if (entry.Value.AvailChannels.ContainsKey(channelId))
                        {
                            if (entry.Value.Channel == channelId)
                            {
                                entry.Value.Mode = 0;
                            }

                            // Send data to player
                            COMChat.Instance.Networking.SendToPlayer(new PacketChatData(ActivePlayers[entry.Key], entry.Key), ActivePlayers[entry.Key].SteamId);
                        }
                    }

                    // Remove channel cache
                    ChannelPlayers.Remove(channelId);

                    Storage.Channels.Remove(channelId);
                }
                else
                {
                    MyVisualScriptLogicProvider.SendChatMessage("You are not the owner of that channel", "COMChat", playerId);
                }
            }
            else
            {
                MyVisualScriptLogicProvider.SendChatMessage("Channel Id does not exists", "COMChat", playerId);
            }
        }

        public void JoinChannel(long channelId, string password, long playerId)
        {
            // Check if Channel exists
            if (Storage.Channels.ContainsKey(channelId))
            {
                // Check password
                if (Storage.Channels[channelId].Password.Equals(password))
                {
                    // Add player to channel
                    Storage.Channels[channelId].Members.Add(playerId, MyVisualScriptLogicProvider.GetPlayersName(playerId));

                    // Add to data of that player
                    ActivePlayers[playerId].AvailChannels.Add(channelId, Storage.Channels[channelId].Name);

                    // Send new data to player
                    COMChat.Instance.Networking.SendToPlayer(new PacketChatData(ActivePlayers[playerId], playerId), ActivePlayers[playerId].SteamId);
                }
                else
                {
                    MyVisualScriptLogicProvider.SendChatMessage("Password incorrect", "COMChat", playerId);
                }
            }
            else
            {
                MyVisualScriptLogicProvider.SendChatMessage("Channel Id does not exists", "COMChat", playerId);
            }
        }

        public void RemovePlayer(long channelId, string name, long playerId)
        {

            // Check if Channel exists
            if (Storage.Channels.ContainsKey(channelId))
            {
                // Check if player is Creator or Admin
                if (Storage.Channels[channelId].Creator == playerId || Storage.Channels[channelId].Admins.ContainsKey(playerId))
                {
                    long delmember = 0;
                    // Check for playername that is in the list
                    foreach (KeyValuePair<long, string> entry in Storage.Channels[channelId].Members)
                    {
                        if (entry.Value.Contains(name))
                        {
                            // Member found
                            delmember = entry.Key;
                            break;
                        }
                    }

                    if(delmember > 0)
                    {
                        MyVisualScriptLogicProvider.SendChatMessage("Member " + Storage.Channels[channelId].Members[delmember] + "deleted", "COMChat", playerId);
                        Storage.Channels[channelId].Members.Remove(delmember);
                        return;
                    }
                    else
                    {
                        MyVisualScriptLogicProvider.SendChatMessage("No member with that name found", "COMChat", playerId);
                    }

                    // Only check admins if its the owner
                    if(Storage.Channels[channelId].Creator == playerId)
                    {
                        // Reset delmember
                        delmember = 0;

                        foreach (KeyValuePair<long, string> entry in Storage.Channels[channelId].Admins)
                        {
                            if (entry.Value.Contains(name))
                            {
                                // Member found
                                delmember = entry.Key;
                                break;
                            }
                        }

                        if (delmember > 0)
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Admin " + Storage.Channels[channelId].Admins[delmember] + "deleted", "COMChat", playerId);
                            Storage.Channels[channelId].Admins.Remove(delmember);
                        }
                        else
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("No admin with that name found", "COMChat", playerId);
                        }
                    }
                }
                else
                {
                    MyVisualScriptLogicProvider.SendChatMessage("You are not the owner or admin of that channel", "COMChat", playerId);
                }
            }
        }

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="password"></param>
        /// <param name="playerId"></param>
        public void ChangePassword(long channelId, string password, long playerId)
        {
            // Check password length
            if (password.Length < 3 || password.Length > 10)
            {
                MyVisualScriptLogicProvider.SendChatMessage("Password length must be between 3 and 10 characters", "COMChat", playerId);
                return;
            }

            // Check if Channel exists
            if (Storage.Channels.ContainsKey(channelId))
            {
                bool isAdmin = false;
                if ((MyAPIGateway.Session.Player.PromoteLevel.Equals(MyPromoteLevel.Admin) || MyAPIGateway.Session.Player.PromoteLevel.Equals(MyPromoteLevel.Owner)))
                {
                    isAdmin = true;
                }

                // Check if player is Creator or Admin
                if (Storage.Channels[channelId].Creator == playerId || isAdmin)
                {
                    // Change password
                    Storage.Channels[channelId].Password = password;

                    MyVisualScriptLogicProvider.SendChatMessage("Channel password changed", "COMChat", playerId);
                }
                else
                {
                    MyVisualScriptLogicProvider.SendChatMessage("You are not the owner of that channel", "COMChat", playerId);
                }
            }
            else
            {
                MyVisualScriptLogicProvider.SendChatMessage("Channel Id does not exists", "COMChat", playerId);
            }
        }

        /// <summary>
        /// SendConfig
        /// </summary>
        /// <param name="playerId"></param>
        public void SendConfig(long playerId)
        {
            ulong steamId = MyAPIGateway.Players.TryGetSteamId(playerId);
            if (steamId != 0 && Storage.PlayerConfigs.ContainsKey(playerId))
            {
                COMChat.Instance.Networking.SendToPlayer(new PacketPlayerCfg(Storage.PlayerConfigs[playerId], playerId), steamId);
            }
            else
            {
                if(COMChat.Instance.isDebug)COMChat.Instance.ServerLogger.WriteInfo("Communicator::SendConfig Could not find player steam id or config");

            }
        }



        /*
         * Chat functions
         */
        public void IncomingChat(long playerId, string message)
        {
            switch (ActivePlayers[playerId].Mode)
            {
                case 0: // Global
                    ChatGlobal(playerId, message);
                    break;
                case 1: // Faction
                    ChatFaction(playerId, message);
                    break;
                case 2: // Radius
                    ChatRadius(playerId, message);
                    break;
                case 3: // Channel
                    ChatChannel(playerId, message);
                    break;
                case 4: // Wisper
                    ChatWisper(playerId, message);
                    break;
            }
        }

        public void ChatGlobal(long playerId, string message)
        {
            // Send message global
            MyVisualScriptLogicProvider.SendChatMessage(message, Storage.PlayerConfigs[playerId].Name);
        }

        public void ChatFaction(long playerId, string message)
        {
            string fTag = MyVisualScriptLogicProvider.GetPlayersFactionTag(playerId);
            if(fTag != "")
            {
                // Get all members of that faction
                List<long> fMembers = new List<long>();
                fMembers = MyVisualScriptLogicProvider.GetFactionMembers(fTag);
                foreach(long fPlayerId in fMembers)
                {
                    MyVisualScriptLogicProvider.SendChatMessage(message, Storage.PlayerConfigs[playerId].Name, fPlayerId, MyFontEnum.Green);
                }
            }
            else
            {
                MyVisualScriptLogicProvider.SendChatMessage("You are in no faction!", "COMChat", playerId);
            }
        }

        public void ChatRadius(long playerId, string message)
        {

        }

        public void ChatChannel(long playerId, string message)
        {

        }

        public void ChatWisper(long playerId, string message)
        {

        }




        /// <summary>
        /// Event handlers
        /// </summary>
        /// <param name="playerId"></param>
        private void PlayerConnected(long playerId)
        {
            // Check if this player has an config in our store
            if (!Storage.PlayerConfigs.ContainsKey(playerId))
            {
                if(COMChat.Instance.isDebug) COMChat.Instance.ServerLogger.WriteInfo("Communicator::PlayerConnected New player added => "+playerId.ToString());

                // Add to storage
                Storage.PlayerConfigs.Add(playerId, new COMStorage.PlayerConfig(playerId, MyVisualScriptLogicProvider.GetPlayersName(playerId), true, true));
            }

            // Add player if not allready added
            ulong steamId = MyAPIGateway.Players.TryGetSteamId(playerId);
            if (steamId != 0 && Storage.PlayerConfigs.ContainsKey(playerId))
            {
                if (ActivePlayers.ContainsKey(playerId))
                {
                    ActivePlayers[playerId].Active = false;
                }
                else ActivePlayers.Add(playerId, new ChatData(true, 0, 0, 0, 0, "", 0, steamId, new Dictionary<long, string>()));
            }
            else
            {
                if (COMChat.Instance.isDebug) COMChat.Instance.ServerLogger.WriteInfo("Communicator::PlayerConnected CRITICAL ERROR. Cant find steam id of connecting player " + playerId.ToString());
            }
        }
    }
}
