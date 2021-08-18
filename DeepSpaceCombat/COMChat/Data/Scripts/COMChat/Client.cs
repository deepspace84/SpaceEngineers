using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI;

namespace DeepSpace
{
    public class CommunicatorClient
    {
        public CommunicatorClient() { }

        public bool ChatOpen = false;
        public COMStorage.PlayerConfig PlayerConfig;
        public ChatData ChatConfig = new ChatData(false, 0, 0, 0, 0, "", 0, 0, new Dictionary<long, string>());

        public void Init()
        {
            // Register client message handler
            MyAPIGateway.Utilities.MessageEntered += GotMessage;
        }

        public void Unload()
        {
            // Unregister client message handler
            if (MyAPIGateway.Utilities != null)
            {
                MyAPIGateway.Utilities.MessageEntered -= GotMessage;
            }
        }



        public void CheckChat()
        {
            if (MyAPIGateway.Gui.ChatEntryVisible)
            {
                if (!ChatOpen)
                {
                    // Check for config, if not load it
                    if(null == PlayerConfig)
                    {
                        LoadConfig();
                    }

                    if (PlayerConfig.ShowHints) ShowHints();
                    if (PlayerConfig.ShowPlayers) ShowPlayers();

                    ChatOpen = true;
                }
            }
            else if(ChatOpen)
            {
                ChatOpen = false;
            }
        }


        public void LoadConfig()
        {
            // Send request to server
            COMChat.Instance.Networking.SendToServer(new PacketCommand("#p_config", MyAPIGateway.Session.Player.IdentityId, false));
        }


        private void ShowHints()
        {

        }

        private void ShowPlayers()
        {

        }


        /*
         * GotMessage
         * Local message handler function
         * 
         */
        private void GotMessage(string messageText, ref bool sendToOthers)
        {
            // Check for command message
            if (messageText.StartsWith(Mod_Config._commandStart.ToString()))
            {

                bool isAdmin = false;
                if ((MyAPIGateway.Session.Player.PromoteLevel.Equals(MyPromoteLevel.Admin) || MyAPIGateway.Session.Player.PromoteLevel.Equals(MyPromoteLevel.Owner)))
                {
                    isAdmin = true;
                }

                // Send command to server
                COMChat.Instance.Networking.SendToServer(new PacketCommand(messageText, MyAPIGateway.Session.Player.IdentityId, isAdmin));
                sendToOthers = false;
            }
            else if (ChatConfig.Active) // Check if comchat is active
            {
                // Send chat message to server
                COMChat.Instance.Networking.SendToServer(new PacketMessage(messageText, MyAPIGateway.Session.Player.IdentityId));
                sendToOthers = false;
            }
        }

    }
}
