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

        public Faction()
        {
        }


        private string sTag; // Faction tag
        private List<long> lMemberlist; // Memberlist
        private int iFactionScore; // FactionScore

        // Return members
        public List<long> getMembers()
        {
            return lMemberlist;
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
