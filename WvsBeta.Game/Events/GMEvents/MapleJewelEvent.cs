using System.Linq;
using static WvsBeta.MasterThread;

namespace WvsBeta.Game.Events.GMEvents
{
    public class MapleJewelEvent : Event
    {
        private static readonly int JewelItem = 4031004;
        private static readonly Map HubMap = GameDataProvider.Maps[109010000];
        private static readonly Map[] Maps = {
            HubMap,
            GameDataProvider.Maps[109010100],
            GameDataProvider.Maps[109010101],
            GameDataProvider.Maps[109010102],
            GameDataProvider.Maps[109010103],
            GameDataProvider.Maps[109010200],
            GameDataProvider.Maps[109010201],
            GameDataProvider.Maps[109010202],
            GameDataProvider.Maps[109010203],
            GameDataProvider.Maps[109010300],
            GameDataProvider.Maps[109010301],
            GameDataProvider.Maps[109010302],
            GameDataProvider.Maps[109010303],
            GameDataProvider.Maps[109010400],
            GameDataProvider.Maps[109010401],
            GameDataProvider.Maps[109010402],
            GameDataProvider.Maps[109010403]
        };
        private static readonly Map ExitMap = GameDataProvider.Maps[109050001];
        private static readonly Map RewardMap = GameDataProvider.Maps[109050000];
        private static readonly long EventRuntimeSeconds = 15 * 60 ;

        private RepeatingAction EndAction = null;

        public override void Prepare()
        {
            EventHelper.CloseAllPortals(Maps);
            base.Prepare();
        }

        public override void Join(GameCharacter chr)
        {
            base.Join(chr);
            chr.ChangeMap(HubMap.ID, HubMap.Portals["join00"]);
        }

        public override void Start(bool joinDuringEvent = false)
        {
            foreach (var map in Maps)
            {
                map.PortalsOpen = true;
                map.StartTimer(EventRuntimeSeconds);
            }

            foreach (var chr in HubMap.Characters.ToList())
            {
                //start everyone in same location
                chr.ChangeMap(HubMap.ID, HubMap.Portals["join00"]);
            }

            EndAction = RepeatingAction.Start("FTJWatcher", Stop, EventRuntimeSeconds * 1000, 0);

            base.Start(joinDuringEvent);
        }

        public override void Stop()
        {
            EndAction?.Stop();
            EndAction = null;

            var chrList = Maps.SelectMany(m => m.Characters).ToList();
            foreach (var chr in chrList)
            {
                chr.Inventory.TakeItem(JewelItem, (short)chr.Inventory.ItemAmountAvailable(JewelItem));
                chr.ChangeMap(ExitMap.ID);
            }

            foreach (var map in Maps)
            {
                map.Mobs.Clear();
                map.ReactorPool.Reset(false);
                map.DropPool.Clear();

                map.TimerEndTime = MasterThread.CurrentTime;
                map.OnEnter = null;
                map.OnExit = null;
                map.OnTimerEnd = null;
            }

            EventHelper.CloseAllPortals(Maps);

            base.Stop();
        }
    }
}
