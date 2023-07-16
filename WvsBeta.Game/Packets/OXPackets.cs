using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Events.Packets
{
    public static class OXPackets
    {
        public static Packet QuizQuestion(bool isQuestion, byte questionPage, short questionIndex)
        {
            Packet pw = new Packet(ServerMessages.QUIZ);
            pw.WriteBool(isQuestion);
            pw.WriteByte(questionPage);
            pw.WriteShort(questionIndex);
            return pw;
        }
    }
}
