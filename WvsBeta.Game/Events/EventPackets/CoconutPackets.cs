using System.Windows.Forms;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game
{
    public static class CoconutPackets
    {
        public class CoconutAttackPacket : Packet
        {
            public enum Type : byte
            {
                Spawn,
                Hit,
                Break,
                Destroy
            }
            public CoconutAttackPacket(short coconutId, short delay, Type type) : base(ServerMessages.COCONUT_ATTACK)
            {
                WriteShort(coconutId);
                WriteShort(delay);
                WriteByte((byte)type);
            }
        }
        public class CoconutScorePacket : Packet
        {
            public CoconutScorePacket(short maple, short story) : base(ServerMessages.COCONUT_SCORE)
            {
                WriteShort(maple);
                WriteShort(story);
            }
        }
        public static void HandleEvent(GameCharacter chr, Packet packet)
        {
            short CoconutID = packet.ReadShort();
            short CharStance = packet.ReadShort();
            HitCoconut(chr, CoconutID, CharStance);

        }

        public static void CoconutScore(GameCharacter chr, short maple, short story)
        {
            var pw = new CoconutScorePacket(maple, story);
            chr.Field.SendPacket(pw, chr, false);
        }

        public static void SpawnCoconut(GameCharacter chr, bool spawn, int id, int type)
        {
            var pw = new CoconutAttackPacket(0, 0, CoconutAttackPacket.Type.Spawn);
            chr.Field.SendPacket(pw);
        }

        public static void HitCoconut(GameCharacter chr, short cID, short Stance)
        {
            var pw = new CoconutAttackPacket(cID, Stance, CoconutAttackPacket.Type.Hit);
            chr.SendPacket(pw);
        }

        public static void ForcedEquip(GameCharacter chr, byte team)
        {
            Packet pw = new Packet(ServerMessages.FIELD_SPECIFIC_DATA); // 44, pressumably 47 in v12
            pw.WriteByte(team); //0 : red, 1 : blue
            chr.Field.SendPacket(pw);
        }


    }
}
