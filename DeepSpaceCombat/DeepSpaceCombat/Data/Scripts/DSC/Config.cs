using System;
using System.Collections.Generic;

namespace DSC
{

    public class DSC_Config
    {
        // Actual Mod Version
        public static double modVersion = 0.42;

        // This is used to indicate the base communication version.
        // If we change Message classes or add a new Message class in any way, we need to update this number.
        // This is because of potentional conflict in communications when we release a new version of the mod.
        // ie., An established server will be running with version 1. We release a new version with different 
        // communications classes. A Player will connect to the server, and will automatically download version 2.
        // We would now have a Client running newer communication classes trying to talk to the Server with older classes.
        public static int ModCommunicationVersion = 1;

        // The is the Id which this mod registers iteself for sending and receiving messages through SE. 
        // This Id needs to be unique with SE and other mods, otherwise it can send/receive  
        // messages to/from the other registered mod by mistake, and potentially cause SE to crash.
        public const ushort ConnectionId = 46573;

        /*
         * Ship & Missile speeds
         */
        public static float largeShipSpeed = 150;
        public static float smallShipSpeed = 225;
        public static float missileMinSpeed = 240;
        public static float missileMaxSpeed = 360;
        public static float missileExplosionRange = 2500;


        // Main neutral faction
        public static string MainFaction = "SRI";
        public static string MainFactionNPC = "Marvin";

        public static string EnemyFaction = "GOG"; // Guardians of the Galaxy
        public static string EnemyFactionNPC = "Groot";

        // Damage 
        public static int DamageAllyMultiplier = 10;

        // Research Blocks
        public static Dictionary<string, List<int>> ResearchBlocks = new Dictionary<string, List<int>>() // Blockname, techArea
        {
            {"DSC_MainBase_Res_0", new List<int>(){0,1}},
        };

        // Research steps for 5 Factions
        public static float[] ResearchSteps = new float[]{1, 0.8f, 0.6f, 0.5f, 0.4f};
    }
}
