using System;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using static WvsBeta.Common.Constants;
using static WvsBeta.Common.Objects.AttackData;
using static WvsBeta.Game.AttackPacket;

namespace WvsBeta.Game.Characters
{
    public enum WeaponType : int
    {
        None = 0,
        Sword1H = 30,
        Axe1H = 31,
        BW1H = 32,
        Dagger = 33,
        Wand = 37,
        Staff = 38,
        Sword2H = 40,
        Axe2H = 41,
        BW2H = 42,
        Spear = 43,
        Polearm = 44,
        Bow = 45,
        Crossbow = 46,
        Claw = 47
    }
    public enum AttackAction : int
    {
        Swing_1H_1 = 0x05,
        Swing_1H_2 = 0x06,
        Swing_1H_3 = 0x07,
        Swing_1H_4 = 0x08,

        Swing_2H_1 = 0x09,
        Swing_2H_2 = 0x0A,
        Swing_2H_3 = 0x0B,
        Swing_2H_4 = 0x0C,
        Swing_2H_5 = 0x0D,
        Swing_2H_6 = 0x0E,
        Swing_2H_7 = 0x0F,

        Stab_1 = 0x10,
        Stab_2 = 0x11,
        Stab_3 = 0x12,
        Stab_4 = 0x13,
        Stab_5 = 0x14,
        Stab_6 = 0x15,

        Bullet_1 = 0x16,
        Bullet_2 = 0x17,
        Bullet_3 = 0x18,
        Bullet_4 = 0x19,
        Bullet_5 = 0x1A,
        Bullet_6 = 0x1B,

        Prone = 0x20,
        Heal = 0x28,
        Unk35 = 0x35
    }
    [Flags]
    public enum AttackOption : byte
    {
        Normal = 0,
        SlashBlastFA = 1,
        MortalBlowProp = 4,
        ShadowPartner = 8,
        MortalBlowMelee = 16,
        MortalBlow = MortalBlowProp | MortalBlowMelee
    }
    public static class CalcConstants
    {
        public const int MAX_HITS = 15;
        public const int MAX_TARGETS = 15;
        public static readonly double[] SLASH_BLAST_FA_MODIFIERS = new double[MAX_TARGETS] {
            0.666667,
            0.222222,
            0.074074,
            0.024691,
            0.008229999999999,
            0.002743,
            0.000914,
            0.000305,
            0.000102,
            0.000033,
            0.000011,
            0.000004,
            0.000001,
            0.0,
            0.0
        };
        public static readonly double[] IRON_ARROW_MODIFIERS = new double[MAX_TARGETS]
        {
            1.0,
            0.8,
            0.64,
            0.512,
            0.4096,
            0.32768,
            0.262144,
            0.209715,
            0.167772,
            0.134218,
            0.107374,
            0.085899,
            0.068719,
            0.054976,
            0.04398
        };
    }
    public class Roller
    {
        int rollIndex;
        uint[] rolls;

        public const double STAT_MODIFIER = 0.000000100000010000001;
        public const double PROP_MODIFIER = 0.0000100000010000001;

        public Roller(Rand32 randomizer, int numRolls)
        {
            rolls = new uint[numRolls];
            for (int i = 0; i < numRolls; i++)
            {
                rolls[i] = randomizer.Random();
            }
        }
        public double Roll(double modifier)
        {
            int idx = rollIndex % rolls.Length;
            rollIndex++;
            uint roll = rolls[idx];
            double rollValue = (roll % 10000000) * modifier;
            return rollValue;
        }
    }
    public class CalcTargetDamage
    {
        public double TotalDamage => Hits.Sum(h => h.Damage);
        SkillLevelData skill;
        AttackData data;
        WeaponType weaponType;
        bool isRanged;
        GameCharacter chr;
        AttackOption attackOption;
        public AttackAction AttackAction { get; }
        public CalcHit[] Hits { get; }
        public CalcTargetDamage(GameCharacter chr, AttackData data)
        {
            this.chr = chr;
            this.data = data;
            Hits = new CalcHit[CalcConstants.MAX_HITS];
            isRanged = (AttackTypes)data.AttackType == AttackTypes.Ranged;
            AttackAction = (AttackAction)data.Action;
            int weaponID = chr.Inventory.Equipped[Common.Enums.EquippedType.Normal][EquipSlots.Slots.Weapon]?.ItemID ?? 0;
            weaponType = GetWeaponType(weaponID);
            double weaponMastery = GetWeaponMastery();
            double masteryModifier = weaponMastery * 5.0 + 10 * 0.009000000000000001;
            var critLevel = GetCritSkill(out var critSkill);
            attackOption = (AttackOption)data.Option;

            if (data.SkillID > 0)
            {
                skill = chr.Skills.GetSkillLevelData(data.SkillID);
                if (attackOption == AttackOption.MortalBlow)
                {
                    chr.CalcDamageRandomizer.Random();
                }
            }

            for (int targetIdx = 0; targetIdx < data.Attacks.Count; targetIdx++)
            {
                var info = data.Attacks[targetIdx];
                var mob = chr.Field.GetMob(info.MobMapId);
                if (mob == null) continue;
                Roller roller = new Roller(chr.CalcDamageRandomizer, 7);

                int levelDiff = Math.Max(0, mob.Level - chr.Level);
                double targetAccuracy = chr.PrimaryStats.TotalACC * 100 / (levelDiff * 10.0 + 255.0);

                for (int hitIdx = 0; hitIdx < info.Damages.Count; hitIdx++)
                {
                    Hits[hitIdx] = new CalcHit(this, chr, data, info, mob.Data, roller, critSkill, critLevel, masteryModifier, targetAccuracy, weaponType, skill, isRanged, hitIdx);
                }

                if (ApplyAfterModifiers(targetIdx)) break;
            }
        }

        private bool ApplyAfterModifiers(int targetIdx)
        {
            if (skill == null) return false;
            double modifier;
            bool brk = false;
            if (attackOption == AttackOption.SlashBlastFA)
            {
                modifier = CalcConstants.SLASH_BLAST_FA_MODIFIERS[targetIdx];
            }
            else if (data.SkillID == Hunter.Skills.ArrowBomb)
            {
                if (targetIdx > 0)
                {
                    modifier = skill.Damage * 0.01;
                }
                else
                {
                    modifier = 0.5;
                    if (TotalDamage == 0) brk = true;
                }
            }
            else if (data.SkillID == Crossbowman.Skills.IronArrow)
            {
                modifier = CalcConstants.IRON_ARROW_MODIFIERS[targetIdx];
            }
            else return false;
            foreach (CalcHit hit in Hits)
            {
                hit.Damage *= modifier;
            }
            return brk;
        }
        private static WeaponType GetWeaponType(int weaponID)
        {
            if (weaponID / 1000000 != 1) return WeaponType.None;
            int weaponType = weaponID / 10000 % 100;
            if (weaponType < 30) return WeaponType.None;
            if (weaponType < 33 && (weaponType <= 36 || weaponType > 38 && (weaponType <= 39 || weaponType > 47))) return WeaponType.None;
            return (WeaponType)weaponType;
        }

        private byte GetCritSkill(out SkillLevelData skill)
        {
            skill = null;
            byte level = 0;
            if (!isRanged) return level;

            if (weaponType == WeaponType.Bow || weaponType == WeaponType.Crossbow)
            {
                skill = chr.Skills.GetSkillLevelData(Archer.Skills.CriticalShot, out level);
            }
            else if (weaponType == WeaponType.Claw)
            {
                skill = chr.Skills.GetSkillLevelData(Assassin.Skills.CriticalThrow, out level);
            }
            return level;
        }

        private double GetWeaponMastery()
        {
            double accuracy = 0;
            int skillID = 0;
            switch (weaponType)
            {
                case WeaponType.Bow:
                case WeaponType.Crossbow:
                case WeaponType.Claw:
                    if (!isRanged) return 0;
                    break;
                default:
                    if (isRanged) return 0;
                    break;
            }
            switch (weaponType)
            {
                case WeaponType.Sword1H:
                case WeaponType.Sword2H:
                    if (chr.Job / 10 == 11) // Fighter
                        skillID = Fighter.Skills.SwordMastery;
                    else
                        skillID = Page.Skills.SwordMastery;
                    break;
                case WeaponType.Axe1H:
                case WeaponType.Axe2H:
                    skillID = Fighter.Skills.AxeMastery;
                    break;
                case WeaponType.BW1H:
                case WeaponType.BW2H:
                    skillID = Page.Skills.BwMastery;
                    break;
                case WeaponType.Spear:
                    skillID = Spearman.Skills.SpearMastery;
                    break;
                case WeaponType.Polearm:
                    skillID = Spearman.Skills.PolearmMastery;
                    break;
                case WeaponType.Bow:
                    skillID = Hunter.Skills.BowMastery;
                    break;
                case WeaponType.Crossbow:
                    skillID = Crossbowman.Skills.CrossbowMastery;
                    break;
                case WeaponType.Claw:
                    skillID = Assassin.Skills.ClawMastery;
                    break;
                default: break;
            }
            if (skillID != 0)
            {
                var skill = chr.Skills.GetSkillLevelData(skillID);
                accuracy = skill?.Mastery ?? 0;
            }
            return accuracy;
        }
    }
    public class CalcHit
    {
        public bool IsCrit { get; private set; }
        public bool IsMiss { get; private set; }
        public double Damage { get; set; }
        CalcTargetDamage calcTargetDamage;
        Roller roller;
        AttackData data;
        AttackInfo info;
        SkillLevelData critSkill;
        byte critLevel;
        double masteryModifier;
        double targetAccuracy;
        MobData mob;
        WeaponType weaponType;
        GameCharacter chr;
        SkillLevelData skill;
        bool isRanged;
        int hitIdx;
        readonly int str;
        readonly int dex;
        readonly int intl;
        readonly int luk;
        readonly int watk;
        public CalcHit(
            CalcTargetDamage calcTargetDamage,
            GameCharacter chr,
            AttackData data,
            AttackInfo info,
            MobData mob,
            Roller roller,
            SkillLevelData critSkill,
            byte critLevel,
            double masteryModifier,
            double targetAccuracy,
            WeaponType weaponType,
            SkillLevelData skill,
            bool isRanged,
            int hitIdx
        )
        {
            this.calcTargetDamage = calcTargetDamage;
            this.chr = chr;
            this.data = data;
            this.info = info;
            this.mob = mob;
            this.roller = roller;
            this.critSkill = critSkill;
            this.critLevel = critLevel;
            this.masteryModifier = masteryModifier;
            this.targetAccuracy = targetAccuracy;
            this.weaponType = weaponType;
            this.skill = skill;
            this.isRanged = isRanged;
            this.hitIdx = hitIdx;
            var stats = chr.PrimaryStats;
            str = stats.TotalStr;
            dex = stats.TotalDex;
            intl = stats.TotalInt;
            watk = stats.GetTotalWatk(data.StarID);

            if (GetIsMiss()) return;
            ApplyBaseDamage();
            ApplyMobLevelModifier();
            ApplySpecialElementModifiers();
            ApplyChargeElementModifiers();
            ApplyMobDefReduction();
            double baseDmg = Damage;

            ApplySkillDamage();
            ApplyComboAttack();
            ApplyCrit(baseDmg);
            ApplyShadowPartner();

            Damage = Math.Min(99999, Math.Max(1, Damage));
        }

        private void ApplyBaseDamage()
        {
            if (BowMeleeBaseDmg()
                || ClawMeleeBaseDmg()
                || ProneBaseDmg()
            ) return;
            double statModifier;
            int skillID = data.SkillID;
            var attackAction = calcTargetDamage.AttackAction;
            bool isSwing = attackAction >= AttackAction.Swing_1H_1 && attackAction <= AttackAction.Swing_2H_7;
            switch (weaponType)
            {
                case WeaponType.Bow:
                    {
                        double dex = RollStat(this.dex);
                        statModifier = dex * 3.4 + str;
                    }
                    break;
                case WeaponType.Crossbow:
                    {
                        double dex = RollStat(this.dex);
                        statModifier = dex * 3.6 + str;
                    }
                    break;
                case WeaponType.Axe2H:
                case WeaponType.BW2H:
                    {
                        double str = RollStat(this.str);
                        if (isSwing)
                        {
                            statModifier = str * 4.8 + dex;
                        }
                        else
                        {
                            statModifier = str * 3.4 + dex;
                        }
                    }
                    break;
                case WeaponType.Spear:
                case WeaponType.Polearm:
                    {
                        double str = RollStat(this.str);
                        if (skillID == DragonKnight.Skills.DragonRoar)
                        {
                            statModifier = str * 4.0 + dex;
                        }
                        else if (isSwing != (weaponType == WeaponType.Spear))
                        {
                            statModifier = str * 5.0 + dex;
                        }
                        else
                        {
                            statModifier = str * 3.0 + dex;
                        }
                    }
                    break;
                case WeaponType.Sword2H:
                    {
                        double str = RollStat(this.str);
                        statModifier = str * 4.6 + dex;
                    }
                    break;
                case WeaponType.Axe1H:
                case WeaponType.BW1H:
                case WeaponType.Wand:
                case WeaponType.Staff:
                    {
                        double str = RollStat(this.str);
                        if (isSwing)
                        {
                            statModifier = str * 4.4 + dex;
                        }
                        else
                        {
                            statModifier = str * 3.2 + dex;
                        }
                    }
                    break;
                case WeaponType.Sword1H:
                case WeaponType.Dagger:
                    {
                        if (chr.Job / 100 == 4 && weaponType == WeaponType.Dagger)
                        {
                            double luk = RollStat(this.luk);
                            double primary = luk;
                            double secondary = str + dex;
                            statModifier = CalcStatModifier(primary, secondary);
                        }
                        else
                        {
                            double str = RollStat(this.str);
                            statModifier = str * 4.0 + dex;
                        }
                    }
                    break;
                case WeaponType.Claw:
                    {
                        if (skillID == Rogue.Skills.LuckySeven)
                        {
                            double luk = RollStat(this.luk, 0.5);
                            statModifier = luk * 5.0;
                        }
                        else if (skillID == Hermit.Skills.ShadowMeso)
                        {
                            double moneyCon = skill.MesosUsage * 0.5;
                            double stat = RollStat(moneyCon, 3.0);
                            Damage = 10.0 * stat;

                            var propRoll = roller.Roll(Roller.PROP_MODIFIER);
                            if (skill.Property > propRoll) // Crit
                            {
                                IsCrit = true;
                                var bonusDmg = 100 + skill.XValue;
                                Damage *= bonusDmg * 0.01;
                            }
                            return;
                        }
                        else
                        {
                            double luk = RollStat(this.luk);
                            double secondary = dex + str;
                            statModifier = CalcStatModifier(luk, secondary);
                        }
                    }
                    break;
                default: return;
            }
            Damage = statModifier * watk * 0.01;
        }

        private double CalcStatModifier(double primary, double secondary)
        {
            return secondary + primary * 3.6;
        }

        private bool GetIsMiss()
        {
            double roll = roller.Roll(Roller.STAT_MODIFIER);
            var v31 = 0;
            var v32 = 0;

            var minTACC = targetAccuracy * 0.7;
            var randTACC = targetAccuracy * 1.3;
            var maxTACC = targetAccuracy * 1.3;
            if (v31 == 0 && v32 == 0)
            {
                randTACC = minTACC;
            }
            randTACC += (minTACC - randTACC) * roll;
            var mobAvoid = Math.Min(999, mob.Eva);

            IsMiss = randTACC < mobAvoid;
            return IsMiss;
        }

        private bool BowMeleeBaseDmg()
        {
            if (weaponType != WeaponType.Bow && weaponType != WeaponType.Crossbow) return false;
            var action = calcTargetDamage.AttackAction;
            if (AttackAction.Bullet_1 <= action && action <= AttackAction.Bullet_6 || action == AttackAction.Unk35) return false;

            int skillID = data.SkillID;
            double dex = RollStat(this.dex);
            if (skillID != Crossbowman.Skills.PowerKnockback && skillID != Hunter.Skills.PowerKnockback)
            {
                Damage = (str + dex) * watk * 0.05;
            }
            else
            {
                Damage = (dex * 3.4 + str) * watk * 0.005;
            }
            return true;
        }

        private bool ClawMeleeBaseDmg()
        {
            var action = calcTargetDamage.AttackAction;
            if (weaponType != WeaponType.Claw || (AttackAction.Bullet_1 <= action && action <= AttackAction.Bullet_6) || action == AttackAction.Unk35) return false;

            double luk = RollStat(this.luk);

            Damage = (str + dex + luk) * watk * 0.006666666666666667;
            return true;
        }

        private bool ProneBaseDmg()
        {
            if (calcTargetDamage.AttackAction != AttackAction.Prone) return false;
            double str = RollStat(this.str);
            Damage = (dex + str) * watk * 0.005;
            return true;
        }

        private void ApplyMobLevelModifier()
        {
            if (chr.Level < mob.Level)
            {
                Damage = (100.0 - (mob.Level - chr.Level)) * Damage * 0.01;
            }
        }

        void ApplySpecialElementModifiers()
        {
            var skillID = data.SkillID;
            if (skillID == 0) return;
            if (skillID == FPMage.Skills.ElementComposition)
            {
                var dmg = Damage * 0.5;
                var total = ApplyMobElemModifier(dmg, (ElementModifier)mob.elemModifiers[SkillElement.Fire]);
                total += ApplyMobElemModifier(dmg, (ElementModifier)mob.elemModifiers[SkillElement.Poison]);
                Damage = total;
            }
            else if (skillID == ILMage.Skills.ElementComposition)
            {
                var dmg = Damage * 0.5;
                var total = ApplyMobElemModifier(dmg, (ElementModifier)mob.elemModifiers[SkillElement.Ice]);
                total += ApplyMobElemModifier(dmg, (ElementModifier)mob.elemModifiers[SkillElement.Lightning]);
                Damage = total;
            }
            else
            {
                var modifier = 1.0;
                if (skillID == Ranger.Skills.Inferno || skillID == Sniper.Skills.Blizzard)
                {
                    modifier = (20 + skill.Level) * 0.0099999998;
                }
                var elemModifier = (ElementModifier)mob.elemModifiers.GetValue(skill.ElementFlags);
                Damage = ApplyMobElemModifier(Damage, elemModifier, modifier);
            }
        }
        void ApplyChargeElementModifiers()
        {
            if (chr.Job != WhiteKnight.ID) return;
            var buff = chr.PrimaryStats.BuffCharges;
            if (!buff.IsSet()) return;

            int skillID = buff.R;
            if (skillID < WhiteKnight.Skills.SwordFireCharge || skillID > WhiteKnight.Skills.BwLitCharge) return;

            var element = skill.ElementFlags;
            double specialModifier = (skill.Level * 3 + 10) * 0.01;
            double damageModifier = skill.Damage * 0.01;
            var elemModifier = (ElementModifier)mob.elemModifiers.GetValue(element);
            double dmg = damageModifier * Damage;
            Damage = ApplyMobElemModifier(dmg, elemModifier, specialModifier);
        }

        enum ElementModifier : int
        {
            Normal = 0,
            Nullify = 1,
            Half = 2,
            OneAndHalf = 3,
        }

        static double ApplyMobElemModifier(double dmg, ElementModifier modifier, double extraModifier = 1.0)
        {
            double newDmg = dmg;

            if (modifier == ElementModifier.Nullify)
            {
                return 0.0;
            }
            else if (modifier == ElementModifier.Half)
            {
                return (1.0 - extraModifier * 0.5) * dmg;
            }
            else if (modifier == ElementModifier.OneAndHalf)
            {
                newDmg = (extraModifier * 0.5 + 1.0) * dmg;
                if (dmg >= newDmg) newDmg = dmg;
                newDmg = Math.Min(99999, newDmg);
            }
            return newDmg;
        }

        private void ApplyMobDefReduction()
        {
            int skillID = data.SkillID;
            if (skillID == DragonKnight.Skills.Sacrifice
                || skillID == Hermit.Skills.ShadowMeso
                || skillID == ChiefBandit.Skills.Assaulter
            ) return;

            double mobPDD = Math.Min(999, mob.PDD);
            double redMin = mobPDD * 0.5;
            double redMax = mobPDD * 0.6;
            double reduction = redMin;
            double statRoll = roller.Roll(Roller.STAT_MODIFIER);
            reduction += (redMax - reduction) * statRoll;
            Damage -= reduction;
        }

        private void ApplySkillDamage()
        {
            if (skill == null || skill.Damage <= 0) return;
            Damage *= skill.Damage * 0.01;
        }

        private void ApplyComboAttack()
        {
            var comboSkill = chr.Skills.GetSkillLevelData(Crusader.Skills.ComboAttack);
            var orbs = chr.Buffs.mComboCount;
            if (comboSkill == null || orbs <= 0) return;
            var comboDmg = comboSkill.Damage;
            int skillID = data.SkillID;
            double modifier;

            if (orbs == 1) modifier = comboDmg;
            else if (skillID >= Crusader.Skills.SwordPanic && skillID <= Crusader.Skills.AxeComa)
            {
                switch (orbs)
                {
                    case 2:
                        modifier = (24 * comboSkill.Level - 24) / 29 + comboDmg + 6;
                        break;
                    case 3:
                        modifier = ((comboSkill.Level << 6) - 64) / 29 + comboDmg + 16;
                        break;
                    case 4:
                        modifier = (120 * comboSkill.Level - 120) / 29 + comboDmg + 30;
                        break;
                    case 5:
                        modifier = (184 * comboSkill.Level - 184) / 29 + comboDmg + 46;
                        break;
                    default: return;
                }
            }
            else
            {
                double dmgBonus;
                switch (orbs)
                {
                    case 2:
                        dmgBonus = 5 * comboSkill.Level - 5;
                        break;
                    case 3:
                        dmgBonus = 10 * comboSkill.Level - 10;
                        break;
                    case 4:
                        dmgBonus = 15 * comboSkill.Level - 15;
                        break;
                    case 5:
                        dmgBonus = 20 * comboSkill.Level - 20;
                        break;
                    default: return;
                }
                modifier = comboDmg + dmgBonus / 29;
            }
            Damage *= modifier * 0.01;
        }

        private void ApplyCrit(double baseDmg)
        {
            int skillID = data.SkillID;
            if (skillID == Sniper.Skills.Blizzard
                || skillID == Hermit.Skills.ShadowMeso
                || critSkill == null
                || critSkill.Property <= 0
                || critSkill.Damage <= 0
                || critLevel <= 0
            ) return;
            var roll = roller.Roll(Roller.PROP_MODIFIER);

            if (roll < critSkill.Property)
            {
                IsCrit = true;
                var critBonus = critSkill.Damage - 100;
                Damage += critBonus * 0.01 * baseDmg;
            }
        }

        private void ApplyShadowPartner()
        {
            if (!isRanged) return;
            var spBuff = chr.PrimaryStats.BuffShadowPartner;
            if (!spBuff.IsSet()) return;
            var spSkill = chr.Skills.GetSkillLevelData(Hermit.Skills.ShadowPartner);
            if (spSkill == null) return;
            var spHits = info.Damages.Count / 2;
            if (hitIdx < spHits) return;

            short dmgModifier;
            if (skill != null)
            {
                dmgModifier = spSkill.YValue;
            }
            else
            {
                dmgModifier = spSkill.XValue;
            }

            var clonedDmg = calcTargetDamage.Hits[hitIdx - spHits];
            Damage = (dmgModifier * clonedDmg.Damage) / 100;
            IsCrit = clonedDmg.IsCrit;
        }

        private double RollStat(double stat, double? modifier = null)
        {
            if (!modifier.HasValue) modifier = masteryModifier;
            var statRoll = roller.Roll(Roller.STAT_MODIFIER);
            double modifiedStat = stat * modifier.Value;
            if (stat > modifiedStat)
            {
                stat = modifiedStat;
            }
            if (stat != modifiedStat)
            {
                stat += (modifiedStat - stat) * statRoll;
            }
            return stat;
        }
    }
}
