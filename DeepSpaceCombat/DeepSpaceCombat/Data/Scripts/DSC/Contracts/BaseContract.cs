using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;

namespace DSC
{
    /// <summary>
    /// Base class for all contracts
    /// </summary>
    abstract class DSC_BaseContract
    {
        protected string _name;
        protected int _reward;
        protected long _startBlockId;
        protected int _collateral;
        protected int _duration;
        protected long _targetGridId;
        protected double _searchRadius;
        protected long _playerId;

        protected IMyContractSearch _contract;

        protected string _description;

        public string Name
        {
            get
            {
                return _name;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
        }

        public int Reward
        {
            get
            {
                return _reward;
            }
        }

        protected DSC_BaseContract(string name, int reward, long startBlockId, int collateral, int duration, long targetGridId, double searchRadius, string description, long playerId)
        {
            _name = name;
            _reward = reward;
            _startBlockId = startBlockId;
            _collateral = collateral;
            _duration = duration;
            _targetGridId = targetGridId;
            _searchRadius = searchRadius;
            _description = description;
            _playerId = playerId;
        }

        public bool StartContract()
        {
            MyCubeBlock block = MyAPIGateway.Entities.GetEntityById(_startBlockId) as MyCubeBlock;
            bool result = false;

            IMyPlayer player = Util.FindPlayerById(_playerId);

            if(player == null)
            {
                MyVisualScriptLogicProvider.SendChatMessage($"Player could not be found", "[Server]", _playerId);
            }

            long balance = -1;

            lock (block)
            {
                long ownerID = block.OwnerId;
                try
                {
                    // change owner of block and then get the owner as player
                    block.ChangeOwner(_playerId, VRage.Game.MyOwnershipShareModeEnum.All);

                    player.TryGetBalanceInfo(out balance);

                    player.RequestChangeBalance(_reward);
                    MyVisualScriptLogicProvider.SendChatMessage($"Player account changed {balance + _reward}", "[Server]", _playerId);

                    result = MyAPIGateway.ContractSystem.AddContract(_contract).Success;

                }
                catch
                {
                    MyVisualScriptLogicProvider.SendChatMessage($"Something went wrong {_playerId} : {balance} ", "[Server]", _playerId);
                    if (balance >= 0)
                    {
                        long newBalance = -1;
                        player.TryGetBalanceInfo(out newBalance);

                        if (newBalance > 0 && balance != newBalance)
                            player.RequestChangeBalance(balance - newBalance);

                    }
                }
                finally
                {
                    block.ChangeOwner(ownerID, VRage.Game.MyOwnershipShareModeEnum.All);                    
                }
            }

            return result;
        }

        public static bool CreateFromXML(string file)
        {
            // MyAPIGateway.Utilities.SerializeToXML
            using (TextReader reader = MyAPIGateway.Utilities.ReadFileInGlobalStorage(file))
            {
                MyAPIGateway.Utilities.SerializeFromXML<Object>(reader.ReadToEnd());

            }
            return false;
        }
    }
}
