using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace DSC
{
    public class CommandHandler
    {

        public enum ECommand
        {
            ReloadStoreConfig = 0,
            ResetPrices = 1,
            AddFaction = 2,
            FreeBuild = 3,
            Dev = 4,
            MyDamage = 5,
            DelDamage=6,
            ResetSpawn=7,
            ReloadCoreConfig = 8,
            ReloadRespawns = 9,
            RemovePlayer = 10
        };

        public enum ECommanPublic
        {
            ShowTechs = 0
        };

        public CommandHandler() { }

        public void HandleCommand(string messageText, long playerId, bool isAdmin)
        {
            bool messageHandled = false;
            //string command = messageText.ToLower().Replace(" ", "");//TODO: use exact message. ToLower and replace make it impossible to use commands containing parameters.
            string[] scommand = messageText.Split(' ');
            if (null == scommand) { scommand = new string[1]; scommand[0] = messageText; }
            List<string> lcommand = new List<string>();
            foreach (string s in scommand)
            {
                if ((null == s) || "".Equals(s))
                    continue;
                lcommand.Add(s);
            }

            if (isAdmin)
            {
                
                ECommand cmd;
                if (Enum.TryParse<ECommand>(lcommand[0], true, out cmd))//Ignore Case
                {
                    messageHandled = true;
                    switch (cmd)
                    {
                        case ECommand.ReloadStoreConfig:
                            try
                            {
                                DeepSpaceCombat.Instance.TradeManager.LoadConfig();
                                MyVisualScriptLogicProvider.SendChatMessage("Store config loaded", "Server", playerId);
                                DeepSpaceCombat.Instance.TradeManager.CheckTrades(true);
                            }
                            catch (Exception e)
                            {
                                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Config load failed");
                            }

                            break;
                        case ECommand.ResetPrices:
                            DeepSpaceCombat.Instance.TradeManager.Storage.TradesSell.Clear();
                            DeepSpaceCombat.Instance.TradeManager.Storage.TradesBuy.Clear();
                            DeepSpaceCombat.Instance.TradeManager.CheckTrades(true);
                            MyVisualScriptLogicProvider.SendChatMessage("Prices cleared", "Server", playerId);
                            break;
                        case ECommand.AddFaction:
                            DeepSpaceCombat.Instance.Factions.AddFaction(lcommand[1], false);
                            break;
                        case ECommand.FreeBuild:
                            if (DeepSpaceCombat.Instance.Factions.PlayerFreeBuild.Contains(playerId))
                            {
                                // Deactivate freebuild
                                DeepSpaceCombat.Instance.Factions.RebuildPlayerMenu(playerId);
                                DeepSpaceCombat.Instance.Factions.PlayerFreeBuild.Remove(playerId);
                                MyVisualScriptLogicProvider.SendChatMessage("Freebuild deactivated", "Server", playerId);
                                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Freebuild deactivated");
                            }
                            else
                            {
                                // Activate freebuild
                                DeepSpaceCombat.Instance.Factions.FreebuildPlayerMenu(playerId);
                                DeepSpaceCombat.Instance.Factions.PlayerFreeBuild.Add(playerId);
                                MyVisualScriptLogicProvider.SendChatMessage("Freebuild activated", "Server", playerId);
                                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Freebuild activated");
                            }

                            break;
                        case ECommand.Dev:

                            if (lcommand[1].Equals("test"))
                            {
                                DeepSpaceCombat.Instance.Config.Respawns.Add(new DSC_Config_Main.Respawn("TESA_Startroverbutton", "StartingRover", 188250.77, 140028.55, 145483.24, 188246.98, 140039.95, 145481.83));
                            }

                            if (lcommand[1].Equals("blockdefs"))
                            {
                                foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
                                {
                                    var cubeDef = def as MyCubeBlockDefinition;
                                    if (cubeDef != null)
                                    {
                                        if (cubeDef.Public)
                                        {
                                            // Print new definition to server log
                                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo(cubeDef.Id.ToString());
                                        }
                                    }
                                }
                            }

                            if (lcommand[1].Equals("blockdef"))
                            {
                                foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
                                {

                                    var cubeDef = def as MyCubeBlockDefinition;
                                    if (cubeDef != null)
                                    {
                                        // Get component definition
                                        MyCubeBlockDefinition.Component[] compDef = cubeDef.Components;

                                        // Only save component subtype
                                        string compname = compDef[0].Definition.ToString().Replace("MyObjectBuilder_Component/", "");

                                        if (cubeDef.Public)
                                        {
                                            // Print new definition to server log
                                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("{ \"" + cubeDef.Id.ToString() + "\", new DSC_BlockDef(\"" + cubeDef.Id.ToString() + "\",\"" + cubeDef.DisplayNameText + "\",\"" + compname + "\", \"\", 0)},");
                                        }
                                    }
                                }
                            }

                            if (lcommand[1].Equals("blockdefnew"))
                            {
                                // Prepare blocks
                                List<string> allTechs = new List<string>();
                                foreach(DSC_Config_TechTree.TechLevel techlevel in DeepSpaceCombat.Instance.Techtree.TechLevels.Values)
                                {
                                    allTechs.AddList(techlevel.Blocks);
                                }

                                foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
                                {

                                    var cubeDef = def as MyCubeBlockDefinition;
                                    if (cubeDef != null)
                                    {
                                        // Get component definition
                                        MyCubeBlockDefinition.Component[] compDef = cubeDef.Components;

                                        // Check if exists
                                        if (allTechs.Contains(cubeDef.Id.ToString()))
                                        {
                                            continue;
                                        }

                                        // Only save component subtype
                                        string compname = compDef[0].Definition.ToString().Replace("MyObjectBuilder_Component/", "");

                                        if (cubeDef.Public)
                                        {
                                            // Print new definition to server log
                                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("{ \"" + cubeDef.Id.ToString() + "\", new DSC_BlockDef(\"" + cubeDef.Id.ToString() + "\",\"" + cubeDef.DisplayNameText + "\",\"" + compname + "\", \"\", 0)},");
                                        }
                                    }
                                }
                            }

                            if (lcommand[1].Equals("compdef"))
                            {
                                foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
                                {
                                    var compDef = def as MyComponentDefinition;
                                    if (compDef != null)
                                    {
                                        if (compDef.Public)
                                        {
                                            string compname = compDef.Id.ToString().Replace("MyObjectBuilder_Component/", "");
                                            // Print new definition to server log
                                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("{ \"" + compname + "\",  MyVisualScriptLogicProvider.GetDefinitionId(\"Component\", \"" + compname + "\")},");
                                            //{ "ResearchPoint", MyVisualScriptLogicProvider.GetDefinitionId("Component", "ResearchPoint")}
                                        }
                                    }
                                }
                            }

                            if (lcommand[1].Equals("icondev"))
                            {
                                foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
                                {

                                    var cubeDef = def as MyCubeBlockDefinition;
                                    if (cubeDef != null)
                                    {
                                        if (cubeDef.Public)
                                        {
                                            string Icon = "";
                                            foreach (string el in cubeDef.Icons)
                                            {
                                                Icon = el;
                                                break;
                                            }

                                            // Print new definition to server log
                                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("{ \"" + cubeDef.Id.ToString() + "\", " + Icon + "},");
                                        }
                                    }
                                }
                            }

                            break;
                        case ECommand.ResetSpawn:
                            try
                            {
                                if (lcommand.Count > 1)
                                {
                                    IMyFaction factObj = MyAPIGateway.Session.Factions.TryGetFactionByTag(lcommand[1]);

                                    if (factObj != null)
                                    {
                                        if (DeepSpaceCombat.Instance.CoreStorage.Respawns.ContainsKey(factObj.FactionId))
                                        {
                                            MyVisualScriptLogicProvider.SendChatMessage("Reset spwan for faction" + lcommand[1], "Server", playerId);
                                            DeepSpaceCombat.Instance.CoreStorage.Respawns.Remove(factObj.FactionId);
                                        }
                                    }

                                }

                                // Check if player is in an active faction
                                if (!DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction.ContainsKey(playerId)) return;

                                // Check if player allready spawned a ship
                                if (DeepSpaceCombat.Instance.CoreStorage.Respawns.ContainsKey(DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId]))
                                {
                                    DeepSpaceCombat.Instance.CoreStorage.Respawns.Remove(DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId]);
                                    MyVisualScriptLogicProvider.SendChatMessage("Reset spwan for faction" + DeepSpaceCombat.Instance.Factions.Storage.PlayerFactions[DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId]], "Server", playerId);
                                }
                            }
                            catch (Exception e)
                            {
                                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Config load failed");
                            }
                            break;
                        case ECommand.ReloadCoreConfig:
                            try
                            {
                                DeepSpaceCombat.Instance.LoadCoreConfig();
                                MyVisualScriptLogicProvider.SendChatMessage("Core config loaded", "Server", playerId);
                            }
                            catch (Exception e)
                            {
                                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Config load failed");
                            }

                            break;
                        case ECommand.ReloadRespawns:
                            try
                            {
                                DeepSpaceCombat.Instance.RespawnManager.LoadRespawns();
                                MyVisualScriptLogicProvider.SendChatMessage("Respawn config loaded", "Server", playerId);
                            }
                            catch (Exception e)
                            {
                                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Config load failed");
                            }

                            break;
                        case ECommand.RemovePlayer:
                            try
                            {
                                if (lcommand.Count > 1)
                                {
                                    // Check if player exists
                                    List<IMyPlayer> players = new List<IMyPlayer>();
                                    MyAPIGateway.Players.GetPlayers(players, i => i.DisplayName.ToLower().Equals(lcommand[1].ToLower()));
                                    if (players.Count == 0)
                                    {
                                        MyVisualScriptLogicProvider.SendChatMessage("Player not found", "Server", playerId);
                                        return;
                                    }
                                    IMyPlayer player = players.FirstOrDefault();
                                    MyVisualScriptLogicProvider.SendChatMessage("Removing player from factions =>" + player.IdentityId, "Server", playerId);

                                    // Remove from all storages
                                    foreach (long factions in DeepSpaceCombat.Instance.Factions.Storage.FactionPlayers.Keys)
                                    {
                                        DeepSpaceCombat.Instance.Factions.Storage.FactionPlayers[factions].Remove(player.IdentityId);
                                    }
                                    DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction.Remove(player.IdentityId);
                                }
                            }
                            catch (Exception e)
                            {
                                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Config load failed");
                            }

                            break;
                        default:
                            messageHandled = false;
                            break;
                    }
                }
            }

            ECommanPublic cmdp;
            if (Enum.TryParse<ECommanPublic>(lcommand[0], true, out cmdp))//Ignore Case
            {
                messageHandled = true;
                switch (cmdp)
                {
                    case ECommanPublic.ShowTechs:
                        // Check if player is in a player faction
                        if (DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction.ContainsKey(playerId))
                        {
                            string Message = "Researched Techs:";
                            foreach (string techlevel in DeepSpaceCombat.Instance.Factions.Storage.FactionTechs[DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId]])
                            {
                                Message += "| -" + techlevel;
                            }

                            Message += "||Avail Techs:";
                            foreach (string techLevel in DeepSpaceCombat.Instance.Factions.FactionNextTech[DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId]])
                            {
                                Message += "| -"+techLevel;
                            }

                            DeepSpaceCombat.Instance.Factions.SendMissionScreen(playerId, "Factions Tech", Message);

                        }
                        else
                        {
                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("HandleCommandPublic::ShowTechs not in a faction");
                        }
                        break;
                    default:
                        messageHandled = false;
                        break;
                }
            }
            

            if (!messageHandled)
            {
                MyVisualScriptLogicProvider.SendChatMessage($"Command {messageText} not found", "[Server]", playerId, "Red");
            }

        }
    }
}
