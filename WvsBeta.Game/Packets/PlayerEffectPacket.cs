using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    public enum PlayerEffectType
    {
        SkillOnSelf = 1,
        SkillOnOther = 2,
        Pet = 4,
        UseEXPCharm = 6,
    }
    public enum PetEffectType
    {
        LevelUp = 0,
        Tp = 1,
        TpBack = 2
    }
    public class PlayerEffectPacket : Packet
    {
        private Packet foreignpw;
        private GameCharacter chr;
        private PlayerEffectPacket(GameCharacter chr, PlayerEffectType type) : base(ServerMessages.PLAYER_EFFECT)
        {
            this.chr = chr;
            foreignpw = new Packet(ServerMessages.SHOW_FOREIGN_EFFECT);
            foreignpw.WriteInt(chr.ID);
            foreignpw.WriteByte((byte)type);

            WriteByte((byte)type);
        }
        private void Send(bool foreignOnly, bool localOnly)
        {
            if (!localOnly)
            {
                chr.Field.SendPacket(chr, foreignpw);
            }

            if (!foreignOnly)
            {
                chr.SendPacket(this);
            }
        }
        public static void SendSkill(GameCharacter chr, int skillId, byte skillLevel, bool foreignOnly = false, bool localOnly = false, bool onOther = false)
        {
            var p = new PlayerEffectPacket(chr, onOther ? PlayerEffectType.SkillOnOther : PlayerEffectType.SkillOnSelf);
            if (!localOnly)
            {
                p.foreignpw.WriteInt(skillId);
                p.foreignpw.WriteByte(skillLevel);
            }
            if (!foreignOnly)
            {
                p.WriteInt(skillId);
                p.WriteByte(skillLevel);
            }
            p.Send(foreignOnly, localOnly);
        }

        public static void SendPetEffect(GameCharacter chr, PetEffectType type, bool foreignOnly = false, bool localOnly = false)
        {
            var p = new PlayerEffectPacket(chr, PlayerEffectType.Pet);
            if (!localOnly)
            {
                p.foreignpw.WriteByte((byte)type);
            }
            if (!foreignOnly)
            {
                p.WriteByte((byte)type);
            }
            p.Send(foreignOnly, localOnly);
        }

        public static void SendUseEXPCharm(GameCharacter chr, bool isSafetyCharm, int itemid, byte daysLeft, byte timesLeft)
        {
            var pw = new PlayerEffectPacket(chr, PlayerEffectType.UseEXPCharm);
            pw.WriteBool(isSafetyCharm);
            pw.WriteByte(timesLeft);
            pw.WriteByte(daysLeft);
            if (!isSafetyCharm)
            {
                pw.WriteInt(itemid);
            }
            pw.Send(false, true);
        }
    }
}
