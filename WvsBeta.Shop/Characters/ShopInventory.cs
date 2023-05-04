using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Sessions;
using WvsBeta.Game;

namespace WvsBeta.Shop
{
    public class ShopInventory : Inventory
    {
        private ShopCharacter Character { get; set; }

        public ShopInventory(ShopCharacter character) : base(character.UserID, character.ID)
        {
            Character = character;
        }

        public void SaveInventory()
        {
            base.SaveInventory(null);
        }

        public new void LoadInventory()
        {
            base.LoadInventory();
        }
    }
}
