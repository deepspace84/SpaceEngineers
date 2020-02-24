using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;

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

        protected DSC_BaseContract(string name, int reward, long startBlockId, int collateral, int duration, long targetGridId, double searchRadius, string description)
        {
            _name = name;
            _reward = reward;
            _startBlockId = startBlockId;
            _collateral = collateral;
            _duration = duration;
            _targetGridId = targetGridId;
            _searchRadius = searchRadius;
            _description = description;
        }

        public abstract long StartContract();
    }
}
