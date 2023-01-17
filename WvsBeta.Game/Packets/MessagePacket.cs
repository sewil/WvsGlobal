using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    public class MessagePacket
    {
        MessageType type;
        string message;
        public MessagePacket(MessageType type, string message)
        {
            this.type = type;
            this.message = message;
        }
        public Packet Encode()
        {
            var packet = new Packet(ServerMessages.MESSAGE);
            packet.WriteByte((byte)type);
            return packet;
        }
    }
}
