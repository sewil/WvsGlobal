using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects.Stats;

namespace WvsBeta.Common.Objects.BuffStats
{
    public class BuffStat_ComboAttack : BuffStat
    {
        public int MaxOrbs { get; set; }

        public BuffStat_ComboAttack(BuffValueTypes flag) : base(flag)
        {
        }

        public override BuffValueTypes Set(int referenceId, short nValue, long expireTime)
        {
            MaxOrbs = nValue;
            return base.Set(referenceId, 1, expireTime);
        }
    }
}
