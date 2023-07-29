using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game
{
    public class DropPool
    {
        private const bool _MergeDrops = false;
        public Map Field { get; set; }
        private LoopingID _DropIdCounter { get; set; }
        public const long DropExpireTime = 3 * 60 * 1000;
        public bool DropEverlasting { get; set; }
        public Dictionary<int, Drop> Drops { get; private set; } = new Dictionary<int, Drop>();

        public DropPool(Map field)
        {
            Field = field;
            _DropIdCounter = new LoopingID();
        }

        public bool Create(Reward reward, int ownerId, int ownPartyId, DropType dropType, int sourceId, Pos currentPos, int x2, short delay, bool byPet, bool byPlayer)
        {
            bool big = !reward.Mesos && reward.Data.BigSize;
            if (big && sourceId > 0 && !byPlayer) x2 += 10;

            var Foothold = Field.GetFootholdUnderneath(x2, currentPos.Y - 100, out int y2);

            if (Foothold == null || !Field.IsPointInMBR(x2, y2, true))
                Foothold = Field.GetFootholdClosest(x2, currentPos.Y, ref x2, ref y2, currentPos.X);

            if (big)
            {
                y2 -= 50;
            }

            Drop drop = new Drop(_DropIdCounter.NextValue(), reward, ownerId, ownPartyId, dropType, sourceId, currentPos.X, currentPos.Y, (short)x2, (short)y2, byPet, byPlayer)
            {
                Field = Field,
                CreateTime = MasterThread.CurrentTime,
                Everlasting = DropEverlasting,
                ConsumeOnPickup = (!reward.Mesos && false/*DataProvider.ConsumeOnPickup.Contains(Reward.ItemID)*/)
            };

            if (byPlayer && !drop.Reward.Mesos && (reward.Data.IsQuest || reward.Data.IsTradeBlock))
                DropPacket.SendMakeEnterFieldPacket(drop, RewardEnterType.DisappearDuringDrop, delay);
            else
            {
                Drops.Add(drop.DropID, drop);
                DropPacket.SendMakeEnterFieldPacket(drop, RewardEnterType.DropAnimation, delay);
            }
            return false;
        }

        #region Update
        public void Update(long tCur)
        {
            if (DropEverlasting) return;

            foreach (var Drop in Drops.Values.ToList())
            {
                if ((!Drop.Everlasting && (tCur - Drop.CreateTime) > DropExpireTime) || Drop.Field.ReactorPool.CheckDropTriggers(tCur, Drop))
                {
                    RemoveDrop(Drop);
                }
            }
        }
        #endregion

        public Drop GetDrop(int DropID)
        {
            Drops.TryGetValue(DropID, out Drop Result);

            return Result;
        }

        public void Clear(RewardLeaveType rlt = RewardLeaveType.Normal)
        {
            foreach (Drop Drop in new List<Drop>(Drops.Values))
            {
                RemoveDrop(Drop, rlt);
            }
        }

        public void OnEnter(GameCharacter User)
        {
            foreach (Drop Drop in Drops.Values)
            {
                DropPacket.SendMakeEnterFieldPacket(Drop, RewardEnterType.ShowExisting, 0, User);
            }
        }

        public void RemoveDrop(Drop Drop, RewardLeaveType Type = RewardLeaveType.Normal, int Option = 0)
        {
            if (Drops.Remove(Drop.DropID))
                DropPacket.SendMakeLeaveFieldPacket(Drop, Type, Option);
        }

        public void EncodeForMigration(Packet pw)
        {
            pw.WriteInt(_DropIdCounter.Current);
            pw.WriteInt(Drops.Count);
            Drops.ForEach(x => x.Value.EncodeForMigration(pw));
        }

        public void DecodeForMigration(Packet pr)
        {
            _DropIdCounter.Reset(pr.ReadInt());
            int amount = pr.ReadInt();
            Drops = new Dictionary<int, Drop>(amount);

            Program.MainForm.LogAppend(Field.ID + " has " + amount + " drops...");
            for (var i = 0; i < amount; i++)
            {
                var drop = Drop.DecodeForMigration(pr);
                drop.Field = Field;
                Drops.Add(drop.DropID, drop);
            }
        }
    }

    public enum DropType : byte
    {
        Normal = 0,
        Party = 1,
        FreeForAll = 2,
        Explosive = 3
    }

    public enum RewardEnterType
    {
        ShowDrop = 0,
        DropAnimation = 1,
        ShowExisting = 2,
        DisappearDuringDrop = 3
    }

    public enum RewardLeaveType
    {
        Normal = 0,
        Party = 1,
        FreeForAll = 2,
        Remove = 3,
        Explode = 4,
        PetPickup = 5
    }
}
