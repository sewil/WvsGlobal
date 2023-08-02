using System;
using System.Diagnostics;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Objects.Stats;
using WvsBeta.Common.Sessions;
using WvsBeta.Common.Tracking;
using WvsBeta.Game.Packets;
using static WvsBeta.Common.Constants.EquipSlots;

namespace WvsBeta.Game
{
    public class DamageReflectorSkillData
    {
        public byte Reduction = 0;
        public int Damage = 0;
        public int ObjectID = 0;
        public bool IsPhysical = false;
        public Pos Position = new Pos(0, 0);
    }
    [Flags]
    public enum StatFlags : uint
    {
        Skin = 0x01,
        Eyes = 0x02,
        Hair = 0x04,
        Pet = 0x08,
        Level = 0x10,
        Job = 0x20,
        Str = 0x40,
        Dex = 0x80,
        Int = 0x100,
        Luk = 0x200,
        Hp = 0x400,
        MaxHp = 0x800,
        Mp = 0x1000,
        MaxMp = 0x2000,
        Ap = 0x4000,
        Sp = 0x8000,
        Exp = 0x10000,
        Fame = 0x20000,
        Mesos = 0x40000,
        MaxHpMp = MaxHp|MaxMp,
        APReset = MaxHpMp|Str|Dex|Int|Luk
    };

    public static class CharacterStatsPacket
    {
        public static void HandleStats(GameCharacter chr, Packet packet)
        {
            StatFlags flag = (StatFlags)packet.ReadUInt();
            if (chr.AssertForHack(chr.CharacterStat.AP <= 0, "Trying to use AP, but nothing left."))
            {
                InventoryOperationPacket.NoChange(chr);
                return;
            }
            HandleStats(chr, flag, 1, true, true);
        }
        public static void HandleStats(GameCharacter chr, StatFlags flag, sbyte value, bool deductAp, bool useHPMPGainFormula)
        {
            short jobTrack = Constants.getJobTrack(chr.CharacterStat.Job);

            switch (flag)
            {
                case StatFlags.Str:
                    {
                        if (chr.CharacterStat.Str >= Constants.MaxStat)
                        {
                            InventoryOperationPacket.NoChange(chr);
                            return;
                        }
                        chr.AddStr(value);
                        break;
                    }
                case StatFlags.Dex:
                    {
                        if (chr.CharacterStat.Dex >= Constants.MaxStat)
                        {
                            InventoryOperationPacket.NoChange(chr);
                            return;
                        }
                        chr.AddDex(value);
                        break;
                    }
                case StatFlags.Int:
                    {
                        if (chr.CharacterStat.Int >= Constants.MaxStat)
                        {
                            InventoryOperationPacket.NoChange(chr);
                            return;
                        }
                        chr.AddInt(value);
                        break;
                    }
                case StatFlags.Luk:
                    {
                        if (chr.CharacterStat.Luk >= Constants.MaxStat)
                        {
                            InventoryOperationPacket.NoChange(chr);
                            return;
                        }
                        chr.AddLuk(value);
                        break;
                    }
                case StatFlags.MaxHp:
                    {
                        if (chr.CharacterStat.MaxHP >= Constants.MaxMaxHp)
                        {
                            InventoryOperationPacket.NoChange(chr);
                            return;
                        }
                        short hpGain = 0;
                        if (useHPMPGainFormula)
                        {
                            hpGain += RNG.Range.generate(
                                Constants.HpMpFormulaArguments[jobTrack, 1, (int)Constants.HpMpFormulaFields.HPMin],
                                Constants.HpMpFormulaArguments[jobTrack, 1, (int)Constants.HpMpFormulaFields.HPMax],
                                true
                            );

                            byte improvedMaxHpIncreaseLvl = chr.Skills.GetSkillLevel(Constants.Swordsman.Skills.ImprovedMaxHpIncrease);
                            if (improvedMaxHpIncreaseLvl > 0)
                            {
                                hpGain += GameCharacterSkills.GetSkillLevelData(Constants.Swordsman.Skills.ImprovedMaxHpIncrease, improvedMaxHpIncreaseLvl).XValue;
                            }
                        }
                        else hpGain = value;


                        chr.ModifyMaxHP(hpGain);
                        break;
                    }
                case StatFlags.MaxMp:
                    {
                        if (chr.CharacterStat.MaxMP >= Constants.MaxMaxMp)
                        {
                            InventoryOperationPacket.NoChange(chr);
                            return;
                        }
                        short mpGain = 0;
                        if (useHPMPGainFormula)
                        {
                            short intt = chr.PrimaryStats.GetIntAddition(true);

                            mpGain += RNG.Range.generate(
                                Constants.HpMpFormulaArguments[jobTrack, 1, (int)Constants.HpMpFormulaFields.MPMin],
                                Constants.HpMpFormulaArguments[jobTrack, 1, (int)Constants.HpMpFormulaFields.MPMax],
                                true
                            );

                            // Additional buffing through INT stats
                            mpGain += (short)(
                                intt *
                                Constants.HpMpFormulaArguments[jobTrack, 1, (int)Constants.HpMpFormulaFields.MPIntStatMultiplier] /
                                200
                            );

                            byte improvedMaxMpIncreaseLvl = chr.Skills.GetSkillLevel(Constants.Magician.Skills.ImprovedMaxMpIncrease);
                            if (improvedMaxMpIncreaseLvl > 0)
                            {
                                mpGain += GameCharacterSkills.GetSkillLevelData(Constants.Magician.Skills.ImprovedMaxMpIncrease, improvedMaxMpIncreaseLvl).XValue;
                            }
                        }
                        else mpGain = value;

                        chr.ModifyMaxMP(mpGain);
                        break;
                    }
                default:
                    {
                        Program.MainForm.LogAppend("Unknown type {0:X4}", flag);
                        break;
                    }
            }

            if (deductAp) chr.AddAP(-1, true);
            chr.PrimaryStats.CalculateAdditions(false, false);
        }

        public static void HandleAPReset(GameCharacter chr, StatFlags up, StatFlags down)
        {
            if (!chr.Inventory.HasItemAmount(ItemUseIds.APReset, 1))
            {
                Program.MainForm.LogAppend("Chr {0} tried resetting without AP reset item!", chr.ID);
                return;
            }
            if ((up & StatFlags.APReset) == 0 || (down & StatFlags.APReset) == 0)
            {
                Program.MainForm.LogAppend("Chr {0} tried resetting non-ap reset stats! ({1} -> {2})", chr.ID, down, up);
                return;
            }

            var jobTrack = Constants.getJobTrack(chr.Job);
            Func<Constants.HpMpFormulaFields, sbyte> GetHpMpInc = (field) => (sbyte)Constants.HpMpFormulaArguments[jobTrack, 1, (int)field];

            sbyte upValue, downValue;
            if (up == StatFlags.MaxHp) upValue = GetHpMpInc(Constants.HpMpFormulaFields.HPMin);
            else if (up == StatFlags.MaxMp) upValue = GetHpMpInc(Constants.HpMpFormulaFields.MPMin);
            else upValue = 1;
            if (down == StatFlags.MaxHp) downValue = (sbyte)-GetHpMpInc(Constants.HpMpFormulaFields.HPMax);
            else if (down == StatFlags.MaxMp) downValue = (sbyte)-GetHpMpInc(Constants.HpMpFormulaFields.MPMax);
            else downValue = -1;

            HandleStats(chr, up, upValue, false, false);
            HandleStats(chr, down, downValue, false, false);

            chr.Inventory.TakeItem(ItemUseIds.APReset, 1);

            // TODO: HP/MP validation (crazy formula, ap/class/level/skill dependent)
        }
        public static void HandleSPReset(GameCharacter chr, int itemid, int upSkillID, int downSkillID)
        {
            short jobUp = Constants.getSkillJob(upSkillID);
            short jobDown = Constants.getSkillJob(upSkillID);
            short jobTrackUp = Constants.getJobTrack(jobUp);
            short jobTrackDown = Constants.getJobTrack(jobDown);
            byte upSP = 0;
            var chrSkills = chr.Skills.Skills;
            chrSkills.TryGetValue(upSkillID, out upSP);
            if (!chr.Inventory.HasItemAmount(itemid, 1))
            {
                Program.MainForm.LogAppend("Chr {0} tried resetting without SP reset item {1}!", chr.ID, itemid);
            }
            else if (!chrSkills.TryGetValue(downSkillID, out byte downSP) || !GameDataProvider.Skills.TryGetValue(upSkillID, out SkillData upSD) || !GameDataProvider.Skills.TryGetValue(downSkillID, out SkillData downSD))
            {
                Program.MainForm.LogAppend("SP reset skills not found for chr {0}! ({1} -> {2})", chr.ID, downSkillID, upSkillID);
            }
            else if (jobTrackUp != jobTrackDown || jobTrackUp != Constants.getJobTrack(chr.Job))
            {
                Program.MainForm.LogAppend("SP reset job tracks non-matching for chr {0} with job {1}! ({2} -> {3})", chr.ID, chr.Job, downSkillID, upSkillID);
            }
            else if (itemid == ItemUseIds.SPReset1st && (!Constants.IsFirstJob(jobDown) || !Constants.IsFirstJob(jobUp)))
            {
                Program.MainForm.LogAppend("SP reset chr {0} tried setting non-1st job with 1st job SP reset! ({1} -> {2})", chr.ID, downSkillID, upSkillID);
            }
            else if (itemid == ItemUseIds.SPReset2nd && (Constants.IsThirdJob(jobDown) || !Constants.IsSecondJob(jobUp)))
            {
                Program.MainForm.LogAppend("SP reset chr {0} tried setting non-2nd job with 2nd job SP reset! ({1} -> {2})", chr.ID, downSkillID, upSkillID);
            }
            else if (itemid == ItemUseIds.SPReset3rd && !Constants.IsThirdJob(jobUp))
            {
                Program.MainForm.LogAppend("SP reset chr {0} tried setting non-3rd job with 3rd job SP reset! ({1} -> {2})", chr.ID, downSkillID, upSkillID);
            }
            else if (downSP == 0 || upSP == upSD.MaxLevel)
            {
                Program.MainForm.LogAppend("SP reset invalid reset for chr {0}! Tried removing SP from skills with no SP or apply SP on skills already maxed! ({1} -> {2})", chr.ID, downSkillID, upSkillID);
            }
            else if (upSD.RequiredSkills != null && upSD.RequiredSkills.Where(rs => chrSkills.ContainsKey(rs.Key) && chrSkills[rs.Key] >= rs.Value).Count() != upSD.RequiredSkills.Count)
            {
                Program.MainForm.LogAppend("SP reset chr {0} missing required skills for set ({1} -> {2})", chr.ID, downSkillID, upSkillID);
            }
            else
            {
                chr.Skills.AddSkillPoint(upSkillID);
                chr.Skills.SetSkillPoint(downSkillID, (byte)(downSP - 1));
                chr.Inventory.TakeItem(itemid, 1);
            }
        }
        public static void HandleHeal(GameCharacter chr, Packet packet)
        {
            // 2B 00 14 00 00 00 00 03 00 00
            int flag = packet.ReadInt();


            short hp = (flag & 0x0400) != 0 ? packet.ReadShort() : (short)0;
            short mp = (flag & 0x1000) != 0 ? packet.ReadShort() : (short)0;

            byte extraHealEffect = packet.ReadByte();

            if (chr.HP == 0) return;


            if (hp > 400 ||
                mp > 1000 ||
                (hp > 0 && mp > 0))
            {
                return;
            }

            if (hp > 0)
            {
                // Check endure and stuff here...
                chr.ModifyHP(hp);
                /**
                if (chr.mParty != null)
                {
                    chr.mParty.UpdatePartyMemberHP(chr);
                    chr.mParty.ReceivePartyMemberHP(chr);
                }
                 * **/
            }

            if (mp > 0)
            {
                chr.ModifyMP(mp);
            }
        }

        public static void SendStatChanged(GameCharacter chr, StatFlags flags, bool isSelf = false)
        {
            if (isSelf || flags > 0)
            {
                Packet pw = new Packet(ServerMessages.STAT_CHANGED);
                pw.WriteBool(isSelf);
                pw.WriteUInt((uint)flags);

                if ((flags & StatFlags.Skin) == StatFlags.Skin)
                    pw.WriteByte(chr.Skin);
                if ((flags & StatFlags.Eyes) == StatFlags.Eyes)
                    pw.WriteInt(chr.Face);
                if ((flags & StatFlags.Hair) == StatFlags.Hair)
                    pw.WriteInt(chr.Hair);

                if ((flags & StatFlags.Pet) == StatFlags.Pet)
                    pw.WriteLong(chr.CharacterStat.PetCashId);

                if ((flags & StatFlags.Level) == StatFlags.Level)
                    pw.WriteByte(chr.Level);
                if ((flags & StatFlags.Job) == StatFlags.Job)
                    pw.WriteShort(chr.CharacterStat.Job);
                if ((flags & StatFlags.Str) == StatFlags.Str)
                    pw.WriteShort(chr.CharacterStat.Str);
                if ((flags & StatFlags.Dex) == StatFlags.Dex)
                    pw.WriteShort(chr.CharacterStat.Dex);
                if ((flags & StatFlags.Int) == StatFlags.Int)
                    pw.WriteShort(chr.CharacterStat.Int);
                if ((flags & StatFlags.Luk) == StatFlags.Luk)
                    pw.WriteShort(chr.CharacterStat.Luk);

                if ((flags & StatFlags.Hp) == StatFlags.Hp)
                    pw.WriteShort(chr.HP);
                if ((flags & StatFlags.MaxHp) == StatFlags.MaxHp)
                    pw.WriteShort(chr.CharacterStat.MaxHP);
                if ((flags & StatFlags.Mp) == StatFlags.Mp)
                    pw.WriteShort(chr.CharacterStat.MP);
                if ((flags & StatFlags.MaxMp) == StatFlags.MaxMp)
                    pw.WriteShort(chr.CharacterStat.MaxMP);

                if ((flags & StatFlags.Ap) == StatFlags.Ap)
                    pw.WriteShort(chr.CharacterStat.AP);
                if ((flags & StatFlags.Sp) == StatFlags.Sp)
                    pw.WriteShort(chr.CharacterStat.SP);

                if ((flags & StatFlags.Exp) == StatFlags.Exp)
                    pw.WriteInt(chr.CharacterStat.EXP);

                if ((flags & StatFlags.Fame) == StatFlags.Fame)
                    pw.WriteShort(chr.CharacterStat.Fame);

                if ((flags & StatFlags.Mesos) == StatFlags.Mesos)
                    pw.WriteInt(chr.Inventory.Mesos);

                if ((flags & StatFlags.Pet) == StatFlags.Pet)
                    pw.WriteByte(0); // Movement info index

                chr.SendPacket(pw);
            }
        }

        public static void HandleCharacterDamage(GameCharacter chr, Packet pr)
        {
            //1A FF 03 00 00 00 00 00 00 00 00 04 87 01 00 00 00
            sbyte attack = pr.ReadSByte();
            int damage = pr.ReadInt();
            int reducedDamage = damage;
            int actualHPEffect = -damage;
            int actualMPEffect = 0;
            int healSkillId = 0;
            Mob mob = null;

            if (chr.AssertForHack(damage < -1, "Less than -1 (" + damage + ") damage in HandleCharacterDamage"))
            {
                return;
            }

            if (chr.HP == 0) return;

            byte mobSkillId = 0, mobSkillLevel = 0;

            if (attack <= -2)
            {
                mobSkillLevel = pr.ReadByte();
                mobSkillId = pr.ReadByte(); // (short >> 8)

                //Trace.WriteLine($"Got a hit with {attack} attack, mobSkillLevel {mobSkillLevel}, mobSkillId {mobSkillId}");

            }
            else
            {
                int magicAttackElement = 0;
                if (pr.ReadBool())
                {
                    magicAttackElement = pr.ReadInt();
                    // 0 = no element (Grendel the Really Old, 9001001)
                    // 1 = Ice (Celion? blue, 5120003)
                    // 2 = Lightning (Regular big Sentinel, 3000000)
                    // 3 = Fire (Fire sentinel, 5200002)
                }

                var mobMapId = pr.ReadInt();
                var mobId = pr.ReadInt();

                mob = chr.Field.GetMob(mobMapId);
                if (mob == null ||
                    mobId != mob.MobID)
                {
                    return;
                }

                // Newer ver: int nCalcDamageMobStatIndex
                var stance = pr.ReadByte();
                var isReflected = pr.ReadBool();

                byte reflectHitAction = 0;
                short reflectX = 0, reflectY = 0;
                if (isReflected)
                {
                    reflectHitAction = pr.ReadByte();
                    reflectX = pr.ReadShort();
                    reflectY = pr.ReadShort();
                }

                if (chr.PrimaryStats.BuffMagicGuard.HasReferenceId(Constants.Magician.Skills.MagicGuard) &&
                    chr.CharacterStat.MP > 0)
                {
                    // Absorbs X amount of damage. :)
                    var skillId = chr.PrimaryStats.BuffMagicGuard.R;
                    byte skillLevel;
                    var sld = chr.Skills.GetSkillLevelData(skillId, out skillLevel);

                    int damageEaten = (int)Math.Round((damage * (sld.XValue / 100.0d)));

                    // MagicGuard doesn't show reduced damage.


                    //Trace.WriteLine($"Reducing damage by MG. Reflected {damageEaten}");

                    //Program.MainForm.LogAppend("MG Damage before change: " + actualHPEffect);
                    actualHPEffect += damageEaten;
                    //Program.MainForm.LogAppend("MG Damage after change: " + actualHPEffect);
                    actualMPEffect = -damageEaten;

                    healSkillId = skillId;
                }

                if (chr.PrimaryStats.BuffPowerGuard.HasReferenceId(Constants.Fighter.Skills.PowerGuard) ||
                    chr.PrimaryStats.BuffPowerGuard.HasReferenceId(Constants.Page.Skills.PowerGuard))
                {
                    var skillId = chr.PrimaryStats.BuffPowerGuard.R;
                    byte skillLevel;
                    var sld = chr.Skills.GetSkillLevelData(skillId, out skillLevel);

                    int damageReflectedBack = (int)(damage * (sld.XValue / 100.0d));

                    if (damageReflectedBack > mob.MaxHP)
                        damageReflectedBack = (int)(mob.MaxHP * 0.1);

                    if (mob.IsBoss)
                        damageReflectedBack /= 2;

                    mob.GiveDamage(chr, damageReflectedBack);
                    MobPacket.SendMobDamageOrHeal(chr, mobId, damageReflectedBack, false, false);

                    mob.CheckDead(mob.Position);

                    //Trace.WriteLine($"Reducing damage by PG. Reflected {damageReflectedBack}");
                    actualHPEffect += damageReflectedBack; // Buff 'damaged' hp, so its less
                    healSkillId = skillId;
                }

                
                if (chr.PrimaryStats.BuffMesoGuard.IsSet())
                {
                    var skillId = Constants.ChiefBandit.Skills.MesoGuard;
                    var sld = chr.Skills.GetSkillLevelData(
                        skillId,
                        out var skillLevel
                    );

                    if (sld != null)
                    {
                        var percentage = sld.XValue;

                        var damageReduction = reducedDamage / 2;
                        var mesoLoss = damageReduction * percentage / 100;
                        if (damageReduction != 0)
                        {
                            var playerMesos = chr.Inventory.Mesos;
                            var maxMesosUsable = Math.Min(chr.PrimaryStats.BuffMesoGuard.MesosLeft, playerMesos);
                            if (mesoLoss > maxMesosUsable)
                            {
                                // New calculation. in our version it should actually 'save' the
                                // mesos for a bit.
                                damageReduction = 100 * maxMesosUsable / percentage;
                                mesoLoss = maxMesosUsable;
                            }

                            if (mesoLoss > 0)
                            {
                                chr.PrimaryStats.BuffMesoGuard.MesosLeft -= mesoLoss;
                                MesosTransfer.PlayerUsedSkill(chr.ID, mesoLoss, skillId);

                                chr.Inventory.AddMesos(-(mesoLoss), false);

                                //Trace.WriteLine($"Reducing damage by mesos. Mesos: {mesoLoss}, maxMesos {maxMesosUsable}, reduction {damageReduction}");
                                actualHPEffect += damageReduction;
                                reducedDamage -= reducedDamage;
                            }

                            if (chr.PrimaryStats.BuffMesoGuard.MesosLeft <= 0)
                            {
                                // Debuff when out of mesos
                                chr.PrimaryStats.RemoveByReference(skillId);
                            }
                        }
                    }
                }
                
                SendCharacterDamageByMob(
                    chr,
                    attack,
                    damage,
                    reducedDamage,
                    healSkillId,
                    mobMapId,
                    mobId,
                    stance,
                    isReflected,
                    reflectHitAction,
                    reflectX,
                    reflectY
                );

            }

            //Trace.WriteLine($"Showing damage: {reducedDamage}, {damage}");
            //Trace.WriteLine($"Applying damage: HP {actualHPEffect}, MP: {actualMPEffect}");

            if (actualHPEffect < 0) chr.ModifyHP((short)actualHPEffect);
            if (actualMPEffect < 0) chr.ModifyMP((short)actualMPEffect);

            if (mobSkillLevel != 0 && mobSkillId != 0)
            {
                // Check if the skill exists and has any extra effect.

                if (!GameDataProvider.MobSkills.TryGetValue(mobSkillId, out var skillLevels)) return;

                // Still going strong
                if (!skillLevels.TryGetValue(mobSkillLevel, out var msld)) return;
                OnStatChangeByMobSkill(chr, msld);
            }
            else if (mob != null)
            {
                // CUser::OnStatChangeByMobAttack
                if (mob.Data.Attacks == null ||
                    !mob.Data.Attacks.TryGetValue((byte)attack, out var mad)) return;
                // Okay, we've got an attack...
                if (mad.Disease <= 0) return;

                // Shit's poisonous!
                // Hmm... We could actually make snails give buffs... hurr

                if (!GameDataProvider.MobSkills.TryGetValue(mad.Disease, out var skillLevels)) return;

                // Still going strong
                if (!skillLevels.TryGetValue(mad.SkillLevel, out var msld)) return;
                OnStatChangeByMobSkill(chr, msld);
            }
        }

        public static void OnStatChangeByMobSkill(GameCharacter chr, MobSkillLevelData msld, short delay = 0)
        {

            // See if we can actually set the effect...
            int prop = 100;
            if (msld.Prop != 0)
                prop = msld.Prop;

            if (Rand32.Next() % 100 >= prop) return; // Luck.

            BuffStat setStat = null;
            int rValue = msld.SkillID | (msld.Level << 16);
            var ps = chr.PrimaryStats;
            int nValue = 1;
            switch ((Constants.MobSkills.Skills)msld.SkillID)
            {
                case Constants.MobSkills.Skills.Seal: setStat = ps.BuffSeal; break;
                case Constants.MobSkills.Skills.Darkness: setStat = ps.BuffDarkness; break;
                case Constants.MobSkills.Skills.Weakness: setStat = ps.BuffWeakness; break;
                case Constants.MobSkills.Skills.Stun: setStat = ps.BuffStun; break;
                case Constants.MobSkills.Skills.Curse: setStat = ps.BuffCurse; break;
                case Constants.MobSkills.Skills.Poison:
                    setStat = ps.BuffPoison;
                    nValue = msld.X;
                    break;
            }

            if (setStat != null && !setStat.IsSet())
            {
                var buffTime = msld.Time * 1000;
                var stat = setStat.Set(rValue, (short)nValue, MasterThread.CurrentTime + buffTime + delay);

                if (stat != 0)
                {
                    chr.Buffs.FinalizeBuff(stat, delay);
                }
            }
        }

        public static void SendCharacterDamageByMob(
            GameCharacter chr,
            sbyte attack,
            int initialDamage,
            int reducedDamage,
            int healSkillId,
            int mobMapId,
            int mobId,
            byte stance,
            bool isReflected,
            byte reflectHitAction,
            short reflectX,
            short reflectY)
        {
            Packet pw = new Packet(ServerMessages.DAMAGE_PLAYER);
            pw.WriteInt(chr.ID);
            pw.WriteSByte(attack);
            pw.WriteInt(initialDamage);

            pw.WriteInt(mobMapId);
            pw.WriteInt(mobId);
            pw.WriteByte(stance);
            pw.WriteBool(isReflected);
            if (isReflected)
            {
                pw.WriteByte(reflectHitAction);
                pw.WriteShort(reflectX);
                pw.WriteShort(reflectY);
            }

            pw.WriteInt(reducedDamage);
            // Not used in client
            // if (reducedDamage < 0) pw.WriteInt(healSkillId);

            chr.Field.SendPacket(chr, pw);
        }

        public static void SendCharacterDamage(GameCharacter chr, sbyte attack, int initialDamage, int reducedDamage, int healSkillId)
        {
            Packet pw = new Packet(ServerMessages.DAMAGE_PLAYER);
            pw.WriteInt(chr.ID);
            pw.WriteSByte(attack);
            pw.WriteInt(initialDamage);

            pw.WriteInt(reducedDamage);
            // Not used in client
            // if (reducedDamage < 0) pw.WriteInt(healSkillId);

            chr.Field.SendPacket(chr, pw);
        }
    }
}