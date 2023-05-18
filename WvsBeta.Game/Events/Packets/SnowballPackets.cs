using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Events.Packets
{
    public static class SnowballPackets
    {
        public static Packet SnowballState(byte State, int snowmanhp1, int snowmanhp2, short x1, byte hp1, short x2, byte hp2, short DamageSnowBall, short SnowmanDmg1, short SnowmanDmg2)
        {
            Packet pw = new Packet(ServerMessages.SNOWBALL_STATE);
            pw.WriteByte(State);
            pw.WriteInt(snowmanhp1); // Not used in v12, used later
            pw.WriteInt(snowmanhp2); // Not used in v12, used later
            pw.WriteShort(x1);
            pw.WriteByte(hp1);
            pw.WriteShort(x2);
            pw.WriteByte(hp2);
            pw.WriteShort(DamageSnowBall);
            pw.WriteShort(SnowmanDmg1);
            pw.WriteShort(SnowmanDmg2);
            return pw;
        }

        public static Packet SnowballHit(byte type, short damage, short delay)
        {
            Packet pw = new Packet(ServerMessages.SNOWBALL_HIT);
            pw.WriteByte(type);
            pw.WriteShort(damage);
            pw.WriteShort(delay);
            return pw;
        }
    }

}
