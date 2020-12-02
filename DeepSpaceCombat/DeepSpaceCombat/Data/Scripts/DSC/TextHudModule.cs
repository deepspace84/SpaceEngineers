using Draygo.API;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;
using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum;

namespace DSC
{
    public class DSC_TextHudModule
    {
        public DSC_TextHudModule() { }

        HudAPIv2 HUD_Base;
        HudAPIv2.HUDMessage HUD_Message;
        StringBuilder text = new StringBuilder("");

        private bool toggle = false;

        public void Init()
        {
            HUD_Base = new HudAPIv2(HUD_Init_Complete);
        }



        private void HUD_Init_Complete()
        {
            HUD_Message = new HudAPIv2.HUDMessage(text, new Vector2D(-1, 0), null, -1, 1, true, false, null, BlendTypeEnum.PostPP, "white");
            HUD_Message.InitialColor = Color.Red;
            HUD_Message.Scale *= 1.2;
            HUD_Message.Origin = new Vector2D(-0.2, 0.8);


        }


        public void Check()
        {
            // Texthud test
            if (HUD_Message != null)
            {
                if (toggle == true)
                {
                    text.Clear();
                    text.Append("Its working");
                    HUD_Message.Visible = true;

                    toggle = false;
                }
                else
                {
                    HUD_Message.Visible = false;
                    toggle = true;
                }
            }
        }


    }
}
