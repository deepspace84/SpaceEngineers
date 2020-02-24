using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSC
{
    class DSC_SearchContractBase : DSC_BaseContract
    {
        public DSC_SearchContractBase(string name, int reward, long startBlockId, int collateral, int duration, long targetGridId, double searchRadius, string description) :
            base(name, reward, startBlockId, collateral, duration, targetGridId, searchRadius, description)
        {
        }

        public override long StartContract()
        {
            long contractId;
            MyVisualScriptLogicProvider.AddSearchContract(_startBlockId, _reward, _collateral, _duration, _targetGridId, _searchRadius, out contractId);

            MyVisualScriptLogicProvider.SendChatMessage(_description,"", 0, "Red");
            
            return contractId;
        }
    }
}
