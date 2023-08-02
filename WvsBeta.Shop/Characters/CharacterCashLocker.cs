using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using WvsBeta.Game;

namespace WvsBeta.Shop
{
    public class CharacterCashLocker : CharacterCashItems
    {
        public ShopCharacter Character { get; private set; }
        
        // No CharacterID as this is the cross-account locker
        public CharacterCashLocker(ShopCharacter chr) : base(chr.UserID, 0)
        {
            Character = chr;
        }

        public void SortItems()
        {
            short slot = 0;
            foreach (var lockerItem in Items)
            {
                var item = GetItemFromCashID(lockerItem.CashId, lockerItem.ItemId);
                item.InventorySlot = slot++;
            }
        }

        public void Save()
        {
            // Don't actually save
            return;
        }
        
        public static Item CreateCashItem(LockerItem li)
        {
            li.CashId = (long)((long)(Rand32.Next()) << 32 | Rand32.Next());
            li.CashId &= 0x00FFFFFFFFFFFFFF; // Get rid of the first byte

            var item = Item.CreateFromItemID(li.ItemId);
            item.Amount = li.Amount;
            item.CashId = li.CashId;
            item.Expiration = li.Expiration;

            if (item is PetItem pi)
            {
                pi.Name = ShopDataProvider.Pets[pi.ItemID].Name;
                pi.Closeness = 0;
                pi.Fullness = 100;
                pi.Level = 1;
                pi.DeadDate = MasterThread.CurrentTime + 90 * TimeExtensions.DayMillis;
            }

            return item;
        }
    }
}
