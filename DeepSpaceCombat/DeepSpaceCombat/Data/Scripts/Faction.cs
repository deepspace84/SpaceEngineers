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

namespace DSC
{
    public class Faction
    {
        public static Dictionary<string, Faction> factions = new Dictionary<string, Faction>();

        public static void initFactions()
        {
            factions.Add("[ADM]", new Faction("[ADM]"));
            factions.Add("[DSC]", new Faction("[DSC]"));
        }

        public Faction()
        {
        }

        public Faction(string sTag)
        {
            this.sTag = sTag;
            lMemberlist = new List<long>();
            iFactionScore = 0;
            licences = new Dictionary<MyDefinitionId, int>();
        }

        private string sTag; // Faction tag
        public List<long> lMemberlist; // Memberlist
        private int iFactionScore; // FactionScore
        public Dictionary<MyDefinitionId, int> licences;

        // Return members
        public List<long> getMembers()
        {
            return lMemberlist;
        }

        public void unlock(MyDefinitionId id)
        {
            licences[id] = 1;
        }

        public void lockResearch(MyDefinitionId id)
        {
            licences[id] = 0;
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
                if(MyAPIGateway.Utilities.GetVariable<int>("faction_"+tag+"_score", out fscore))
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
