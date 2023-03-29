using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using WvsBeta.Common;
using WvsBeta.Common.Character;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public class CharacterInventory : BaseCharacterInventory
    {
        private GameCharacter Character { get; set; }

        public CharacterInventory(GameCharacter character) : base(character.UserID, character.ID)
        {
            Character = character;
        }

        public void SaveInventory()
        {
            base.SaveInventory(Program.MainForm.LogAppend);
        }

        public new void LoadInventory()
        {
            base.LoadInventory();

            UpdateChocoCount(false);
        }

        public void AddItem(int itemId, short amount)
        {
            var item = BaseItem.CreateFromItemID(itemId, amount);
            AddItem(item);
        }
        public override void AddItem(Inventory inventory, short slot, BaseItem item, bool isLoading)
        {
            base.AddItem(inventory, slot, item, isLoading);

            if (slot < 0 && item is EquipItem equipItem)
            {
                slot = Math.Abs(slot);
                Character.PrimaryStats.AddEquipStats((sbyte)slot, equipItem, isLoading);
            }

            if (!isLoading)
                UpdateChocoCount();
        }

        public override void SetItem(Inventory inventory, short slot, BaseItem item)
        {
            if (item != null) item.InventorySlot = slot;
            if (slot < 0)
            {

                EquipItem equipItem = item as EquipItem;
                slot = Math.Abs(slot);
                if (slot > 100)
                {
                    Equipped[EquippedType.Cash][(byte)(slot - 100)] = equipItem;
                }
                else
                {
                    Equipped[EquippedType.Normal][(byte)slot] = equipItem;
                    Character.PrimaryStats.AddEquipStats((sbyte)slot, equipItem, false);
                }
            }
            else
            {
                Items[inventory][slot] = item;
            }

            UpdateChocoCount();
        }

        public override int GetEquippedItemId(Constants.EquipSlots.Slots slot, bool cash) => GetEquippedItemId((short)slot, cash);

        public BaseItem GetEquippedItem(int itemid, out short slotfrom, out EquippedType type)
        {
            slotfrom = 0;
            type = (EquippedType)(-1);
            foreach (var equips in Equipped)
            {
                foreach (var item in equips.Value)
                {
                    if (item?.ItemID == itemid)
                    {
                        slotfrom = item.InventorySlot;
                        type = equips.Key;
                        return item;
                    }
                }
            }
            return null;
        }

        public override int GetEquippedItemId(short slot, bool cash)
        {
            if (!cash)
            {
                slot = Math.Abs(slot);
                if (Equipped[EquippedType.Normal].Length > slot)
                {
                    if (Equipped[EquippedType.Normal][slot] != null)
                    {
                        return Equipped[EquippedType.Normal][slot].ItemID;
                    }
                }
            }
            else
            {
                if (slot < -100)
                {
                    slot += 100;
                }
                slot = Math.Abs(slot);
                if (Equipped[EquippedType.Cash].Length > slot)
                {
                    if (Equipped[EquippedType.Cash][slot] != null)
                    {
                        return Equipped[EquippedType.Cash][slot].ItemID;
                    }
                }
            }
            return 0;
        }

        public void UpdateChocoCount(bool sendPacket = true)
        {

            int prevChocoCount = ChocoCount;
            ChocoCount = Items[Constants.getInventory(Constants.Items.Choco)].Count(x => x?.ItemID == Constants.Items.Choco);
            ActiveItemID = ChocoCount > 0 ? Constants.Items.Choco : 0;

            if (sendPacket && prevChocoCount != ChocoCount)
            {
                MapPacket.SendAvatarModified(Character, MapPacket.AvatarModFlag.ItemEffects);
            }
        }

        public override short AddItem(BaseItem item, bool sendpacket = true)
        {
            Inventory inventory = Constants.getInventory(item.ItemID);
            short slot = 0;
            // see if there's a free slot
            BaseItem temp = null;
            short maxSlots = 1;
            if (DataProvider.Items.TryGetValue(item.ItemID, out ItemData itemData))
            {
                maxSlots = (short)itemData.MaxSlot;
                if (maxSlots == 0)
                {
                    // 1, 100 or specified
                    maxSlots = 100;
                }
            }
            for (short i = 1; i <= MaxSlots[inventory]; i++)
            { // Slot 1 - 24, not 0 - 23
                temp = GetItem(inventory, i);
                if (temp != null)
                {
                    if (Constants.isStackable(item.ItemID) && item.ItemID == temp.ItemID && temp.Amount < maxSlots)
                    {
                        if (item.Amount + temp.Amount > maxSlots)
                        {

                            short amount = (short)(maxSlots - temp.Amount);
                            item.Amount -= amount;
                            temp.Amount = maxSlots;
                            if (sendpacket)
                                InventoryOperationPacket.ChangeAmount(Character, temp, inventory);
                        }
                        else
                        {
                            item.Amount += temp.Amount;
                            // Removing the item looks a bit odd to me?
                            SetItem(inventory, i, null);
                            AddItem(inventory, i, item, false);
                            if (sendpacket)
                                InventoryOperationPacket.ChangeAmount(Character, item, inventory);
                            return 0;
                        }
                    }
                }
                else if (slot == 0)
                {
                    slot = i;
                    if (!Constants.isStackable(item.ItemID))
                    {
                        break;
                    }
                }
            }
            if (slot != 0)
            {
                SetItem(inventory, slot, item);
                if (sendpacket)
                    InventoryOperationPacket.Add(Character, item, inventory);
                return 0;
            }
            else
            {
                return item.Amount;
            }
        }

        public override short AddNewItem(int id, short amount) // Only normal items!
        {
            if (!DataProvider.Items.ContainsKey(id) &&
                !DataProvider.Equips.ContainsKey(id) &&
                !DataProvider.Pets.ContainsKey(id))
            {
                return 0;
            }

            short max = 1;
            if (!Constants.isEquip(id) && !Constants.isPet(id))
            {
                max = (short)DataProvider.Items[id].MaxSlot;
                if (max == 0)
                {
                    max = 100;
                }
            }
            short thisAmount = 0, givenAmount = 0;

            if (Constants.isRechargeable(id))
            {
                thisAmount = (short)(max + Character.Skills.GetRechargeableBonus());
                amount -= 1;
            }
            else if (Constants.isEquip(id) || Constants.isPet(id))
            {
                thisAmount = 1;
                amount -= 1;
            }
            else if (amount > max)
            {
                thisAmount = max;
                amount -= max;
            }
            else
            {
                thisAmount = amount;
                amount = 0;
            }

            if (Constants.isPet(id))
            {
                givenAmount = 0;
            }
            else
            {
                var item = BaseItem.CreateFromItemID(id);
                item.Amount = thisAmount;
                if (item is EquipItem)
                    (item as EquipItem).GiveStats(ItemVariation.None);
                givenAmount += thisAmount;
                if (AddItem(item) == 0 && amount > 0)
                {
                    givenAmount += AddNewItem(id, amount);
                }
            }

            return givenAmount;
        }

        public override bool HasSlotsFreeForItem(int itemid, short amount)
        {
            short slotsRequired = 0;
            Inventory inventory = Constants.getInventory(itemid);
            bool stackable = Constants.isStackable(itemid);
            if (!stackable)
            {
                slotsRequired = amount;
            }
            else if (Constants.isStar(itemid))
            {
                slotsRequired = 1;
            }
            else
            {
                short maxPerSlot = (short)DataProvider.Items[itemid].MaxSlot;
                if (maxPerSlot == 0) maxPerSlot = 100; // default 100 O.o >_>
                short hasAmounts = (short)(ItemAmounts.ContainsKey(itemid) ? ItemAmounts[itemid] : 0);
                if (stackable && hasAmounts > 0)
                {
                    // We should try to see which slots we can fill, and determine how much new slots are left

                    short amountLeft = amount;
                    Inventory inv = Constants.getInventory(itemid);
                    foreach (var item in Items[inv].ToList().FindAll(x => x != null && x.ItemID == itemid && x.Amount < maxPerSlot))
                    {
                        amountLeft -= (short)(maxPerSlot - item.Amount); // Substract the amount of 'slots' left for this slot
                        if (amountLeft <= 0)
                        {
                            amountLeft = 0;
                            break;
                        }
                    }

                    // Okay, so we need to figure out where to keep these stackable items.

                    // Apparently we've got space left on slots
                    if (amountLeft == 0) return true;

                    // Hmm, still need to get more slots
                    amount = amountLeft;
                }

                slotsRequired = (short)(amount / maxPerSlot);
                // Leftover slots to handle
                if ((amount % maxPerSlot) > 0)
                    slotsRequired++;

            }
            return GetOpenSlotsInInventory(inventory) >= slotsRequired;
        }

        public override int ItemAmountAvailable(int itemid)
        {
            Inventory inv = Constants.getInventory(itemid);
            int available = 0;
            short maxPerSlot = (short)(DataProvider.Items.ContainsKey(itemid) ? DataProvider.Items[itemid].MaxSlot : 1); // equip
            if (maxPerSlot == 0) maxPerSlot = 100; // default 100 O.o >_>

            short openSlots = GetOpenSlotsInInventory(inv);
            available += (openSlots * maxPerSlot);

            BaseItem temp = null;

            for (short i = 1; i <= MaxSlots[inv]; i++)
            {
                temp = GetItem(inv, i);
                if (temp != null && temp.ItemID == itemid)
                    available += (maxPerSlot - temp.Amount);
            }

            return available;
        }

        public override short GetNextFreeSlotInInventory(Inventory inventory)
        {
            for (short i = 1; i <= MaxSlots[inventory]; i++)
            {
                if (GetItem(inventory, i) == null)
                    return i;
            }
            return -1;
        }

        public override short DeleteFirstItemInInventory(Inventory inv)
        {
            for (short i = 1; i <= MaxSlots[inv]; i++)
            {
                if (Items[inv][i] != null)
                {
                    Items[inv][i] = null;
                    UpdateChocoCount();
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// Set the MaxSlots for <param name="inventory"/> to <param name="slots" />.
        /// If the Items array is already initialized, it will either expand the array,
        /// or, when <param name="slots" /> is less, will remove items and shrink it.
        /// </summary>
        /// <param name="inventory">Inventory ID</param>
        /// <param name="slots">Amount of slots</param>
        public override void SetInventorySlots(Inventory inventory, byte slots, bool sendPacket = true)
        {
            base.SetInventorySlots(inventory, slots, sendPacket);

            if (sendPacket)
                InventoryPacket.IncreaseSlots(Character, inventory, slots);
        }

        public override void TakeItem(BaseItem item, Inventory inventory, short slot, short amount)
        {
            var isRechargeable = Constants.isRechargeable(item.ItemID);
            if (amount > item.Amount) return;
            item.Amount -= amount;
            if (item.Amount == 0 && !isRechargeable)
            {
                // Your item. Gone.
                SetItem(inventory, slot, null);
                TryRemoveCashItem(item);
                InventoryOperationPacket.SwitchSlots(Character, inventory, slot, 0);
            }
            else
            {
                // Update item with new amount
                InventoryOperationPacket.ChangeAmount(Character, item, inventory);
            }
        }

        public bool HasItemAmount(int itemId, short amount)
        {
            if (amount <= 0) return false;

            short actualAmount = 0;
            var isRechargeable = Constants.isRechargeable(itemId);
            var inv = Constants.getInventory(itemId);
            for (short slot = 1; slot <= MaxSlots[inv]; slot++)
            {
                var item = GetItem(inv, slot);
                if (item == null || item.ItemID != itemId) continue;

                actualAmount += isRechargeable ? (short)1 : item.Amount;
                if (actualAmount >= amount) return true;
            }
            return false;
        }

        public void AddMesos(int value, bool isSelf = false)
        {
            AddMesos(value, isSelf, out int _);
        }
        public void AddMesos(int value, bool isSelf, out int mesosDiff)
        {
            int newMesos = Math.Max(0, Math.Min(int.MaxValue, Mesos + value));
            mesosDiff = newMesos - Mesos;
            Mesos = newMesos;
            CharacterStatsPacket.SendStatChanged(Character, StatFlags.Mesos, isSelf);
        }
        /// <summary>
        /// Try to remove <paramref name="amount"/> amount of itemid <paramref name="itemid"/>.
        /// Does not 'remove' stacks, keeps them as-is (with 0 items).
        /// </summary>
        /// <param name="itemid">The Item ID</param>
        /// <param name="amount">Amount</param>
        /// <returns>Amount of items that were _not_ taken away</returns>
        public void TakeItem(int itemid, short amount)
        {
            if (amount == 0) return;

            var isRechargeable = Constants.isRechargeable(itemid);
            Inventory inventory = Constants.getInventory(itemid);
            short amountLeft = (short)amount;
            for (short slot = 1; slot <= MaxSlots[inventory]; slot++)
            {
                if (amountLeft == 0) break; // Nothing more to take
                BaseItem item = GetItem(inventory, slot);
                if (item == null || item.ItemID != itemid) continue;

                short maxTake = Math.Min(item.Amount, amountLeft);
                item.Amount -= (short)maxTake;
                amountLeft -= maxTake;
                if (item.Amount == 0 && !isRechargeable)
                {
                    // Your item. Gone.
                    SetItem(inventory, slot, null);
                    TryRemoveCashItem(item);
                    InventoryOperationPacket.SwitchSlots(Character, inventory, slot, 0);
                }
                else
                {
                    // Update item with new amount
                    InventoryOperationPacket.ChangeAmount(Character, item, inventory);
                }
            }
        }

        public override BaseItem TakeItemAmountFromSlot(Inventory inventory, short slot, short amount, bool takeStars)
        {
            var item = GetItem(inventory, slot);

            if (item == null) return null;

            if (!takeStars)
            {
                if (item.Amount - amount < 0) return null;
            }

            bool removeItem = false;
            BaseItem newItem;
            if (takeStars && Constants.isStar(item.ItemID))
            {
                // Take the whole item
                newItem = item;
                removeItem = true;
            }
            else
            {
                newItem = item.SplitInTwo(amount);
                removeItem = item.Amount == 0 && !Constants.isStar(item.ItemID);
            }

            if (removeItem)
            {
                SetItem(inventory, slot, null);
                TryRemoveCashItem(item);
                InventoryOperationPacket.SwitchSlots(Character, inventory, slot, 0);
            }
            else
            {
                // Update item
                InventoryOperationPacket.ChangeAmount(Character, item, inventory);
            }

            return newItem;
        }

        public Dictionary<byte, int> GetVisibleEquips()
        {
            Dictionary<byte, int> shown = new Dictionary<byte, int>();


            foreach (var item in Equipped[EquippedType.Cash])
            {
                if (item != null)
                {
                    byte slotuse = (byte)Math.Abs(item.InventorySlot);
                    if (slotuse > 100) slotuse -= 100;
                    shown.Add(slotuse, item.ItemID);
                }
            }

            foreach (var item in Equipped[0])
            {
                if (item != null && !shown.ContainsKey((byte)Math.Abs(item.InventorySlot)))
                {
                    shown.Add((byte)Math.Abs(item.InventorySlot), item.ItemID);
                }
            }
            return shown;
        }

        public override int GetTotalWAttackInEquips(bool star)
        {
            int totalWat = 0;

            foreach (var item in Equipped[EquippedType.Normal])
            {
                if (item?.Watk > 0)
                {
                    totalWat += item.Watk;
                }
            }

            if (star == true)
            {
                foreach (BaseItem item in Items[Inventory.Use])
                {
                    if (item != null)
                    {
                        if (Constants.isStar(item.ItemID))
                        {
                            switch (item.ItemID)
                            {
                                case 2070000: totalWat += 15; break; // Subi Throwing Star +15
                                case 2070001:                        // Wolbi Throwing Star +17
                                case 2070008: totalWat += 17; break; // Snowball +17
                                case 2070002:                        // Mokbi Throwing Star +19
                                case 2070009: totalWat += 19; break; // Top +19
                                case 2070012: totalWat += 20; break; // Paper Airplane +20
                                case 2070003:                        // Kumbi Throwing Star +21
                                case 2070010:                        // Icicle +21
                                case 2070011: totalWat += 21; break; // Maple Throwing Star +21
                                case 2070004: totalWat += 23; break; // Tobi Throwing Star +23
                                case 2070005: totalWat += 25; break; // Steely Throwing Star +25
                                case 2070006:                        // Ilbi Throwing Star +27
                                case 2070007: totalWat += 27; break; // Hwabi Throwing Star +27
                            }

                            break;
                        }

                        else if (Constants.isArrow(item.ItemID))
                        {
                            switch (item.ItemID)
                            {
                                case 2060000:                       // Arrow For Bow
                                case 2061000: break;                // Arrow for Crossbow
                                case 2060001:                       // Bronze Arrow for Bow
                                case 2061001:                       // Bronze Arrow for Crossbow
                                case 2060002:                       // Steel Arrow for Bow
                                case 2061002: totalWat += 1; break; // Steel Arrow for Crossbow
                            }
                        }
                    }
                }
            }

            return totalWat;
        }

        public double GetExtraExpRate()
        {
            // Holiday stuff here.
            double rate = 1;

            foreach (BaseItem item in Items[Inventory.Etc])
            {
                if (item == null || item.ItemID < 4100000 || item.ItemID >= 4200000) continue; // ???
                ItemData id = DataProvider.Items[item.ItemID];
                if (ItemData.RateCardEnabled(id, false))
                {
                    if (rate < id.Rate) rate = id.Rate;
                }
            }
            return rate;
        }


        private long lastCheck = 0;
        public void GetExpiredItems(long time, Action<List<BaseItem>> callback)
        {
            if (time - lastCheck < 45000) return;
            lastCheck = time;

            var allItems = Equipped.SelectMany(i => i.Value)
                .Concat(Items.SelectMany(i => i.Value))
                .Where(x =>
                    x != null &&
                    x.Expiration < time
                )
                .ToList();

            if (allItems.Count == 0) return;

            callback(allItems);
        }

        public override void CheckExpired()
        {
            var currentTime = MasterThread.CurrentDate.ToFileTimeUtc();
            _cashItems.GetExpiredItems(currentTime, expiredItems =>
            {
                var dict = new Dictionary<Inventory, List<short>>();
                expiredItems.ForEach(x =>
                {
                    InventoryPacket.SendCashItemExpired(Character, x.ItemId);
                    Inventory inventory = Constants.getInventory(x.ItemId);
                    var baseItem = GetItemByCashID(x.CashId, inventory);

                    if (baseItem != null)
                    {
                        if (dict.TryGetValue(inventory, out var curList)) curList.Add(baseItem.InventorySlot);
                        else
                        {
                            dict[inventory] = new List<short> { baseItem.InventorySlot };
                        }
                    }
                    RemoveLockerItem(x, baseItem, true);
                });

                dict.ForEach(x => InventoryOperationPacket.MultiDelete(Character, x.Key, x.Value.ToArray()));
            });

            GetExpiredItems(currentTime, expiredItems =>
            {
                var dict = new Dictionary<Inventory, List<short>>();
                var itemIds = new List<int>();
                expiredItems.ForEach(x =>
                {
                    Inventory inventory = Constants.getInventory(x.ItemID);
                    if (x.CashId != 0)
                    {
                        var baseItem = GetItemByCashID(x.CashId, inventory);
                        if (dict.TryGetValue(inventory, out var curList)) curList.Add(baseItem.InventorySlot);
                        else
                        {
                            dict[inventory] = new List<short> {baseItem.InventorySlot};
                        }
                        TryRemoveCashItem(x);
                    }
                    SetItem(inventory, x.InventorySlot, null);
                    itemIds.Add(x.ItemID);
                });

                InventoryPacket.SendItemsExpired(Character, itemIds);
                dict.ForEach(x => InventoryOperationPacket.MultiDelete(Character, x.Key, x.Value.ToArray()));
            });
        }
        #region Script helpers
        public int SlotCount(byte inventory)
        {
            return SlotCount((Inventory)inventory);
        }
        public int SlotCount(Inventory inventory)
        {
            return MaxSlots[inventory];
        }
        public int HoldCount(byte inventory)
        {
            return HoldCount((Inventory)inventory);
        }
        public int HoldCount(Inventory inventory)
        {
            int holdCount = 0;
            for (short slot = 1; slot <= MaxSlots[inventory]; slot++)
            {
                if (GetItem(inventory, slot) != null) holdCount++;
            }
            return holdCount;
        }
        public int ItemCount(int itemid)
        {
            return GetItemAmount(itemid);
        }
        public bool SetPetLife(string petcashid, params int[] useItems)
        {
            foreach (var item in useItems)
            {
                if (!HasItemAmount(item, 1)) return false;
            }
            var pets = GetPets();
            return false; // TODO: Implement pet revival
        }
        public int MovePetStat(string petcashidfrom, string petcashidto, int useItem)
        {
            /*
                0 = The pet's closeness was transferred successfully.
                1 = Please see if you have the appropriate item.
                2 = The closeness of the new pet seems to be higher than the existing pet. Check again.
            */
            return -1; // TODO: Implement move pet closeness, useItem is pet ap reset scroll
        }
        public bool CanExchange(int mesos, params (int itemid, short amount)[] items)
        {
            if (mesos != 0)
            {
                long newM = (long)Mesos + (long)mesos;
                bool canExchangeMesos = 0 <= newM && newM <= int.MaxValue;
                if (!canExchangeMesos) return false;
            }
            foreach (var (itemid,amount) in items)
            {
                if (amount == 0) continue;
                if (amount < 0 && !HasItemAmount(itemid, Math.Abs(amount))) // Take item
                {
                    return false;
                }
                else if (amount > 0 && !HasSlotsFreeForItem(itemid, amount)) // Give item
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Mass exchange, used by scripts when exchanging with NPCs
        /// </summary>
        /// <param name="mesos">mesos to exchange, can be positive or negative</param>
        /// <param name="items">items to exchange, alternating between itemid and amount: itemid,amount,itemid,amount...</param>
        /// <returns>Status code: 1 = success, 0 = error</returns>
        public int Exchange(int mesos, params int[] items)
        {
            if (items.Length % 2 != 0)
            {
                Program.MainForm.LogDebug("Invalid exchange args {0} {1}", mesos, items);
                return 0;
            }
            // Parse
            var parsedItems = new List<(int itemid, short amount)>();
            for (int i = 0; i < items.Length; i++)
            {
                if (i % 2 != 0) continue;
                short amount = (short)items[i + 1];
                if (amount == 0) continue;
                parsedItems.Add((items[i], amount));
            }
            return MassExchange(mesos, parsedItems.ToArray()) ? 1 : 0;
        }
        public bool MassExchange(int mesos, params (int itemid, short amount)[] items)
        {
            if (!CanExchange(mesos, items)) return false;

            AddMesos(mesos);
            if (mesos != 0)
            {
                Character.SendPacket(MessagePacket.GainMesos(mesos));
            }
            foreach(var (itemid,amount) in items)
            {
                if (amount < 0) // Take item
                {
                    TakeItem(itemid, (short)-amount);
                }
                else // Give item
                {
                    AddItem(itemid, amount);
                }
            }
            PlayerEffectPacket.SendInventoryChanged(Character, items);
            return true;
        }
        /// <summary>
        /// Mass exchange for rechargables:
        /// ExchangeEx(0, "2070006,Count:800", 800, "2070006,Count:800", 800, "2070006,Count:800", 800, "2070006,Count:800", 800, "2070006,Count:800", 800)
        /// </summary>
        /// <returns>Status code: 1 = success, 0 = error</returns>
        /// <exception cref="ArgumentException"></exception>
        public int ExchangeEx(int mesos, params object[] items)
        {
            int[] parsedItems = new int[items.Length];
            try
            {
                for (int i = 0; i < items.Length; i++)
                {
                    object item = items[i];
                    if (item is string)
                    {
                        int itemid = string.Join(",", item.ToString())[0];
                        int amount = (int)items[i + 1];
                        parsedItems[i] = itemid;
                        parsedItems[i + 1] = amount;
                    }
                }
            }
            catch
            {
             // fu then
                throw new ArgumentException();
            }
            return Exchange(mesos, parsedItems);
        }

        public void IncSlotCount(byte inventory, byte slots)
        {
            SetInventorySlots((Inventory)inventory, slots);
        }
        /// <summary>
        /// Remove equipped item
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns>-1 = item not found, 0 = success, 1 = failed to unequip</returns>
        public int RemoveEquippedItem(int itemid)
        {
            var item = GetEquippedItem(itemid, out short slotfrom, out EquippedType type);
            if (item == null) return -1;
            Equipped[type][-slotfrom] = null;
            InventoryOperationPacket.SwitchSlots(Character, Inventory.Equip, slotfrom, 0);
            MapPacket.SendAvatarModified(Character, MapPacket.AvatarModFlag.AvatarLook);
            return 0;
        }

        public int HasThisItemInHisParty(int itemid)
        {
            var party = Character.Party;
            if (party == null) return 0;
            foreach (var m in party.Characters)
            {
                if (m.Inventory.HasItemAmount(itemid, 1))
                {
                    return 1;
                }
            }
            return 0;
        }
        #endregion
    }
}