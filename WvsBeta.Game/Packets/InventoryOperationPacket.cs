using System;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using static WvsBeta.Common.Constants.EquipSlots;

namespace WvsBeta.Game.Packets
{
    public struct OperationOut
    {
        public InventoryOperationPacket.Type type;
        public Inventory inventory;
        public short slot;
        public short slot2;
        public short amount;
        public Item item;
    }
    public class InventoryOperationPacket
    {
        public enum Type
        {
            Add = 0,
            ChangeAmount = 1,
            SwitchSlots = 2,
            Unk3 = 3
        }

        public static OperationOut AddOperation(Item item)
        {
            return new OperationOut
            {
                type = Type.Add,
                inventory = Constants.getInventory(item.ItemID),
                slot = item.InventorySlot,
                item = item
            };
        }
        public static void Add(GameCharacter chr, Item item)
        {
            Run(chr, true, AddOperation(item));
        }

        public static void NoChange(GameCharacter chr)
        {
            Run(chr, true);
        }

        public static void ChangeAmount(GameCharacter chr, Item item)
        {
            ChangeAmount(chr, item, item.Amount);
        }
        public static OperationOut ChangeAmountOperation(Inventory inventory, short slot, short amount)
        {
            return new OperationOut
            {
                type = Type.ChangeAmount,
                inventory = inventory,
                slot = slot,
                amount = amount
            };
        }
        public static void ChangeAmount(GameCharacter chr, Item item, short amount)
        {
            Run(chr, true, ChangeAmountOperation(item.Inventory, item.InventorySlot, amount));
        }
        public static void SwitchSlots(GameCharacter chr, Item item, short newSlot)
        {
            SwitchSlots(chr, Constants.getInventory(item.ItemID), item.InventorySlot, newSlot);
        }
        public static OperationOut SwitchSlotsOperation(Inventory inventory, short slot1, short slot2)
        {
            return new OperationOut
            {
                type = Type.SwitchSlots,
                inventory = inventory,
                slot = slot1,
                slot2 = slot2
            };
        }
        public static void SwitchSlots(GameCharacter chr, Inventory inventory, short slot1, short slot2)
        {
            Run(chr, true, SwitchSlotsOperation(inventory, slot1, slot2));
        }

        public static void MultiDelete(GameCharacter chr, Inventory inventory, params short[] slots)
        {
            OperationOut[] operations = slots.Select(slot => new OperationOut
            {
                type = Type.SwitchSlots,
                slot = slot,
                slot2 = 0,
                inventory = inventory
            }).ToArray();
            Run(chr, true, operations);
        }

        private const byte MaxPacketOperations = 100;
        
        private static Packet Init(byte operations, bool isSelf)
        {
            var pw = new Packet(ServerMessages.INVENTORY_OPERATION);
            pw.WriteBool(isSelf);
            pw.WriteByte(operations);
            return pw;
        }
        public static void Run(GameCharacter chr, bool isSelf, params OperationOut[] operations)
        {
            if (operations.Length == 0)
            {
                var pw = Init(0, isSelf);
                chr.SendPacket(pw);
                return;
            }
            for (int opIdx = 0; opIdx < operations.Length;)
            {
                byte chunk = (byte)Math.Min(MaxPacketOperations, operations.Length - opIdx);
                Packet pw = Init(chunk, isSelf);
                bool equipitem = false;
                for (byte chunkIdx = 0; chunkIdx < chunk; chunkIdx++)
                {
                    var opout = operations[opIdx + chunkIdx];
                    pw.WriteByte((byte)opout.type);
                    pw.WriteByte((byte)opout.inventory);
                    pw.WriteShort(opout.slot);

                    if (opout.type == Type.Add)
                    {
                        new GW_ItemSlotBase(opout.item).Encode(pw, false, false);
                    }
                    else if (opout.type == Type.ChangeAmount)
                    {
                        pw.WriteShort(opout.amount);
                    }
                    else if (opout.type == Type.SwitchSlots)
                    {
                        pw.WriteShort(opout.slot2);
                        equipitem = opout.inventory == Inventory.Equip && (opout.slot < 0 || opout.slot2 < 0);
                    }
                    //if (type == Type.Unk3)
                    //{
                    //    if (inventory == Inventory.Equip && slot < 0)
                    //    {
                    //        equipitem = true;
                    //    }
                    //}
                }
                if (equipitem)
                {
                    byte movementInfoIndex = 0;
                    pw.WriteByte(movementInfoIndex);
                    // CWvsContext::HandleMovementInfoIndex
                }
                chr.SendPacket(pw);
                opIdx += chunk;
            }
        }
    }
}
