using System;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Objects.BuffStats;
using WvsBeta.Common.Objects.Stats;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Characters
{
    public class BaseCharacterPrimaryStats
    {
        public byte Level { get => CharacterStat.Level; }
        public BonusSet EquipBonuses = new BonusSet();
        public BonusSet BuffBonuses = new BonusSet();
        public Objects.Stats.BuffStat BuffWeaponAttack { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.WeaponAttack);
        public Objects.Stats.BuffStat BuffWeaponDefense { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.WeaponDefense);
        public Objects.Stats.BuffStat BuffMagicAttack { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.MagicAttack);
        public Objects.Stats.BuffStat BuffMagicDefense { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.MagicDefense);
        public Objects.Stats.BuffStat BuffAccurancy { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Accurancy);
        public Objects.Stats.BuffStat BuffAvoidability { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Avoidability);
        public Objects.Stats.BuffStat BuffHands { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Hands);
        public Objects.Stats.BuffStat BuffSpeed { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Speed);
        public Objects.Stats.BuffStat BuffJump { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Jump);
        public Objects.Stats.BuffStat BuffMagicGuard { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.MagicGuard);
        public Objects.Stats.BuffStat BuffDarkSight { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.DarkSight);
        public Objects.Stats.BuffStat BuffBooster { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Booster);
        public Objects.Stats.BuffStat BuffPowerGuard { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.PowerGuard);
        public Objects.Stats.BuffStat BuffMaxHP { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.MaxHP);
        public Objects.Stats.BuffStat BuffMaxMP { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.MaxMP);
        public Objects.Stats.BuffStat BuffInvincible { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Invincible);
        public Objects.Stats.BuffStat BuffSoulArrow { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.SoulArrow);
        public Objects.Stats.BuffStat BuffStun { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Stun);
        public Objects.Stats.BuffStat BuffPoison { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Poison);
        public Objects.Stats.BuffStat BuffSeal { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Seal);
        public Objects.Stats.BuffStat BuffDarkness { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Darkness);
        public BuffStat_ComboAttack BuffComboAttack { get; } = new BuffStat_ComboAttack(Enums.BuffValueTypes.ComboAttack);
        public Objects.Stats.BuffStat BuffCharges { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Charges);
        public BuffStat_DragonBlood BuffDragonBlood { get; protected set; }
        public Objects.Stats.BuffStat BuffHolySymbol { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.HolySymbol);
        public Objects.Stats.BuffStat BuffMesoUP { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.MesoUP);
        public Objects.Stats.BuffStat BuffShadowPartner { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.ShadowPartner);
        public Objects.Stats.BuffStat BuffPickPocketMesoUP { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.PickPocketMesoUP);
        public BuffStat_MesoGuard BuffMesoGuard { get; } = new BuffStat_MesoGuard(Enums.BuffValueTypes.MesoGuard);
        public Objects.Stats.BuffStat BuffThaw { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Thaw);
        public Objects.Stats.BuffStat BuffWeakness { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Weakness);
        public Objects.Stats.BuffStat BuffCurse { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Curse);
        public Objects.Stats.BuffStat BuffSlow { get; } = new Objects.Stats.BuffStat(Enums.BuffValueTypes.Slow);

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

        public virtual Enums.BuffValueTypes AllActiveBuffs()
        {
            throw new NotImplementedException();
        }

        public virtual Enums.BuffValueTypes RemoveByReference(int pBuffValue, bool onlyReturn = false)
        {
            throw new NotImplementedException();
        }

        public virtual void EncodeForLocal(Packet pPacket, Enums.BuffValueTypes pSpecificFlag = Enums.BuffValueTypes.ALL)
        {
            throw new NotImplementedException();
        }

        public virtual bool HasBuff(int skillOrItemID)
        {
            throw new NotImplementedException();
        }
    }
}
