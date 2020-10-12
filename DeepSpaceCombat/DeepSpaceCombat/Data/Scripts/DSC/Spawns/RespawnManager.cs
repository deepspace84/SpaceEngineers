using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace DSC
{
    public class DSC_RespawnManager
    {

        public DSC_RespawnManager() { }


        private Dictionary<long, string> RespawnStations = new Dictionary<long, string>(); // blockId - respawnName

        public void Load()
        {
            MyVisualScriptLogicProvider.ButtonPressedEntityName += ButtonPressedFull;

            // Load all stations from config
            foreach (string respawnName in DSC_Config.Respawns.Keys)
            {
                DSC_RespawnLocation respawnLocation = DSC_Config.Respawns[respawnName];

                // Check if block exists
                long blockId = DeepSpaceCombat.Instance.DSCReference.AddBlockWithName(respawnLocation.BlockName);
                if (blockId > 0)
                {
                    RespawnStations.Add(blockId, respawnName);
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Load: Add RespawnStation=>" + respawnLocation.BlockName);
                }
                else
                {
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::load: Could not add/find block in Reference. Blockname=>" + respawnLocation.BlockName + " | Error=>" + blockId.ToString());
                }

            }

        }


        private void ButtonPressedFull(string name, int button, long playerId, long blockId)
        {
            try
            {
                // Check if button is in reference
                if (!RespawnStations.ContainsKey(blockId)) return;
                string spawnName = RespawnStations[blockId];

                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("RespawnManager::ButtonPressedFull block in reference");

                // Check if button id exists
                if (!DSC_Config.Respawns[spawnName].Prefabs.ContainsKey(button)) return;

                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("RespawnManager::ButtonPressedFull Button Id exists");

                // Check if player allready spawned a ship
                if (DeepSpaceCombat.Instance.CoreStorage.Respawns.ContainsKey(playerId))
                {
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("RespawnManager::ButtonPressed: Player allready got one " + MyVisualScriptLogicProvider.GetPlayersName(playerId));
                    MyVisualScriptLogicProvider.SendChatMessage("Only one start ship allowed", "Server", playerId);
                    return;
                }

                // Add player to storage
                DeepSpaceCombat.Instance.CoreStorage.Respawns.Add(playerId, DateTime.Now);

                // Spawn ship
                DSC_SpawnShip spawnShipObj = new DSC_SpawnShip(playerId, DSC_Config.Respawns[spawnName].Prefabs[button], DSC_Config.Respawns[spawnName].StartPosition, DSC_Config.Respawns[spawnName].StartDirection, true);
                if(spawnShipObj == null)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("WTF...");
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo(playerId.ToString()+"--"+DSC_Config.Respawns[spawnName].Prefabs[button].ToString()+"--"+DSC_Config.Respawns[spawnName].StartPosition.ToString()+"--"+DSC_Config.Respawns[spawnName].StartDirection.ToString());
                    return;
                }
                else
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Was ist nur los...");
                }

                DeepSpaceCombat.Instance.SpawnManager.Spawn(spawnShipObj);
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("RespawnManager::ButtonPressedFull ship spawned");
            }catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "RespawnManager ButtonPressedFull failed");
            }


        }


        public void Unload()
        {
            MyVisualScriptLogicProvider.ButtonPressedEntityName -= ButtonPressedFull;
        }
    }

    public struct DSC_RespawnLocation{

        public readonly string BlockName;
        public readonly Vector3D StartPosition;
        public readonly Vector3D StartDirection;
        public readonly Dictionary<int, string> Prefabs; // ButtonId, prefabName

        public DSC_RespawnLocation(string blockName, Vector3D startPosition, Vector3D startDirection, Dictionary<int, string> prefabs)
        {
            BlockName = blockName;
            StartPosition = startPosition;
            StartDirection = startDirection;
            Prefabs = prefabs;
        }

    }
}
