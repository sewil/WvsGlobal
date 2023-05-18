using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Game.Events.GMEvents;

namespace WvsBeta.Game.Events
{
    abstract class TeamEvent : EventFieldSet
    {
        protected readonly HashSet<GameCharacter> MapleTeam;
        protected readonly HashSet<GameCharacter> StoryTeam;
        protected TeamEvent(ConfigReader.Node fsNode) : base(fsNode)
        {
            MapleTeam = new HashSet<GameCharacter>();
            StoryTeam = new HashSet<GameCharacter>();
        }
        public override void Enable()
        {
            base.Enable();
            MapleTeam.Clear();
            StoryTeam.Clear();
        }
    }
}
