using System.Collections.Generic;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public static class PlayerShopPackets
    {
        public static void PersonalShopRefresh(GameCharacter pCharacter, PlayerShop ps)
        {

            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x15);
            pw.WriteByte((byte)ps.Items.Count);
            foreach (KeyValuePair<byte, PlayerShopItem> pst in ps.Items)
            {
                pw.WriteShort(pst.Value.Bundles);
                pw.WriteShort(pst.Value.BundleAmount);
                pw.WriteInt(pst.Value.Price);
                new GW_ItemSlotBase(pst.Value.sItem).Encode(pw, false, false);
            }
            ps.BroadcastPacket(pw);
        }

        public static void SendItemResult(GameCharacter pCharacter, PlayerShopItemResult type)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.PlayerShopItemResult);
            pw.WriteByte((byte)type);
            pCharacter.SendPacket(pw);
        }

        public static void MoveItemToInventory(GameCharacter pCharacter, byte amount, short slot2)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x17);
            pw.WriteByte(amount);
            pw.WriteShort(slot2);
            pCharacter.SendPacket(pw);
        }

        public static void SoldItemResult(GameCharacter pCharacter, GameCharacter pBuyer, byte slot, short amount)
        {
            //Shows the information on who has bought an item.
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.PlayerShopSoldItemResult);
            pw.WriteByte(slot); //Slot in shop 
            pw.WriteShort(amount); //Number of purchases
            pw.WriteString(pBuyer.Name);
            pCharacter.SendPacket(pw);
        }
    }
}
