using WvsBeta.Common;

namespace WvsBeta.Game.Events.GMEvents
{
    class MapleFitnessEvent : EventFieldSet
    {
        public MapleFitnessEvent(ConfigReader.Node fsNode) : base(fsNode)
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
            RunTimer();
            EnablePortals();
        }
    }
}
