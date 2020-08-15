using Sandbox.Game.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;
using VRageRender.Messages;

namespace DSC
{
    public class DSC_SpawnTemplates
    {

        public DSC_SpawnTemplates() { }

        public Dictionary<string, DSC_RespawnTemplate> Respawns { get; } = new Dictionary<string, DSC_RespawnTemplate>();
        public Dictionary<string, DSC_EventTemplate> EventSpawns { get; } = new Dictionary<string, DSC_EventTemplate>();


        public void Load()
        {
            Respawns.Add("TestSpawn", new DSC_RespawnTemplate("Testspawn", new Vector3D(), new Vector3D(), "RespwanPanel_01"));
        }
    }


    public struct DSC_RespawnTemplate
    {
        public readonly string Name;
        public readonly Vector3D DropPosition;
        public readonly Vector3D DropDirection;
        public readonly string ButtonBlockName;

        public DSC_RespawnTemplate(string name, Vector3D dropPosition, Vector3D dropDirection, string buttonBlockName)
        {
            Name = name;
            DropPosition = dropPosition;
            DropDirection = dropDirection;
            ButtonBlockName = buttonBlockName;
        }
    }

    public struct DSC_PrefabTemplate
    {
        public readonly string Name;
        public readonly int Stage;
        public readonly string Cargo;

        public DSC_PrefabTemplate(string name, int stage, string cargo)
        {
            Name = name;
            Stage = stage;
            Cargo = cargo;
        }
    }

    public struct DSC_EventTemplate
    {
        public readonly string Name;
        public readonly int Stage;
        public readonly Dictionary<string, int> CargoItems; // Item name - Amount
        public readonly int PreTime; // Time in seconds
        public readonly string PreText;
        public readonly bool ShowGps; // Show gps to players?
        public readonly int MinPlayers;
        public readonly int MinPlayersPerFaction;
        public readonly int MinOnlineTime;
        public readonly bool IsStatic;
        public readonly bool IsSpace;



        public DSC_EventTemplate(string name, int stage, Dictionary<string,int> cargoItems, int preTime, string preText, bool showGps, int minPlayers, int minPlayersPerFaction, int minOnlineTime, bool isStatic, bool isSpace)
        {
            Name = name;
            Stage = stage;
            CargoItems = cargoItems;
            PreTime = preTime;
            PreText = preText;
            ShowGps = showGps;
            MinPlayers = minPlayers;
            MinPlayersPerFaction = minPlayersPerFaction;
            MinOnlineTime = minOnlineTime;
            IsStatic = isStatic;
            IsSpace = isSpace;
        }
    }

}
