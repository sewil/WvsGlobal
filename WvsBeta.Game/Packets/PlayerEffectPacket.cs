using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    public enum PlayerEffectType
    {
        SkillOnSelf = 1,
        SkillOnOther = 2,
        Pet = 4
    }
    public enum PetEffectType
    {
        LevelUp = 0,
        Tp = 1,
        TpBack = 2
    }
    public class PlayerEffectPacket
    {
        private Packet foreignpw;
        private Packet localpw;
        private GameCharacter chr;
        private PlayerEffectPacket(GameCharacter chr, PlayerEffectType type)
        {
            this.chr = chr;
            foreignpw = new Packet(ServerMessages.SHOW_FOREIGN_EFFECT);
            foreignpw.WriteInt(chr.ID);
            foreignpw.WriteByte((byte)type);

            localpw = new Packet(ServerMessages.PLAYER_EFFECT);
            localpw.WriteByte((byte)type);
        }
        private void Send(bool foreignOnly, bool localOnly)
        {
            if (!localOnly)
            {
                chr.Field.SendPacket(chr, foreignpw);
            }

            if (!foreignOnly)
            {
                chr.SendPacket(localpw);
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
                p.localpw.WriteInt(skillId);
                p.localpw.WriteByte(skillLevel);
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
                p.localpw.WriteByte((byte)type);
            }
            p.Send(foreignOnly, localOnly);
        }
    }
}
