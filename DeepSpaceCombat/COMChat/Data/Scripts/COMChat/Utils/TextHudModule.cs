using Draygo.API;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;
using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum;

namespace DeepSpace
{
    public class TextHudModule
    {
        public TextHudModule() { }

        HudAPIv2 HUD_Base;

        HudAPIv2.HUDMessage HUD_Status;
        StringBuilder StatusText = new StringBuilder("");

        HudAPIv2.HUDMessage HUD_Info;
        StringBuilder InfoText = new StringBuilder("");

        public void Init()
        {
            HUD_Base = new HudAPIv2(HUD_Init_Complete);
        }

        private void HUD_Init_Complete()
        {
            try
            {
                if(COMChat.Instance.isDebug)COMChat.Instance.ClientLogger.WriteInfo("TextHudModule::HUD_Init_Complete called");

                // Init status line
                HUD_Status = new HudAPIv2.HUDMessage(StatusText, new Vector2D(-0.7, -0.6), null, -1, 0.7, true, false, null, BlendTypeEnum.PostPP, "white");

                // Init info line
                HUD_Info = new HudAPIv2.HUDMessage(InfoText, new Vector2D(-0.7, -0.65), null, -1, 1, true, false, null, BlendTypeEnum.PostPP, "white");
            }
            catch (Exception e)
            {
                COMChat.Instance.ServerLogger.WriteException(e, "TextHudModule::HUD_Init_Complete");
            }
        }


        public void SetStatus(bool status)
        {
            StatusText.Clear();
            // Check for active
            if (status)
            {
                HUD_Status.InitialColor = Color.Green;
                StatusText.Append("COMChat active");
            }
            else
            {
                HUD_Status.InitialColor = Color.Red;
                StatusText.Append("COMChat inactive");
            }
            HUD_Status.Visible = true;
        }

        public void SetInfo()
        {
            InfoText.Clear();

            switch (COMChat.Instance.COMClient.ChatConfig.Mode)
            {
                case 0: // Global
                    HUD_Info.InitialColor = Color.Blue;
                    InfoText.Append("Global");

                    break;
                case 1: // Faction
                    HUD_Info.InitialColor = Color.Green;
                    InfoText.Append("Faction");

                    break;
                case 2: // Radius
                    HUD_Info.InitialColor = Color.Yellow;
                    InfoText.Append("Radius: "+COMChat.Instance.COMClient.ChatConfig.Radius.ToString());

                    break;
                case 3: // Channel
                    HUD_Info.InitialColor = Color.Magenta;
                    InfoText.Append("Channel: " + COMChat.Instance.COMClient.ChatConfig.AvailChannels[COMChat.Instance.COMClient.ChatConfig.Channel]);

                    break;
                case 4: // Wisper
                    HUD_Info.InitialColor = Color.Orange;
                    InfoText.Append("Wisper to: " + MyVisualScriptLogicProvider.GetPlayersName(COMChat.Instance.COMClient.ChatConfig.WPlayer));

                    break;
            }
        }
    }
}
