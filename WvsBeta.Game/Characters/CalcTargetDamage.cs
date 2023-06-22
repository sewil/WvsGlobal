using System;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using static WvsBeta.Common.Constants;
using static WvsBeta.Game.Characters.AttackData;

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
    public class ElementAmpData
    {
        public int Magic;
        public int Mana;
    }
    public class CalcTargetAttack
    {
        public CalcHit[] Hits { get; }
        public Mob Mob { get; }
        public double TotalDamage => Hits.Sum(h => h.Damage);
        public AttackInfo Info { get; }
        public GameCharacter Character { get; }

        public CalcTargetAttack(
            GameCharacter chr,
            Roller roller,
            AttackInfo info,
            Mob mob,
            SkillLevelData critSkill,
            byte critLevel,
            double masteryModifier,
            WeaponType weaponType,
            AttackAction attackAction,
            SkillLevelData skill,
            int skillID,
            bool isRanged,
            int watk
        )
        {
            Character = chr;
            Mob = mob;
            Info = info;
            Hits = new CalcHit[info.Damages.Count];

            ElementAmpData ampData = GetElementAmplification();

            double targetAccuracy = GetTargetAccuracy();

            for (int hitIdx = 0; hitIdx < Hits.Length; hitIdx++)
            {
                Hits[hitIdx] = new CalcHit(chr, Hits, info, mob, roller, critSkill, critLevel, masteryModifier, targetAccuracy, weaponType, attackAction, skill, skillID, isRanged, hitIdx, watk, ampData);
            }
        }

        public static bool IsJob(short charJobID, short jobID)
        {
            if (jobID % 100 != 0)
                return jobID / 10 == charJobID / 10 && charJobID % 10 >= jobID % 10;
            else
                return jobID / 100 == charJobID / 100;
        }

        public ElementAmpData GetElementAmplification()
        {
            short jobID = Character.Job;
            int ampSkillID = 0;
            int skillID = Info.Data.SkillID;
            if (IsJob(jobID, FPMage.ID))
            {
                ampSkillID = FPMage.Skills.ElementAmplification;
            }
            else if (IsJob(jobID, ILMage.ID))
            {
                ampSkillID = ILMage.Skills.ElementAmplification;
            }
            var ampData = new ElementAmpData { Magic = 100, Mana = 100 };
            if (ampSkillID > 0)
            {
                var ampSkill = Character.Skills.GetSkillLevelData(ampSkillID);
                bool skipMP = false;
                // Basically only draw extra mp if its an attack skill, taken from client
                if (skillID > FPMage.Skills.ElementComposition && (
                    skillID < ILWizard.Skills.ColdBeam || skillID > ILWizard.Skills.ThunderBolt && (
                        skillID <= 2211001 || skillID > ILMage.Skills.Lightening && skillID != ILMage.Skills.ElementComposition
                    )
                )) skipMP = true;
                else if (skillID != FPMage.Skills.ElementComposition
                    && (skillID < Magician.Skills.EnergyBolt
                    || skillID > Magician.Skills.MagicClaw
                    && (skillID <= FPWizard.Skills.Slow || skillID > FPWizard.Skills.PoisonBreath && (skillID <= FPMage.Skills.ElementAmplification || skillID > FPMage.Skills.PoisonMyst))
                )) skipMP = true;
                if (!skipMP)
                {
                    ampData.Mana = ampSkill.XValue;
                }
                ampData.Magic = ampSkill.YValue;
            }
            return ampData;
        }

        public double GetTargetAccuracy()
        {
            int levelDiff = Math.Max(0, Mob.Level - Character.Level);
            var stats = Character.PrimaryStats;
            int accuracy;
            if (Info.Data.AttackTypes == AttackTypes.Magic)
            {
                accuracy = 5 * (stats.TotalInt / 10 + stats.TotalLuk / 10);
            }
            else
            {
                accuracy = stats.TotalACC;
            }
            return accuracy * 100 / (levelDiff * 10.0 + 255.0);
        }
    }
    public class CalcDamage
    {
        SkillLevelData skill;
        public AttackData Data { get; }
        WeaponType weaponType;
        bool isRanged;
        GameCharacter chr;
        AttackOption attackOption;
        public AttackAction AttackAction { get; }
        public CalcTargetAttack[] TargetAttacks { get; }
        public CalcDamage(GameCharacter chr, AttackData data, AttackTypes attackType)
        {
            this.chr = chr;
            Data = data;
            TargetAttacks = new CalcTargetAttack[data.Attacks.Count];
            isRanged = attackType == AttackTypes.Ranged;
            AttackAction = (AttackAction)data.Action;
            int weaponID = chr.Inventory.Equipped[Common.Enums.EquippedType.Normal][EquipSlots.Slots.Weapon]?.ItemID ?? 0;
            weaponType = GetWeaponType(weaponID);
            
            int skillID = data.SkillID;
            if (skillID > 0) skill = chr.Skills.GetSkillLevelData(skillID);

            double masteryModifier = GetMasteryModifier();
            var critLevel = GetCritSkill(out var critSkill);
            attackOption = (AttackOption)data.Option;
            int watk = chr.PrimaryStats.GetTotalWatk(data.StarID);

            if ((attackOption & AttackOption.MortalBlow) != 0)
            {
                chr.CalcDamageRandomizer.Random();
            }

            for (int targetIdx = 0; targetIdx < TargetAttacks.Length; targetIdx++)
            {
                var info = data.Attacks[targetIdx];
                var mob = chr.Field.GetMob(info.MobMapId);
                Roller roller = new Roller(chr.CalcDamageRandomizer, 7);
                if (mob == null) continue;

                var targetDmg = new CalcTargetAttack(chr, roller, info, mob, critSkill, critLevel, masteryModifier, weaponType, AttackAction, skill, skillID, isRanged, watk);
                TargetAttacks[targetIdx] = targetDmg;
                if (ApplyAfterModifiers(targetDmg, targetIdx)) break;
            }
        }

        private double GetMasteryModifier()
        {
            double mastery;
            if (Data.AttackTypes == AttackTypes.Magic)
            {
                mastery = skill.Mastery;
            }
            else
            {
                mastery = GetWeaponMastery();
            }
            return (mastery * 5.0 + 10) * 0.009000000000000001;
        }

        private bool ApplyAfterModifiers(CalcTargetAttack targetDmg, int targetIdx)
        {
            if (skill == null) return false;
            double modifier;
            bool brk = false;
            if (attackOption == AttackOption.SlashBlastFA)
            {
                modifier = CalcConstants.SLASH_BLAST_FA_MODIFIERS[targetIdx];
            }
            else if (Data.SkillID == Hunter.Skills.ArrowBomb)
            {
                if (targetIdx > 0)
                {
                    modifier = skill.XValue * 0.01;
                }
                else
                {
                    modifier = 0.5;
                    if (targetDmg.TotalDamage == 0) brk = true;
                }
            }
            else if (Data.SkillID == Crossbowman.Skills.IronArrow)
            {
                modifier = CalcConstants.IRON_ARROW_MODIFIERS[targetIdx];
            }
            else return false;
            foreach (CalcHit hit in targetDmg.Hits)
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
            if (weaponType > 33 && (weaponType <= 36 || weaponType > 38 && (weaponType <= 39 || weaponType > 47))) return WeaponType.None;
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
            double mastery = 0;
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
                case WeaponType.Dagger:
                    skillID = Bandit.Skills.DaggerMastery;
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
                mastery = skill?.Mastery ?? 0;
            }
            return mastery;
        }
    }
    public class CalcHit
    {
        public bool IsCrit { get; private set; }
        public bool IsMiss { get; private set; }
        public double Damage { get; set; }
        Roller roller;
        AttackInfo info;
        SkillLevelData critSkill;
        byte critLevel;
        double masteryModifier;
        double targetAccuracy;
        Mob mob;
        WeaponType weaponType;
        AttackAction attackAction;
        GameCharacter chr;
        SkillLevelData skill;
        bool isRanged;
        int hitIdx;
        int skillID;
        CalcHit[] hits;
        AttackTypes attackType;
        readonly int str;
        readonly int dex;
        readonly int intl;
        readonly int luk;
        readonly int watk;
        public CalcHit(
            GameCharacter chr,
            CalcHit[] hits,
            AttackInfo info,
            Mob mob,
            Roller roller,
            SkillLevelData critSkill,
            byte critLevel,
            double masteryModifier,
            double targetAccuracy,
            WeaponType weaponType,
            AttackAction attackAction,
            SkillLevelData skill,
            int skillID,
            bool isRanged,
            int hitIdx,
            int watk,
            ElementAmpData ampData
        )
        {
            this.attackAction = attackAction;
            this.chr = chr;
            this.hits = hits;
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
            this.skillID = skillID;
            var stats = chr.PrimaryStats;
            str = stats.TotalStr;
            dex = stats.TotalDex;
            intl = stats.TotalInt;
            luk = stats.TotalLuk;
            this.watk = watk;
            attackType = info.Data.AttackTypes;

            bool isDragon = info.Data.SummonID == Priest.Skills.SummonDragon;

            if (
                ((attackType == AttackTypes.Magic || isDragon) && mob.Status.BuffMagicImmune.IsSet()) ||
                (attackType != AttackTypes.Magic && mob.Status.BuffPhysicalImmune.IsSet())
            )
            {
                Damage = 1;
                return;
            }

            if (attackType == AttackTypes.Summon)
            {
                if (isDragon)
                    CalcDragonDamage();
                else 
                    CalcSummonDamage();
                return;
            }
            else if (skillID == Constants.ChiefBandit.Skills.MesoExplosion)
            {
                CalcMesoExplosion();
                return;
            }

            if (GetIsMiss()) return;
            ApplyBaseDamage();
            if (attackType != AttackTypes.Magic)
                ApplyMobLevelModifier();
            ApplySpecialElementModifiers(Damage * ampData.Magic * 0.01);
            if (attackType != AttackTypes.Magic) ApplyChargeElementModifiers();
            ApplyMobDefReduction();

            if (attackType != AttackTypes.Magic)
            {
                int baseDmg = (int)Damage;
                ApplySkillDamage();
                ApplyComboAttack();
                ApplyCrit(baseDmg);
                ApplyShadowPartner();
            }
            else
            {
                // Check mob+248
                int mob248 = 0;
                if (mob248 > 0)
                {
                    Damage *= mob248 * 0.01;
                }
            }

            Damage = (int)Math.Min(99999, Math.Max(1, Damage));
        }
        void CalcMesoExplosion()
        {
            double mesosUsed = info.MesoExplosionDrops[hitIdx].Reward.Drop;
            double mesoModifier;
            if (mesosUsed <= 1000)
            {
                mesoModifier = (mesosUsed * 0.82 + 28.0) * 0.0001886792452830189;
            }
            else
            {
                mesoModifier = mesosUsed / (mesosUsed + 5250);
            }
            double skillDmgX = skill.XValue;
            var roll = roller.Roll(0.0000000500000050000005);
            Damage = (50 * skill.XValue)
                * (roll + 0.5)
                * mesoModifier;
        }
        void CalcMagicBaseDamage()
        {
            short totalMAD = Math.Min((short)999, chr.PrimaryStats.TotalMAD);
            if (skillID == Cleric.Skills.Heal)
            {
                int targets = info.Data.Attacks.Count + 1; // Targets including self
                double rolledStat = RollStat(intl, 0.8, 0.2);
                Damage = (rolledStat * 1.5 + luk)
                    * (targets * 0.3 + 1.0)
                    * (skill.HPProperty
                     * 0.01)
                    * totalMAD
                    * 0.005
                    / targets;
            }
            else
            {
                double rolledStat = RollStat(totalMAD);
                Damage = (intl * 0.5 + totalMAD * 0.058 * (totalMAD * 0.058) + rolledStat * 3.3)
                    * skill.MagicAttack
                    * 0.01;
            }
        }
        void CalcSummonDamage()
        {
            var summonSkill = chr.Skills.GetSkillLevelData(info.Data.SummonID);
            if (summonSkill == null) return;
            double roll = roller.Roll(0.00000003000000300000031);
            Damage = (dex * (roll + 0.7) * 2.5 + str) * summonSkill.WeaponAttack * 0.01;
        }

        void CalcDragonDamage()
        {
            var skill = chr.Skills.GetSkillLevelData(Priest.Skills.SummonDragon);
            if (skill == null) return;

            var magic = chr.PrimaryStats.TotalMAD;
            var totalInt = chr.PrimaryStats.TotalInt;
            var statModifier = (mob.Data.FS * 5.0 + 10.0) * 0.009000000000000001;
            var rolledStat = RollStat(magic, statModifier);

            Damage = (magic * 0.058 * (magic * 0.058) + totalInt * 0.5 + rolledStat * 3.3) * skill.MagicAttack * 0.01;
        }

        private void ApplyBaseDamage()
        {
            if (BowMeleeBaseDmg()
                || ClawMeleeBaseDmg()
                || ProneBaseDmg()
            ) return;
            if (attackType == AttackTypes.Magic)
            {
                CalcMagicBaseDamage();
                return;
            }
            double statModifier;
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
                            Damage = 10.0 * (int)stat;

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

            double minModifier;
            double maxModifier;
            if (info.Data.AttackTypes == AttackTypes.Magic)
            {
                minModifier = 0.5;
                maxModifier = 1.2;
            }
            else
            {
                minModifier = 0.7;
                maxModifier = 1.3;
            }

            double minTACC = targetAccuracy * minModifier;
            double randTACC = minTACC;
            double maxTACC = targetAccuracy * maxModifier;
            
            randTACC += (maxTACC - randTACC) * roll;
            int mobAvoid = Math.Min(999, mob.Status.TotalEVA);

            IsMiss = randTACC < mobAvoid;
            return IsMiss;
        }

        private bool BowMeleeBaseDmg()
        {
            if (weaponType != WeaponType.Bow && weaponType != WeaponType.Crossbow) return false;
            if (AttackAction.Bullet_1 <= attackAction && attackAction <= AttackAction.Bullet_6 || attackAction == AttackAction.Unk35) return false;

            double dex = RollStat(this.dex);
            if (skillID != Crossbowman.Skills.PowerKnockback && skillID != Hunter.Skills.PowerKnockback)
            {
                Damage = (str + dex) * watk * 0.005;
            }
            else
            {
                Damage = (dex * 3.4 + str) * watk * 0.005;
            }
            return true;
        }

        private bool ClawMeleeBaseDmg()
        {
            if (weaponType != WeaponType.Claw || (AttackAction.Bullet_1 <= attackAction && attackAction <= AttackAction.Bullet_6) || attackAction == AttackAction.Unk35) return false;

            double luk = RollStat(this.luk);

            Damage = (str + dex + luk) * watk * 0.006666666666666667;
            return true;
        }

        private bool ProneBaseDmg()
        {
            if (attackAction != AttackAction.Prone) return false;
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

        void ApplySpecialElementModifiers(double dmg)
        {
            if (skill == null) return;
            double newDmg;
            if (skillID == FPMage.Skills.ElementComposition || skillID == ILMage.Skills.ElementComposition)
            {
                double halfDmg = dmg * 0.5;
                SkillElement[] elements;
                if (skillID == FPMage.Skills.ElementComposition) elements = new SkillElement[] { SkillElement.Fire, SkillElement.Poison };
                else elements = new SkillElement[] { SkillElement.Ice, SkillElement.Lightning };

                double total = 0.0;
                foreach (SkillElement element in elements)
                {
                    var mobifier = (ElementModifier)mob.Data.elemModifiers.GetValue(element);
                    total += ApplyMobElemModifier(halfDmg, mobifier);
                }
                newDmg = total;
            }
            else
            {
                var modifier = 1.0;
                if (skillID == Ranger.Skills.Inferno || skillID == Sniper.Skills.Blizzard)
                {
                    modifier = (20 + skill.Level) * 0.0099999998;
                }
                var elemModifier = (ElementModifier)mob.Data.elemModifiers.GetValue(skill.ElementFlags);
                newDmg = ApplyMobElemModifier(dmg, elemModifier, modifier);
            }
            Damage = newDmg;
        }
        void ApplyChargeElementModifiers()
        {
            if (chr.Job != WhiteKnight.ID) return;
            var buff = chr.PrimaryStats.BuffCharges;
            if (!buff.IsSet()) return;

            int buffSkillID = buff.R;
            if (buffSkillID < WhiteKnight.Skills.SwordFireCharge || buffSkillID > WhiteKnight.Skills.BwLitCharge) return;
            var chargeSkill = chr.Skills.GetSkillLevelData(buffSkillID);
            if (chargeSkill == null) return;

            var element = chargeSkill.ElementFlags;
            double specialModifier = chargeSkill.ZValue * 0.0099999998;
            double damageModifier = chargeSkill.Damage * 0.0099999998;
            var elemModifier = (ElementModifier)mob.Data.elemModifiers.GetValue(element);
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
            if (skillID == DragonKnight.Skills.Sacrifice
                || skillID == Hermit.Skills.ShadowMeso
                || skillID == ChiefBandit.Skills.Assaulter
            ) return;

            double mobDef = attackType == AttackTypes.Magic ? mob.Status.TotalMDD : mob.Status.TotalPDD;
            mobDef = Math.Min(999, mobDef);

            double redMin = mobDef * 0.5;
            double redMax = mobDef * 0.6;
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

        private void ApplyCrit(int baseDmg)
        {
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
                int critBonus = critSkill.Damage - 100;
                Damage += (double)critBonus * 0.01 * (double)baseDmg;
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

            var clonedDmg = hits[hitIdx - spHits];
            Damage = (dmgModifier * clonedDmg.Damage) / 100;
            IsCrit = clonedDmg.IsCrit;
        }

        private double RollStat(double stat, double? masteryModifier = null, double statModifier = 1.0)
        {
            if (!masteryModifier.HasValue) masteryModifier = this.masteryModifier;
            var statRoll = roller.Roll(Roller.STAT_MODIFIER);
            double modifiedStat = stat * statModifier;
            double masteryStat = stat * masteryModifier.Value;
            if (modifiedStat == masteryStat) return modifiedStat;
            else
            {
                double least = Math.Min(modifiedStat, masteryStat);
                double diff = Math.Abs(modifiedStat - masteryStat);
                modifiedStat = least + diff * statRoll;
            }
            return modifiedStat;
        }
    }
}
