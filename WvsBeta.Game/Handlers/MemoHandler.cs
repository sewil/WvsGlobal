using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers
{
    public static class MemoHandler
    {
        public static void HandleNoteOperation(GameCharacter chr, Packet pr)
        {
            pr.ReadByte();
            int[] memos = new int[pr.ReadByte()];
            for (int i = 0; i < memos.Length; i++)
            {
                memos[i] = pr.ReadInt();
            }
            ISSendMemoRead(chr, memos);
        }
        public static void ISSendMemoRead(GameCharacter chr, int[] memos)
        {
            var pw = new Packet(ISClientMessages.MemoRead);
            pw.WriteInt(chr.ID);
            pw.WriteByte((byte)memos.Length);
            foreach (int memo in memos)
            {
                pw.WriteInt(memo);
            }
            Server.Instance.CenterConnection.SendPacket(pw);
        }
    }
}
