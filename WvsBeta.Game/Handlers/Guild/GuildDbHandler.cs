using MySql.Data.MySqlClient;
using System;
using System.Runtime.ConstrainedExecution;
using WvsBeta.Database;

namespace WvsBeta.Game.Handlers.Guild
{
    public static class GuildDbHandler
    {
        public static MySQL_Connection Db => Server.Instance.CharacterDatabase;
        public static void SaveRanks(int guildId, string[] ranks)
        {
            Db.RunQuery(@"
                            UPDATE guilds
                            SET
                              `rank1` = @rank1,
                              `rank2` = @rank2,
                              `rank3` = @rank3,
                              `rank4` = @rank4,
                              `rank5` = @rank5
                            WHERE id = @guildId
                        ",
            "@rank1", ranks[0],
            "@rank2", ranks[1],
            "@rank3", ranks[2],
            "@rank4", ranks[3],
            "@rank5", ranks[4],
            "@guildId", guildId
            );
        }
        public static bool IsNameTaken(string guildName)
        {
            using (var nameCount = Db.RunQuery("SELECT COUNT(*) AS count FROM guilds WHERE `name` = @name", "@name", guildName) as MySqlDataReader)
            {
                return !nameCount.Read() || nameCount.GetInt32("count") > 0;
            }
        }
        public static bool RemoveMember(int guildId, int charId)
        {
            bool result = false;
            Db.RunTransaction(comm => {
                comm.CommandText = "DELETE FROM guild_members WHERE guild_id = @guildId AND character_id = @charId";
                comm.Parameters.AddWithValue("@guildId", guildId);
                comm.Parameters.AddWithValue("@charId", charId);
                result = comm.ExecuteNonQuery() == 1;
            }, Program.MainForm.LogAppend);
            return result;
        }
        public static bool AddMember(int guildId, int charId, GuildRank rank)
        {
            bool result = false;
            Db.RunTransaction(comm =>
            {
                comm.CommandText = @"INSERT INTO guild_members (guild_id, character_id, `rank`) VALUES (@guildId, @charId, @rank)";
                comm.Parameters.AddWithValue("@guildId", guildId);
                comm.Parameters.AddWithValue("@charId", charId);
                comm.Parameters.AddWithValue("@rank", (int)rank);
                result = comm.ExecuteNonQuery() == 1;
            }, Program.MainForm.LogAppend);
            return result;
        }
        public static void SaveMember(int guildId, int charId, GuildRank rank)
        {
            Db.RunQuery(@"
                UPDATE guild_members
                SET `rank` = @rank
                WHERE character_id = @cid AND guild_id = @guildId
            ", "@rank", (int)rank, "@cid", charId, "@guildId", guildId);
        }
        public static bool AddGuild(GuildData guild)
        {
            try
            {
                Db.RunTransaction(comm =>
                {
                    comm.CommandText = @"
                        INSERT INTO guilds (name, capacity, rank1, rank2, rank3, rank4, rank5)
                        VALUES (
                            @name,
                            @capacity,
                            @rank1,
                            @rank2,
                            @rank3,
                            @rank4,
                            @rank5
                        )
                    ";
                    comm.Parameters.AddWithValue("@name", guild.Name);
                    comm.Parameters.AddWithValue("@capacity", guild.Capacity);
                    comm.Parameters.AddWithValue("@rank1", guild.RankNames[0]);
                    comm.Parameters.AddWithValue("@rank2", guild.RankNames[1]);
                    comm.Parameters.AddWithValue("@rank3", guild.RankNames[2]);
                    comm.Parameters.AddWithValue("@rank4", guild.RankNames[3]);
                    comm.Parameters.AddWithValue("@rank5", guild.RankNames[4]);
                    comm.ExecuteNonQuery();
                    comm.Parameters.Clear();

                    guild.ID = (int)comm.LastInsertedId;

                    foreach (var member in guild.Members)
                    {
                        comm.CommandText = "INSERT INTO guild_members (guild_id, character_id, `rank`) VALUES (@guildId, @cid, @rank)";
                        comm.Parameters.AddWithValue("@guildId", guild.ID);
                        comm.Parameters.AddWithValue("@cid", member.Key);
                        comm.Parameters.AddWithValue("@rank", (int)member.Value.Rank);
                        comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                    }
                }, Program.MainForm.LogAppend);
                return true;
            }
            catch (Exception e)
            {
                Program.MainForm.LogDebug(e.Message);
                return false;
            }
        }
        public static GuildData LoadGuild(GameCharacter chr, int guildId)
        {
            try
            {
                GuildData guild;
                using (var guildReader = Db.RunQuery(@"SELECT * FROM guilds WHERE id = @id", "@id", guildId) as MySqlDataReader)
                {
                    if (!guildReader.Read())
                    {
                        throw new ArgumentException("Failed reading guild with guild id " + guildId);
                    }
                    guild = GuildData.LoadFromReader(guildReader);
                }
                using (var membersReader = Db.RunQuery(@"
                    SELECT g.*, c.level, c.job, c.name
                    FROM guild_members g
                    LEFT JOIN characters c ON c.id = g.character_id
                    WHERE g.guild_id = @guild_id
                ", "@guild_id", guildId) as MySqlDataReader)
                {
                    while (membersReader.Read())
                    {
                        int cid = membersReader.GetInt32("character_id");
                        GuildMember member = GuildMember.LoadFromReader(membersReader, cid == chr.ID);
                        guild.Members[cid] = member;
                    }

                    return guild;
                }
            }
            catch (Exception e)
            {
                Program.MainForm.LogAppend($"Loading guild {guildId} failed! {e.Message}");
                return null;
            }
        }
        public static bool RemoveGuild(int guildId)
        {
            bool result = false;
            Db.RunTransaction(comm => {
                comm.CommandText = "DELETE FROM guilds WHERE id = @guildId";
                comm.Parameters.AddWithValue("@guildId", guildId);
                int r = comm.ExecuteNonQuery();
                result = r > 0;
            }, Program.MainForm.LogAppend);
            return result;
        }
        public static bool UpdateGuildCapacity(int guildId, int capacity)
        {
            bool result = false;
            Db.RunTransaction(comm => {
                comm.CommandText = "UPDATE guilds SET capacity = @capacity WHERE id = @guildId";
                comm.Parameters.AddWithValue("@guildId", guildId);
                comm.Parameters.AddWithValue("@capacity", capacity);
                result = comm.ExecuteNonQuery() > 0;
            }, Program.MainForm.LogAppend);
            return result;
        }
        public static bool SaveGuildEmblem(int guildId, GuildEmblem emblem)
        {
            bool result = false;
            Db.RunTransaction(comm => {
                comm.CommandText = @"
                    UPDATE guilds
                    SET
                        emblem_bg = @emblemBg,
                        emblem_bgc = @emblemBgColor,
                        emblem_logo = @emblemLogo,
                        emblem_logoc = @emblemLogoColor
                    WHERE id = @guildId
                ";
                comm.Parameters.AddWithValue("@guildId", guildId);
                comm.Parameters.AddWithValue("@emblemBg", emblem.Background);
                comm.Parameters.AddWithValue("@emblemBgColor", emblem.BackgroundColor);
                comm.Parameters.AddWithValue("@emblemLogo", emblem.Logo);
                comm.Parameters.AddWithValue("@emblemLogoColor", emblem.LogoColor);
                result = comm.ExecuteNonQuery() > 0;
            }, Program.MainForm.LogAppend);
            return result;
        }
    }
}
