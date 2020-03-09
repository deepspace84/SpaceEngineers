using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;

namespace DSC
{
    public class CommandHandler
    {
        //private static CommandHandler _instance;

        //public static CommandHandler Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new CommandHandler();
        //        return _instance;
        //    }
        //}

        public enum ECommand
        {
            Test = 0,
            Help = 1,
            FindGrids = 2,
            SearchContract = 3,
            Load = 4,
            Save = 5,
            ReloadBlocksAndGrids = 6,
            Players = 7,
            Get_NPC = 8,
            AddFaction = 9,
            Dev = 10,
            //fg=2,
            //csc=3
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
                long gridID = 0;
                long blockID = 0;
                messageHandled = true;
                switch(cmd)
                {
                    case ECommand.Test:
                        MyVisualScriptLogicProvider.SendChatMessage("Test command called.", "[Server]", playerId);
                        break;
                    case ECommand.Help:
                        PrintHelp(playerId);
                        break;
                    case ECommand.FindGrids:
                        string findBlock = "DSC_Start";
                        if (lcommand.Count > 1)
                            findBlock = lcommand[1];
                        string findGrid = "DSC_End";
                        if (lcommand.Count > 2)
                            findGrid = lcommand[2];
                        try
                        {
                            blockID = DeepSpaceCombat.Instance.BlockRef.AddBlockWithName(findBlock);
                            gridID = DeepSpaceCombat.Instance.GridRef.AddGridWithName(findGrid);
                        } catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("ERROR: " + ex.Message, "[Server]", playerId); }
                        MyVisualScriptLogicProvider.SendChatMessage($"Block found: " + blockID.ToString() + " | grid: " + gridID.ToString(), "[Server]", playerId);
                        break;
                    case ECommand.SearchContract:
                        string startBlock = "DSC_Start";
                        if (lcommand.Count > 1)
                            startBlock = lcommand[1];
                        string searchGrid = "DSC_End";
                        if (lcommand.Count > 2)
                            searchGrid = lcommand[2];
                        string contractName = "Test";
                        if (lcommand.Count > 3)
                            contractName = lcommand[3];
                        int reward = 1000;
                        if (lcommand.Count > 4)
                            contractName = lcommand[4];
                        try
                        {
                            blockID = DeepSpaceCombat.Instance.BlockRef.GetBlockWithName(startBlock);
                            gridID = DeepSpaceCombat.Instance.GridRef.GetGridWithName(searchGrid);
                            DSC_SearchContractBase searchContract = new DSC_SearchContractBase(contractName, reward, blockID, 0, 60 * 10, gridID, 10, "Find the Target!", playerId);

                            MyAddContractResultWrapper result = searchContract.StartContract();
                            if (result.Success)
                                MyVisualScriptLogicProvider.SendChatMessage($"Contract Started: {searchContract.Name}\n {searchContract.Description}","[Server]", playerId, "Green");
                            else
                            {
                                MyVisualScriptLogicProvider.SendChatMessage("Creation of the contract failed", "[Server]", playerId, "Red");
                            }
                        }
                        catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("ERROR: " + ex.Message, "[Server]", playerId); }
                        break;
                    case ECommand.Load:
                        //TODO: Implement equivalent to existing... use parameter to select what is to be loaded
                        break;
                    case ECommand.Save:
                        //TODO: Implement equivalent to existing... use parameter to select what is to be saved
                        break;
                    case ECommand.ReloadBlocksAndGrids:
                        // TODO delete grids/blocks from lists and research them
                        break;
                    case ECommand.Players:

                        break;
                    case ECommand.Get_NPC:
                        DeepSpaceCombat.Instance.GetNPC();
                        break;
                    case ECommand.AddFaction:
                        DeepSpaceCombat.Instance.Factions.AddFaction(lcommand[1], false);
                        break;
                    case ECommand.Dev:

                        if (lcommand[1].Equals("blockdef"))
                        {
                            foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
                            {
                                var cubeDef = def as MyCubeBlockDefinition;
                                if (cubeDef != null)
                                {
                                    MyCubeBlockDefinition.Component[] test = cubeDef.Components;
                                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("public static string[] " + cubeDef.Id.ToString().Replace("/", "_") + " = {\"" + cubeDef.DisplayNameText + "\",\"" + cubeDef.Id.ToString() + "\",\"" + test[0].Definition.ToString() + "\"};");
                                }
                            }
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

            //Below is to be replaced, but kept until finished.

            //else if (command.Equals("fg".Replace(" ", ""))) // find grids
            
            if (lcommand[0].Equals("fg"))
            {   
                long blockID = DSC_Blocks.Instance.AddBlockWithName("DSC_Start");
                long gridID = DSC_Grids.Instance.AddGridWithName("DSC_End");
                MyVisualScriptLogicProvider.SendChatMessage($"Block found: "+blockID.ToString()+" | grid: "+gridID.ToString(), "[Server]", playerId);

                MyVisualScriptLogicProvider.SendChatMessage($"Blocks found: " + 
                    $"{DSC_Blocks.Instance.GetBlockWithName("DSC_Start") > 0 && DSC_Grids.Instance.GetGridWithName("DSC_End") > 0}", "[Server]", playerId);

                messageHandled = true;
            }
            else if (lcommand[0].Equals("csc".Replace(" ", ""))) // create search contract
            {
                if (MyAPIGateway.Session.IsServer)
                {
                    DSC_SearchContractBase searchContract = new DSC_SearchContractBase("Test", 1000,
                    DSC_Blocks.Instance.GetBlockWithName("DSC_Start"), 0, 60 * 10,
                    DSC_Grids.Instance.GetGridWithName("DSC_End"), 10, "Find the Target!",
                    playerId);

                    MyAddContractResultWrapper result = searchContract.StartContract();
                    if (result.Success)
                        MyVisualScriptLogicProvider.SendChatMessage($"Contract Started: {searchContract.Name}\n {searchContract.Description}",
                            "[Server]", playerId, "Green");
                    else
                    {
                        MyVisualScriptLogicProvider.SendChatMessage("Creation of the contract failed", "[Server]", playerId, "Red");
                    }
                    messageHandled = true;
                }
            }
            else if (lcommand[0].Equals("load gb".Replace(" ", ""))) // load grids/blocks
            {
                DSC_Blocks.Instance.Load();
                DSC_Grids.Instance.Load();
                MyVisualScriptLogicProvider.SendChatMessage($"Blocks and grids loaded", "[Server]", playerId, "Red");
                messageHandled = true;
            }
            else if (lcommand[0].Equals("save gb".Replace(" ", ""))) // save grids/blocks
            {
                DSC_Blocks.Instance.Save();
                DSC_Grids.Instance.Save();
                MyVisualScriptLogicProvider.SendChatMessage($"Blocks and grids saved", "[Server]", playerId, "Red");
                messageHandled = true;
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
