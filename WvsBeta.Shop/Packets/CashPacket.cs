using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Sessions;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Packets;
using System.Collections.Generic;
using WvsBeta.Shop.GameObjects;

namespace WvsBeta.Shop
{
    public static class CashPacket
    {
        private static readonly ILog _log = LogManager.GetLogger("CashPacket");

        public struct BoughtItem
        {
            public LockerItem lockerItem { get; set; }
            public bool withMaplePoints { get; set; }
            public int cashAmount { get; set; }
        }

        public struct BuySlotIncrease
        {
            public Inventory inventory { get; set; }
            public byte newSlots { get; set; }
            public bool withMaplePoints { get; set; }
            public int cashAmount { get; set; }
        }

        public enum CashErrors
        {
            UnknownError = 0x00, // Default statement

            UnknownErrorDC_1 = 80,
            TimeRanOutTryingToProcessRequest_TryAgain = 81,
            UnknownErrorDC_2 = 82,

            NotEnoughCash = 83,
            CantGiftUnder14Year = 84,
            ExceededAllottedLimitOfPriceForGifts = 85,
            CheckExceededNumberOfCashItems = 86,
            CheckCharacterNameOrItemGenderRestrictions = 87,
            CheckCouponNumber = 88,

            RegisterCouponAtWebsite = 91,
            DueGenderRestrictionsNoCouponUse = 92,
            CouponOnlyForRegularItemsThusNoGifting = 93,
            CheckFullInventory = 94,
            ItemOnlyAvailableForUsersAtPremiumInternetCafe = 95,
            IneligibleRecipientCheckNameAndGender = 96,
            ItemsAreNotAvailableForPurchaseAtThisHour = 97,
            OutOfStock = 98,
            ExceededSpendingLimitOfCash = 99,
            NotEnoughMesos = 100,
            UnavailableDuringBetaTestPhase = 101,
            InvalidDoBEntered = 102,
        }

        public enum CashPacketOpcodes
        {
            // Note: Storage == inventory
            // Client packets (C)
            C_BuyItem = 2,
            C_GiftItem = 3,
            C_UpdateWishlist = 4,
            C_IncreaseSlots = 5,
            C_MoveLtoS = 10,
            C_MoveStoL = 11,
            C_BuyCoupleRing = 0x18,
            C_BuyPackage = 25,
            C_GiftPackage = 26,
            C_BuyQuestItem = 27,

            // Server packets (S)
            S_LoadLocker_Done = 28,
            S_LoadLocker_Failed,
            S_LoadWish_Done = 30,
            S_LoadWish_Failed,
            S_UpdateWish_Done = 32,
            S_UpdateWish_Failed = 33,
            S_Buy_Done = 34,
            S_Buy_Failed,

            S_UseCoupon_Done = 36,
            S_UseGiftCoupon_Done = 38,
            S_UseCoupon_Failed = 39,

            S_Gift_Done = 41,
            S_Gift_Failed = 42, //|73|77

            S_IncSlotCount_Done,
            S_IncSlotCount_Failed,
            S_IncTrunkCount_Done,
            S_IncTrunkCount_Failed,

            S_MoveLtoS_Done = 47,
            S_MoveLtoS_Failed = 48,
            S_MoveStoL_Done,
            S_MoveStoL_Failed = 50,
            S_Delete_Done = 51, // + long SN
            S_Delete_Failed = 52,
            S_Expired_Done = 53, // + long SN
            S_Expired_Failed = 54,

            S_Refund_Done = 70,
            S_Refund_Failed = 71,
            S_GiftCoupleRing_Done = 72,
            //S_GiftCoupleRing_Failed = 73,
            S_BuyEquipped_Done = 74,
            S_BuyEquipped_Failed = 75,
            S_GiftPackage_Done = 76,
            //S_GiftPackage_Failed = 77
            S_BuyUnk78_Done = 78,
            S_BuyUnk79_Failed = 79
        }

        public static void HandleCashPacket(Character chr, Packet packet)
        {
            var header = (CashPacketOpcodes)packet.ReadByte();
            switch (header)
            {
                case CashPacketOpcodes.C_IncreaseSlots:
                    {
                        var maplepoints = packet.ReadBool();
                        Inventory inventory = (Inventory)packet.ReadByte();

                        if (!Enum.IsDefined(typeof(Inventory), inventory))
                        {
                            _log.Warn("Increase slots failed: Invalid inventory");
                            SendError(chr, CashPacketOpcodes.S_IncSlotCount_Failed, CashErrors.OutOfStock);
                            return;
                        }

                        var points = chr.GetCashStatus();
                        var price = 4000;

                        if (price > (maplepoints ? points.maplepoints : points.nx))
                        {
                            _log.Warn("Increase slots failed: Not enough NX or maplepoints");
                            SendError(chr, CashPacketOpcodes.S_IncSlotCount_Failed, CashErrors.NotEnoughCash);
                            return;
                        }

                        var slots = chr.Inventory.MaxSlots[inventory - 1];

                        // Client sided limit
                        if (slots > 80)
                        {
                            _log.Warn($"Increase slots failed: already has {slots} slots on inventory {inventory}");
                            SendError(chr, CashPacketOpcodes.S_IncSlotCount_Failed, CashErrors.UnknownErrorDC_1);
                            return;
                        }

                        // no limiting atm
                        slots += 4;
                        chr.Inventory.SetInventorySlots(inventory, slots, false);

                        chr.AddSale($"Bought inventory expansion for inventory type {inventory} character {chr.ID}", price, 0, maplepoints ? TransactionType.MaplePoints : TransactionType.NX);

                        Character.CashLog.Info(new BuySlotIncrease
                        {
                            cashAmount = price,
                            inventory = inventory,
                            newSlots = slots,
                            withMaplePoints = maplepoints
                        });

                        SendIncreasedSlots(chr, inventory, slots);
                        SendCashAmounts(chr);
                        break;
                    }
                case CashPacketOpcodes.C_BuyQuestItem:
                case CashPacketOpcodes.C_BuyItem:
                    {
                        BuyItem(chr, packet, header == CashPacketOpcodes.C_BuyQuestItem);
                        break;
                    }
                case CashPacketOpcodes.C_BuyCoupleRing:
                case CashPacketOpcodes.C_GiftItem:
                    {
                        var dob = packet.ReadUInt();
                        var sn = packet.ReadInt();
                        var recipient = packet.ReadString();
                        string message = packet.ReadString();
                        GiftItem(chr, dob, sn, recipient, message, header == CashPacketOpcodes.C_BuyCoupleRing);
                        break;
                    }
                case CashPacketOpcodes.C_UpdateWishlist:
                    {
                        for (byte i = 0; i < 10; i++)
                        {
                            var val = packet.ReadInt();

                            if (val == 0 || DataProvider.Commodity.ContainsKey(val))
                            {
                                chr.Wishlist[i] = val;
                            }
                            else
                            {
                                chr.Wishlist[i] = 0;
                                _log.Warn($"While updating wishlist, commodity not found for SN {val}");
                            }
                        }

                        SendWishlist(chr, true);
                        break;
                    }
                case CashPacketOpcodes.C_MoveStoL:
                    {
                        var cashid = packet.ReadLong();
                        Inventory inv = (Inventory)packet.ReadByte();

                        var lockerItem = chr.Inventory.GetLockerItemByCashID(cashid);
                        if (lockerItem == null)
                        {
                            _log.Warn($"Moving Storage to Locker failed: locker item not found with cashid {cashid}");
                            SendError(chr, CashPacketOpcodes.S_MoveStoL_Failed, CashErrors.UnknownError);
                            return;
                        }

                        if (Constants.getInventory(lockerItem.ItemId) != inv)
                        {
                            _log.Warn($"Moving Storage to Locker failed: inventory didn't match.");
                            SendError(chr, CashPacketOpcodes.S_MoveStoL_Failed, CashErrors.UnknownError);
                            return;
                        }

                        var item = chr.Inventory.GetItemByCashID(cashid, inv);

                        lockerItem.CharacterId = 0; // Reset

                        chr.Inventory.RemoveLockerItem(lockerItem, item, false);
                        chr.Locker.AddItem(lockerItem, item);

                        SendPlacedItemInStorage(chr, lockerItem);

                        break;
                    }
                case CashPacketOpcodes.C_MoveLtoS:
                    {
                        var cashid = packet.ReadLong();
                        Inventory inv = (Inventory)packet.ReadByte();
                        var slot = packet.ReadShort();

                        var lockerItem = chr.Locker.GetLockerItemFromCashID(cashid);
                        if (lockerItem == null)
                        {
                            _log.Warn($"Moving Locker to Storage failed: locker item not found with cashid {cashid}");
                            SendError(chr, CashPacketOpcodes.S_MoveLtoS_Failed, CashErrors.UnknownError);
                            return;
                        }

                        if (Constants.getInventory(lockerItem.ItemId) != inv)
                        {
                            _log.Warn($"Moving Locker to Storage failed: inventory didn't match.");
                            SendError(chr, CashPacketOpcodes.S_MoveLtoS_Failed, CashErrors.UnknownError);
                            return;
                        }

                        if (lockerItem.UserId != chr.UserID)
                        {
                            _log.Warn($"Moving Locker to Storage failed: tried to move cash item that was not from himself (packethack?)");
                            SendError(chr, CashPacketOpcodes.S_MoveLtoS_Failed, CashErrors.UnknownError);
                            return;
                        }

                        var item = chr.Locker.GetItemFromCashID(cashid, lockerItem.ItemId);
                        if (item == null)
                        {
                            _log.Warn($"Moving Locker to Storage failed: item not found with cashid {cashid} itemid {lockerItem.ItemId}");
                            SendError(chr, CashPacketOpcodes.S_MoveLtoS_Failed, CashErrors.UnknownError);
                            return;
                        }

                        if (slot < 1 || slot > chr.Inventory.MaxSlots[inv])
                        {
                            _log.Warn($"Moving Locker to Storage failed: not enough slots left.");
                            SendError(chr, CashPacketOpcodes.S_MoveLtoS_Failed, CashErrors.CheckFullInventory);
                            return;
                        }

                        if (chr.Inventory.GetItem(inv, slot) != null)
                        {
                            _log.Warn($"Moving Locker to Storage failed: slot is not empty");
                            SendError(chr, CashPacketOpcodes.S_MoveLtoS_Failed, CashErrors.UnknownError);
                            return;
                        }

                        lockerItem.CharacterId = chr.ID;

                        chr.Inventory.AddLockerItem(lockerItem);
                        chr.Inventory.AddItem(inv, slot, item, false);
                        chr.Locker.RemoveItem(lockerItem, item);

                        SendPlacedItemInInventory(chr, item);
                        break;
                    }
                default:
                    {
                        //string what = "[" + DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond.ToString("D3") + "] Unknown packet found: " + packet.ToString();
                        //FileWriter.WriteLine(@"connection_log\" + chr.mID.ToString() + ".txt", what, true);
                        ////Console.WriteLine("Unknown packet received! " + packet.ToString());
                        Program.MainForm.LogAppend("Unknown data: " + packet);
                        break;
                    }
            }
        }
        public static void GiftItem(Character chr, uint dob, int sn, string recipient, string message, bool isCoupleRing)
        {
            // Check SN
            if (!DataProvider.Commodity.TryGetValue(sn, out var ci))
            {
                _log.Warn($"Gifting failed: commodity not found for SN {sn}");
                SendError(chr, CashPacketOpcodes.S_Gift_Failed, CashErrors.OutOfStock);
                return;
            }

            // check DoB
            if (chr.DoB != dob)
            {
                _log.Warn($"Gifting failed: invalid DoB entered");
                SendError(chr, CashPacketOpcodes.S_Gift_Failed, CashErrors.InvalidDoBEntered);
                return;
            }

            // check age
            TimeSpan age = (MasterThread.CurrentDate - chr.DoB.DateFromDoB());
            if (age.GetYears() < 14)
            {
                _log.Warn("Gifting failed: too young");
                SendError(chr, CashPacketOpcodes.S_Gift_Failed, CashErrors.CantGiftUnder14Year);
                return;
            }
            
            if (ci.OnSale == false ||
                ci.StockState == StockState.NotAvailable ||
                ci.StockState == StockState.OutOfStock)
            {
                _log.Warn($"Gifting failed: commodity {sn} not on sale {ci.OnSale} or out of stock {ci.StockState}");
                SendError(chr, CashPacketOpcodes.S_Gift_Failed, CashErrors.OutOfStock);
            }

            // Check price
            var points = chr.GetCashStatus();
            if (ci.Price > points.nx)
            {
                _log.Warn("Gifting failed: not enough NX");
                SendError(chr, CashPacketOpcodes.S_Gift_Failed, CashErrors.NotEnoughCash);
                return;
            }

            // Check recipient
            int recipientId = 0;
            int recipientUserId = 0;
            int recipientGender = 0;
            using (var mdr = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery(
                "SELECT ID, userid, gender FROM characters WHERE `name` = @name",
                "@name", recipient
            ))
            {
                if (!mdr.Read())
                {
                    // Not found
                    _log.Warn($"Gifting failed: character named {recipient} not found");
                    SendError(chr, CashPacketOpcodes.S_Gift_Failed, CashErrors.CheckCharacterNameOrItemGenderRestrictions);
                    return;
                }

                recipientId = mdr.GetInt32(0);
                recipientUserId = mdr.GetInt32(1);
                recipientGender = mdr.GetInt32(2);
            }

            if (ci.Gender != CommodityGenders.Both && recipientGender != (int)ci.Gender)
            {
                _log.Warn($"Gifting failed: receipient not {ci.Gender}"); ;
                SendError(chr, CashPacketOpcodes.S_Gift_Failed, CashErrors.CheckCharacterNameOrItemGenderRestrictions);
                return;
            }

            LockerItem giftedItem = new LockerItem(recipientUserId, ci, chr.Name, message, chr.ID, chr.Name);
            chr.Locker.AddItem(giftedItem, CharacterCashLocker.CreateCashItem(giftedItem));

            if (isCoupleRing)
            {
                LockerItem boughtItem = new LockerItem(chr.UserID, ci, coupleCharId: recipientId, coupleName: recipient);
                chr.Locker.AddItem(boughtItem, CharacterCashLocker.CreateCashItem(boughtItem));
                giftedItem.CoupleCashId = boughtItem.CashId;
                boughtItem.CoupleCashId = giftedItem.CashId;
                SendGiftCoupleRing(chr, boughtItem, recipient, ci.ItemID, ci.Count);
            }

            chr.Inventory.SaveCashItems(chr.Locker);
             
            chr.AddSale($"Bought cash item {giftedItem.ItemId} amount {giftedItem.Amount} (ref: {giftedItem.CashId:X16}) as a gift for {recipient}", ci.Price, ci.SerialNumber, TransactionType.NX);

            Character.CashLog.Info(new BoughtItem
            {
                cashAmount = ci.Price,
                lockerItem = giftedItem,
                withMaplePoints = false
            });

            if (!isCoupleRing) SendGiftDone(chr, ci.ItemID, ci.Count, recipient);
            SendCashAmounts(chr);
            MemoPacket.SendMemo(Server.Instance.CenterConnection, chr.ID, recipientId, message, giftedItem.CashId, false);
        }

        public static void SendGiftCoupleRing(Character chr, LockerItem item, string recipient, int itemid, short amount)
        {
            var pw = new Packet(ServerMessages.CASHSHOP_ACTION);
            pw.WriteByte((byte)CashPacketOpcodes.S_GiftCoupleRing_Done);
            item.Encode(pw);
            pw.WriteString(recipient);
            pw.WriteInt(itemid);
            pw.WriteShort(amount);
            chr.SendPacket(pw);
        }

        public static void SendGiftPackageDone(Character chr, string recipient, int itemid, short amount)
        {
            var pw = new Packet(ServerMessages.CASHSHOP_ACTION);
            pw.WriteByte((byte)CashPacketOpcodes.S_GiftPackage_Done);
            pw.WriteString(recipient);
            pw.WriteInt(itemid);
            pw.WriteShort(amount);
            chr.SendPacket(pw);
        }

        public static void SendBuyEquippedDone(Character chr, IList<LockerItem> items)
        {
            if (items.Count > byte.MaxValue) throw new ArgumentException("Too many items!");
            var pw = new Packet(ServerMessages.CASHSHOP_ACTION);
            pw.WriteByte((byte)CashPacketOpcodes.S_BuyEquipped_Done);
            pw.WriteByte((byte)items.Count);
            foreach (var item in items)
            {
                item.Encode(pw);
            }
            chr.SendPacket(pw);
        }

        public static void SendWishlist(Character chr, bool update)
        {
            var pw = GetPacketWriter(update ? CashPacketOpcodes.S_UpdateWish_Done : CashPacketOpcodes.S_LoadWish_Done);
            foreach (var val in chr.Wishlist)
            {
                pw.WriteInt(val);
            }
            chr.SendPacket(pw);
        }

        public static void SendInfo(Character chr)
        {
            SendLocker(chr);
            SendWishlist(chr, false);
            SendCashAmounts(chr);
        }

        private static Packet GetPacketWriter(CashPacketOpcodes opcode)
        {
            var pw = new Packet(ServerMessages.CASHSHOP_ACTION);
            pw.WriteByte((byte)opcode);
            return pw;
        }

        public static void SendLocker(Character chr)
        {
            var pw = GetPacketWriter(CashPacketOpcodes.S_LoadLocker_Done);

            var userLocker = chr.Locker.Items.Where(x => x.UserId == chr.UserID).ToList();
            var gifts = userLocker.Where(i => i.GiftUnread).ToList();

            pw.WriteShort((short)userLocker.Count);

            foreach (var item in userLocker)
            {
                item.Encode(pw);
            }

            pw.WriteShort((short)gifts.Count);

            foreach (var gift in gifts)
            {
                pw.WriteLong(gift.CashId);
                pw.WriteInt(gift.ItemId);
                pw.WriteString(gift.GiftName, 13);
                pw.WriteString(gift.GiftMessage, 72);
                pw.WriteByte(1); // ?
                gift.GiftUnread = false;
            }

            pw.WriteShort(3); // Storage slots
            chr.SendPacket(pw);
        }

        public static void SendBoughtItem(Character chr, LockerItem item)
        {
            var pw = GetPacketWriter(CashPacketOpcodes.S_Buy_Done);

            item.Encode(pw);
            chr.SendPacket(pw);
        }

        public static void SendBoughtUnk78(Character chr, IList<long> cashIds)
        {
            var pw = GetPacketWriter(CashPacketOpcodes.S_BuyUnk78_Done);
            pw.WriteInt(cashIds.Count);
            foreach (long cashId in cashIds)
            {
                pw.WriteLong(cashId);
            }
            chr.SendPacket(pw);
        }

        public static void SendGiftDone(Character chr, int itemid, short amount, string receipient)
        {
            var pw = GetPacketWriter(CashPacketOpcodes.S_Gift_Done);

            pw.WriteString(receipient);
            pw.WriteInt(itemid);
            pw.WriteShort(amount);
            chr.SendPacket(pw);
        }


        public static void SendIncreasedSlots(Character chr, Inventory inventory, short slots)
        {
            var pw = GetPacketWriter(CashPacketOpcodes.S_IncSlotCount_Done);
            pw.WriteByte((byte)inventory);
            pw.WriteShort(slots);
            chr.SendPacket(pw);
        }

        public static void SendPlacedItemInInventory(Character chr, BaseItem item)
        {
            var pw = GetPacketWriter(CashPacketOpcodes.S_MoveLtoS_Done);
            pw.WriteShort(item.InventorySlot);
            new GW_ItemSlotBase(item).Encode(pw, false, false);
            chr.SendPacket(pw);
        }

        public static void SendPlacedItemInStorage(Character chr, LockerItem item)
        {
            var pw = GetPacketWriter(CashPacketOpcodes.S_MoveStoL_Done);
            item.Encode(pw);
            chr.SendPacket(pw);
        }


        public static void SendError(Character chr, CashPacketOpcodes opcode, CashErrors error, int v = 0)
        {
            var pw = new Packet(ServerMessages.CASHSHOP_ACTION);
            pw.WriteByte((byte)opcode);
            pw.WriteByte((byte)error);
            pw.WriteInt(v);

            chr.SendPacket(pw);
        }

        public static void SendCashAmounts(Character chr)
        {
            var points = chr.GetCashStatus();

            var pw = new Packet(ServerMessages.CASHSHOP_UPDATE_AMOUNTS);
            pw.WriteInt(points.nx);
            pw.WriteInt(points.maplepoints);
            chr.SendPacket(pw);
        }

        public static void Charge(Character chr)
        {
            //This minimizes your client :O 
            var pw = new Packet(ServerMessages.CASHSHOP_RECHARGE);
            pw.WriteString("C:\\Program Files (x86)\\Internet Explorer\\iexplore.exe");
            pw.WriteString("C:\\Program Files (x86)\\Internet Explorer\\iexplore.exe"); //path? xd
            pw.WriteByte(1);
            pw.WriteShort(1);
            pw.WriteInt(5000000);
            pw.WriteInt(5000000);
            chr.SendPacket(pw);
        }

        public static void SendUseCouponDone(Character chr, IList<LockerItem> lockerItems, int maplePoints, IList<long> cashIds, int mesos)
        {
            if (lockerItems.Count > byte.MaxValue) throw new ArgumentException("Too many locker items!");
            else if (cashIds.Count > byte.MaxValue) throw new ArgumentException("Too many cash ids!");

            var pw = new Packet(ServerMessages.CASHSHOP_ACTION);
            pw.WriteByte((byte)CashPacketOpcodes.S_UseCoupon_Done);

            pw.WriteByte((byte)lockerItems.Count);
            foreach (LockerItem lockerItem in lockerItems)
            {
                lockerItem.Encode(pw);
            }

            pw.WriteInt(maplePoints);

            pw.WriteByte((byte)cashIds.Count);
            foreach (long cashId in cashIds)
            {
                pw.WriteLong(cashId);
            }

            pw.WriteInt(mesos);

            chr.SendPacket(pw);
        }

        public static void SendUseGiftCouponDone(Character chr, string name, IList<LockerItem> lockerItems, int maplePoints)
        {
            if (lockerItems.Count > byte.MaxValue) throw new ArgumentException("Too many locker items!");

            var pw = new Packet(ServerMessages.CASHSHOP_ACTION);
            pw.WriteByte((byte)CashPacketOpcodes.S_UseGiftCoupon_Done);
            pw.WriteString(name);

            pw.WriteByte((byte)lockerItems.Count);
            foreach (LockerItem lockerItem in lockerItems)
            {
                lockerItem.Encode(pw);
            }

            pw.WriteInt(maplePoints); // Maplepoints

            chr.SendPacket(pw);
        }

        public static void BuyItem(Character chr, Packet pr, bool isQuestItem)
        {
            bool maplePoints = false;
            if (!isQuestItem)
            {
                maplePoints = pr.ReadBool();
            }
            int sn = pr.ReadInt();
            if (!DataProvider.Commodity.TryGetValue(sn, out CommodityInfo ci) || !ci.OnSale || ci.StockState == StockState.NotAvailable || ci.StockState == StockState.OutOfStock)
            {
                SendError(chr, CashPacketOpcodes.S_Buy_Failed, CashErrors.OutOfStock);
                return;
            }

            if ((ci.Gender != CommodityGenders.NotApplicable && ci.Gender != CommodityGenders.Both) && (byte)ci.Gender != chr.Gender)
            {
                _log.Warn("Buying failed: invalid gender");
                SendError(chr, CashPacketOpcodes.S_Buy_Failed, CashErrors.CheckCharacterNameOrItemGenderRestrictions);
                return;
            }

            int has;
            if (isQuestItem) has = chr.Inventory.Mesos;
            else
            {
                var (nx, maplepoints) = chr.GetCashStatus();
                has = maplePoints ? maplepoints : nx;
            }

            if (ci.Price > has)
            {
                _log.Warn("Buying failed: not enough cash");
                SendError(chr, CashPacketOpcodes.S_Buy_Failed, isQuestItem ? CashErrors.NotEnoughMesos : CashErrors.NotEnoughCash);
                return;
            }

            var lockerItem = new LockerItem(chr.UserID, ci, "");
            var baseItem = CharacterCashLocker.CreateCashItem(lockerItem);
            chr.Locker.AddItem(lockerItem, baseItem);

            chr.AddSale($"Bought cash item {lockerItem.ItemId} amount {lockerItem.Amount} (ref: {lockerItem.CashId:X16})", ci.Price, ci.SerialNumber, maplePoints ? TransactionType.MaplePoints : TransactionType.NX);

            Character.CashLog.Info(new BoughtItem
            {
                cashAmount = ci.Price,
                lockerItem = lockerItem,
                withMaplePoints = maplePoints
            });

            SendBoughtItem(chr, lockerItem);
            SendCashAmounts(chr);
        }
    }
}
