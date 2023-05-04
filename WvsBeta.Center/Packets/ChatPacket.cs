using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Center.Packets
{
    static class ChatPacket
    {
        public static void SendGroupMessage(CenterCharacter victim, GroupMessageType type, string fromName, string message)
        {
            Packet pw = new Packet(ServerMessages.GROUP_MESSAGE);
            pw.WriteByte((byte)type);
            pw.WriteString(fromName);
            pw.WriteString(message);
            victim.SendPacket(pw);
        }
    }
}
