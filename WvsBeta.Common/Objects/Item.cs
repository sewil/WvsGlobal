using System;
using MySql.Data.MySqlClient;
using WvsBeta.Common.DataProviders;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Objects
{
    public abstract class Item
    {
        public readonly int ItemID;
        public int CharacterID { get; set; }
        public short Amount { get; set; }
        public short InventorySlot { get; set; } = 0;
        public long CashId { get; set; }
        /// <summary>
        /// FileTime
        /// </summary>
        public long Expiration { get; set; } = NoItemExpiration;
        public bool AlreadyInDatabase { get; set; } = false;

        public const long NoItemExpiration = 150842304000000000L;
        public bool IsOnly { get; set; }
        public bool IsQuest { get; set; }
        public bool IsTradeBlock { get; set; }
        public bool BigSize { get; set; }
        public InventoryType Inventory { get; }

        protected Item(int itemId)
        {
            ItemID = itemId;
            Inventory = Constants.getInventory(itemId);
        }

        protected Item(Item itemBase) : this(itemBase.ItemID)
        {
            Amount = itemBase.Amount;
            CashId = itemBase.CashId;
            Expiration = itemBase.Expiration;
            IsOnly = itemBase.IsOnly;
            IsQuest = itemBase.IsQuest;
            IsTradeBlock = itemBase.IsTradeBlock;
            BigSize = itemBase.BigSize;
            CharacterID = itemBase.CharacterID;
        }

        public Item Duplicate()
        {
            if (this is EquipItem ei) return new EquipItem(ei);
            if (this is PetItem pi) return new PetItem(pi);
            if (this is BundleItem bi) return new BundleItem(bi);
            return null;
        }

        public Item SplitInTwo(short secondPairAmount)
        {
            if (this.Amount < secondPairAmount) return null;

            var dupe = Duplicate();
            this.Amount -= secondPairAmount;

            dupe.Amount = secondPairAmount;
            return dupe;
        }
        public static Item CreateFromItemID(int itemId, short amount = 1, int periodMinutes = 0, ItemVariation variation = ItemVariation.None)
        {
            if (itemId == 0) throw new Exception("Invalid ItemID in CreateFromItemID");

            var slotType = Constants.getItemSlotType(itemId);

            Item ret;
            if (slotType == ItemSlotType.Equip)
            {
                ret = new EquipItem(itemId);
                if (variation != ItemVariation.None)
                {
                    (ret as EquipItem).GiveStats(variation);
                }
            }
            else if (slotType == ItemSlotType.Pet) ret = new PetItem(itemId); // TODO: Non-pet cash items, effects etc
            else ret = new BundleItem(itemId);

            ret.Amount = amount;
            if (periodMinutes > 0)
            {
                ret.Expiration = new TimeSpan(0, 0, periodMinutes, 0).GetFileTimeWithAddition();
            }
            return ret;
        }

        public virtual void Load(MySqlDataReader data)
        {
            // Load ItemID manually

            if (ItemID == 0) throw new Exception("Tried to Load() an item while CreateFromItemID was not used.");

            AlreadyInDatabase = true;
            CashId = data.GetInt64("cashid");
            Expiration = data.GetInt64("expiration");
            CharacterID = data.GetInt32("charid");
        }

        public void EncodeForMigration(Packet pw)
        {
            pw.WriteInt(ItemID);

            pw.WriteShort(Amount);

            if (this is EquipItem equipItem)
            {
                pw.WriteByte(equipItem.Slots);
                pw.WriteByte(equipItem.Scrolls);
                pw.WriteShort(equipItem.Str);
                pw.WriteShort(equipItem.Dex);
                pw.WriteShort(equipItem.Int);
                pw.WriteShort(equipItem.Luk);
                pw.WriteShort(equipItem.HP);
                pw.WriteShort(equipItem.MP);
                pw.WriteShort(equipItem.Watk);
                pw.WriteShort(equipItem.Matk);
                pw.WriteShort(equipItem.Wdef);
                pw.WriteShort(equipItem.Mdef);
                pw.WriteShort(equipItem.Acc);
                pw.WriteShort(equipItem.Avo);
                pw.WriteShort(equipItem.Hands);
                pw.WriteShort(equipItem.Jump);
                pw.WriteShort(equipItem.Speed);
                pw.WriteLong(equipItem.CoupleCashId);
            }
            else
            {
                pw.WriteByte(0);
                pw.WriteByte(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
                pw.WriteShort(0);
            }

            pw.WriteInt(CharacterID);
            pw.WriteLong(CashId);
            pw.WriteLong(Expiration);
            pw.WriteBool(IsOnly);
            pw.WriteBool(IsQuest);
            pw.WriteBool(IsTradeBlock);
            pw.WriteBool(BigSize);

            pw.WriteString("");
        }

        public static Item DecodeForMigration(Packet pr)
        {
            var itemId = pr.ReadInt();

            var item = CreateFromItemID(itemId, pr.ReadShort());

            if (item is EquipItem equipItem)
            {
                equipItem.Slots = pr.ReadByte();
                equipItem.Scrolls = pr.ReadByte();
                equipItem.Str = pr.ReadShort();
                equipItem.Dex = pr.ReadShort();
                equipItem.Int = pr.ReadShort();
                equipItem.Luk = pr.ReadShort();
                equipItem.HP = pr.ReadShort();
                equipItem.MP = pr.ReadShort();
                equipItem.Watk = pr.ReadShort();
                equipItem.Matk = pr.ReadShort();
                equipItem.Wdef = pr.ReadShort();
                equipItem.Mdef = pr.ReadShort();
                equipItem.Acc = pr.ReadShort();
                equipItem.Avo = pr.ReadShort();
                equipItem.Hands = pr.ReadShort();
                equipItem.Jump = pr.ReadShort();
                equipItem.Speed = pr.ReadShort();
                equipItem.CoupleCashId = pr.ReadInt();
            }
            else
            {
                pr.ReadByte();
                pr.ReadByte();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
                pr.ReadShort();
            }

            item.CharacterID = pr.ReadInt();
            item.CashId = pr.ReadLong();
            item.Expiration = pr.ReadLong();
            item.IsOnly = pr.ReadBool();
            item.IsQuest = pr.ReadBool();
            item.IsTradeBlock = pr.ReadBool();
            item.BigSize = pr.ReadBool();

            pr.ReadString();

            return item;
        }
        public virtual void Encode(Packet packet)
        {
            packet.WriteInt(ItemID);
            packet.WriteBool(CashId != 0);
            if (CashId != 0)
                packet.WriteLong(CashId);

            packet.WriteLong(Expiration);
        }

        /// <summary>
        /// Build a full insert statement that is not optimized.
        /// </summary>
        /// <returns>A comma delimited set of fields</returns>
        public abstract string GetFullSaveColumns();

        public abstract string GetFullUpdateColumns();
    }

    public class BundleItem : Item
    {
        public BundleItem(int itemId) : base(itemId)
        {
            if (!DataProvider.Items.TryGetValue(base.ItemID, out ItemData itemData)) return;
            IsQuest = itemData.IsQuest;
            IsOnly = itemData.IsOnly;
            IsTradeBlock = itemData.IsTradeBlock;
            BigSize = itemData.BigSize;
        }

        public BundleItem(BundleItem itemBase) : base(itemBase) { }

        public override void Load(MySqlDataReader data)
        {
            base.Load(data);
            Amount = data.GetInt16("amount");
        }

        public override void Encode(Packet packet)
        {
            base.Encode(packet);
            packet.WriteShort(Amount);
            packet.WriteString(""); // Creator name?
        }

        public override string GetFullSaveColumns()
        {
            return
                ItemID + ", " +
                Amount + ", " +
                CashId + ", " +
                Expiration;
        }

        public override string GetFullUpdateColumns()
        {
            return
                "itemid = " + ItemID + ", " +
                "amount = " + Amount + ", " +
                "cashid = " + CashId + ", " +
                "expiration = " + Expiration;
        }
    }

    public enum ItemVariation
    {
        None = 0,
        Better = 1,
        Normal = 2,
        Great = 3,
        Gachapon = 4,
    }

    public class EquipItem : Item
    {
        public byte Slots { get; set; } = 7;
        public byte Scrolls { get; set; } = 0;
        public short Str { get; set; } = 0;
        public short Dex { get; set; } = 0;
        public short Int { get; set; } = 0;
        public short Luk { get; set; } = 0;
        public short HP { get; set; } = 0;
        public short MP { get; set; } = 0;
        public short Watk { get; set; } = 0;
        public short Matk { get; set; } = 0;
        public short Wdef { get; set; } = 0;
        public short Mdef { get; set; } = 0;
        public short Acc { get; set; } = 0;
        public short Avo { get; set; } = 0;
        public short Hands { get; set; } = 0;
        public short Jump { get; set; } = 0;
        public short Speed { get; set; } = 0;
        public long CoupleCashId { get; set; } = 0;

        public static EquipItem DummyEquipItem { get; } = new EquipItem(0);

        public EquipItem(int itemId) : base(itemId) 
        {
            if (DataProvider.Equips == null || !DataProvider.Equips.TryGetValue(base.ItemID, out EquipData data)) return;
            IsQuest = data.IsQuest;
            IsOnly = data.IsOnly;
            IsTradeBlock = data.IsTradeBlock;
            Slots = data.Slots;
        }

        public EquipItem(EquipItem itemBase) : base(itemBase)
        {
            Amount = 1;
            Slots = itemBase.Slots;
            Scrolls = itemBase.Scrolls;
            Str = itemBase.Str;
            Dex = itemBase.Dex;
            Int = itemBase.Int;
            Luk = itemBase.Luk;
            HP = itemBase.HP;
            MP = itemBase.MP;
            Watk = itemBase.Watk;
            Matk = itemBase.Matk;
            Wdef = itemBase.Wdef;
            Mdef = itemBase.Mdef;
            Acc = itemBase.Acc;
            Avo = itemBase.Avo;
            Hands = itemBase.Hands;
            Jump = itemBase.Jump;
            Speed = itemBase.Speed;
            CoupleCashId = itemBase.CoupleCashId;
        }

        public void GiveStats(ItemVariation enOption)
        {
            if (!DataProvider.Equips.TryGetValue(ItemID, out EquipData data))
            {
                return;
            }

            Amount = 1; // Force it to be 1.

            if (enOption != ItemVariation.None)
            {
                Str = GetVariation(data.Strength, enOption);
                Dex = GetVariation(data.Dexterity, enOption);
                Int = GetVariation(data.Intellect, enOption);
                Luk = GetVariation(data.Luck, enOption);
                HP = GetVariation(data.HP, enOption);
                MP = GetVariation(data.MP, enOption);
                Watk = GetVariation(data.WeaponAttack, enOption);
                Wdef = GetVariation(data.WeaponDefense, enOption);
                Matk = GetVariation(data.MagicAttack, enOption);
                Mdef = GetVariation(data.MagicDefense, enOption);
                Acc = GetVariation(data.Accuracy, enOption);
                Avo = GetVariation(data.Avoidance, enOption);
                Hands = GetVariation(data.Hands, enOption);
                Speed = GetVariation(data.Speed, enOption);
                Jump = GetVariation(data.Jump, enOption);
            }
            else
            {
                Str = data.Strength;
                Dex = data.Dexterity;
                Int = data.Intellect;
                Luk = data.Luck;
                HP = data.HP;
                MP = data.MP;
                Watk = data.WeaponAttack;
                Wdef = data.WeaponDefense;
                Matk = data.MagicAttack;
                Mdef = data.MagicDefense;
                Acc = data.Accuracy;
                Avo = data.Avoidance;
                Hands = data.Hands;
                Speed = data.Speed;
                Jump = data.Jump;
            }
        }


        public override void Load(MySqlDataReader data)
        {
            base.Load(data);

            Slots = (byte)data.GetInt16("slots");
            Scrolls = (byte)data.GetInt16("scrolls");
            Str = data.GetInt16("istr");
            Dex = data.GetInt16("idex");
            Int = data.GetInt16("iint");
            Luk = data.GetInt16("iluk");
            HP = data.GetInt16("ihp");
            MP = data.GetInt16("imp");
            Watk = data.GetInt16("iwatk");
            Matk = data.GetInt16("imatk");
            Wdef = data.GetInt16("iwdef");
            Mdef = data.GetInt16("imdef");
            Acc = data.GetInt16("iacc");
            Avo = data.GetInt16("iavo");
            Hands = data.GetInt16("ihand");
            Speed = data.GetInt16("ispeed");
            Jump = data.GetInt16("ijump");
        }

        public override void Encode(Packet packet)
        {
            base.Encode(packet);

            packet.WriteByte(Slots);
            packet.WriteByte(Scrolls);
            packet.WriteShort(Str);
            packet.WriteShort(Dex);
            packet.WriteShort(Int);
            packet.WriteShort(Luk);
            packet.WriteShort(HP);
            packet.WriteShort(MP);
            packet.WriteShort(Watk);
            packet.WriteShort(Matk);
            packet.WriteShort(Wdef);
            packet.WriteShort(Mdef);
            packet.WriteShort(Acc);
            packet.WriteShort(Avo);
            packet.WriteShort(Hands);
            packet.WriteShort(Speed);
            packet.WriteShort(Jump);
            packet.WriteString(""); // Creator Name
        }

        public void EncodeRing(Packet pw)
        {
            pw.WriteLong(CashId);
            pw.WriteLong(CoupleCashId);
        }

        public override string GetFullSaveColumns()
        { 
            return (
                ItemID + ", " +
                Slots + ", " +
                Scrolls + ", " +
                Str + ", " +
                Dex + ", " +
                Int + ", " +
                Luk + ", " +
                HP + ", " +
                MP + ", " +
                Watk + ", " +
                Matk + ", " +
                Wdef + ", " +
                Mdef + ", " +
                Acc + ", " +
                Avo + ", " +
                Hands + ", " +
                Speed + ", " +
                Jump + ", " +
                CashId + ", " +
                Expiration
            );
        }

        public override string GetFullUpdateColumns()
        {
            return (
                "itemid = " + ItemID + ", " +
                "slots = " + Slots + ", " +
                "scrolls = " + Scrolls + ", " +
                "istr = " + Str + ", " +
                "idex = " + Dex + ", " +
                "iint = " + Int + ", " +
                "iluk = " + Luk + ", " +
                "ihp = " + HP + ", " +
                "imp = " + MP + ", " +
                "iwatk = " + Watk + ", " +
                "imatk = " + Matk + ", " +
                "iwdef = " + Wdef + ", " +
                "imdef = " + Mdef + ", " +
                "iacc = " + Acc + ", " +
                "iavo = " + Avo + ", " +
                "ihand = " + Hands + ", " +
                "ispeed = " + Speed + ", " +
                "ijump = " + Jump + ", " +
                "cashid = " + CashId + ", " +
                "expiration = " + Expiration
            );
        }

        public static short GetVariation(short v, ItemVariation enOption)
        {
            if (v <= 0) return 0;
            if (enOption == ItemVariation.Gachapon)
            {
                // TODO: Gacha
                return v;
            }
            // This logic has 2 bonus bits.

            int maxDiff = Math.Min(v / 10 + 1, 5); // Max stat

            // Maximum amount of bits to set
            // Note:
            // Default: 1 << (1 + 2) == 0x08 (3 bits)
            // Max:     1 << (5 + 2) == 0x80 (7 bits)
            uint maxBits = (uint)(1 << (maxDiff + 2));
            int randBits = (int)(Rand32.Next() % maxBits);

            // Trace.WriteLine($"{(v11 >> 6) & 1} {(v11 >> 5) & 1} | {(v11 >> 4) & 1} {(v11 >> 3) & 1} {(v11 >> 2) & 1} {(v11 >> 1) & 1} {(v11 >> 0) & 1} ");

            // 0 - 3 range
            int calculatedBoost =
                0
                + ((randBits >> 4) & 1)
                + ((randBits >> 3) & 1)
                + ((randBits >> 2) & 1)
                + ((randBits >> 1) & 1)
                + ((randBits >> 0) & 1)
                // Additional bonus
                - 2
                + ((randBits >> 5) & 1)
                + ((randBits >> 6) & 1);

            // Trace.WriteLine($"Boost w/ bonus: {calculatedBoost}");

            // Make sure we don't give negative boost
            calculatedBoost = Math.Max(0, calculatedBoost);

            //Trace.WriteLine($"Actual boost: {calculatedBoost}");


            // Normal is the only one that can go down. The rest goes up
            if (enOption == ItemVariation.Normal)
            {
                if ((Rand32.Next() & 1) == 0)
                    return (short)(v - calculatedBoost);
                else
                    return (short)(v + calculatedBoost);
            }
            else if (enOption == ItemVariation.Better)
            {
                if ((Rand32.Next() % 10) < 3)
                    return v;
                else
                    return (short)(v + calculatedBoost);
            }
            else if (enOption == ItemVariation.Great)
            {
                if ((Rand32.Next() % 10) < 1)
                    return v;
                else
                    return (short)(v + calculatedBoost);
            }
            else
            {
                throw new Exception("Invalid ItemVariation");
            }
        }

    }

    public class PetItem : Item
    {
        public string Name { get; set; }
        public byte Level { get; set; }
        public short Closeness { get; set; }
        public byte Fullness { get; set; }
        public long DeadDate { get; set; }

        public MovableLife MovableLife { get; } = new MovableLife();

        public static PetItem DummyPetItem { get; } = new PetItem(0);

        public PetItem(PetItem itemBase) : base(itemBase)
        {
            Name = itemBase.Name;
            Level = itemBase.Level;
            Closeness = itemBase.Closeness;
            Fullness = itemBase.Fullness;
            DeadDate = itemBase.DeadDate;
        }

        public PetItem(int itemId) : base (itemId) { }


        public override void Load(MySqlDataReader data)
        {
            base.Load(data);

            Name = data.GetString("name");
            Level = data.GetByte("level");
            Closeness = data.GetInt16("closeness");
            Fullness = data.GetByte("fullness");
            DeadDate = data.GetInt64("deaddate");
        }

        public override void Encode(Packet packet)
        {
            base.Encode(packet);

            packet.WriteString(Name, 13);
            packet.WriteByte(Level);
            packet.WriteShort(Closeness);
            packet.WriteByte(Fullness);
            packet.WriteLong(DeadDate);
        }

        public void EncodeForRemote(Packet pw)
        {
            pw.WriteInt(ItemID);
            pw.WriteString(Name);
            pw.WriteLong(CashId);
            pw.WriteShort(MovableLife.Position.X);
            pw.WriteShort(MovableLife.Position.Y);
            pw.WriteByte(MovableLife.Stance);
            pw.WriteShort(MovableLife.Foothold);
        }

        public override string GetFullSaveColumns()
        {
            return
                CashId + "," +
                ItemID + "," +
                "'" + MySqlHelper.EscapeString(Name) + "'," +
                Level + "," +
                Closeness + "," +
                Fullness + "," +
                Expiration + "," +
                DeadDate + "";
        }

        public override string GetFullUpdateColumns()
        {
            return
                "cashid = " + CashId + "," +
                "itemid = " + ItemID + "," +
                "name = '" + MySqlHelper.EscapeString(Name) + "'," +
                "level = " + Level + "," +
                "closeness = " + Closeness + "," +
                "fullness = " + Fullness + "," +
                "expiration = " + Expiration + "," +
                "deaddate = " + DeadDate + "";
        }
    }
}
