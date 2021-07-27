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
using VRage.ModAPI;
using VRageMath;

namespace DSC.NPCCleanup
{
    public class CommandHandler
    {

        public enum ECommand
        {
            Test = 0,

        };

        public enum ECommanPublic
        {
            Test = 0
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
                        case ECommand.Test:

                            // Get all grid entities
                            HashSet<IMyEntity> entList = new HashSet<IMyEntity>();
                            MyAPIGateway.Entities.GetEntities(entList, e => e is IMyCubeGrid);
                            if (entList.Count == 0)
                                return;

                            // Loop through all Grids
                            foreach (IMyEntity ent in entList)
                            {
                                MyCubeGrid grid = ent as MyCubeGrid;

                                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Available grid=> -" + grid.DisplayName + "- | EntityId=> -" + grid.EntityId.ToString()+ " Owner=>" + grid.BigOwners.FirstOrDefault().ToString()+" - Name=>"+MyVisualScriptLogicProvider.GetPlayersName(grid.BigOwners.FirstOrDefault()));
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
                    case ECommanPublic.Test:


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
