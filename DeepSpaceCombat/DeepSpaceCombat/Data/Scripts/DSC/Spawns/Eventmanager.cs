using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.Scripting;

namespace DSC
{
    public class DSC_Eventmanager
    {

        public DSC_Eventmanager() {}


        public void Load()
        {
            MyVisualScriptLogicProvider.PlayerConnected += PlayerConnected;
            MyVisualScriptLogicProvider.PlayerDisconnected += PlayerDisconnected;
        }

        public void Unload()
        {
            MyVisualScriptLogicProvider.PlayerConnected -= PlayerConnected;
            MyVisualScriptLogicProvider.PlayerDisconnected -= PlayerDisconnected;
        }

        public void Check()
        {
            //MyAPIGateway.Players.Count
            List<long> playerList = MyVisualScriptLogicProvider.GetPlayers();
            IMyStoreBlock test;
            //test.InsertOrder
            MyStoreItemData test2 = new MyStoreItemData();
            test.CancelStoreItem1

        }

        private void PlayerConnected(long playerId)
        {

        }

        private void PlayerDisconnected(long playerId)
        {

        }


    }
}
