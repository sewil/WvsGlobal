using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static WvsBeta.Common.Constants.EquipSlots;

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
        public static void Add(GameCharacter chr, BaseItem item, Inventory inventory)
        {
            Run(chr, () => new OperationOut
            {
                type = Type.Add,
                inventory = inventory,
                slot = item.InventorySlot,
                item = item
            });
        }

        public static void NoChange(GameCharacter chr)
        {
            Run(chr, new List<Func<OperationOut>>());
        }

        public static void ChangeAmount(GameCharacter chr, BaseItem item, Inventory inventory, short amount)
        {
            Run(chr, () => new OperationOut
            {
                type = Type.ChangeAmount,
                inventory = inventory,
                slot = item.InventorySlot,
                amount = amount
            });
        }
        public static void ChangeAmount(GameCharacter chr, BaseItem item, Inventory inventory)
        {
            ChangeAmount(chr, item, inventory, item.Amount);
        }
        public static void SwitchSlots(GameCharacter chr, BaseItem item, Inventory inventory, short newSlot)
        {
            SwitchSlots(chr, inventory, item.InventorySlot, newSlot);
        }
        public static void SwitchSlots(GameCharacter chr, Inventory inventory, short slot1, short slot2)
        {
            Run(chr, () => new OperationOut
            {
                type = Type.SwitchSlots,
                inventory = inventory,
                slot = slot1,
                slot2 = slot2
            });
        }

        public static void MultiDelete(GameCharacter chr, Inventory inventory, params short[] slots)
        {
            IList<Func<OperationOut>> operations = slots.Select(slot => {
                OperationOut f()
                {
                    return new OperationOut
                    {
                        type = Type.SwitchSlots,
                        slot = slot,
                        slot2 = 0,
                        inventory = inventory
                    };
                }
                return (Func<OperationOut>)f;
            }).ToList();
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
        private static void Run(GameCharacter chr, Func<OperationOut> operation)
        {
            Run(chr, new List<Func<OperationOut>>()
            {
                operation
            });
        }
        private static Packet Init(byte operations)
        {
            var pw = new Packet(ServerMessages.INVENTORY_OPERATION);
            pw.WriteBool(true);
            pw.WriteByte(operations);
            return pw;
        }
        private static void Run(GameCharacter chr, IList<Func<OperationOut>> operations)
        {
            if (operations.Count == 0)
            {
                var pw = Init(0);
                chr.SendPacket(pw);
                return;
            }
            for (int opIdx = 0; opIdx < operations.Count;)
            {
                byte chunk = (byte)Math.Min(MaxPacketOperations, operations.Count - opIdx);
                Packet pw = Init(chunk);
                bool equipitem = false;
                for (byte chunkIdx = 0; chunkIdx < chunk; chunkIdx++)
                {
                    var opout = operations[opIdx + chunkIdx]();
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
                    byte v27 = 0;
                    pw.WriteByte(v27);
                }
                chr.SendPacket(pw);
                opIdx += chunk;
            }
        }
    }
}
