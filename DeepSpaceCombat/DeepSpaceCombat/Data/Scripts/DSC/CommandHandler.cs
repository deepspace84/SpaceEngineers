using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSC
{
    class CommandHandler
    {
        private static CommandHandler _instance;

        public static CommandHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CommandHandler();
                return _instance;
            }
        }

        public void HandleCommand(string messageText, long playerId)
        {
            string command = messageText.ToLower().Replace(" ", "");
            bool messageHandled = false;
            if (command.Equals("test"))
            {
                messageHandled = true;
            }
            if (command.Equals("help"))
            {
                PrintHelp(playerId);
                messageHandled = true;
            }
            else if (command.Equals("fg".Replace(" ", ""))) // find grids
            {
                long blockID = DSC_Blocks.Instance.AddBlockWithName("DSC_Start");
                long gridID = DSC_Grids.Instance.AddGridWithName("DSC_End");
                MyVisualScriptLogicProvider.SendChatMessage($"Block found: "+blockID.ToString()+" | grid: "+gridID.ToString(), "[Server]", playerId);

                MyVisualScriptLogicProvider.SendChatMessage($"Blocks found: " + 
                    $"{DSC_Blocks.Instance.GetBlockWithName("DSC_Start") > 0 && DSC_Grids.Instance.GetGridWithName("DSC_End") > 0}", "[Server]", playerId);

                messageHandled = true;
            }
            else if (command.Equals("csc".Replace(" ", ""))) // create search contract
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
            else if (command.Equals("load gb".Replace(" ", ""))) // load grids/blocks
            {
                DSC_Blocks.Instance.Load();
                DSC_Grids.Instance.Load();
                MyVisualScriptLogicProvider.SendChatMessage($"Blocks and grids loaded", "[Server]", playerId, "Red");
                messageHandled = true;
            }
            else if (command.Equals("save gb".Replace(" ", ""))) // save grids/blocks
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
