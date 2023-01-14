using System;
using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Common.Enums;

namespace WvsBeta.Game
{
    public class BuffDataProvider
    {
        public static readonly Dictionary<int, BuffValueTypes> SkillBuffValues = new Dictionary<int, BuffValueTypes>
        {
            {Constants.Fighter.Skills.AxeBooster, BuffValueTypes.Booster},
            {Constants.Fighter.Skills.SwordBooster, BuffValueTypes.Booster},
            {Constants.Page.Skills.BwBooster, BuffValueTypes.Booster},
            {Constants.Page.Skills.SwordBooster, BuffValueTypes.Booster},
            {Constants.Spearman.Skills.SpearBooster, BuffValueTypes.Booster},
            {Constants.Spearman.Skills.PolearmBooster, BuffValueTypes.Booster},
            {Constants.FPMage.Skills.SpellBooster, BuffValueTypes.Booster},
            {Constants.ILMage.Skills.SpellBooster, BuffValueTypes.Booster},
            {Constants.Hunter.Skills.BowBooster, BuffValueTypes.Booster},
            {Constants.Crossbowman.Skills.CrossbowBooster, BuffValueTypes.Booster},
            {Constants.Assassin.Skills.ClawBooster, BuffValueTypes.Booster},
            {Constants.Bandit.Skills.DaggerBooster, BuffValueTypes.Booster},

            {Constants.Magician.Skills.MagicGuard, BuffValueTypes.MagicGuard},

            {Constants.Magician.Skills.MagicArmor, BuffValueTypes.WeaponDefense},
            {Constants.Swordsman.Skills.IronBody, BuffValueTypes.WeaponDefense},

            {Constants.Archer.Skills.Focus, BuffValueTypes.Accurancy | BuffValueTypes.Avoidability},
            //{ (int)Constants.Archer.Skills.Focus,  BuffValueTypes.Avoidability },



            {Constants.Fighter.Skills.Rage, BuffValueTypes.WeaponAttack | BuffValueTypes.WeaponDefense},

            {Constants.Fighter.Skills.PowerGuard, BuffValueTypes.PowerGuard},
            {Constants.Page.Skills.PowerGuard, BuffValueTypes.PowerGuard},

            {Constants.Spearman.Skills.IronWill, BuffValueTypes.WeaponDefense | BuffValueTypes.MagicDefense},

            {Constants.Spearman.Skills.HyperBody, BuffValueTypes.MaxHP | BuffValueTypes.MaxMP},

            {Constants.FPWizard.Skills.Meditation, BuffValueTypes.MagicAttack},
            {Constants.ILWizard.Skills.Meditation, BuffValueTypes.MagicAttack},

            {Constants.Cleric.Skills.Invincible, BuffValueTypes.Invincible},

            {
                Constants.Cleric.Skills.Bless,
                BuffValueTypes.WeaponDefense | BuffValueTypes.MagicDefense | BuffValueTypes.Accurancy |
                BuffValueTypes.Avoidability
            },
            {
                Constants.Gm.Skills.Bless,
                BuffValueTypes.WeaponAttack | BuffValueTypes.WeaponDefense | BuffValueTypes.MagicAttack |
                BuffValueTypes.MagicDefense | BuffValueTypes.Accurancy | BuffValueTypes.Avoidability
            },

            {Constants.ChiefBandit.Skills.MesoGuard, BuffValueTypes.MesoGuard},

            {Constants.Priest.Skills.HolySymbol, BuffValueTypes.HolySymbol},
            {Constants.Gm.Skills.HolySymbol, BuffValueTypes.HolySymbol},

            {Constants.ChiefBandit.Skills.Pickpocket, BuffValueTypes.PickPocketMesoUP},
            {Constants.Hermit.Skills.MesoUp, BuffValueTypes.PickPocketMesoUP},

            {Constants.DragonKnight.Skills.DragonRoar, BuffValueTypes.Stun},

            {Constants.WhiteKnight.Skills.BwFireCharge, BuffValueTypes.MagicAttack | BuffValueTypes.Charges},
            {Constants.WhiteKnight.Skills.BwIceCharge, BuffValueTypes.MagicAttack | BuffValueTypes.Charges},
            {Constants.WhiteKnight.Skills.BwLitCharge, BuffValueTypes.MagicAttack | BuffValueTypes.Charges},
            {Constants.WhiteKnight.Skills.SwordFireCharge, BuffValueTypes.MagicAttack | BuffValueTypes.Charges},
            {Constants.WhiteKnight.Skills.SwordIceCharge, BuffValueTypes.MagicAttack | BuffValueTypes.Charges},
            {Constants.WhiteKnight.Skills.SwordLitCharge, BuffValueTypes.MagicAttack | BuffValueTypes.Charges},

            {Constants.Assassin.Skills.Haste, BuffValueTypes.Speed | BuffValueTypes.Jump},
            {Constants.Bandit.Skills.Haste, BuffValueTypes.Speed | BuffValueTypes.Jump},
            {Constants.Gm.Skills.Haste, BuffValueTypes.Speed | BuffValueTypes.Jump},

            {Constants.Rogue.Skills.DarkSight, BuffValueTypes.Speed | BuffValueTypes.DarkSight},
            {Constants.Gm.Skills.Hide, BuffValueTypes.Invincible | BuffValueTypes.DarkSight},

            {Constants.Hunter.Skills.SoulArrow, BuffValueTypes.SoulArrow},
            {Constants.Crossbowman.Skills.SoulArrow, BuffValueTypes.SoulArrow},

            {Constants.Hermit.Skills.ShadowPartner, BuffValueTypes.ShadowPartner},
            {Constants.Gm.Skills.ShadowPartner, BuffValueTypes.ShadowPartner},

            {Constants.Crusader.Skills.ComboAttack, BuffValueTypes.ComboAttack},

            {Constants.DragonKnight.Skills.DragonBlood, BuffValueTypes.WeaponAttack | BuffValueTypes.DragonBlood},
        };
    }
}