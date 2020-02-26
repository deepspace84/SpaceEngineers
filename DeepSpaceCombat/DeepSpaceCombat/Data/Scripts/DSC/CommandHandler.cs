using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

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
            Save = 5//,
            //fg=2,
            //csc=3
        };

        public CommandHandler() { }

        public void HandleCommand(string messageText, long playerId)
        {
            bool messageHandled = false;
            //string command = messageText.ToLower().Replace(" ", "");//TODO: use exact message. ToLower and replace make it impossible to use commands containing parameters.
            string[] scommand = messageText.Split(" ");
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
                    case ECommand.FindGrids:
                        string startBlock = "DSC_Start";
                        if (lcommand.Count > 1)
                            startBlock = lcommand[1];
                        long blockID = 0l;
                        try
                        {
                            blockID = DSC_Blocks.Instance.AddBlockWithName(startBlock);//TODO: Use same instanceing as for CommandHandler @see Core.cs
                        } catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("ERROR: "+ex.Message, "[Server]", playerId); }
                        string searchGrid = "DSC_End";
                        if (lcommand.Count > 1)
                            searchGrid = lcommand[1];
                        long gridID = 0l;
                        try
                        {
                            gridID = DSC_Grids.Instance.AddGridWithName(searchGrid);//TODO: Use same instanceing as for CommandHandler @see Core.cs
                        } catch (Exception ex) { MyVisualScriptLogicProvider.SendChatMessage("ERROR: " + ex.Message, "[Server]", playerId); }
                        MyVisualScriptLogicProvider.SendChatMessage($"Block found: " + blockID.ToString() + " | grid: " + gridID.ToString(), "[Server]", playerId);
                        break;
                    case ECommand.SearchContract:
                        //TODO: Implement equivalent to existing...
                        break;
                    case ECommand.Load:
                        //TODO: Implement equivalent to existing... use parameter to select what is to be loaded
                        break;
                    case ECommand.Save:
                        //TODO: Implement equivalent to existing... use parameter to select what is to be saved
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
                    DSC_Grids.Instance.GetGridWithName("DSC_End"), 10, "Find the Target!");

                    bool contract = searchContract.StartContract();
                    if (contract)
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
