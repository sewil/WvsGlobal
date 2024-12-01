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
        public static MessagePacket GainMesos(int amount)
        {
            var pw = new MessagePacket(MessageType.GainMesos);
            pw.WriteInt(amount);
            return pw;
        }
        public static MessagePacket DropPickup(bool isMesos, int itemIdOrMesos, int itemAmount)
        {
            var pw = new MessagePacket(MessageType.DropPickup);
            pw.WriteBool(isMesos);
            pw.WriteInt(itemIdOrMesos);
            if (!isMesos)
            {
                InventoryType inv = Constants.getInventory(itemIdOrMesos);
                pw.WriteInt(inv == InventoryType.Equip ? (int)InventoryType.Equip : itemAmount);
            }
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

        public static MessagePacket GainFame(short amount)
        {
            var pw = new MessagePacket(MessageType.GainFame);
            pw.WriteInt(amount);
            return pw;
        }
    }
}
