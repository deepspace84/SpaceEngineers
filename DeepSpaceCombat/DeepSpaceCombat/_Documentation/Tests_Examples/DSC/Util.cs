using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace DSC_TEST
{
    public class Util
    {
        public static IMyPlayer FindPlayerById(long id)
        {
            List<IMyPlayer> list = new List<IMyPlayer>();

            MyAPIGateway.Players.GetPlayers(list, p => p.IdentityId == id);

            if(list.Count > 0)
                return list[0];

            return null;
        }

        public static List<long> GetNPCs()
        {
            List<long> res = MyVisualScriptLogicProvider.GetFactionMembers("DSC");
            // TODO create faction and ann npc

            return MyVisualScriptLogicProvider.GetFactionMembers("DSC");
        }

    }
}
