using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;

namespace DSC
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
    }
}
