using System;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Common.Tracking;

namespace WvsBeta.Game
{
    public static class StoragePacket
    {

        public enum StorageResultOp : byte
        {
            InventoryFullOrNot = 9, // Please check if your inventory is full or not
            NotEnoughMesos = 12, // You have not enough mesos. (Yes, that's a typo).
            StorageIsFull = 13, // The storage is full.
            DueToAnError = 14, // Due to an error, the trade did not happen.
            Success = 15,
        }

        public enum StorageEncode
        {
            EncodeWithdraw = 8,
            EncodeDeposit = 10
        }

        public enum StorageAction
        {
            Withdraw = 4,
            Deposit = 5,
            StoreMesos = 6,
            Exit = 7,
        }

        [Flags]
        public enum StorageEncodeFlags
        {
            EncodeMesos = 0x02,
            EncodeInventoryEquip = 0x04,
            EncodeInventoryUse = 0x08,
            EncodeInventorySetUp = 0x10,
            EncodeInventoryEtc = 0x20,
            EncodeInventoryPet = 0x40,

            EncodeAll = EncodeMesos | EncodeInventoryEquip | EncodeInventoryUse | EncodeInventorySetUp | EncodeInventoryEtc | EncodeInventoryPet,
        }

        public static void HandleStorage(GameCharacter chr, Packet pr)
        {
            if (chr.TrunkNPCID == 0) return;

            byte opcode = pr.ReadByte();
            switch ((StorageAction)opcode)
            {
                case StorageAction.Withdraw: // Remove
                    {
                        InventoryType inventory = (InventoryType)pr.ReadByte();
                        byte slot = pr.ReadByte();
                        Item item = chr.Storage.GetItem(inventory, slot);
                        if (item == null)
                        {
                            return;
                        }

                        short amount = item.Amount;
                        if (!Constants.isStackable(item.ItemID))
                        {
                            amount = 1; // 1 'set'
                        }

                        if (chr.Inventory.HasSlotsFreeForItem(item.ItemID, amount))
                        {
                            // AddItem2 will distribute stackable items
                            chr.Inventory.AddItemOperations(item, out short _);
                            chr.Storage.TakeItemOut(inventory, slot);

                            ItemTransfer.PlayerStorageWithdraw(chr.ID, chr.TrunkNPCID, item.ItemID, item.Amount, null, item);

                            EncodeStorage(chr, StorageEncode.EncodeWithdraw, GetEncodeFlagForInventory(Constants.getInventory(item.ItemID)));
                        }
                        else
                        {
                            SendError(chr, StorageResultOp.InventoryFullOrNot);
                        }
                        break;
                    }
                case StorageAction.Deposit: // Add
                    {
                        byte slot = (byte)pr.ReadShort();
                        int itemid = pr.ReadInt();
                        short amount = pr.ReadShort();
                        NPCData data = GameDataProvider.NPCs[chr.TrunkNPCID];
                        var storageCost = data.Trunk;
                        if (chr.Inventory.Mesos < storageCost)
                        {
                            SendError(chr, StorageResultOp.NotEnoughMesos);
                            return;
                        }

                        InventoryType inventory = Constants.getInventory(itemid);
                        Item item = chr.Inventory.GetItem(inventory, slot);
                        if (item == null || item.ItemID != itemid || item.CashId != 0)
                        {
                            // hax
                            return;
                        }
                        if (!chr.Storage.SlotsAvailable())
                        {
                            SendError(chr, StorageResultOp.StorageIsFull);
                            return;
                        }

                        var isRechargable = Constants.isRechargeable(item.ItemID);
                        if (isRechargable) amount = item.Amount;

                        var possibleNewItem = chr.Inventory.TakeItemAmountFromSlot(inventory, slot, amount, true);
                        if (chr.AssertForHack(possibleNewItem == null, "Storage hack (amount > item.amount)")) return;

                        chr.Storage.AddItem(possibleNewItem);

                        ItemTransfer.PlayerStorageStore(chr.ID, chr.TrunkNPCID, item.ItemID, item.Amount, "" + item.GetHashCode() + " " + possibleNewItem.GetHashCode(), possibleNewItem);

                        EncodeStorage(chr, StorageEncode.EncodeDeposit, GetEncodeFlagForInventory(Constants.getInventory(item.ItemID)));
                        
                        chr.Inventory.AddMesos(-storageCost); //why did you forget this diamondo :P
                        
                        MesosTransfer.PlayerGaveToNPC(chr.ID, chr.TrunkNPCID, storageCost, "" + item.GetHashCode());
                        break;
                    }
                case StorageAction.StoreMesos:
                    {
                        int mesos = pr.ReadInt();
                        if (mesos < 0)
                        {
                            // Store
                            if (chr.AssertForHack(Math.Abs(mesos) > chr.Inventory.Mesos, "Trying to store more mesos than he has") == false)
                            {
                                Common.Tracking.MesosTransfer.PlayerStoreMesos(chr.ID, Math.Abs(mesos));
                                chr.Inventory.AddMesos(mesos);
                                chr.Storage.ChangeMesos(mesos);
                            }
                        }
                        else
                        {
                            // Withdraw
                            if (chr.AssertForHack(Math.Abs(mesos) > chr.Storage.Mesos, "Trying to withdraw more mesos than he has") == false)
                            {
                                Common.Tracking.MesosTransfer.PlayerRetrieveMesos(chr.ID, Math.Abs(mesos));
                                chr.Inventory.AddMesos(mesos);
                                chr.Storage.ChangeMesos(mesos);
                            }
                        }
                        break;
                    }
                case StorageAction.Exit:
                    {
                        chr.TrunkNPCID = 0;
                        break;
                    }
                default:
                    {
                        Program.MainForm.LogAppend("Unknown Storage action: {0}", pr);
                        break;
                    }
            }
        }

        public static void SendShowStorage(GameCharacter chr, int NPCID)
        {
            Packet pw = new Packet(ServerMessages.STORAGE);
            pw.WriteInt(NPCID);

            EncodeStorage(chr, pw, StorageEncodeFlags.EncodeAll);

            chr.SendPacket(pw);
        }

        public static void SendChangedMesos(GameCharacter chr)
        {
            Packet pw = new Packet(ServerMessages.STORAGE_RESULT);
            pw.WriteByte((byte)StorageResultOp.Success);
            EncodeStorage(chr, pw, StorageEncodeFlags.EncodeMesos);
            chr.SendPacket(pw);
        }

        public static void SendError(GameCharacter chr, StorageResultOp what)
        {
            Packet pw = new Packet(ServerMessages.STORAGE_RESULT);
            pw.WriteByte((byte)what);
            chr.SendPacket(pw);
        }

        public static void EncodeStorage(GameCharacter chr, StorageEncode enc, StorageEncodeFlags flags)
        {
            Packet packet = new Packet(ServerMessages.STORAGE_RESULT);
            packet.WriteByte((byte)enc);
            EncodeStorage(chr, packet, flags);
            chr.SendPacket(packet);
        }

        private static StorageEncodeFlags GetEncodeFlagForInventory(InventoryType inventory)
        {
            StorageEncodeFlags flag;
            switch (inventory)
            {
                case InventoryType.Equip: flag = StorageEncodeFlags.EncodeInventoryEquip; break;
                case InventoryType.Use: flag = StorageEncodeFlags.EncodeInventoryUse; break;
                case InventoryType.Setup: flag = StorageEncodeFlags.EncodeInventorySetUp; break;
                case InventoryType.Etc: flag = StorageEncodeFlags.EncodeInventoryEtc; break;
                case InventoryType.Cash: flag = StorageEncodeFlags.EncodeInventoryPet; break;
                default: flag = 0; break;
            }
            return flag;
        }

        private static void EncodeStorage(GameCharacter chr, Packet packet, StorageEncodeFlags flags)
        {
            packet.WriteByte(chr.Storage.MaxSlots);

            packet.WriteShort((short)flags);

            if (flags.HasFlag(StorageEncodeFlags.EncodeMesos))
                packet.WriteInt(chr.Storage.Mesos);

            foreach (InventoryType inv in Enum.GetValues(typeof(InventoryType)))
            {
                StorageEncodeFlags flag = GetEncodeFlagForInventory(inv);
                if (flags.HasFlag(flag))
                {
                    AddInvItems(chr, packet, inv);
                }
            }
        }

        public static void AddInvItems(GameCharacter chr, Packet pw, InventoryType inv)
        {
            var itemsInInventory = chr.Storage.GetInventoryItems(inv).ToArray();
            pw.WriteByte((byte)itemsInInventory.Length);

            foreach (Item item in itemsInInventory)
            {
                new GW_ItemSlotBase(item).Encode(pw, false, false);
            }
        }
    }
}