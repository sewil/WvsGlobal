using System.Linq;
using WvsBeta.Common;

namespace WvsBeta.Game.Handlers.Contimove
{
    public class MobGenEvent : IContiEvent
    {
        public bool EventInProgress { get; private set; }
        private readonly int mobItemId;
        private long mobGenTime;
        private readonly Pos mobSpawnPoint;
        private int spawnChance;
        private int withinMin;
        private int minOffset;
        private bool doSpawn;
        private readonly CONTIMOVE conti;
        public MobGenEvent(CONTIMOVE conti, int mobItemId, Pos mobSpawnPoint, int spawnChance, int withinMin, int minOffset)
        {
            this.conti = conti;
            this.mobItemId = mobItemId;
            this.mobSpawnPoint = mobSpawnPoint;
            this.spawnChance = spawnChance;
            this.withinMin = withinMin;
            this.minOffset = minOffset;
        }
        public void ResetEvent()
        {
            if (EventInProgress)
            {
                DestroyMob();
            }
            doSpawn = (Rand32.Next() % 100) <= spawnChance;

            if (doSpawn)
            {
                int randomMinutes = (int)(Rand32.Next() % withinMin);
                mobGenTime = conti.NextBoardingTime + (60000 * (conti.WaitMin + randomMinutes + minOffset));
                var curTime = MasterThread.CurrentTime;
                var timeTillSpawn = mobGenTime - curTime;
                Program.MainForm.LogAppend($"Will spawn mobs on trip {conti.Name} (map {conti.FieldsShip[0]}) in {(timeTillSpawn / 1000):D} seconds");
            }
        }

        void SummonMob()
        {
            if (conti.FieldsShip[0] == Constants.InvalidMap) return;
            if (!GameDataProvider.Maps.TryGetValue(conti.FieldsShip[0], out var field)) return;
            if (field.Limitations.HasFlag(FieldLimit.SummonLimit)) return;
            if (!GameDataProvider.Items.TryGetValue(mobItemId, out var itemData)) return;

            EventInProgress = true;

            Program.MainForm.LogAppend($"Spawning mobs for contimove trip {conti.Name} on map " + field.ID);

            var fh = field.GetFootholdUnderneath(mobSpawnPoint.X, mobSpawnPoint.Y, out var maxY);

            foreach (var itemDataSummon in itemData.Summons)
            {
                if (Rand32.Next() % 100 >= itemDataSummon.Chance) continue;
                field.SpawnMobWithoutRespawning(itemDataSummon.MobID, mobSpawnPoint, (byte)(fh.HasValue ? fh.Value.ID : 0));
            }
            ContinentMan.SendContiPacket(
                conti.FieldsShip[0],
                Conti.TargetFieldMove,
                Conti.Mobgen
            );
        }

        void DestroyMob()
        {
            doSpawn = false;
            EventInProgress = false;
            if (!GameDataProvider.Maps.TryGetValue(conti.FieldsShip[0], out var field)) return;
            if (field.Mobs.Count == 0) return;

            Program.MainForm.LogAppend($"Removing mobs for contimove trip {conti.Name} on map " + field.ID);
            foreach (var mob in field.Mobs.Values.ToArray())
            {
                mob.ForceDead();
            }
        }
        public bool HandleMove(long systemTime)
        {
            bool handled = false;
            if (!EventInProgress && doSpawn && systemTime - mobGenTime >= 0)
            {
                doSpawn = false;
                SummonMob();
                handled = true;
            }
            return handled;
        }
    }
}
