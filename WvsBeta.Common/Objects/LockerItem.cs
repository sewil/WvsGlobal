using MySql.Data.MySqlClient;
using System;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Objects
{
    public class LockerItem
    {
        public long CashId { get; set; }
        public int UserId { get; set; }
        public int CharacterId { get; set; }
        public int ItemId { get; set; }
        public int CommodityId { get; set; }
        public short Amount { get; set; }
        public long Expiration { get; set; }
        public string GiftName { get; set; }
        public bool GiftUnread { get; set; }
        public string GiftMessage { get; set; }
        public long CoupleCashId { get; set; }
        public int CoupleCharId { get; set; }
        public string CoupleCharName { get; set; }
        public bool SavedToDatabase { get; set; } = false;

        public LockerItem(int userId, ICommodityInfo ci, string giftName = "", string giftMessage = "", int coupleCharId = 0, string coupleName = "")
        {
            long expiration = ci.Period > 0 ? new TimeSpan(ci.Period, 0, 0, 0).GetFileTimeWithAddition() : BaseItem.NoItemExpiration;
            ItemId = ci.ItemID;
            Amount = ci.Count;
            CashId = 0; // Will be created on insert
            Expiration = expiration;
            CharacterId = 0; // 0, as its in the locker
            CommodityId = ci.SerialNumber;
            GiftUnread = string.IsNullOrEmpty(giftName) == false;
            GiftName = giftName;
            GiftMessage = giftMessage;
            UserId = userId;
            CoupleCharId = coupleCharId;
            CoupleCharName = coupleName;
        }

        public LockerItem(MySqlDataReader data)
        {
            CashId = data.GetInt64("cashid");
            UserId = data.GetInt32("userid");
            CharacterId = data.GetInt32("charid");
            ItemId = data.GetInt32("itemid");
            CommodityId = data.GetInt32("commodity_id");
            Amount = data.GetInt16("amount");
            GiftName = data.GetFieldValue<string>("memo_fromname") ?? "";
            Expiration = data.GetInt64("expiration");
            GiftUnread = data.GetBoolean("gift_unread");
            GiftMessage = data.GetFieldValue<string>("memo_message") ?? "";
            CoupleCashId = data.GetInt64("couplecashid");
            CoupleCharId = data.GetInt32("couple_charid");
            CoupleCharName = data.GetFieldValue<string>("couple_charname");
        }

        public string GetFullUpdateColumns()
        {
            return
                "cashid = " + CashId + ", " +
                "userid = " + UserId + ", " +
                "charid = " + CharacterId + ", " +
                "itemid = " + ItemId + ", " +
                "commodity_id = " + CommodityId + ", " +
                "amount = " + Amount + ", " +
                "expiration = " + Expiration + ", " +
                "gift_unread = " + GiftUnread + ", " +
                "couplecashid = " + CoupleCashId + ""
            ;
        }

        public string GetFullSaveColumns(BaseItem baseItem, byte worldId)
        {
            return $"{CashId}, {baseItem.InventorySlot}, {UserId}, {CharacterId}, {ItemId}, {CommodityId}, {Amount}, {Expiration}, {GiftUnread}, {worldId}, {CoupleCashId}, {CoupleCharId}";
        }

        public void Encode(Packet packet)
        {
            packet.WriteLong(CashId);
            packet.WriteInt(UserId);
            packet.WriteInt(CharacterId);
            packet.WriteInt(ItemId);
            packet.WriteInt(CommodityId);
            packet.WriteShort(Amount);

            packet.WriteString(GiftName, 13);
            packet.WriteLong(Expiration);
        }
    }
}
