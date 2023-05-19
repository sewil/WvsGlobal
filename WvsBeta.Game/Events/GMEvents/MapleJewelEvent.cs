using WvsBeta.Common;

namespace WvsBeta.Game.Events.GMEvents
{
    public class MapleJewelEvent : EventFieldSet
    {
        private static readonly int JewelItem = 4031004;

        public MapleJewelEvent(ConfigReader.Node fsNode) : base(fsNode)
        {
        }

        public override EnterStatus Enter(GameCharacter chr, int mapIdx)
        {
            string portalName = chr.IsAdmin ? "gm00" : "join00";
            return Enter(chr, mapIdx, portalName);
        }

        public override void Start()
        {
            base.Start();
            EnablePortals();
            RunTimer();
        }

        public override void End()
        {
            foreach (var chr in Characters)
            {
                chr.Inventory.TakeItem(JewelItem, (short)chr.Inventory.ItemAmountAvailable(JewelItem));
            }
            base.End();
        }
    }
}
