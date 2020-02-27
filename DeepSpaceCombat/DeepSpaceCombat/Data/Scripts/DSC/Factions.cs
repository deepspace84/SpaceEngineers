using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.ModAPI;
using VRage.Game;

namespace DSC
{
    public class DSC_Factions
    {
        private static DSC_Factions _instance;
        private Dictionary<string, long> _factionReference = new Dictionary<string, long>();

        public static DSC_Factions Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DSC_Factions();
                return _instance;
            }
        }



        public void load()
        {

        }

        public void unload()
        {

        }


        /*
        public bool AddFaction(string ftag)
        {
            return false;
        }
        */

        public void Research(List<string> lcommand, long playerId)
        {

            switch (lcommand[1])
            {
                case "whitelist":
                    MyVisualScriptLogicProvider.ResearchListWhitelist(true);
                    break;
                case "dewhitelist":
                    MyVisualScriptLogicProvider.ResearchListWhitelist(false);
                    break;
                case "clearall":
                    MyVisualScriptLogicProvider.PlayerResearchClearAll();
                    break;
                case "clearplayer":
                    MyVisualScriptLogicProvider.PlayerResearchClear(playerId);
                    break;
                case "cleartoolbar":
                    MyVisualScriptLogicProvider.ClearAllToolbarSlots(playerId);
                    break;
                case "lock":
                    MyDefinitionId id = MyVisualScriptLogicProvider.GetDefinitionId(lcommand[2], lcommand[3]);
                    MyVisualScriptLogicProvider.PlayerResearchLock(playerId, id);
                    break;
                case "unlock":
                    MyDefinitionId id2 = MyVisualScriptLogicProvider.GetDefinitionId(lcommand[2], lcommand[3]);
                    MyVisualScriptLogicProvider.PlayerResearchUnlock(playerId, id2);
                    break;
                default:
                    break;
            }

        }
    }
}