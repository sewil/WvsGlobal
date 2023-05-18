using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;

namespace WvsBeta.Game.Events.GMEvents
{
    public class MapleOlaEvent : EventFieldSet
    {
        public MapleOlaEvent(ConfigReader.Node fsNode) : base(fsNode)
        {
        }
        public override EnterStatus Enter(GameCharacter chr, int mapIdx)
        {
            string portalName = chr.IsAdmin ? "t002" : "join00";
            return Enter(chr, mapIdx, portalName);
        }

        public override void Start()
        {
            base.Start();
            EnablePortals();
        }
    }
}
