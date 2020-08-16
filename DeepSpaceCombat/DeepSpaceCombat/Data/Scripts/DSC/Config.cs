using System;
using System.Collections.Generic;
using VRageMath;

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
        public static int DamageAllyMultiplier = 1;

        // Research Blocks
        public static Dictionary<string, List<int>> ResearchBlocks = new Dictionary<string, List<int>>() // Blockname, techArea
        {
            {"DSC_Research_1_0", new List<int>(){0,1}},
            {"DSC_Research_1_1", new List<int>(){0,1}},
            {"DSC_Research_1_2", new List<int>(){0,1}},
            {"DSC_Research_1_3", new List<int>(){0,1}},
            {"DSC_Research_1_4", new List<int>(){0,1}},

            {"DSC_Research_2_0", new List<int>(){0,1,2}},
            {"DSC_Research_2_1", new List<int>(){0,1,2}},
            {"DSC_Research_2_2", new List<int>(){0,1,2}},
            {"DSC_Research_2_3", new List<int>(){0,1,2}},
            {"DSC_Research_2_4", new List<int>(){0,1,2}},

            {"DSC_Research_3_0", new List<int>(){0,1,2,3}},
            {"DSC_Research_3_1", new List<int>(){0,1,2,3}},
            {"DSC_Research_3_2", new List<int>(){0,1,2,3}},
            {"DSC_Research_3_3", new List<int>(){0,1,2,3}},
            {"DSC_Research_3_4", new List<int>(){0,1,2,3}},
        };

        // Respawns
        public static string respawn_pb_trigger = "respawnClicked"; // Trigger string in customname => DSC_Respawn_Hangar_Left;respawnClicked
        public static Dictionary<string, DSC_RespawnLocation> Respawns = new Dictionary<string, DSC_RespawnLocation>() // PrefabName, DSC_RespawnLocation
        {
            // Hangar Left
            {"DSC_Respawn_Hangar_Left", new DSC_RespawnLocation("DSC_Respawn_Hangar_Left", new Vector3D(-14125.26, -5619.04, -58662.52), new Vector3D(-14091.02, -5590.72, -58677.20),
                new Dictionary<int, string>(){
                    {0, "SR-Combat" },
                    {1, "SR-Industrial"},
                    {2, "SR-Medical"}
                })
            },

            /*
            // Hangar Left
            {"DSC_Respawn_Hangar_Left", new DSC_RespawnLocation("DSC_Respawn_Hangar_Left", new Vector3D(120973.31, 132846.72, 371873.93), new Vector3D(120969.52, 132805.13, 371874.40),
                new Dictionary<int, string>(){
                    {0, "SR-Combat" },
                    {1, "SR-Industrial"},
                    {2, "SR-Medical"}
                })
            },
            */
            // Hanger Right
            {"DSC_Respawn_Hangar_Right", new DSC_RespawnLocation("DSC_Respawn_Hangar_Right", new Vector3D(120948.91, 132849.26, 371877.74), new Vector3D(120944.97, 132811.58, 371880.23),
                new Dictionary<int, string>(){
                    {0, "SR-Combat" },
                    {1, "SR-Industrial"},
                    {2, "SR-Medical"}
                })
            },
        };

        // Research steps for 5 Factions
        public static float[] ResearchSteps = new float[]{1, 0.8f, 0.6f, 0.5f, 0.4f};
    }
}
