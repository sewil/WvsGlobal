using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WvsBeta.Common.DataProviders;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Database;

namespace WvsBeta.Common.Characters
{
    public class CharacterSkills
    {
        public Dictionary<int, byte> Skills { get; } = new Dictionary<int, byte>();

        public virtual void AddSkills(Packet packet)
        {
            packet.WriteShort((short)Skills.Count);

            foreach (var kvp in Skills)
            {
                packet.WriteInt(kvp.Key); // Skill ID
                packet.WriteInt(kvp.Value); // Skill points
            }
        }

        public virtual void SaveSkills()
        {
            throw new NotImplementedException();
        }

        public void LoadSkills(int characterID, MySQL_Connection db)
        {
            using (var reader = db.RunQuery(
                    "SELECT skillid, points FROM skills WHERE charid = @charid",
                    "@charid", characterID) as MySqlDataReader)
            {
                while (reader.Read())
                {
                    Skills.Add(reader.GetInt32("skillid"), (byte)reader.GetInt16("points"));
                }
            }
        }
        public virtual void LoadSkills()
        {
            throw new NotImplementedException();
        }

        public virtual void AddSkillPoint(int skillid)
        {
            throw new NotImplementedException();
        }

        public virtual void SetSkillPoint(int skillid, byte level, bool packet = true)
        {
            throw new NotImplementedException();
        }

        public virtual void DoSkillCost(int skillid, byte level)
        {
            throw new NotImplementedException();
        }

        public virtual void UseMeleeAttack(int skillid, AttackData attackData)
        {
            throw new NotImplementedException();
        }

        public virtual void UseRangedAttack(int skillid, short pos)
        {
            throw new NotImplementedException();
        }

        public virtual byte GetSkillLevel(int skillid)
        {
            throw new NotImplementedException();
        }

        public virtual byte GetSkillLevel(int skillid, out SkillLevelData data)
        {
            throw new NotImplementedException();
        }

        public virtual double GetSpellAttack(int spellId)
        {
            throw new NotImplementedException();
        }

        public virtual double GetSpellMastery(int spellId)
        {
            throw new NotImplementedException();
        }

        public virtual ushort GetRechargeableBonus()
        {
            throw new NotImplementedException();
        }

        public virtual int GetMastery()
        {
            throw new NotImplementedException();
        }

        public virtual int GetMpStealSkillData(int attackType, out int prop, out int precent, out byte level)
        {
            throw new NotImplementedException();
        }

        public SkillLevelData GetSkillLevelData(int skill) => GetSkillLevelData(skill, out byte level);

        public SkillLevelData GetSkillLevelData(int skill, out byte level)
        {
            if (Skills.TryGetValue(skill, out level))
            {
                return GetSkillLevelData(skill, level);
            }
            return null;
        }

        public static SkillLevelData GetSkillLevelData(int skill, byte level)
        {
            if (DataProvider.Skills.TryGetValue(skill, out var skillData))
            {
                if (skillData.MaxLevel >= level) return skillData.Levels[level];
            }

            return null;
        }
    }
}
