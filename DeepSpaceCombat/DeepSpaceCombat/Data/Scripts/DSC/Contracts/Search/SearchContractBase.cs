using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;

namespace DSC
{
    class DSC_SearchContractBase : DSC_BaseContract
    {
        public DSC_SearchContractBase(string name, int reward, long startBlockId, int collateral, int duration, long targetGridId, double searchRadius, string description) :
            base(name, reward, startBlockId, collateral, duration, targetGridId, searchRadius, description)
        {
        }

        public override bool StartContract()
        {
            IMyContractSearch contract;
            contract = new MyContractSearch(_startBlockId, _reward, _collateral, _duration, _targetGridId, _searchRadius);
            MyAddContractResultWrapper result = MyAPIGateway.ContractSystem.AddContract(contract);

            return result.Success;
        }
    }
}
