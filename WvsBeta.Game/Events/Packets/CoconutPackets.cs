using WvsBeta.Common.Sessions;
using WvsBeta.Game.Events.GMEvents;

namespace WvsBeta.Game.Events.Packets
{
    public enum CoconutType : byte
    {
        Spawn,
        Hit,
        Break,
        Destroy,
    }
    public static class CoconutPackets
    {
        public static Packet CoconutAttackPacket(short coconutId, short delay, CoconutType type)
        {
            var pw = new Packet(ServerMessages.COCONUT_ATTACK);
            pw.WriteShort(coconutId);
            pw.WriteShort(delay);
            pw.WriteByte((byte)type);
            return pw;
        }
        public static Packet CoconutScorePacket(short maple, short story)
        {
            var pw = new Packet(ServerMessages.COCONUT_SCORE);
            pw.WriteShort(maple);
            pw.WriteShort(story);
            return pw;
        }
        public static void HandleEvent(GameCharacter chr, Packet packet)
        {
            short CoconutID = packet.ReadShort();
            short CharStance = packet.ReadShort();
            HitCoconut(chr, CoconutID, CharStance);
        }

        public static void SendScore(Map map, short mapleScore, short storyScore)
        {
            var pw = CoconutScorePacket(mapleScore, storyScore);
            map.SendPacket(pw);
        }

        public static void SendScore(GameCharacter chr, short mapleScore, short storyScore)
        {
            var pw = CoconutScorePacket(mapleScore, storyScore);
            chr.SendPacket(pw);
        }

        public static void SendCoconut(Map map, short coconutId, short delay, CoconutType type)
        {
            var pw = CoconutAttackPacket(coconutId, delay, type);
            map.SendPacket(pw);
        }

        public static void HitCoconut(GameCharacter chr, short cID, short Stance)
        {
            var pw = CoconutAttackPacket(cID, Stance, CoconutType.Hit);
            chr.SendPacket(pw);
        }

        public static void ForcedEquip(GameCharacter chr, EventTeam team)
        {
            Packet pw = new Packet(ServerMessages.FIELD_SPECIFIC_DATA);
            pw.WriteByte((byte)team);
            chr.SendPacket(pw);
        }
    }
}
