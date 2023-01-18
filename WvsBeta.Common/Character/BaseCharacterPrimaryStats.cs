using System;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Objects.BuffStats;
using WvsBeta.Common.Objects.Stats;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Character
{
    public class BaseCharacterPrimaryStats
    {
        public byte Level { get => CharacterStat.Level; }
        public BonusSet EquipBonuses = new BonusSet();
        public BonusSet BuffBonuses = new BonusSet();
        public BuffStat BuffWeaponAttack { get; } = new BuffStat(BuffValueTypes.WeaponAttack);
        public BuffStat BuffWeaponDefense { get; } = new BuffStat(BuffValueTypes.WeaponDefense);
        public BuffStat BuffMagicAttack { get; } = new BuffStat(BuffValueTypes.MagicAttack);
        public BuffStat BuffMagicDefense { get; } = new BuffStat(BuffValueTypes.MagicDefense);
        public BuffStat BuffAccurancy { get; } = new BuffStat(BuffValueTypes.Accurancy);
        public BuffStat BuffAvoidability { get; } = new BuffStat(BuffValueTypes.Avoidability);
        public BuffStat BuffHands { get; } = new BuffStat(BuffValueTypes.Hands);
        public BuffStat BuffSpeed { get; } = new BuffStat(BuffValueTypes.Speed);
        public BuffStat BuffJump { get; } = new BuffStat(BuffValueTypes.Jump);
        public BuffStat BuffMagicGuard { get; } = new BuffStat(BuffValueTypes.MagicGuard);
        public BuffStat BuffDarkSight { get; } = new BuffStat(BuffValueTypes.DarkSight);
        public BuffStat BuffBooster { get; } = new BuffStat(BuffValueTypes.Booster);
        public BuffStat BuffPowerGuard { get; } = new BuffStat(BuffValueTypes.PowerGuard);
        public BuffStat BuffMaxHP { get; } = new BuffStat(BuffValueTypes.MaxHP);
        public BuffStat BuffMaxMP { get; } = new BuffStat(BuffValueTypes.MaxMP);
        public BuffStat BuffInvincible { get; } = new BuffStat(BuffValueTypes.Invincible);
        public BuffStat BuffSoulArrow { get; } = new BuffStat(BuffValueTypes.SoulArrow);
        public BuffStat BuffStun { get; } = new BuffStat(BuffValueTypes.Stun);
        public BuffStat BuffPoison { get; } = new BuffStat(BuffValueTypes.Poison);
        public BuffStat BuffSeal { get; } = new BuffStat(BuffValueTypes.Seal);
        public BuffStat BuffDarkness { get; } = new BuffStat(BuffValueTypes.Darkness);
        public BuffStat_ComboAttack BuffComboAttack { get; } = new BuffStat_ComboAttack(BuffValueTypes.ComboAttack);
        public BuffStat BuffCharges { get; } = new BuffStat(BuffValueTypes.Charges);
        public BuffStat_DragonBlood BuffDragonBlood { get; protected set; }
        public BuffStat BuffHolySymbol { get; } = new BuffStat(BuffValueTypes.HolySymbol);
        public BuffStat BuffMesoUP { get; } = new BuffStat(BuffValueTypes.MesoUP);
        public BuffStat BuffShadowPartner { get; } = new BuffStat(BuffValueTypes.ShadowPartner);
        public BuffStat BuffPickPocketMesoUP { get; } = new BuffStat(BuffValueTypes.PickPocketMesoUP);
        public BuffStat_MesoGuard BuffMesoGuard { get; } = new BuffStat_MesoGuard(BuffValueTypes.MesoGuard);
        public BuffStat BuffThaw { get; } = new BuffStat(BuffValueTypes.Thaw);
        public BuffStat BuffWeakness { get; } = new BuffStat(BuffValueTypes.Weakness);
        public BuffStat BuffCurse { get; } = new BuffStat(BuffValueTypes.Curse);

        public int Craft => CharacterStat.Dex + CharacterStat.Luk + CharacterStat.Int;

        public int TotalStr => CharacterStat.Str + EquipBonuses.Str;
        public int TotalDex => CharacterStat.Dex + EquipBonuses.Dex;
        public int TotalInt => CharacterStat.Int + EquipBonuses.Int;
        public int TotalLuk => CharacterStat.Luk + EquipBonuses.Luk;
        public int TotalMaxHP => CharacterStat.MaxHP + EquipBonuses.MaxHP + BuffBonuses.MaxHP;
        public int TotalMaxMP => CharacterStat.MaxMP + EquipBonuses.MaxMP + BuffBonuses.MaxMP;
        public virtual int EVA { get { throw new NotImplementedException(); } }
        public virtual int ACC { get { throw new NotImplementedException(); } }
        public short MAD => CharacterStat.Int;
        public short MDD => CharacterStat.Int;
        public short TotalMAD => (short)Math.Max(0, Math.Min(MAD + EquipBonuses.MAD + BuffBonuses.MAD, 1999));
        public short TotalMDD => (short)Math.Max(0, Math.Min(MDD + EquipBonuses.MDD + BuffBonuses.MDD, 1999));
        public short TotalPAD => (short)Math.Max(0, Math.Min(EquipBonuses.PAD + BuffBonuses.PAD, 1999));
        public short TotalPDD => (short)Math.Max(0, Math.Min(EquipBonuses.PDD + BuffBonuses.PDD, 1999));

        public short TotalACC => (short)Math.Max(0, Math.Min(ACC + EquipBonuses.ACC + BuffBonuses.ACC, 999));
        public short TotalEVA => (short)Math.Max(0, Math.Min(EVA + EquipBonuses.EVA + BuffBonuses.EVA, 999));
        public short TotalCraft => (short)Math.Max(0, Math.Min(Craft + EquipBonuses.Craft + BuffBonuses.Craft, 999));
        public short TotalJump => (short)Math.Max(100, Math.Min(EquipBonuses.Jump + BuffBonuses.Jump, 123));
        public byte TotalSpeed => (byte)Math.Max(100, Math.Min(EquipBonuses.Speed + BuffBonuses.Speed, 200));

        public GW_CharacterStat CharacterStat { get; private set; }
        public BaseCharacterPrimaryStats(GW_CharacterStat characterStat)
        {
            CharacterStat = characterStat;
        }

        public short GetMaxHP(bool nobonus = false)
        {
            if (!nobonus)
            {
                return (short)((CharacterStat.MaxHP + EquipBonuses.MaxHP + BuffBonuses.MaxHP) > short.MaxValue ? short.MaxValue : (CharacterStat.MaxHP + EquipBonuses.MaxHP + BuffBonuses.MaxHP));
            }
            return CharacterStat.MaxHP;
        }
        public short GetMaxMP(bool nobonus = false)
        {
            if (!nobonus)
            {
                return (short)((CharacterStat.MaxMP + EquipBonuses.MaxMP + BuffBonuses.MaxMP) > short.MaxValue ? short.MaxValue : (CharacterStat.MaxMP + EquipBonuses.MaxMP + BuffBonuses.MaxMP));
            }
            return CharacterStat.MaxMP;
        }
        public virtual void Reset(bool sendPacket) { throw new NotImplementedException(); }
        public virtual void AddEquipStats(sbyte slot, EquipItem equip, bool isLoading)
        {
            throw new NotImplementedException();
        }

        public virtual void CalculateAdditions(bool updateEquips, bool isLoading)
        {
            throw new NotImplementedException();
        }

        public virtual void CheckHPMP()
        {
            throw new NotImplementedException();
        }

        public virtual void CheckBoosters()
        {
            throw new NotImplementedException();
        }

        public virtual short getTotalStr() { return (short)(CharacterStat.Str + EquipBonuses.Str); }
        public virtual short getTotalDex() { return (short)(CharacterStat.Dex + EquipBonuses.Dex); }
        public virtual short getTotalInt() { return (short)(CharacterStat.Int + EquipBonuses.Int); }
        public virtual short getTotalLuk() { return (short)(CharacterStat.Luk + EquipBonuses.Luk); }
        public virtual short getTotalMagicAttack() { return (short)(CharacterStat.Int + EquipBonuses.MAD); }
        public virtual short getTotalMagicDef() { return (short)(CharacterStat.Int + EquipBonuses.MDD); }

        public virtual short GetStrAddition(bool nobonus = false)
        {
            throw new NotImplementedException();
        }
        public virtual short GetDexAddition(bool nobonus = false)
        {
            throw new NotImplementedException();
        }
        public virtual short GetIntAddition(bool nobonus = false)
        {
            throw new NotImplementedException();
        }
        public virtual short GetLukAddition(bool nobonus = false)
        {
            throw new NotImplementedException();
        }

        public virtual void DecodeForCC(Packet packet)
        {
            throw new NotImplementedException();
        }

        public virtual void EncodeForCC(Packet packet)
        {
            throw new NotImplementedException();
        }

        public virtual void CheckExpired(long currentTime)
        {
            throw new NotImplementedException();
        }

        public virtual BuffValueTypes AllActiveBuffs()
        {
            throw new NotImplementedException();
        }

        public virtual BuffValueTypes RemoveByReference(int pBuffValue, bool onlyReturn = false)
        {
            throw new NotImplementedException();
        }

        public virtual void EncodeForLocal(Packet pPacket, BuffValueTypes pSpecificFlag = BuffValueTypes.ALL)
        {
            throw new NotImplementedException();
        }

        public virtual bool HasBuff(int skillOrItemID)
        {
            throw new NotImplementedException();
        }
    }
}
