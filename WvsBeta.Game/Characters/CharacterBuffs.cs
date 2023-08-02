using MySql.Data.MySqlClient;
using System.Diagnostics;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects.Stats;

namespace WvsBeta.Game
{
    public class CharacterBuffs
    {
        public GameCharacter Character { get; set; }
        public byte mComboCount { get; set; }

        public CharacterBuffs(GameCharacter chr)
        {
            Character = chr;
        }

        public bool HasGMHide()
        {
            return Character.PrimaryStats.HasBuff(Constants.Gm.Skills.Hide);
        }

        public void AddItemBuff(int itemid)
        {
            var data = GameDataProvider.Items[itemid];

            var expireTime = MasterThread.CurrentTime + data.BuffTime;
            var ps = Character.PrimaryStats;
            var value = -itemid;
            Common.Enums.BuffValueTypes added = 0;

            if (data.Accuracy > 0)
                added |= ps.BuffAccurancy.Set(value, data.Accuracy, expireTime);

            if (data.Avoidance > 0)
                added |= ps.BuffAvoidability.Set(value, data.Avoidance, expireTime);

            if (data.Speed > 0)
                added |= ps.BuffSpeed.Set(value, data.Speed, expireTime);

            if (data.MagicAttack > 0)
                added |= ps.BuffMagicAttack.Set(value, data.MagicAttack, expireTime);

            if (data.WeaponAttack > 0)
                added |= ps.BuffWeaponAttack.Set(value, data.WeaponAttack, expireTime);

            if (data.WeaponDefense > 0)
                added |= ps.BuffWeaponDefense.Set(value, data.WeaponDefense, expireTime);

            if (data.Thaw != 0)
                added |= ps.BuffThaw.Set(value, data.Thaw, expireTime);

            if (added != 0)
            {
                FinalizeBuff(added, 0);
            }

            Common.Enums.BuffValueTypes removed = 0;

            if (data.Cures.HasFlag(CureFlag.Weakness))
                removed |= ps.BuffWeakness.Reset();

            if (data.Cures.HasFlag(CureFlag.Poison))
                removed |= ps.BuffPoison.Reset();

            if (data.Cures.HasFlag(CureFlag.Curse))
                removed |= ps.BuffCurse.Reset();

            if (data.Cures.HasFlag(CureFlag.Darkness))
                removed |= ps.BuffDarkness.Reset();

            if (data.Cures.HasFlag(CureFlag.Seal))
                removed |= ps.BuffSeal.Reset();

            FinalizeDebuff(removed);
        }

        public void Dispell()
        {
            var ps = Character.PrimaryStats;
            Common.Enums.BuffValueTypes removed = 0;

            removed |= ps.BuffWeakness.Reset();
            removed |= ps.BuffPoison.Reset();
            removed |= ps.BuffCurse.Reset();
            removed |= ps.BuffDarkness.Reset();
            removed |= ps.BuffSeal.Reset();
            removed |= ps.BuffStun.Reset();

            FinalizeDebuff(removed);
        }

        public void CancelHyperBody()
        {
            var primaryStats = Character.PrimaryStats;
            primaryStats.BuffBonuses.MaxHP = 0;
            primaryStats.BuffBonuses.MaxMP = 0;


            if (Character.HP > primaryStats.GetMaxHP(false))
            {
                Character.ModifyHP(primaryStats.GetMaxHP(false));
            }

            if (Character.CharacterStat.MP > primaryStats.GetMaxMP(false))
            {
                Character.ModifyMP(primaryStats.GetMaxMP(false));
            }

            //mCharacter.SetMaxHP(primaryStats.GetMaxHP(false));
            //mCharacter.SetMaxMP(primaryStats.GetMaxMP(false));
        }


        public void AddBuff(int SkillID, byte level, short delay = 0)
        {
            if (!BuffDataProvider.SkillBuffValues.TryGetValue(SkillID, out var flags))
            {
                return;
            }

            
            if (level == 0xFF)
            {
                level = Character.Skills.Skills[SkillID];
            }
            var data = GameDataProvider.Skills[SkillID].Levels[level];


            long buffTime = data.BuffSeconds * 1000 + delay;

            Trace.WriteLine($"Adding buff from skill {SkillID} lvl {level}: {buffTime}. Flags {flags}");

            var expireTime = MasterThread.CurrentTime + buffTime;
            var ps = Character.PrimaryStats;
            Common.Enums.BuffValueTypes added = 0;

            if (flags.HasFlag(Common.Enums.BuffValueTypes.WeaponAttack)) added |= ps.BuffWeaponAttack.Set(SkillID, data.WeaponAttack, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.WeaponDefense)) added |= ps.BuffWeaponDefense.Set(SkillID, data.WeaponDefense, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.MagicAttack)) added |= ps.BuffMagicAttack.Set(SkillID, data.MagicAttack, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.MagicDefense)) added |= ps.BuffMagicDefense.Set(SkillID, data.MagicDefense, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.Accurancy)) added |= ps.BuffAccurancy.Set(SkillID, data.Accurancy, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.Avoidability)) added |= ps.BuffAvoidability.Set(SkillID, data.Avoidability, expireTime);
            //if (flags.Contains(BuffValueTypes.Hands)) added |= ps.BuffHands.Set(SkillID, data.Hands, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.Speed)) added |= ps.BuffSpeed.Set(SkillID, data.Speed, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.Jump)) added |= ps.BuffJump.Set(SkillID, data.Jump, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.MagicGuard)) added |= ps.BuffMagicGuard.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.DarkSight)) added |= ps.BuffDarkSight.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.Booster)) added |= ps.BuffBooster.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.PowerGuard)) added |= ps.BuffPowerGuard.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.MaxHP)) added |= ps.BuffMaxHP.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.MaxMP)) added |= ps.BuffMaxMP.Set(SkillID, data.YValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.Invincible)) added |= ps.BuffInvincible.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.SoulArrow)) added |= ps.BuffSoulArrow.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.ComboAttack)) added |= ps.BuffComboAttack.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.Charges)) added |= ps.BuffCharges.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.DragonBlood)) added |= ps.BuffDragonBlood.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.HolySymbol)) added |= ps.BuffHolySymbol.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.MesoUP)) added |= ps.BuffMesoUP.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.ShadowPartner)) added |= ps.BuffShadowPartner.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.PickPocketMesoUP)) added |= ps.BuffPickPocketMesoUP.Set(SkillID, data.XValue, expireTime);
            if (flags.HasFlag(Common.Enums.BuffValueTypes.MesoGuard))
            {
                added |= ps.BuffMesoGuard.Set(SkillID, data.XValue, expireTime);
                ps.BuffMesoGuard.MesosLeft = data.MesosUsage;
            }
            if (flags.HasFlag(BuffValueTypes.Slow)) added |= ps.BuffSlow.Set(SkillID, data.Speed, expireTime);

            FinalizeBuff(added, delay);
        }

        public void FinalizeBuff(Common.Enums.BuffValueTypes added, short delay, bool sendPacket = true)
        {
            if (added == 0) return;
            Trace.WriteLine($"Added buffs {added}");

            Character.FlushDamageLog();
            Character.PrimaryStats.UpdateBuffBonuses();

            if (!sendPacket) return;
            BuffPacket.AddBuffs(Character, added, delay);
            MapPacket.SendPlayerBuffed(Character, added, delay);
        }

        public void FinalizeDebuff(Common.Enums.BuffValueTypes removed, bool sendPacket = true)
        {
            if (removed == 0) return;
            Trace.WriteLine($"Removed buffs {removed}");

            Character.FlushDamageLog();
            Character.PrimaryStats.UpdateBuffBonuses();

            if (!sendPacket) return;
            BuffPacket.RemoveBuffs(Character, removed);
            MapPacket.SendPlayerDebuffed(Character, removed);
        }
        
    }
}