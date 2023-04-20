using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    public class FieldEffectPacket : Packet
    {
        public enum Type : byte
        {
            Summon = 0,
            QuestEffect = 2,
            Effect = 3,
            Sound = 4,
            BossHPBar = 5,
            Music = 6,
        }
        private FieldEffectPacket(Type type) : base(ServerMessages.FIELD_EFFECT)
        {
            WriteByte((byte)type);
        }
        public static FieldEffectPacket Summon(byte id, int x, int y)
        {
            var pw = new FieldEffectPacket(Type.Summon);
            pw.WriteByte(id);
            pw.WriteInt(x);
            pw.WriteInt(y);
            return pw;
        }
        public static FieldEffectPacket EffectScreen(string path)
        {
            var pw = new FieldEffectPacket(Type.Effect);
            pw.WriteString(path);
            return pw;
        }
        public static FieldEffectPacket EffectSound(string path)
        {
            var pw = new FieldEffectPacket(Type.Sound);
            pw.WriteString(path);
            return pw;
        }
        public static FieldEffectPacket EffectMusic(string path)
        {
            var pw = new FieldEffectPacket(Type.Music);
            pw.WriteString(path);
            return pw;
        }
        public static FieldEffectPacket BossHPBar(int mobId, int pHP, int pMaxHP, byte pColorBottom, byte pColorTop)
        {
            var pw = new FieldEffectPacket(Type.BossHPBar);
            pw.WriteInt(mobId);
            pw.WriteInt(pHP);
            pw.WriteInt(pMaxHP);
            pw.WriteByte(pColorTop);
            pw.WriteByte(pColorBottom);
            return pw;
        }

        public static FieldEffectPacket EffectObject(string objectName)
        {
            var pw = new FieldEffectPacket(Type.QuestEffect);
            pw.WriteByte(2); // Portal type? 0 1 2 3 4
            pw.WriteString(objectName);
            return pw;
        }
    }

}
