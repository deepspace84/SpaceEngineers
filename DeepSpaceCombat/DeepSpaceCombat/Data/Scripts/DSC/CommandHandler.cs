using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace DSC
{
    public class CommandHandler
    {

        public enum ECommand
        {
            Test = 0,
            Help = 1,
            AddFaction = 2,
            FreeBuild = 3,
            Dev = 4,
            MyDamage = 5,
            DelDamage=6

        };

        public CommandHandler() { }

        public void HandleCommand(string messageText, long playerId)
        {
            bool messageHandled = false;
            //string command = messageText.ToLower().Replace(" ", "");//TODO: use exact message. ToLower and replace make it impossible to use commands containing parameters.
            string[] scommand = messageText.Split(' ');
            if (null == scommand) { scommand = new string[1];scommand[0] = messageText; }
            List<string> lcommand = new List<string>();
            foreach (string s in scommand)
            {
                if ((null == s) || "".Equals(s))
                    continue;
                lcommand.Add(s);
            }
            ECommand cmd;
            if(Enum.TryParse<ECommand>(lcommand[0],true,out cmd))//Ignore Case
            {
                messageHandled = true;
                switch(cmd)
                {
                    case ECommand.Test:
                        MyVisualScriptLogicProvider.SendChatMessage("Test command called.", "[Server]", playerId);
                        break;
                    case ECommand.Help:
                        PrintHelp(playerId);
                        break;
                    case ECommand.AddFaction:
                        DeepSpaceCombat.Instance.Factions.AddFaction(lcommand[1], false);
                        break;
                    case ECommand.FreeBuild:
                        DeepSpaceCombat.Instance.Factions.freeBuild = !DeepSpaceCombat.Instance.Factions.freeBuild;
                        DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Freebuild=>"+ DeepSpaceCombat.Instance.Factions.freeBuild.ToString());
                        break;
                    case ECommand.Dev:

                        if (lcommand[1].Equals("spawn"))
                        {

                            try
                            {

                                Vector3D startPosition = new Vector3D(float.Parse(lcommand[3]), float.Parse(lcommand[4]), float.Parse(lcommand[5]));
                                Vector3D startDirection = new Vector3D();
                                if (lcommand.Count > 6)
                                {
                                    startDirection = new Vector3D(float.Parse(lcommand[6]), float.Parse(lcommand[7]), float.Parse(lcommand[8]));
                                }
                                
                                DeepSpaceCombat.Instance.SpawnManager.Spawn(new DSC_SpawnShip(MyVisualScriptLogicProvider.GetLocalPlayerId(), lcommand[2], startPosition, startDirection, true));

                            }
                            catch (Exception e)
                            {
                                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "SpawnManagerCommand failed");
                            }



                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Testspawn called");
                            //DeepSpaceCombat.Instance.SpawnManager.Spawn("TestSpawn", "DSC_TestVehicle");
                        }

                        if (lcommand[1].Equals("check"))
                        {
                            //DeepSpaceCombat.Instance.SpawnManager.WriteStorage();
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

                        break;

                    case ECommand.MyDamage:

                        // Check if player exists
                        if (DeepSpaceCombat.Instance.Factions.Storage.PlayerDamage.ContainsKey(playerId))
                        {
                            MyVisualScriptLogicProvider.SendChatMessage("Your total damage is "+ DeepSpaceCombat.Instance.Factions.Storage.PlayerDamage[playerId].ToString("#,##0"), "[Server]", playerId);
                        }

                        break;
                    case ECommand.DelDamage:

                        // Check if player exists
                        if (DeepSpaceCombat.Instance.Factions.Storage.PlayerDamage.ContainsKey(playerId))
                        {
                            DeepSpaceCombat.Instance.Factions.Storage.PlayerDamage[playerId] = 0;
                            MyVisualScriptLogicProvider.SendChatMessage("Your total damage is " + DeepSpaceCombat.Instance.Factions.Storage.PlayerDamage[playerId].ToString("#,##0"), "[Server]", playerId);
                        }

                        break;
                    default:
                        messageHandled = false;
                        break;
                }
            }
            else
            {
                MyVisualScriptLogicProvider.SendChatMessage("Not a valid command: "+lcommand[0], "[Server]", playerId);
            }

            if (!messageHandled)
            {
                MyVisualScriptLogicProvider.SendChatMessage($"Command {messageText} not found", "[Server]", playerId, "Red");
                PrintHelp(playerId);
            }

        }

        /// <summary>
        /// Help that prints all commands
        /// </summary>
        public  static void PrintHelp(long playerID)
        {
            // TODO
            MyVisualScriptLogicProvider.SendChatMessage("Supported commands:\n" +
                "#test\n" +
                "#help\n" +
                "#fg\n" +
                "#csc\n" +
                "#load gb\n" +
                "#save gb",
                "[Server]", playerID, "Red");
        }
    }
}
