using System;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    class InventoryOperationPacket
    {
        public enum Type
        {
            Add = 0,
            ChangeAmount = 1,
            SwitchSlots = 2,
            Unk3 = 3
        }

        public static void Add(GameCharacter chr, BaseItem item)
        {
            Run(chr, new OperationOut
            {
                type = Type.Add,
                inventory = Constants.getInventory(item.ItemID),
                slot = item.InventorySlot,
                item = item
            });
        }

        public static void NoChange(GameCharacter chr)
        {
            Run(chr);
        }

        public static void ChangeAmount(GameCharacter chr, BaseItem item)
        {
            ChangeAmount(chr, item, item.Amount);
        }
        public static void ChangeAmount(GameCharacter chr, BaseItem item, short amount)
        {
            Run(chr, new OperationOut
            {
                type = Type.ChangeAmount,
                inventory = Constants.getInventory(item.ItemID),
                slot = item.InventorySlot,
                amount = amount
            });
        }
        public static void SwitchSlots(GameCharacter chr, BaseItem item, short newSlot)
        {
            SwitchSlots(chr, Constants.getInventory(item.ItemID), item.InventorySlot, newSlot);
        }
        public static void SwitchSlots(GameCharacter chr, Inventory inventory, short slot1, short slot2)
        {
            Run(chr, new OperationOut
            {
                type = Type.SwitchSlots,
                inventory = inventory,
                slot = slot1,
                slot2 = slot2
            });
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
            Run(chr, operations);
        }

        private const byte MaxPacketOperations = 100;
        struct OperationOut
        {
            public Type type;
            public Inventory inventory;
            public short slot;
            public short slot2;
            public short amount;
            public BaseItem item;
        }
        private static Packet Init(byte operations)
        {
            var pw = new Packet(ServerMessages.INVENTORY_OPERATION);
            pw.WriteBool(true);
            pw.WriteByte(operations);
            return pw;
        }
        private static void Run(GameCharacter chr, params OperationOut[] operations)
        {
            if (operations.Length == 0)
            {
                var pw = Init(0);
                chr.SendPacket(pw);
                return;
            }
            for (int opIdx = 0; opIdx < operations.Length;)
            {
                byte chunk = (byte)Math.Min(MaxPacketOperations, operations.Length - opIdx);
                Packet pw = Init(chunk);
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
                        break;
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
