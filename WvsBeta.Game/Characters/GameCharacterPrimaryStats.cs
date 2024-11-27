using System;
using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Objects.BuffStats;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Characters;

namespace WvsBeta.Game
{
    public struct PrimaryStatsAddition
    {
        public int ItemID { get; set; }
        public short Slot { get; set; }
        public short Str { get; set; }
        public short Dex { get; set; }
        public short Int { get; set; }
        public short Luk { get; set; }
        public short MaxHP { get; set; }
        public short MaxMP { get; set; }
        public short Speed { get; set; }
    }

    public class EquipBonus : BonusSet
    {
        public int ID { get; set; }
    }

    public class GameCharacterPrimaryStats : CharacterPrimaryStats
    {
        private GameCharacter Char { get; }

        public float speedMod => TotalSpeed + 100.0f;

        public override int EVA
        {
            get
            {
                int eva = CharacterStat.Luk / 2 + CharacterStat.Dex / 4;

                var buff = Char.Skills.GetSkillLevelData(4000000, out byte lvl2);
                if (buff != null)
                {
                    eva += buff.YValue;
                }

                return eva;
            }
        }

        public int ACC
        {
            get
            {
                int acc = 0;

                if (CharacterStat.Job / 100 == 3 || CharacterStat.Job / 100 == 4)
                    acc = (int)((CharacterStat.Luk * 0.3) + (CharacterStat.Dex * 0.6));
                else
                    acc = (int)((CharacterStat.Luk * 0.5) + (CharacterStat.Dex * 0.8));

                var buff = Char.Skills.GetSkillLevelData(Constants.Archer.Skills.BlessingOfAmazon, out byte lvl1);
                if (buff != null)
                {
                    acc += buff.XValue;
                }

                buff = Char.Skills.GetSkillLevelData(Constants.Rogue.Skills.NimbleBody, out byte lvl2);
                if (buff != null)
                {
                    acc += buff.XValue;
                }

                buff = Char.Skills.GetSkillLevelData(Char.Skills.GetMastery(), out byte lvl3);
                if (buff != null)
                {
                    acc += buff.XValue;
                }

                return Math.Max(0, Math.Min(acc, 999));
            }
        }
        public short TotalACC => (short)Math.Max(0, Math.Min(ACC + EquipBonuses.ACC + BuffBonuses.ACC, 999));

        private Dictionary<byte, EquipBonus> EquipStats { get; } = new Dictionary<byte, EquipBonus>();

        public GameCharacterPrimaryStats(GameCharacter chr) : base(chr.CharacterStat)
        {
            Char = chr;
            BuffDragonBlood = new BuffStat_DragonBlood(Common.Enums.BuffValueTypes.DragonBlood, Char);
        }

        public override void UpdateEquipStats(sbyte slot, EquipItem equip, bool isLoading)
        {
            try
            {
                byte realSlot = (byte)Math.Abs(slot);
                if (equip != null)
                {
                    EquipBonus equipBonus;
                    if (!EquipStats.TryGetValue(realSlot, out equipBonus))
                    {
                        equipBonus = new EquipBonus();
                    }

                    equipBonus.ID = equip.ItemID;
                    equipBonus.MaxHP = equip.HP;
                    equipBonus.MaxMP = equip.MP;
                    equipBonus.Str = equip.Str;
                    equipBonus.Int = equip.Int;
                    equipBonus.Dex = equip.Dex;
                    equipBonus.Luk = equip.Luk;
                    equipBonus.Speed = equip.Speed;
                    equipBonus.PAD = equip.Watk;
                    equipBonus.PDD = equip.Wdef;
                    equipBonus.MAD = equip.Matk;
                    equipBonus.MDD = equip.Mdef;
                    equipBonus.EVA = equip.Avo;
                    equipBonus.ACC = equip.Acc;
                    equipBonus.Craft = equip.Hands;
                    equipBonus.Jump = equip.Jump;
                    EquipStats[realSlot] = equipBonus;
                }
                else
                {
                    EquipStats.Remove(realSlot);
                }
                UpdateEquipBonuses(true, isLoading);
            }
            catch (Exception ex)
            {
                Program.MainForm.LogAppend(ex.ToString());
            }
        }

        public override void UpdateEquipBonuses(bool updateEquips, bool isLoading)
        {
            if (updateEquips)
            {
                EquipBonuses = new BonusSet();
                foreach (var data in EquipStats)
                {
                    EquipBonus item = data.Value;
                    if (EquipBonuses.Dex + item.Dex > short.MaxValue) EquipBonuses.Dex = short.MaxValue;
                    else EquipBonuses.Dex += item.Dex;
                    if (EquipBonuses.Int + item.Int > short.MaxValue) EquipBonuses.Int = short.MaxValue;
                    else EquipBonuses.Int += item.Int;
                    if (EquipBonuses.Luk + item.Luk > short.MaxValue) EquipBonuses.Luk = short.MaxValue;
                    else EquipBonuses.Luk += item.Luk;
                    if (EquipBonuses.Str + item.Str > short.MaxValue) EquipBonuses.Str = short.MaxValue;
                    else EquipBonuses.Str += item.Str;
                    if (EquipBonuses.MaxMP + item.MaxMP > short.MaxValue) EquipBonuses.MaxMP = short.MaxValue;
                    else EquipBonuses.MaxMP += item.MaxMP;
                    if (EquipBonuses.MaxHP + item.MaxHP > short.MaxValue) EquipBonuses.MaxHP = short.MaxValue;
                    else EquipBonuses.MaxHP += item.MaxHP;

                    EquipBonuses.PAD += item.PAD;

                    // TODO: Shield mastery buff
                    if (data.Key == (byte)Constants.EquipSlots.Slots.Shield)
                    {

                    }

                    EquipBonuses.PDD += item.PDD;
                    EquipBonuses.MAD += item.MAD;
                    EquipBonuses.MDD += item.MDD;
                    EquipBonuses.ACC += item.ACC;
                    EquipBonuses.EVA += item.EVA;
                    EquipBonuses.Speed += item.Speed;
                    EquipBonuses.Jump += item.Jump;
                    EquipBonuses.Craft += item.Craft;

                    EquipBonuses.PAD = (short)Math.Max(0, Math.Min((int)EquipBonuses.PAD, 1999));
                    EquipBonuses.PDD = (short)Math.Max(0, Math.Min((int)EquipBonuses.PDD, 1999));
                    EquipBonuses.MAD = (short)Math.Max(0, Math.Min((int)EquipBonuses.MAD, 1999));
                    EquipBonuses.MDD = (short)Math.Max(0, Math.Min((int)EquipBonuses.MDD, 1999));
                    EquipBonuses.ACC = (short)Math.Max(0, Math.Min((int)EquipBonuses.ACC, 999));
                    EquipBonuses.EVA = (short)Math.Max(0, Math.Min((int)EquipBonuses.EVA, 999));
                    EquipBonuses.Craft = (short)Math.Max(0, Math.Min((int)EquipBonuses.Craft, 999));
                    EquipBonuses.Speed = (short)Math.Max(100, Math.Min((int)EquipBonuses.Speed, 200));
                    EquipBonuses.Jump = (short)Math.Max(100, Math.Min((int)EquipBonuses.Jump, 123));
                }

            }
            if (!isLoading)
            {
                CheckHPMP();
                Char.FlushDamageLog();
            }
        }

        public override void CheckHPMP()
        {
            short mhp = GetMaxHP(false);
            short mmp = GetMaxMP(false);
            if (Char.HP > mhp)
            {
                Char.ModifyHP(mhp);
            }
            if (CharacterStat.MP > mmp)
            {
                Char.ModifyMP(mmp);
            }
        }
        
        public void CheckBoosters()
        {
            if (Char.Inventory.Equipped[EquippedType.Normal].ContainsKey(Constants.EquipSlots.Slots.Weapon)) return;

            BuffValueTypes removed = 0;
            var currentTime = MasterThread.CurrentTime;
            if (BuffBooster.IsSet(currentTime)) removed |= RemoveByReference(BuffBooster.R, true);
            if (BuffCharges.IsSet(currentTime)) removed |= RemoveByReference(BuffCharges.R, true);
            if (BuffComboAttack.IsSet(currentTime)) removed |= RemoveByReference(BuffComboAttack.R, true);
            if (BuffSoulArrow.IsSet(currentTime)) removed |= RemoveByReference(BuffSoulArrow.R, true);
            
            Char.Buffs.FinalizeDebuff(removed);
        }

        public override short GetStrAddition(bool nobonus = false)
        {
            if (!nobonus)
            {
                return (short)((CharacterStat.Str + EquipBonuses.Str + BuffBonuses.Str) > short.MaxValue ? short.MaxValue : (CharacterStat.Str + EquipBonuses.Str + BuffBonuses.Str));
            }
            return CharacterStat.Str;
        }
        public override short GetDexAddition(bool nobonus = false)
        {
            if (!nobonus)
            {
                return (short)((CharacterStat.Dex + EquipBonuses.Dex + BuffBonuses.Dex) > short.MaxValue ? short.MaxValue : (CharacterStat.Dex + EquipBonuses.Dex + BuffBonuses.Dex));
            }
            return CharacterStat.Dex;
        }
        public override short GetIntAddition(bool nobonus = false)
        {
            if (!nobonus)
            {
                return (short)((CharacterStat.Int + EquipBonuses.Int + BuffBonuses.Int) > short.MaxValue ? short.MaxValue : (CharacterStat.Int + EquipBonuses.Int + BuffBonuses.Int));
            }
            return CharacterStat.Int;
        }
        public override short GetLukAddition(bool nobonus = false)
        {
            if (!nobonus)
            {
                return (short)((CharacterStat.Luk + EquipBonuses.Luk + BuffBonuses.Luk) > short.MaxValue ? short.MaxValue : (CharacterStat.Luk + EquipBonuses.Luk + BuffBonuses.Luk));
            }
            return CharacterStat.Luk;
        }
        
        public int GetTotalWatk(int bulletID)
        {
            return BuffWeaponAttack.N + Char.Inventory.GetTotalWAttackInEquips(bulletID);
        }

        public override void Reset(bool sendPacket)
        {
            Common.Enums.BuffValueTypes flags = 0;
            flags |= BuffWeaponAttack.Reset();
            flags |= BuffWeaponDefense.Reset();
            flags |= BuffMagicAttack.Reset();
            flags |= BuffMagicDefense.Reset();
            flags |= BuffAccurancy.Reset();
            flags |= BuffAvoidability.Reset();
            flags |= BuffHands.Reset();
            flags |= BuffSpeed.Reset();
            flags |= BuffJump.Reset();
            flags |= BuffMagicGuard.Reset();
            flags |= BuffDarkSight.Reset();
            flags |= BuffBooster.Reset();
            flags |= BuffPowerGuard.Reset();
            flags |= BuffMaxHP.Reset();
            flags |= BuffMaxMP.Reset();
            if (flags.HasFlag(Common.Enums.BuffValueTypes.MaxHP))
                Char.Buffs.CancelHyperBody();
            flags |= BuffInvincible.Reset();
            flags |= BuffSoulArrow.Reset();
            flags |= BuffStun.Reset();
            flags |= BuffPoison.Reset();
            flags |= BuffSeal.Reset();
            flags |= BuffDarkness.Reset();
            flags |= BuffComboAttack.Reset();
            flags |= BuffCharges.Reset();
            flags |= BuffDragonBlood.Reset();
            flags |= BuffHolySymbol.Reset();
            flags |= BuffMesoUP.Reset();
            flags |= BuffShadowPartner.Reset();
            flags |= BuffPickPocketMesoUP.Reset();
            flags |= BuffMesoGuard.Reset();
            flags |= BuffThaw.Reset();
            flags |= BuffWeakness.Reset();
            flags |= BuffCurse.Reset();
            flags |= BuffSlow.Reset();

            Char.Buffs.FinalizeDebuff(flags, sendPacket);
        }

        public override void DecodeForCC(Packet packet)
        {
            var flags = (Common.Enums.BuffValueTypes)packet.ReadUInt();

            BuffWeaponAttack.DecodeForCC(packet, flags);
            BuffWeaponDefense.DecodeForCC(packet, flags);
            BuffMagicAttack.DecodeForCC(packet, flags);
            BuffMagicDefense.DecodeForCC(packet, flags);
            BuffAccurancy.DecodeForCC(packet, flags);
            BuffAvoidability.DecodeForCC(packet, flags);
            BuffHands.DecodeForCC(packet, flags);
            BuffSpeed.DecodeForCC(packet, flags);
            BuffJump.DecodeForCC(packet, flags);
            BuffMagicGuard.DecodeForCC(packet, flags);
            BuffDarkSight.DecodeForCC(packet, flags);
            BuffBooster.DecodeForCC(packet, flags);
            BuffPowerGuard.DecodeForCC(packet, flags);
            BuffMaxHP.DecodeForCC(packet, flags);
            BuffMaxMP.DecodeForCC(packet, flags);
            if (BuffMaxHP.IsSet())
            {
                short hpmpBonus = (short)((double)CharacterStat.MaxHP * ((double)BuffMaxHP.N / 100.0d));
                Char.PrimaryStats.BuffBonuses.MaxHP = hpmpBonus;
                hpmpBonus = (short)((double)CharacterStat.MaxMP * ((double)BuffMaxMP.N / 100.0d));
                Char.PrimaryStats.BuffBonuses.MaxMP = hpmpBonus;
            }
            BuffInvincible.DecodeForCC(packet, flags);
            BuffSoulArrow.DecodeForCC(packet, flags);
            BuffStun.DecodeForCC(packet, flags);
            BuffPoison.DecodeForCC(packet, flags);
            BuffSeal.DecodeForCC(packet, flags);
            BuffDarkness.DecodeForCC(packet, flags);
            BuffComboAttack.DecodeForCC(packet, flags);
            BuffCharges.DecodeForCC(packet, flags);
            BuffDragonBlood.DecodeForCC(packet, flags);
            BuffHolySymbol.DecodeForCC(packet, flags);
            BuffMesoUP.DecodeForCC(packet, flags);
            BuffShadowPartner.DecodeForCC(packet, flags);
            BuffPickPocketMesoUP.DecodeForCC(packet, flags);
            BuffMesoGuard.DecodeForCC(packet, flags);
            BuffThaw.DecodeForCC(packet, flags);
            BuffWeakness.DecodeForCC(packet, flags);
            BuffCurse.DecodeForCC(packet, flags);
            BuffSlow.DecodeForCC(packet, flags);

            if (BuffComboAttack.IsSet())
            {
                var sld = Char.Skills.GetSkillLevelData(BuffComboAttack.R);
                if (sld != null)
                {
                    BuffComboAttack.MaxOrbs = sld.XValue;
                }
            }

            if (BuffMesoGuard.IsSet())
            {
                var sld = Char.Skills.GetSkillLevelData(BuffMesoGuard.R);
                if (sld != null)
                {
                    BuffMesoGuard.MesosLeft = sld.MesosUsage;
                }
            }
        }

        public override void EncodeForCC(Packet packet)
        {
            long currentTime = MasterThread.CurrentTime;
            int offset = packet.Position;
            packet.WriteUInt(0);
            Common.Enums.BuffValueTypes flags = 0;

            BuffWeaponAttack.EncodeForCC(packet, ref flags, currentTime);
            BuffWeaponDefense.EncodeForCC(packet, ref flags, currentTime);
            BuffMagicAttack.EncodeForCC(packet, ref flags, currentTime);
            BuffMagicDefense.EncodeForCC(packet, ref flags, currentTime);
            BuffAccurancy.EncodeForCC(packet, ref flags, currentTime);
            BuffAvoidability.EncodeForCC(packet, ref flags, currentTime);
            BuffHands.EncodeForCC(packet, ref flags, currentTime);
            BuffSpeed.EncodeForCC(packet, ref flags, currentTime);
            BuffJump.EncodeForCC(packet, ref flags, currentTime);
            BuffMagicGuard.EncodeForCC(packet, ref flags, currentTime);
            BuffDarkSight.EncodeForCC(packet, ref flags, currentTime);
            BuffBooster.EncodeForCC(packet, ref flags, currentTime);
            BuffPowerGuard.EncodeForCC(packet, ref flags, currentTime);
            BuffMaxHP.EncodeForCC(packet, ref flags, currentTime);
            BuffMaxMP.EncodeForCC(packet, ref flags, currentTime);
            BuffInvincible.EncodeForCC(packet, ref flags, currentTime);
            BuffSoulArrow.EncodeForCC(packet, ref flags, currentTime);
            BuffStun.EncodeForCC(packet, ref flags, currentTime);
            BuffPoison.EncodeForCC(packet, ref flags, currentTime);
            BuffSeal.EncodeForCC(packet, ref flags, currentTime);
            BuffDarkness.EncodeForCC(packet, ref flags, currentTime);
            BuffComboAttack.EncodeForCC(packet, ref flags, currentTime);
            BuffCharges.EncodeForCC(packet, ref flags, currentTime);
            BuffDragonBlood.EncodeForCC(packet, ref flags, currentTime);
            BuffHolySymbol.EncodeForCC(packet, ref flags, currentTime);
            BuffMesoUP.EncodeForCC(packet, ref flags, currentTime);
            BuffShadowPartner.EncodeForCC(packet, ref flags, currentTime);
            BuffPickPocketMesoUP.EncodeForCC(packet, ref flags, currentTime);
            BuffMesoGuard.EncodeForCC(packet, ref flags, currentTime);
            BuffThaw.EncodeForCC(packet, ref flags, currentTime);
            BuffWeakness.EncodeForCC(packet, ref flags, currentTime);
            BuffCurse.EncodeForCC(packet, ref flags, currentTime);
            BuffSlow.EncodeForCC(packet, ref flags, currentTime);

            packet.SetUInt(offset, (uint)flags);
        }

        public override void CheckExpired(long currentTime)
        {
            Common.Enums.BuffValueTypes endFlag = 0;

            BuffWeaponAttack.TryReset(currentTime, ref endFlag);
            BuffWeaponDefense.TryReset(currentTime, ref endFlag);
            BuffMagicAttack.TryReset(currentTime, ref endFlag);
            BuffMagicDefense.TryReset(currentTime, ref endFlag);
            BuffAccurancy.TryReset(currentTime, ref endFlag);
            BuffAvoidability.TryReset(currentTime, ref endFlag);
            BuffHands.TryReset(currentTime, ref endFlag);
            BuffSpeed.TryReset(currentTime, ref endFlag);
            BuffJump.TryReset(currentTime, ref endFlag);
            BuffMagicGuard.TryReset(currentTime, ref endFlag);
            BuffDarkSight.TryReset(currentTime, ref endFlag);
            BuffBooster.TryReset(currentTime, ref endFlag);
            BuffPowerGuard.TryReset(currentTime, ref endFlag);
            if (BuffMaxHP.TryReset(currentTime, ref endFlag) &&
                BuffMaxMP.TryReset(currentTime, ref endFlag))
                Char.Buffs.CancelHyperBody();
            BuffInvincible.TryReset(currentTime, ref endFlag);
            BuffSoulArrow.TryReset(currentTime, ref endFlag);
            BuffStun.TryReset(currentTime, ref endFlag);
            BuffPoison.TryReset(currentTime, ref endFlag);
            BuffSeal.TryReset(currentTime, ref endFlag);
            BuffDarkness.TryReset(currentTime, ref endFlag);
            BuffComboAttack.TryReset(currentTime, ref endFlag);
            BuffCharges.TryReset(currentTime, ref endFlag);
            BuffDragonBlood.TryReset(currentTime, ref endFlag);
            BuffHolySymbol.TryReset(currentTime, ref endFlag);
            BuffMesoUP.TryReset(currentTime, ref endFlag);
            BuffShadowPartner.TryReset(currentTime, ref endFlag);
            BuffPickPocketMesoUP.TryReset(currentTime, ref endFlag);
            BuffMesoGuard.TryReset(currentTime, ref endFlag);
            BuffThaw.TryReset(currentTime, ref endFlag);
            BuffWeakness.TryReset(currentTime, ref endFlag);
            BuffCurse.TryReset(currentTime, ref endFlag);
            BuffSlow.TryReset(currentTime, ref endFlag);

            Char.Buffs.FinalizeDebuff(endFlag);
        }

        public override Common.Enums.BuffValueTypes AllActiveBuffs()
        {
            long currentTime = MasterThread.CurrentTime;
            Common.Enums.BuffValueTypes flags = 0;
            flags |= BuffWeaponAttack.GetState(currentTime);
            flags |= BuffWeaponDefense.GetState(currentTime);
            flags |= BuffMagicAttack.GetState(currentTime);
            flags |= BuffMagicDefense.GetState(currentTime);
            flags |= BuffAccurancy.GetState(currentTime);
            flags |= BuffAvoidability.GetState(currentTime);
            flags |= BuffHands.GetState(currentTime);
            flags |= BuffSpeed.GetState(currentTime);
            flags |= BuffJump.GetState(currentTime);
            flags |= BuffMagicGuard.GetState(currentTime);
            flags |= BuffDarkSight.GetState(currentTime);
            flags |= BuffBooster.GetState(currentTime);
            flags |= BuffPowerGuard.GetState(currentTime);
            flags |= BuffMaxHP.GetState(currentTime);
            flags |= BuffMaxMP.GetState(currentTime);
            flags |= BuffInvincible.GetState(currentTime);
            flags |= BuffSoulArrow.GetState(currentTime);
            flags |= BuffStun.GetState(currentTime);
            flags |= BuffPoison.GetState(currentTime);
            flags |= BuffSeal.GetState(currentTime);
            flags |= BuffDarkness.GetState(currentTime);
            flags |= BuffComboAttack.GetState(currentTime);
            flags |= BuffCharges.GetState(currentTime);
            flags |= BuffDragonBlood.GetState(currentTime);
            flags |= BuffHolySymbol.GetState(currentTime);
            flags |= BuffMesoUP.GetState(currentTime);
            flags |= BuffShadowPartner.GetState(currentTime);
            flags |= BuffPickPocketMesoUP.GetState(currentTime);
            flags |= BuffMesoGuard.GetState(currentTime);
            flags |= BuffThaw.GetState(currentTime);
            flags |= BuffWeakness.GetState(currentTime);
            flags |= BuffCurse.GetState(currentTime);
            flags |= BuffSlow.GetState(currentTime);

            return flags;

        }

        public override Common.Enums.BuffValueTypes RemoveByReference(int pBuffValue, bool onlyReturn = false)
        {
            if (pBuffValue == 0) return 0;

            Common.Enums.BuffValueTypes endFlag = 0;

            BuffWeaponAttack.TryResetByReference(pBuffValue, ref endFlag);
            BuffWeaponDefense.TryResetByReference(pBuffValue, ref endFlag);
            BuffMagicAttack.TryResetByReference(pBuffValue, ref endFlag);
            BuffMagicDefense.TryResetByReference(pBuffValue, ref endFlag);
            BuffAccurancy.TryResetByReference(pBuffValue, ref endFlag);
            BuffAvoidability.TryResetByReference(pBuffValue, ref endFlag);
            BuffHands.TryResetByReference(pBuffValue, ref endFlag);
            BuffSpeed.TryResetByReference(pBuffValue, ref endFlag);
            BuffJump.TryResetByReference(pBuffValue, ref endFlag);
            BuffMagicGuard.TryResetByReference(pBuffValue, ref endFlag);
            BuffDarkSight.TryResetByReference(pBuffValue, ref endFlag);
            BuffBooster.TryResetByReference(pBuffValue, ref endFlag);
            BuffPowerGuard.TryResetByReference(pBuffValue, ref endFlag);
            BuffMaxHP.TryResetByReference(pBuffValue, ref endFlag);
            BuffMaxMP.TryResetByReference(pBuffValue, ref endFlag);
            BuffInvincible.TryResetByReference(pBuffValue, ref endFlag);
            BuffSoulArrow.TryResetByReference(pBuffValue, ref endFlag);
            BuffStun.TryResetByReference(pBuffValue, ref endFlag);
            BuffPoison.TryResetByReference(pBuffValue, ref endFlag);
            BuffSeal.TryResetByReference(pBuffValue, ref endFlag);
            BuffDarkness.TryResetByReference(pBuffValue, ref endFlag);
            BuffComboAttack.TryResetByReference(pBuffValue, ref endFlag);
            BuffCharges.TryResetByReference(pBuffValue, ref endFlag);
            BuffDragonBlood.TryResetByReference(pBuffValue, ref endFlag);
            BuffHolySymbol.TryResetByReference(pBuffValue, ref endFlag);
            BuffMesoUP.TryResetByReference(pBuffValue, ref endFlag);
            BuffShadowPartner.TryResetByReference(pBuffValue, ref endFlag);
            BuffPickPocketMesoUP.TryResetByReference(pBuffValue, ref endFlag);
            BuffMesoGuard.TryResetByReference(pBuffValue, ref endFlag);
            BuffThaw.TryResetByReference(pBuffValue, ref endFlag);
            BuffWeakness.TryResetByReference(pBuffValue, ref endFlag);
            BuffCurse.TryResetByReference(pBuffValue, ref endFlag);
            BuffSlow.TryResetByReference(pBuffValue, ref endFlag);

            if (!onlyReturn)
            {
                Char.Buffs.FinalizeDebuff(endFlag);
            }
            return endFlag;
        }

        public override void EncodeForLocal(Packet pPacket, Common.Enums.BuffValueTypes pSpecificFlag = Common.Enums.BuffValueTypes.ALL)
        {
            long currentTime = MasterThread.CurrentTime;
            int tmpBuffPos = pPacket.Position;
            Common.Enums.BuffValueTypes endFlag = Common.Enums.BuffValueTypes.None;
            pPacket.WriteULong((ulong)endFlag);

            BuffWeaponAttack.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffWeaponDefense.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffMagicAttack.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffMagicDefense.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffAccurancy.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffAvoidability.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffHands.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffSpeed.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffJump.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffMagicGuard.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);

            // Do not activate it in hide
            if (BuffDarkSight.HasReferenceId(Constants.Gm.Skills.Hide) == false)
                BuffDarkSight.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);

            BuffBooster.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffPowerGuard.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffMaxHP.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffMaxMP.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffInvincible.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffSoulArrow.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffStun.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffPoison.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffSeal.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffDarkness.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffComboAttack.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffCharges.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffDragonBlood.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffHolySymbol.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffMesoUP.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffShadowPartner.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffPickPocketMesoUP.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffMesoGuard.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffThaw.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffWeakness.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffCurse.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);
            BuffSlow.EncodeForLocal(pPacket, ref endFlag, currentTime, pSpecificFlag);

            pPacket.SetULong(tmpBuffPos, (ulong)endFlag); // Make sure correct flag is set
        }

        public override bool HasBuff(int referenceId)
        {
            long currentTime = MasterThread.CurrentTime;
            return
                BuffWeaponAttack.HasReferenceId(referenceId, currentTime) ||
                BuffWeaponDefense.HasReferenceId(referenceId, currentTime) ||
                BuffMagicAttack.HasReferenceId(referenceId, currentTime) ||
                BuffMagicDefense.HasReferenceId(referenceId, currentTime) ||
                BuffAccurancy.HasReferenceId(referenceId, currentTime) ||
                BuffAvoidability.HasReferenceId(referenceId, currentTime) ||
                BuffHands.HasReferenceId(referenceId, currentTime) ||
                BuffSpeed.HasReferenceId(referenceId, currentTime) ||
                BuffJump.HasReferenceId(referenceId, currentTime) ||
                BuffMagicGuard.HasReferenceId(referenceId, currentTime) ||
                BuffDarkSight.HasReferenceId(referenceId, currentTime) ||
                BuffBooster.HasReferenceId(referenceId, currentTime) ||
                BuffPowerGuard.HasReferenceId(referenceId, currentTime) ||
                BuffMaxHP.HasReferenceId(referenceId, currentTime) ||
                BuffMaxMP.HasReferenceId(referenceId, currentTime) ||
                BuffInvincible.HasReferenceId(referenceId, currentTime) ||
                BuffSoulArrow.HasReferenceId(referenceId, currentTime) ||
                BuffStun.HasReferenceId(referenceId, currentTime) ||
                BuffPoison.HasReferenceId(referenceId, currentTime) ||
                BuffSeal.HasReferenceId(referenceId, currentTime) ||
                BuffDarkness.HasReferenceId(referenceId, currentTime) ||
                BuffComboAttack.HasReferenceId(referenceId, currentTime) ||
                BuffCharges.HasReferenceId(referenceId, currentTime) ||
                BuffDragonBlood.HasReferenceId(referenceId, currentTime) ||
                BuffHolySymbol.HasReferenceId(referenceId, currentTime) ||
                BuffMesoUP.HasReferenceId(referenceId, currentTime) ||
                BuffShadowPartner.HasReferenceId(referenceId, currentTime) ||
                BuffPickPocketMesoUP.HasReferenceId(referenceId, currentTime) ||
                BuffMesoGuard.HasReferenceId(referenceId, currentTime) ||
                BuffThaw.HasReferenceId(referenceId, currentTime) ||
                BuffWeakness.HasReferenceId(referenceId, currentTime) ||
                BuffCurse.HasReferenceId(referenceId, currentTime) ||
                BuffSlow.HasReferenceId(referenceId, currentTime);
        }
    }
}