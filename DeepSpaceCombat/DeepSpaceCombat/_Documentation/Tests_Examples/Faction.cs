using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using VRage.ObjectBuilders;
using VRage.Collections;

namespace DSCDEV
{
    public class Faction
    {
        //public static Dictionary<string, Faction> factions = new Dictionary<string, Faction>();

        //public static void initFactions()
        //{
        //    factions.Add("[ADM]", new Faction("[ADM]"));
        //    factions.Add("[DSC]", new Faction("[DSC]"));
        //}

        private string sTag; // Faction tag
        private List<long> lMemberlist; // Memberlist
        private int iFactionScore; // FactionScore
        private Dictionary<string, int> licences;

        public Faction()
        {
        }

        public Faction(string sTag)
        {
            this.sTag = sTag;
            lMemberlist = new List<long>();
            iFactionScore = 0;
            licences = new Dictionary<string, int>();
        }

        public void addMember(long pid)
        {
            lMemberlist.Add(pid);
        }

        // Return members
        public List<long> getMembers()
        {
            return lMemberlist;
        }

        public void unlockResearch(MyDefinitionId id)
        {
            licences[id.ToString()] = 1;
        }

        public void lockResearch(MyDefinitionId id)
        {
            licences[id.ToString()] = 0;
        }

        public HashSet<string> getUnlockedResearch()
        {
            HashSet<string> ret = new HashSet<string>();
            foreach (KeyValuePair<string, int> entry in licences)
            {
                if (entry.Value >= 0)
                    ret.Add(entry.Key);
            }
            return ret;
        }
        public HashSet<string> getAvailableResearch()
        {
            HashSet<string> ret = new HashSet<string>();
            foreach (KeyValuePair<string, int> entry in licences)
            {
                if (entry.Value > 0)
                {
                    ret.Add(entry.Key);
                    MyVisualScriptLogicProvider.SendChatMessage("Available: " + entry.Key);
                }
            }
            return ret;
        }

        public void updateResearch()
        {

            foreach (long pid in lMemberlist)
            {
                MyVisualScriptLogicProvider.SendChatMessage("PID:" + pid);
                MyVisualScriptLogicProvider.PlayerResearchClear(pid);
                MyVisualScriptLogicProvider.ClearAllToolbarSlots(pid);
                foreach (string defid in getAvailableResearch())
                {
                    MyVisualScriptLogicProvider.SendChatMessage("Unlocked: " + defid);
                    MyVisualScriptLogicProvider.PlayerResearchUnlock(pid, MyDefinitionId.Parse(defid));
                }
            }
            MyVisualScriptLogicProvider.SendChatMessage("Updated research.");
        }


        /*
         * Load object
         * --------------------------------------------------------------------------------------------
         */
        public bool load(string tag)
        {
            // Check if faction tag exists
            if (MyAPIGateway.Session.Factions.FactionTagExists(tag))
            {
                // Set tag
                sTag = tag;

                // Load members
                lMemberlist = MyVisualScriptLogicProvider.GetFactionMembers(tag);

                // Load points
                int fscore;
                if (MyAPIGateway.Utilities.GetVariable<int>("faction_" + tag + "_score", out fscore))
                {
                    iFactionScore = fscore;
                }
                else
                {
                    // Never set, so save it for future
                    iFactionScore = 0;
                    MyAPIGateway.Utilities.SetVariable<int>("faction_" + tag + "_score", 0);
                }





            }

            return false;
        }

        // Players


        /*
        private void updatePlayers()
        {
            
        }
        */


    }
}
