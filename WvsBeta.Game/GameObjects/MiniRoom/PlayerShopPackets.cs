using System.Collections.Generic;
using WvsBeta.Common.Character;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public static class PlayerShopPackets
    {
        public static void OpenPlayerShop(GameCharacter pOwner, MiniRoomBase mrb)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomBaseType.EnterResult);
            pw.WriteByte((byte)mrb.Type);
            pw.WriteByte(mrb.MaxUsers);
            pw.WriteBool(mrb.Users[0] == pOwner ? false : true); //owner 
            for (byte i = 0; i < 4; i++)
            {
                GameCharacter pUser = mrb.Users[i];
                if (pUser != null)
                {
                    pw.WriteByte(i);
                    new AvatarLook(pUser).Encode(pw);
                    pw.WriteString(pUser.Name);
                }
            }
            pw.WriteByte(0xFF);
            pw.WriteString(mrb.Title);
            pw.WriteByte(0x10);
            pw.WriteByte(0);
            pOwner.SendPacket(pw);
        }

        public static void AddPlayer(GameCharacter pCharacter, GameCharacter pTo)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomBaseType.Enter);
            pw.WriteByte(pCharacter.RoomSlotId);
            new AvatarLook(pCharacter).Encode(pw);
            pw.WriteString(pCharacter.Name);
            pTo.SendPacket(pw);
        }

        public static void RemovePlayer(GameCharacter pCharacter, MiniRoomBase mrb)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomBaseType.Leave);
            pw.WriteByte(pCharacter.RoomSlotId);
            mrb.BroadcastPacket(pw, pCharacter);
        }

        public static void CloseShop(GameCharacter pCharacter, MiniRoomBaseError error)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomBaseType.Leave);
            pw.WriteByte(pCharacter.RoomSlotId);
            pw.WriteByte((byte)error);
            pCharacter.SendPacket(pw);
        }

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
                pw.WriteByte(WvsBeta.Common.Constants.getItemTypeInPacket(pst.Value.sItem.ItemID));
                pst.Value.sItem.Encode(pw);
            }
            ps.BroadcastPacket(pw);
        }

        public static void OnItemResult(GameCharacter pCharacter, byte msg)
        {
            //1 : You do not have enough in stock. 
            //2 : You have not enough mesos  o.o
            //3 : The price of the item is too high for the trade
            //4 i cant even read this one lol, something about not possessing enough mesos
            //5 : Please check and see if your inventory is full or not.
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x14);
            pw.WriteByte(msg);
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
            pw.WriteByte(0x16);
            pw.WriteByte(slot); //Slot in shop 
            pw.WriteShort(amount); //Number of purchases
            pw.WriteString(pBuyer.Name);
            pCharacter.SendPacket(pw);
        }
    }
}
