using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers
{
    public class MemoHandler
    {
        public static void HandleNoteOperation(GameCharacter chr, Packet pr)
        {
            pr.ReadByte();
            byte action = pr.ReadByte();
            int memoid = pr.ReadInt();
            switch (action)
            {
                case 0x01: // Read
                    HandleRead(chr, memoid);
                    break;
                default:
                    break;
            }
        }
        private static void HandleRead(GameCharacter chr, int memoid)
        {
            var pw = new Packet(ISClientMessages.MemoRead);
            pw.WriteInt(chr.ID);
            pw.WriteInt(memoid);
            Server.Instance.CenterConnection.SendPacket(pw);
        }
        public static void SendNote(GameCharacter chr, string name, string message)
        {
            var pw = new Packet(ISClientMessages.MemoSendNote);
            pw.WriteInt(chr.ID);
            pw.WriteString(name);
            pw.WriteString(message);
            Server.Instance.CenterConnection.SendPacket(pw);
        }
    }
}
