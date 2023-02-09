using System;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    class WhisperPacket : Packet
    {
        enum WhisperType : byte
        {
            Find = 9,
            NotFound = 10,
            Unk11 = 11,
            DirectionFrom = 10,
            DirectionTo = 18,
        }
        enum FindType : byte
        {
            Map = 1,
            CashShop = 2,
            Channel = 3,
        }
        private WhisperPacket(WhisperType type) : base(ServerMessages.WHISPER)
        {
            WriteByte((byte)type);
        }
        public static void Whisper(GameCharacter victim, string who, byte channel, string message, bool directionFrom)
        {
            var pw = new WhisperPacket(directionFrom ? WhisperType.DirectionFrom : WhisperType.DirectionTo);
            pw.WriteString(who);
            pw.WriteByte(channel);
            pw.WriteString(message);
            victim.SendPacket(pw);
        }

        public static void Find(GameCharacter victim, string who, int map, sbyte dunno, bool sameChannel)
        {
            var pw = new WhisperPacket(map != -1 ? WhisperType.Find : WhisperType.NotFound);

            if (map != -1)
            {
                pw.WriteString(who);
                FindType findType = map == -2 ? FindType.CashShop : sameChannel ? FindType.Map : FindType.Channel;
                int mapId = Math.Max(0, map);
                pw.WriteByte((byte)findType);
                pw.WriteInt(mapId);

                if (findType == FindType.Map)
                {
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                }
            }
            else
            {
                pw.WriteString(who);
                pw.WriteSByte(dunno);
            }
            victim.SendPacket(pw);
        }
    }
}
