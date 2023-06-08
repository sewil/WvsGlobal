using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects.Stats;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game
{
    public static class BuffPacket
    {
        public static void EncodeForRemote(GameCharacter chr, Packet pw, BuffValueTypes pBuffFlags = BuffValueTypes.ALL)
        {
            GameCharacterPrimaryStats ps = (GameCharacterPrimaryStats)chr.PrimaryStats;
            long currentTime = BuffStat.GetTimeForBuff();
            BuffValueTypes added = 0;

            int tmp = pw.Position;
            pw.WriteULong((ulong)pBuffFlags);

            ps.BuffSpeed.EncodeForRemote(ref added, currentTime, stat => pw.WriteByte((byte)stat.N), pBuffFlags);
            ps.BuffComboAttack.EncodeForRemote(ref added, currentTime, stat => pw.WriteByte((byte)stat.N), pBuffFlags);
            ps.BuffCharges.EncodeForRemote(ref added, currentTime, stat => pw.WriteInt(stat.R), pBuffFlags);
            ps.BuffStun.EncodeForRemote(ref added, currentTime, stat => pw.WriteInt(stat.R), pBuffFlags);
            ps.BuffDarkness.EncodeForRemote(ref added, currentTime, stat => pw.WriteInt(stat.R), pBuffFlags);
            ps.BuffSeal.EncodeForRemote(ref added, currentTime, stat => pw.WriteInt(stat.R), pBuffFlags);
            ps.BuffWeakness.EncodeForRemote(ref added, currentTime, stat => pw.WriteInt(stat.R), pBuffFlags);
            ps.BuffPoison.EncodeForRemote(ref added, currentTime, stat =>
            {
                pw.WriteShort(stat.N);
                pw.WriteInt(stat.R);
            }, pBuffFlags);
            ps.BuffSoulArrow.EncodeForRemote(ref added, currentTime, null, pBuffFlags);
            ps.BuffShadowPartner.EncodeForRemote(ref added, currentTime, null, pBuffFlags);
            ps.BuffDarkSight.EncodeForRemote(ref added, currentTime, null, pBuffFlags);

            pw.SetULong(tmp, (ulong)added);
        }

        public static void AddBuffs(GameCharacter chr, BuffValueTypes flags, short pDelay = 0)
        {
            if (flags == BuffValueTypes.None) return;
            Packet pw = new Packet(ServerMessages.BUFFS_ADD);
            chr.PrimaryStats.EncodeForLocal(pw, flags);
            pw.WriteShort(pDelay);
            if ((flags & BuffValueTypes.MOVEMENT_INFO_INDEX) != 0)
            {
                pw.WriteByte(0); // FIX: This should be the 'movement info index'
            }
            chr.SendPacket(pw);
        }

        public static void RemoveBuffs(GameCharacter chr, BuffValueTypes flags)
        {
            if (flags == 0) return;

            Packet pw = new Packet(ServerMessages.BUFFS_REMOVE);
            pw.WriteULong((ulong)flags);
            var diff = (ulong)(flags & BuffValueTypes.MOVEMENT_INFO_INDEX);
            if (diff != 0)
            {
                pw.WriteByte(0); // FIX: This should be the 'movement info index'
            }
            chr.SendPacket(pw);
        }
    }
}