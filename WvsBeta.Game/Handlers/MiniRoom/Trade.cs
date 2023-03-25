using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Common.Tracking;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public class Trade : MiniRoomBase
    {
        private const byte MAX_SLOTS = 10;

        public bool[] Confirmed;
        private BaseItem[][] ItemList;

        private int[] Mesos;

        public Trade(GameCharacter pOwner) : base(pOwner, 2, MiniRoomType.Trade)
        {
            ItemList = new BaseItem[2][] { new BaseItem[MAX_SLOTS], new BaseItem[MAX_SLOTS] };
            Confirmed = new bool[2] { false, false };
            Mesos = new int[2] { 0, 0 };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < MAX_SLOTS; j++)
                {
                    ItemList[i][j] = null;
                }
            }
        }

        public override void RemovePlayer(GameCharacter pCharacter, MiniRoomLeaveReason pReason)
        {
            RevertItems();
            base.RemovePlayer(pCharacter, pReason);
        }

        public override void Close(bool sendPacket = true, MiniRoomLeaveReason reason = MiniRoomLeaveReason.Cancel)
        {
            RevertItems();
            base.Close(sendPacket, reason);
            ItemList = null;
        }

        private void RevertItems()
        {
            foreach (var user in Users)
            {
                for (int itemSlot = 0; itemSlot < MAX_SLOTS; itemSlot++)
                {
                    var item = ItemList[user.Key][itemSlot];

                    if (item != null) //just to make sure that the player actually has items in trade..
                    {
                        user.Value.Inventory.AddItem(item);
                        ItemTransfer.PlayerTradeReverted(user.Value.ID, item.ItemID, item.Amount, _transaction, item);
                        ItemList[user.Key][itemSlot] = null;
                    }
                }

                var mesos = Mesos[user.Key];
                if (mesos != 0)
                {
                    user.Value.Inventory.AddMesos(mesos);
                    MesosTransfer.PlayerTradeReverted(user.Value.ID, mesos, _transaction);
                    Mesos[user.Key] = 0;
                }
            }
        }

        public void CompleteTrade()
        {
            AddItems();
            Close(true, MiniRoomLeaveReason.TradeSuccessful);
        }

        private bool CheckInventories(out byte userSlotFull)
        {
            userSlotFull = 0xFF;
            // Both Inventories are checked, and have room
            for (byte userSlot = 0; userSlot < 2; userSlot++)
            {
                if (!CheckInventory(userSlot))
                {
                    userSlotFull = userSlot;
                    return false;
                }

            }
            return true;
        }

        private bool CheckInventory(byte userSlot)
        {
            var neededSlots = new Dictionary<Inventory, int>();
            var user = Users[userSlot];
            for (byte itemSlot = 0; itemSlot < MAX_SLOTS; itemSlot++)
            {
                BaseItem item = ItemList[userSlot == 0 ? 1 : 0][itemSlot];
                if (item == null) continue;

                Inventory inv = Constants.getInventory(item.ItemID);

                if (!neededSlots.ContainsKey(inv))
                {
                    neededSlots.Add(inv, 1);
                }
                else
                {
                    neededSlots[inv] += 1;
                }
            }
            foreach (var neededSlot in neededSlots)
            {
                if (user.Inventory.GetOpenSlotsInInventory(neededSlot.Key) < neededSlots[neededSlot.Key])
                {
                    return false;
                }
            }
            return true;
        }


        private void AddItems()
        {
            // Note: Exchange logic, so A gets B and B gets A stuff
            foreach (var srcUserKVP in Users)
            {
                byte srcUserSlot = srcUserKVP.Key;
                GameCharacter srcUser = srcUserKVP.Value;
                GameCharacter dstUser = GetOtherUser(srcUserSlot);
                if (dstUser == null) return;
                for (byte itemSlot = 0; itemSlot < MAX_SLOTS; itemSlot++)
                {
                    BaseItem item = ItemList[srcUserSlot][itemSlot];
                    if (item == null) continue;

                    dstUser.Inventory.AddItem(item);
                    ItemTransfer.PlayerTradeExchange(srcUser.ID, dstUser.ID, item.ItemID, item.Amount, _transaction, item);
                    ItemList[srcUserSlot][itemSlot] = null;
                }

                int mesos = Mesos[srcUserSlot];
                if (mesos == 0) continue;
                int deducted = DeductFee(mesos);
                dstUser.Inventory.AddMesos(deducted);
                Mesos[srcUserSlot] = 0;
                MesosTransfer.PlayerTradeExchange(srcUser.ID, dstUser.ID, mesos, _transaction);
            }
        }

        public static int DeductFee(int mesos)
        {
            double fee = 0.0;
            if (mesos >= 10_000_000) fee = 0.04;
            else if (mesos >= 5_000_000) fee = 0.03;
            else if (mesos >= 1_000_000) fee = 0.02;
            else if (mesos >= 100_000) fee = 0.01;
            else if (mesos >= 50_000) fee = 0.005;
            mesos -= (int)(mesos * fee);
            return mesos;
        }

        public override void OnPacket(GameCharacter pCharacter, MiniRoomOpClient pOpcode, Packet pPacket)
        {
            switch (pOpcode)
            {
                case MiniRoomOpClient.TradePutItem:
                    {
                        byte charslot = pCharacter.RoomSlotId;
                        // Put Item
                        if (!IsFull())
                        {
                            // You can't put items while the second char isn't there yet
                            InventoryOperationPacket.NoChange(pCharacter);
                            return;
                        }

                        Inventory inventory = (Inventory)pPacket.ReadByte();
                        short slot = pPacket.ReadShort();
                        short amount = pPacket.ReadShort();
                        byte toslot = pPacket.ReadByte();

                        var demItem = pCharacter.Inventory.GetItem(inventory, slot);

                        if (demItem == null || toslot < 1 || toslot > 9) // Todo: trade check
                        {
                            // HAX
                            var msg = $"Player tried to add an item in trade with to an incorrect slot. Item = null? {demItem == null}; toSlot {toslot}";
                            Program.MainForm.LogAppend(msg);
                            ReportManager.FileNewReport(msg, pCharacter.ID, 0);
                            InventoryOperationPacket.NoChange(pCharacter);
                            return;
                        }

                        BaseItem item = pCharacter.Inventory.TakeItemAmountFromSlot(inventory, slot, amount, Constants.isRechargeable(demItem.ItemID));

                        if (ItemList[charslot][toslot] == null)
                        {
                            ItemList[charslot][toslot] = item;
                        }
                        
                        ItemTransfer.PlayerTradePutUp(pCharacter.ID, demItem.ItemID, slot, amount, _transaction, demItem);

                        bool isUser0 = pCharacter.Name == Users[0].Name;

                        MiniRoomPacket.TradePutItem(Users[0], toslot, item, (byte)(isUser0 ? 0 : 1));
                        MiniRoomPacket.TradePutItem(Users[1], toslot, item, (byte)(isUser0 ? 1 : 0));

                        InventoryOperationPacket.NoChange(pCharacter); // -.-
                        break;
                    }

                case MiniRoomOpClient.TradePutMesos: // Put mesos
                    {
                        //MessagePacket.SendNotice("PUTMESO PACKET: " + pPacket.ToString(), pCharacter);
                        int amount = pPacket.ReadInt();

                        if (amount < 0 || pCharacter.Inventory.Mesos < amount)
                        {
                            // HAX
                            var msg = "Player tried putting an incorrect meso amount in trade. Amount: " + amount;
                            Program.MainForm.LogAppend(msg);
                            ReportManager.FileNewReport(msg, pCharacter.ID, 0);
                            return;
                        }

                        pCharacter.Inventory.AddMesos(-amount, true);
                        MesosTransfer.PlayerTradePutUp(pCharacter.ID, amount, _transaction);
                        Mesos[pCharacter.RoomSlotId] += amount;


                        bool isUser0 = pCharacter.Name == Users[0].Name;

                        MiniRoomPacket.TradePutMesos(Users[0], Mesos[pCharacter.RoomSlotId], (byte)(isUser0 ? 0 : 1));
                        MiniRoomPacket.TradePutMesos(Users[1], Mesos[pCharacter.RoomSlotId], (byte)(isUser0 ? 1 : 0));

                        break;
                    }

                // Accept trade
                case MiniRoomOpClient.TradeSelect:
                    {
                        byte charslot = pCharacter.RoomSlotId;
                        Confirmed[charslot] = true;

                        foreach (var user in Users)
                        {
                            if (user.Value != pCharacter)
                            {
                                MiniRoomPacket.SelectTrade(user.Value);
                            }
                        }

                        if (Confirmed[0] && Confirmed[1])
                        {
                            if (!CheckInventories(out byte userSlotFull))
                            {
                                MiniRoomPacket.SendLeaveRoom(Users[userSlotFull], MiniRoomLeaveReason.TradeInventoryFull);
                                MiniRoomPacket.SendLeaveRoom(GetOtherUser(userSlotFull), MiniRoomLeaveReason.TradeUnsuccessful);
                                Close(false);
                            }
                            else if (Users[0].MapID != Users[1].MapID)
                            {
                                Close(reason: MiniRoomLeaveReason.TradeWrongMap);
                            }
                            else
                            {
                                CompleteTrade();
                            }
                        }
                        break;
                    }
            }
        }

        public static Trade CreateRoom(GameCharacter pOwner)
        {
            Trade trade = new Trade(pOwner);
            return trade;
        }
    }
}