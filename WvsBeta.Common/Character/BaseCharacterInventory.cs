using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Database;

namespace WvsBeta.Common.Character
{
    public abstract class BaseCharacterInventory
    {
        // Shown and hidden
        public readonly Dictionary<EquippedVisibility, EquipItem[]> Equipped = new Dictionary<EquippedVisibility, EquipItem[]>()
        {
            { EquippedVisibility.Visible, new EquipItem[19] },
            { EquippedVisibility.Hidden, new EquipItem[120] }
        };
        public int ChocoCount { get; protected set; }
        public int ActiveItemID { get; protected set; }
        // All inventories
        public readonly Dictionary<Inventory, BaseItem[]> Items = new Dictionary<Inventory, BaseItem[]>();

        protected Dictionary<int, short> ItemAmounts { get; } = new Dictionary<int, short>();
        public Dictionary<Inventory, byte> MaxSlots { get; } = new Dictionary<Inventory, byte>();
        protected int[] TeleportRockLocations { get; } = new int[5];

        public int Mesos { get; set; }

        public static MySQL_Connection Connection { get; set; }
        protected CharacterCashItems _cashItems;

        private int UserID { get; }
        private int CharacterID { get; }

        protected BaseCharacterInventory(int userId, int characterId)
        {
            UserID = userId;
            CharacterID = characterId;
            _cashItems = new CharacterCashItems(UserID, CharacterID);
        }
        public virtual BaseItem TakeItemAmountFromSlot(Inventory inventory, short slot, short amount, bool takeStars) { throw new NotImplementedException(); }
        public virtual int GetTotalWAttackInEquips(bool star) { throw new NotImplementedException(); }
        public int GetTotalMAttInEquips()
        {
            return Equipped[0]
                .Where(i => i != null)
                .Sum(item => item.Matk);
        }

        public int GetTotalAccInEquips()
        {
            return Equipped[0]
                .Where(i => i != null)
                .Sum(item => item.Acc);
        }

        public short GetOpenSlotsInInventory(Inventory inventory)
        {
            short amount = 0;
            for (short slot = 1; slot <= MaxSlots[inventory]; slot++)
            {
                if (GetItem(inventory, slot) == null)
                    amount++;
            }
            return amount;
        }
        public virtual bool TryExchangeItem(int itemId, short amount) { throw new NotImplementedException(); }
        public virtual int ItemAmountAvailable(int itemid) { throw new NotImplementedException(); }
        public virtual bool HasSlotsFreeForItem(int itemid, short amount) { throw new NotImplementedException(); }
        public virtual short AddNewItem(int id, short amount) { throw new NotImplementedException(); }
        public virtual void SetItem(Inventory inventory, short slot, BaseItem item) { throw new NotImplementedException(); }
        public virtual short GetNextFreeSlotInInventory(Inventory inventory) { throw new NotImplementedException(); }
        public virtual short DeleteFirstItemInInventory(Inventory inv) { throw new NotImplementedException(); }
        public virtual void CheckExpired() { throw new NotImplementedException(); }

        public int GetItemAmount(int itemid)
        {
            int amount = 0;
            BaseItem temp = null;

            foreach (Inventory inventory in Enum.GetValues(typeof(Inventory)))
            {
                for (short slot = 1; slot <= MaxSlots[inventory]; slot++)
                { // Slot 1 - 24, not 0 - 23
                    temp = GetItem(inventory, slot);
                    if (temp != null && temp.ItemID == itemid) amount += temp.Amount;
                }
            }

            return amount;
        }

        public int GetItemAmount(Inventory inventory, int itemid)
        {
            int amount = 0;
            BaseItem temp = null;
            for (short i = 1; i <= MaxSlots[inventory]; i++)
            { // Slot 1 - 24, not 0 - 23
                temp = GetItem(inventory, i);
                if (temp != null && temp.ItemID == itemid) amount += temp.Amount;
            }
            return amount;
        }

        public void LoadInventory()
        {
            using (var data = Connection.RunQuery("SELECT mesos, equip_slots, use_slots, setup_slots, etc_slots, cash_slots FROM characters WHERE id = " + CharacterID) as MySqlDataReader)
            {
                data.Read();

                Mesos = data.GetInt32("mesos");
                SetInventorySlots(Inventory.Equip, (byte)data.GetInt16("equip_slots"), false);
                SetInventorySlots(Inventory.Use, (byte)data.GetInt16("use_slots"), false);
                SetInventorySlots(Inventory.Setup, (byte)data.GetInt16("setup_slots"), false);
                SetInventorySlots(Inventory.Etc, (byte)data.GetInt16("etc_slots"), false);
                SetInventorySlots(Inventory.Cash, (byte)data.GetInt16("cash_slots"), false);
            }

            SplitDBInventory.Load(Connection, "inventory", "charid = " + CharacterID, (type, inventory, slot, item) =>
            {
                AddItem((Inventory)inventory, slot, item, true);
            });

            _cashItems.Load();

            // Move items over to the inventory

            foreach (var cashItemsEquip in _cashItems.Equips)
            {
                Console.WriteLine("Adding cash equip on slot {0}", cashItemsEquip.InventorySlot);
                AddItem(Inventory.Equip, cashItemsEquip.InventorySlot, cashItemsEquip, true);
            }

            foreach (var cashItemsBundle in _cashItems.Bundles)
            {
                Console.WriteLine("Adding bundle on slot {0}", cashItemsBundle.InventorySlot);
                AddItem(Constants.getInventory(cashItemsBundle.ItemID), cashItemsBundle.InventorySlot, cashItemsBundle, true);
            }
            
            foreach (var cashItemPets in _cashItems.Pets)
            {
                Console.WriteLine("Adding pet on slot {0}", cashItemPets.InventorySlot);
                AddItem(Constants.getInventory(cashItemPets.ItemID), cashItemPets.InventorySlot, cashItemPets, true);
            }

            _cashItems.Equips.Clear();
            _cashItems.Bundles.Clear();
            _cashItems.Pets.Clear();

            using (var data = Connection.RunQuery("SELECT mapindex, mapid FROM teleport_rock_locations WHERE charid = " + CharacterID) as MySqlDataReader)
            {
                while (data.Read())
                {
                    TeleportRockLocations[data.GetByte("mapindex")] = data.GetInt32("mapid");
                }
            }

            for (int i = 0; i < TeleportRockLocations.Length; i++)
                if (TeleportRockLocations[i] == 0)
                    TeleportRockLocations[i] = Constants.InvalidMap;
        }

        public void SaveCashItems(CharacterCashItems otherItems)
        {
            // Move cashitems back to the _cashItems object

            // 'Hidden' equips
            _cashItems.Equips.AddRange(Equipped[EquippedVisibility.Hidden].Where(y => y != null && y.CashId != 0));
            // Unequipped equips
            _cashItems.Equips.AddRange(Items[Inventory.Equip].Where(y => y is EquipItem && y.CashId != 0).Select(y => y as EquipItem));
            // Bundles
            _cashItems.Bundles.AddRange(Items.SelectMany(x => x.Value.Where(y => y is BundleItem && y.CashId != 0).Select(y => y as BundleItem)));
            // Pets
            _cashItems.Pets.AddRange(Items.SelectMany(x => x.Value.Where(y => y is PetItem && y.CashId != 0).Select(y => y as PetItem)));

            if (otherItems == null)
                CharacterCashItems.SaveMultiple(_cashItems);
            else
                CharacterCashItems.SaveMultiple(_cashItems, otherItems);

            // Cleanup for next save
            _cashItems.Equips.Clear();
            _cashItems.Bundles.Clear();
            _cashItems.Pets.Clear();
        }


        public void SaveInventory(MySQL_Connection.LogAction dbgCallback = null)
        {

            string query = "UPDATE characters SET " +
                           "mesos = " + Mesos + " ," +
                           "equip_slots = " + MaxSlots[Inventory.Equip] + ", " +
                           "use_slots = " + MaxSlots[Inventory.Use] + ", " +
                           "setup_slots = " + MaxSlots[Inventory.Setup] + ", " +
                           "etc_slots = " + MaxSlots[Inventory.Etc] + ", " +
                           "cash_slots = " + MaxSlots[Inventory.Cash] + " " +
                           "WHERE ID = " + CharacterID;

            Connection.RunTransaction(query, dbgCallback);

            Connection.RunTransaction(comm =>
            {
                comm.CommandText = "DELETE FROM teleport_rock_locations WHERE charid = " + CharacterID;
                comm.ExecuteNonQuery();

                var telerockSave = new StringBuilder();
                telerockSave.Append("INSERT INTO teleport_rock_locations VALUES ");
                int idx = 0;
                telerockSave.Append(string.Join(", ", TeleportRockLocations.Select(location => "(" + CharacterID + ", " + (idx++) + ", " + location + ")")));
                comm.CommandText = telerockSave.ToString();
                comm.ExecuteNonQuery();
            }, dbgCallback);


            SplitDBInventory.Save(
                Connection,
                "inventory",
                CharacterID + ", ",
                "charid = " + CharacterID,
                (type, inventory) =>
                {
                    switch (type)
                    {
                        case SplitDBInventory.InventoryType.Eqp:
                            return Equipped.SelectMany(x => x.Value.Where(y => y != null && y.CashId == 0)).Union(Items[Inventory.Equip].Where(x => x != null && x.CashId == 0));
                        case SplitDBInventory.InventoryType.Bundle:
                            return Items[inventory].Where(x => x != null && x.CashId == 0);
                        default: throw new Exception();
                    }
                },
                dbgCallback
            );
        }

        public void TryRemoveCashItem(BaseItem item)
        {
            var lockerItem = GetLockerItemByCashID(item.CashId);
            if (lockerItem != null)
            {
                RemoveLockerItem(lockerItem, item, true);
            }
        }

        public void AddLockerItem(LockerItem item)
        {
            _cashItems.Items.Add(item);
        }

        public void RemoveLockerItem(LockerItem li, BaseItem item, bool deleteFromDB)
        {
            _cashItems.RemoveItem(li, item);
            if (item != null)
                RemoveItem(item);

            if (deleteFromDB && li.SavedToDatabase) _cashItems.DeletedCashItems.Add(li.CashId);
        }

        public LockerItem GetLockerItemByCashID(long cashId)
        {
            return _cashItems.GetLockerItemFromCashID(cashId);
        }

        public BaseItem GetItemByCashID(long cashId, Inventory inventory)
        {
            BaseItem item = Items[inventory].FirstOrDefault(x => x != null && x.CashId == cashId);
            if (item == null) item = Equipped[EquippedVisibility.Visible].FirstOrDefault(x => x != null && x.CashId == cashId);
            if (item == null) item = Equipped[EquippedVisibility.Hidden].FirstOrDefault(x => x != null && x.CashId == cashId);
            return item;
        }

        public virtual void AddItem(Inventory inventory, short slot, BaseItem item, bool isLoading)
        {
            if (slot == 0)
            {
                // Would bug the client, so ignore
                Trace.WriteLine($"Ignoring item {item.ItemID} because its in the wrong slot (0)");
                return;
            }

            int itemid = item.ItemID;

            if (Constants.getInventory(itemid) != inventory)
            {
                Trace.WriteLine($"Ignoring item {item.ItemID} because its in the wrong inventory ({inventory} vs {Constants.getInventory(itemid)})");
                return;
            }

            item.InventorySlot = slot;

            short amount;
            if (!ItemAmounts.TryGetValue(itemid, out amount))
            {
                amount = 0;
            }
            amount += item.Amount;
            ItemAmounts[itemid] = amount;

            if (slot < 0)
            {
                if (item is EquipItem equipItem)
                {
                    slot = Math.Abs(slot);
                    if (slot > 100)
                    {
                        Equipped[EquippedVisibility.Hidden][(byte)(slot - 100)] = equipItem;
                    }
                    else
                    {
                        Equipped[EquippedVisibility.Visible][(byte)slot] = equipItem;
                    }
                }
                else throw new Exception("Tried to AddItem on an equip slot but its not an equip! " + item);
            }
            else
            {
                Items[inventory][slot] = item;
            }
        }

        public virtual short AddItem(BaseItem item, bool sendpacket = true) { throw new NotImplementedException(); }

        public virtual void TakeItem(BaseItem item, Inventory inventory, short slot, short amount) { throw new NotImplementedException(); }
        public virtual BaseItem GetItem(int itemid, out Inventory inventory, out short slot)
        {
            inventory = 0;
            slot = 0;
            foreach (Inventory _inventory in Enum.GetValues(typeof(Inventory)))
            {
                var items = this.Items[_inventory];
                for (int j = 0; j < items.Length; j++)
                {
                    var item = items[j];
                    if (item != null && item.ItemID == itemid)
                    {
                        inventory = _inventory;
                        slot = item.InventorySlot;
                        return item;
                    }
                }
            }
            return null;
        }
        public virtual void RemoveItem(BaseItem item)
        {
            var inventory = Constants.getInventory(item.ItemID);
            var slot = item.InventorySlot;
            int itemid = item.ItemID;

            if (slot == 0)
            {
                // Would bug the client, so ignore
                Trace.WriteLine($"Ignoring item {itemid} because its in the wrong slot (0)");
                return;
            }

            if (ItemAmounts.TryGetValue(itemid, out var amount))
            {
                if (amount - item.Amount <= 0) ItemAmounts.Remove(itemid);
                else ItemAmounts[itemid] -= item.Amount;
            }

            if (slot < 0)
            {
                if (item is EquipItem)
                {
                    slot = Math.Abs(slot);
                    if (slot > 100)
                    {
                        Equipped[EquippedVisibility.Hidden][(byte)(slot - 100)] = null;
                    }
                    else
                    {
                        Equipped[EquippedVisibility.Visible][(byte)slot] = null;
                    }
                }
                else throw new Exception("Tried to RemoveItem on an equip slot but its not an equip! " + item);
            }
            else
            {
                Items[inventory][slot] = null;
            }
        }

        public BaseItem GetItem(Inventory inventory, short slot)
        {
            BaseItem itm;
            if (slot < 0)
            {
                slot = Math.Abs(slot);
                // Equip.
                if (slot > 100)
                {
                    itm = Equipped[EquippedVisibility.Hidden][(short)(slot - 100)];
                }
                else
                {
                    itm = Equipped[EquippedVisibility.Visible][slot];
                }
            }
            else
            {
                itm = Items[inventory][slot];
            }
            return itm;
        }

        public bool AddRockLocation(int map)
        {
            for (int i = 0; i < 5; i++)
            {
                if (TeleportRockLocations[i] == Constants.InvalidMap)
                {
                    TeleportRockLocations[i] = map;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveRockLocation(int map)
        {
            for (int i = 0; i < 5; i++)
            {
                if (TeleportRockLocations[i] == map)
                {
                    TeleportRockLocations[i] = Constants.InvalidMap;
                    return true;
                }
            }
            return false;
        }

        public bool HasRockLocation(int map)
        {
            for (int i = 0; i < 5; i++)
            {
                if (TeleportRockLocations[i] == map)
                {
                    return true;
                }
            }
            return false;
        }

        public void GenerateInventoryPacket(Packet packet, CharacterDataFlag flags = CharacterDataFlag.All)
        {
            if (flags.HasFlag(CharacterDataFlag.Money))
            {
                packet.WriteInt(Mesos);
            }

            if (flags.HasFlag(CharacterDataFlag.MaxSlots))
            {
                foreach (Inventory inventory in Enum.GetValues(typeof(Inventory)))
                {
                    if (flags.HasFlag((CharacterDataFlag)((short)CharacterDataFlag.Equips << ((byte)inventory-1))) == false) continue;
                    packet.WriteByte(MaxSlots[inventory]);
                }
            }

            if (flags.HasFlag(CharacterDataFlag.Equips))
            {
                foreach (var item in Equipped[EquippedVisibility.Visible])
                {
                    if (item == null) continue;
                    new GW_ItemSlotBase(item).Encode(packet, true, false);
                }

                packet.WriteByte(0);

                foreach (var item in Equipped[EquippedVisibility.Hidden])
                {
                    if (item == null) continue;
                    new GW_ItemSlotBase(item).Encode(packet, true, false);
                }
                packet.WriteByte(0);
            }


            foreach (Inventory inventory in Enum.GetValues(typeof(Inventory)))
            {
                if (flags.HasFlag((CharacterDataFlag)((short)CharacterDataFlag.Equips << ((byte)inventory-1))) == false) continue;
                foreach (var item in Items[inventory])
                {
                    if (item != null && item.InventorySlot > 0)
                    {
                        new GW_ItemSlotBase(item).Encode(packet, true, false);
                    }
                }
                packet.WriteByte(0);
            }
        }

        /// <summary>
        /// Set the MaxSlots for <param name="inventory"/> to <param name="slots" />.
        /// If the Items array is already initialized, it will either expand the array,
        /// or, when <param name="slots" /> is less, will remove items and shrink it.
        /// </summary>
        /// <param name="inventory">Inventory type</param>
        /// <param name="slots">Amount of slots</param>
        public virtual void SetInventorySlots(Inventory inventory, byte slots, bool sendPacket = true)
        {
            if (slots < 24) slots = 24;
            if (slots > 100) slots = 100;

            var invArraySlots = slots + 1;
            if (!Items.ContainsKey(inventory)) Items.Add(inventory, new BaseItem[invArraySlots]);
            else
            {
                var items = Items[inventory];
                Array.Resize(ref items, invArraySlots);
            }

            MaxSlots[inventory] = slots;
        }

        public void AddRockPacket(Packet pw)
        {
            for (int i = 0; i < 5; i++)
            {
                pw.WriteInt(TeleportRockLocations[i]);
            }
        }

        public IEnumerable<PetItem> GetPets()
        {
            return Items[Inventory.Cash].Where(x => x != null && Constants.isPet(x.ItemID)).Select(x => x as PetItem);
        }

        public virtual int GetEquippedItemId(Constants.EquipSlots.Slots slot, bool cash) => throw new NotImplementedException();

        public virtual int GetEquippedItemId(short slot, bool cash) { throw new NotImplementedException(); }
    }
}
