using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    public enum MessageAppearType
    {
        None,
        ChatGrey,
        SideWhite,
        SideYellow
    }
    public class MessagePacket : Packet
    {
        private MessagePacket(MessageType type) : base(ServerMessages.MESSAGE)
        {
            WriteByte((byte)type);
        }
        public static MessagePacket RedText(string message)
        {
            var pw = new MessagePacket(MessageType.RedText);
            pw.WriteString(message);
            return pw;
        }
        public static MessagePacket ScrollResult(bool success)
        {
            var pw = new MessagePacket(MessageType.ScrollItemResult);
            pw.WriteBool(success);
            return pw;
        }
        public static MessagePacket GainMesos(int amount, MessageAppearType appearType)
        {
            var type = appearType == MessageAppearType.ChatGrey ? MessageType.GainMesosGrey : MessageType.GainItemOrMesos;
            var pw = new MessagePacket(type);
            if (type == MessageType.GainItemOrMesos) pw.WriteBool(true);
            pw.WriteInt(amount);
            return pw;
        }
        public static MessagePacket GainItem(int itemid, short amount)
        {
            var pw = new MessagePacket(MessageType.GainItemOrMesos);
            pw.WriteBool(false);
            pw.WriteInt(itemid);
            Inventory inv = Constants.getInventory(itemid);
            pw.WriteInt(inv == Inventory.Equip ? (int)Inventory.Equip : amount);
            return pw;
        }

        public static MessagePacket GainEXP(int amount, MessageAppearType type)
        {
            var pw = new MessagePacket(MessageType.IncEXP);
            pw.WriteBool(type == MessageAppearType.SideWhite);
            pw.WriteInt(amount);
            pw.WriteBool(type == MessageAppearType.ChatGrey);
            return pw;
        }
    }
}
