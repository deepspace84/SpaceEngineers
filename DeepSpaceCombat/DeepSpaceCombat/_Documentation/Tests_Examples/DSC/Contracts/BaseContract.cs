using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;

namespace DSC_TEST
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

        /// <summary>
        /// TODO syncronize -> maybe different people want to create contract on same block/grid
        /// </summary>
        /// <returns></returns>
        public MyAddContractResultWrapper StartContract()
        {
            MyCubeBlock block = MyAPIGateway.Entities.GetEntityById(_startBlockId) as MyCubeBlock;
            MyCubeGrid grid = MyAPIGateway.Entities.GetEntityById(_targetGridId) as MyCubeGrid;

            VRage.Collections.ListReader<MyCubeBlock> gridBlocks = grid.GetFatBlocks();
            Dictionary<MyCubeBlock, long> dictBlockUser = new Dictionary<MyCubeBlock, long>(); // dictionary for all cubeblocks and owner ids

            MyAddContractResultWrapper result = new MyAddContractResultWrapper();

            long balance = -1;
            long blockOwnerID = block.OwnerId;
            IMyPlayer player = Util.FindPlayerById(_playerId);
            try
            {
                // change owner of block and then get the owner as player
                block.ChangeOwner(_playerId, VRage.Game.MyOwnershipShareModeEnum.All);
                foreach (MyCubeBlock b in gridBlocks)
                {
                    dictBlockUser.Add(b, blockOwnerID);
                    b.ChangeOwner(_playerId, VRage.Game.MyOwnershipShareModeEnum.All);
                }
                
                // need to recalcualte owners
                grid.RecalculateOwners();

                if (player != null) // dont care about NPC
                    player.TryGetBalanceInfo(out balance);
                MyAPIGateway.Players.RequestChangeBalance(_playerId, _reward);

                result = MyAPIGateway.ContractSystem.AddContract(_contract);

            }
            catch
            {
                if(player != null)
                    MyVisualScriptLogicProvider.SendChatMessage($"Something went wrong {_playerId} : {balance} ", "[Server]", _playerId);
                else
                    MyVisualScriptLogicProvider.SendChatMessage($"Something went wrong {_playerId} : {balance} ", "[Server]");

                if (balance >= 0)
                {
                    long newBalance = -1;
                    if (player != null) {
                        player.TryGetBalanceInfo(out newBalance);

                        if (newBalance > 0 && balance != newBalance)
                            player.RequestChangeBalance(balance - newBalance);
                    }
                    else
                    {
                        MyAPIGateway.Players.RequestChangeBalance(_playerId, -Reward);
                    }
                }
            }
            finally
            {
                block.ChangeOwner(blockOwnerID, VRage.Game.MyOwnershipShareModeEnum.All);
                foreach (KeyValuePair<MyCubeBlock, long> pair in dictBlockUser)
                {
                    pair.Key.ChangeOwner(pair.Value, VRage.Game.MyOwnershipShareModeEnum.All);
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
