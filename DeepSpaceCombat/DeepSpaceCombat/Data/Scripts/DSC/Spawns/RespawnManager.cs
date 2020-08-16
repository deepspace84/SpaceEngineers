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
using VRage.ModAPI;
using VRageMath;

namespace DSC
{
    public class DSC_RespawnManager
    {

        public DSC_RespawnManager() { }


        private Dictionary<string, List<long>> RespawnStations = new Dictionary<string, List<long>>();

        public void Load()
        {
            // Register Area handlers
            MyVisualScriptLogicProvider.AreaTrigger_Entered += Event_Area_Entered;
            MyVisualScriptLogicProvider.AreaTrigger_Left += Event_Area_Left;


            // Load all stations from config
            foreach (string respawnName in DSC_Config.Respawns.Keys)
            {
                DSC_RespawnLocation respawnLocation = DSC_Config.Respawns[respawnName];

                // Check if block exists
                long blockId = DeepSpaceCombat.Instance.DSCReference.AddBlockWithName(respawnLocation.BlockName);
                if (blockId > 0)
                {
                    IMyButtonPanel buttonPanel = MyAPIGateway.Entities.GetEntityById(blockId) as IMyButtonPanel;
                    if(buttonPanel != null)
                    {
                        buttonPanel.ButtonPressed += ButtonPressed;

                        // Get Position
                        Vector3D pos = buttonPanel.GetPosition();

                        // Create area
                        MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(pos, 2, respawnLocation.BlockName);

                        // Add Button to reference
                        RespawnStations.Add(respawnLocation.BlockName, new List<long>());

                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Load: Add RespawnStation=>" + respawnLocation.BlockName);
                    }
                    else
                    {

                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::load: Could not cast as buttonPanel");
                    }
                }
                else
                {
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::load: Could not add/find block in Reference. Blockname=>" + respawnLocation.BlockName + " | Error=>" + blockId.ToString());
                }

            }

        }



        public void Unload()
        {
            // Loop through reference and remove all areas 
            foreach (string areaName in RespawnStations.Keys)
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Unload: Remove RespawnStation=>" + areaName);
                MyVisualScriptLogicProvider.RemoveTrigger(areaName);
            }
            RespawnStations = null;

            // Remove Area handlers
            MyVisualScriptLogicProvider.AreaTrigger_Entered -= Event_Area_Entered;
            MyVisualScriptLogicProvider.AreaTrigger_Left -= Event_Area_Left;

            // Unload all stations from config
            foreach (string respawnName in DSC_Config.Respawns.Keys)
            {
                DSC_RespawnLocation respawnLocation = DSC_Config.Respawns[respawnName];

                // Check if block exists
                long blockId = DeepSpaceCombat.Instance.DSCReference.AddBlockWithName(respawnLocation.BlockName);
                if (blockId > 0)
                {
                    IMyButtonPanel buttonPanel = MyAPIGateway.Entities.GetEntityById(blockId) as IMyButtonPanel;

                    if (buttonPanel != null)
                    {
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Unload: Removed Button handler");
                        buttonPanel.ButtonPressed -= ButtonPressed;
                    }
                    else
                    {
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Unload: Could not cast as buttonPanel");
                    }
                }
                else
                {
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Unload: Could not add/find block in Reference. Blockname=>" + respawnLocation.BlockName + " | Error=>" + blockId.ToString());
                }

            }
        }

        
        public void ButtonPressed(int buttonId)
        {
            // Loop through reference and check customName
            foreach (string areaName in RespawnStations.Keys)
            {
                long blockId = DeepSpaceCombat.Instance.DSCReference.GetBlockWithName(areaName);
                if(blockId > 0)
                {
                    IMyButtonPanel buttonPanel = MyAPIGateway.Entities.GetEntityById(blockId) as IMyButtonPanel;

                    if (buttonPanel != null)
                    {
                        // Check customData
                        if(buttonPanel.CustomData == DSC_Config.respawn_pb_trigger)
                        {
                            // Reset CustomData
                            buttonPanel.CustomData = "";

                            // We found the trigger block, Check player count
                            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::ButtonPressed: Area count=>"+ RespawnStations[areaName].Count.ToString());
                            if (RespawnStations[areaName].Count == 1)
                            {
                                // Check if player allready spawned a ship
                                if (DeepSpaceCombat.Instance.CoreStorage.Respawns.ContainsKey(RespawnStations[areaName].FirstOrDefault()))
                                {
                                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("RespawnManager::ButtonPressed: Player allready got one "+ MyVisualScriptLogicProvider.GetPlayersName(RespawnStations[areaName].FirstOrDefault()));
                                    MyVisualScriptLogicProvider.SendChatMessage("Only one start ship allowed", "Server", RespawnStations[areaName].FirstOrDefault());
                                    return;
                                }

                                // Check if button id exists
                                if (!DSC_Config.Respawns[areaName].Prefabs.ContainsKey(buttonId)) return;

                                

                                // Spawn ship
                                DeepSpaceCombat.Instance.SpawnManager.Spawn(new DSC_SpawnShip(RespawnStations[areaName].FirstOrDefault(), DSC_Config.Respawns[areaName].Prefabs[buttonId], DSC_Config.Respawns[areaName].StartPosition, DSC_Config.Respawns[areaName].StartDirection, true));

                                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("RespawnManager::ButtonPressed: Spawn Ship for "+RespawnStations[areaName].FirstOrDefault()+ " - "+MyVisualScriptLogicProvider.GetPlayersName(RespawnStations[areaName].FirstOrDefault()));

                            }
                            else
                            {
                                foreach(long playerId in RespawnStations[areaName])
                                {
                                    MyVisualScriptLogicProvider.SendChatMessage("You have to be alone at the panel to spawn your ship", "Server", playerId);
                                }

                                // Only allow clicking if its one player
                                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::ButtonPressed: More than one player");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::ButtonPressed: Could not cast as buttonPanel");
                    }
                }
                else
                {

                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::ButtonPressed block not found");
                }
            }
        }

        private void Event_Area_Entered(string name, long playerId)
        {
            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Event_Area_Entered entered");
            // Check if player is in an active faction
            if (!DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction.ContainsKey(playerId)) return;

            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Event_Area_Entered is in faction");

            // Check if this area is for research
            if (RespawnStations.ContainsKey(name))
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Event_Area_Entered player added");
                RespawnStations[name].Add(playerId);
            }
        }

        private void Event_Area_Left(string name, long playerId)
        {
            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Event_Area_Left leaved");
            // Check if player is in an active faction
            if (!DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction.ContainsKey(playerId)) return;

            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Event_Area_Left is in faction");

            // Check if this area is for research
            if (RespawnStations.ContainsKey(name))
            {
                // Remove player
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("RespawnManager::Event_Area_Left player removed");
                RespawnStations[name].Remove(playerId);
            }
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
