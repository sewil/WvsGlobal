using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects.Stats;

namespace WvsBeta.Common.Objects.BuffStats
{
    public class BuffStat_MesoGuard : BuffStat
    {
        public int MesosLeft { get; set; }

        public BuffStat_MesoGuard(BuffValueTypes flag) : base(flag)
        {
        }
    }
}
