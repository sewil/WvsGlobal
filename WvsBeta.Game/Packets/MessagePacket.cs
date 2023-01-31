using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    public class MessagePacket : Packet
    {
        private MessagePacket(MessageType type) : base(ServerMessages.MESSAGE)
        {
            WriteByte((byte)type);
        }
        public static MessagePacket ScrollResult(bool success)
        {
            var pw = new MessagePacket(MessageType.ScrollItemResult);
            pw.WriteBool(success);
            return pw;
        }
    }
}
