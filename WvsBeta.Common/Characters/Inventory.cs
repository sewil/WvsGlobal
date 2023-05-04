using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Database;

namespace WvsBeta.Common.Characters
{
    public class Inventory
    {
        // Shown and hidden
        public readonly Dictionary<EquippedType, Dictionary<Constants.EquipSlots.Slots, EquipItem>> Equipped = new Dictionary<EquippedType, Dictionary<Constants.EquipSlots.Slots, EquipItem>>()
        {
            { EquippedType.Normal, new Dictionary<Constants.EquipSlots.Slots, EquipItem>() },
            { EquippedType.Cash, new Dictionary<Constants.EquipSlots.Slots, EquipItem>() }
        };
        public int ChocoCount { get; protected set; }
        public int ActiveItemID { get; protected set; }
        // All inventories
        public readonly Dictionary<InventoryType, Item[]> Items = new Dictionary<InventoryType, Item[]>();

        protected Dictionary<int, short> ItemAmounts { get; } = new Dictionary<int, short>();
        public Dictionary<InventoryType, byte> MaxSlots { get; } = new Dictionary<InventoryType, byte>();
        protected int[] TeleportRockLocations { get; } = new int[5];

        public int Mesos { get; set; }

        public static MySQL_Connection Connection { get; set; }
        protected CharacterCashItems _cashItems;

        private int UserID { get; }
        private int CharacterID { get; }

        public Inventory(int userId, int characterId)
        {
            UserID = userId;
            CharacterID = characterId;
            _cashItems = new CharacterCashItems(UserID, CharacterID);
        }
        public virtual Item TakeItemAmountFromSlot(InventoryType inventory, short slot, short amount, bool takeStars) { throw new NotImplementedException(); }
        public virtual int GetTotalWAttackInEquips(bool star) { throw new NotImplementedException(); }
        public int GetTotalMAttInEquips()
        {
            return Equipped[EquippedType.Normal].Sum(i => i.Value.Matk);
        }

        public int GetTotalAccInEquips()
        {
            return Equipped[EquippedType.Normal].Sum(item => item.Value.Acc);
        }

        public short GetOpenSlotsInInventory(InventoryType inventory)
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

        public virtual void SetItem(InventoryType inventory, short slot, Item item)
        {
            if (item != null) item.InventorySlot = slot;
            if (slot < 0)
            {
                var eqSlot = Constants.getEquipSlot(slot, out EquippedType type);
                if (eqSlot == Constants.EquipSlots.Slots.Invalid)
                {
                    Trace.WriteLine("Invalid eq slot " + slot + " in SetItem");
                    return;
                }
                EquipItem equipItem = item as EquipItem;

                if (item == null) Equipped[type].Remove(eqSlot);
                else Equipped[type][eqSlot] = equipItem;
            }
            else
            {
                Items[inventory][slot] = item;
            }
        }
        public virtual short GetNextFreeSlotInInventory(InventoryType inventory) { throw new NotImplementedException(); }
        public virtual short DeleteFirstItemInInventory(InventoryType inv) { throw new NotImplementedException(); }

        public int GetItemAmount(int itemid)
        {
            int amount = 0;
            InventoryType inv = Constants.getInventory(itemid);

            for (short i = 1; i <= MaxSlots[inv]; i++)
            { // Slot 1 - 24, not 0 - 23
                Item item = GetItem(inv, i);
                if (item != null && item.ItemID == itemid) amount += item.Amount;
            }
            return amount;
        }

        public void LoadInventory()
        {
            using (var data = Connection.RunQuery("SELECT mesos, equip_slots, use_slots, setup_slots, etc_slots, cash_slots FROM characters WHERE id = " + CharacterID) as MySqlDataReader)
            {
                data.Read();

                Mesos = data.GetInt32("mesos");
                SetInventorySlots(InventoryType.Equip, (byte)data.GetInt16("equip_slots"), false);
                SetInventorySlots(InventoryType.Use, (byte)data.GetInt16("use_slots"), false);
                SetInventorySlots(InventoryType.Setup, (byte)data.GetInt16("setup_slots"), false);
                SetInventorySlots(InventoryType.Etc, (byte)data.GetInt16("etc_slots"), false);
                SetInventorySlots(InventoryType.Cash, (byte)data.GetInt16("cash_slots"), false);
            }

            SplitDBInventory.Load(Connection, "inventory", "charid = " + CharacterID, (type, inventory, slot, item) =>
            {
                AddItem(inventory, slot, item, true);
            });

            _cashItems.Load();

            // Move items over to the inventory

            foreach (var cashItemsEquip in _cashItems.Equips)
            {
                Console.WriteLine("Adding cash equip on slot {0}", cashItemsEquip.InventorySlot);
                AddItem(InventoryType.Equip, cashItemsEquip.InventorySlot, cashItemsEquip, true);
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

            // Cash equips
            _cashItems.Equips.AddRange(Equipped[EquippedType.Cash].Select(i => i.Value).Where(y => y.CashId != 0));
            // Unequipped equips
            _cashItems.Equips.AddRange(Items[InventoryType.Equip].Where(y => y is EquipItem && y.CashId != 0).Select(y => y as EquipItem));
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
                           "equip_slots = " + MaxSlots[InventoryType.Equip] + ", " +
                           "use_slots = " + MaxSlots[InventoryType.Use] + ", " +
                           "setup_slots = " + MaxSlots[InventoryType.Setup] + ", " +
                           "etc_slots = " + MaxSlots[InventoryType.Etc] + ", " +
                           "cash_slots = " + MaxSlots[InventoryType.Cash] + " " +
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
                            return Equipped.SelectMany(x => x.Value.Select(i => i.Value).Where(y => y.CashId == 0)).Union(Items[InventoryType.Equip].Where(x => x != null && x.CashId == 0));
                        case SplitDBInventory.InventoryType.Bundle:
                            return Items[inventory].Where(x => x != null && x.CashId == 0);
                        default: throw new Exception();
                    }
                },
                dbgCallback
            );
        }

        public void TryRemoveCashItem(Item item)
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

        public void RemoveLockerItem(LockerItem li, Item item, bool deleteFromDB)
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

        public Item GetItemByCashID(long cashId, InventoryType inventory)
        {
            Item item = Items[inventory].FirstOrDefault(x => x != null && x.CashId == cashId);
            if (item == null) Equipped[EquippedType.Cash].Select(i => i.Value).TryFind(i => i.CashId == cashId, out item);
            return item;
        }

        public virtual void AddItem(InventoryType inventory, short slot, Item item, bool isLoading)
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

            SetItem(inventory, slot, item);
        }

        public virtual void TakeItem(Item item, InventoryType inventory, short slot, short amount) { throw new NotImplementedException(); }
        /// <summary>
        /// Get first item from an array of item ids
        /// </summary>
        public Item GetFirstItem(InventoryType inv, params int[] itemids)
        {
            for (byte slot = 0; slot < MaxSlots[inv]; slot++)
            {
                var item = Items[inv][slot];
                if (item == null) continue;
                foreach (int itemid in itemids)
                {
                    if (item.ItemID == itemid) return item;
                }
            }
            return null;
        }
        public bool TryGetItem(int itemid, out Item item)
        {
            item = GetItem(itemid);
            return item != null;
        }
        public Item GetItem(int itemid)
        {
            InventoryType inv = Constants.getInventory(itemid);
            foreach (var item in Items[inv])
            {
                if (item == null || item.ItemID != itemid) continue;
                return item;
            }
            return null;
        }
        public virtual void RemoveItem(Item item)
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
            SetItem(inventory, slot, null);
        }

        public Item GetItem(InventoryType inventory, short slot)
        {
            if (slot < 0)
            {
                var eqSlot = Constants.getEquipSlot(slot, out EquippedType type);
                if (eqSlot == Constants.EquipSlots.Slots.Invalid)
                {
                    Trace.WriteLine("Unknown eq slot " + slot + " in GetItem!");
                    return null;
                }
                Equipped[type].TryGetValue(eqSlot, out EquipItem item);
                return item;
            }
            else
            {
                return Items[inventory][slot];
            }
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
                foreach (InventoryType inventory in Enum.GetValues(typeof(InventoryType)))
                {
                    if (flags.HasFlag((CharacterDataFlag)((short)CharacterDataFlag.Equips << ((byte)inventory - 1))) == false) continue;
                    packet.WriteByte(MaxSlots[inventory]);
                }
            }

            if (flags.HasFlag(CharacterDataFlag.Equips))
            {
                foreach (var item in Equipped[EquippedType.Normal].Select(i => i.Value))
                {
                    new GW_ItemSlotBase(item).Encode(packet, true, false);
                }

                packet.WriteByte(0);

                foreach (var item in Equipped[EquippedType.Cash].Select(i => i.Value))
                {
                    new GW_ItemSlotBase(item).Encode(packet, true, false);
                }
                packet.WriteByte(0);
            }


            foreach (InventoryType inventory in Enum.GetValues(typeof(InventoryType)))
            {
                if (flags.HasFlag((CharacterDataFlag)((short)CharacterDataFlag.Equips << ((byte)inventory - 1))) == false) continue;
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
        public virtual void SetInventorySlots(InventoryType inventory, byte slots, bool sendPacket = true)
        {
            if (slots < 24) slots = 24;
            if (slots > 100) slots = 100;

            var invArraySlots = slots + 1;
            if (!Items.ContainsKey(inventory)) Items.Add(inventory, new Item[invArraySlots]);
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

        public bool TryGetPet(long cashId, out PetItem petItem)
        {
            petItem = null;
            if (!Items[InventoryType.Cash].TryFind(i => i?.CashId == cashId, out Item baseItem) || !(baseItem is PetItem pi))
            {
                return false;
            }
            petItem = pi;
            return true;
        }

        public IEnumerable<PetItem> GetPets()
        {
            return Items[InventoryType.Cash].Where(x => x != null && Constants.isPet(x.ItemID)).Select(x => x as PetItem);
        }

        public EquipItem GetEquippedItem(Constants.EquipSlots.Slots slot, EquippedType type)
        {
            return Equipped[type].GetValue(slot);
        }
        public int GetEquippedItemId(Constants.EquipSlots.Slots slot, EquippedType type)
        {
            return GetEquippedItem(slot, type)?.ItemID ?? 0;
        }
    }
}
