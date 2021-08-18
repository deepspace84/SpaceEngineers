using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LST
{
    [ProtoContract]
    [Serializable]
    public class LST_Config_Main
    {

        [ProtoMember(1)]
        public int StationRange = 1000;
        [ProtoMember(2)]
        public int ShipRange = 500;

        internal LST_Config_Main Clone()
        {
            return new LST_Config_Main
            {
                StationRange = StationRange,
                ShipRange = ShipRange
            };
        }
    }
}
