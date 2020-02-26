using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.ModAPI;

namespace DSC
{
    public class DSC_Players
    {
        public List<DSC_Player> Players { get; private set; } = new List<DSC_Player>();
        
        #region player handler

        public void PlayerConnected(long playerId)
        {
            DeepSpaceCombat.Instance.Networking.SendToServer(
                new PackagePlayerLanguage(playerId, MyAPIGateway.Session.Config.Language));
        }

        public void AddPlayer(long playerId, MyLanguagesEnum lang)
        {
            if (!Players.TrueForAll(p => p.Id != playerId))
                return;

            DeepSpaceCombat.Instance.ServerLogger.WriteInfo($"New Player connected - {playerId}: {lang}");

            DSC_Player player = new DSC_Player(Util.FindPlayerById(playerId), playerId, lang);

            Players.Add(player);
        }

        #endregion
    }

    public class DSC_Player
    {
        public IMyPlayer Player { get; private set; }
        public long Id { get; private set; }
        public MyLanguagesEnum Language { get; private set; }

        public DSC_Player(IMyPlayer player, long id, MyLanguagesEnum language)
        {
            Player = player;
            Id = id;
            Language = language;
        }
    }
}
