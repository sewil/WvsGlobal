using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.GameObjects.MiniRoom;

namespace WvsBeta.Game
{
    public class GameStats
    {
        public readonly MiniGameType type;
        public int wins;
        public int ties;
        public int losses;
        public int points;
        public GameStats(MiniGameType type, int wins, int ties, int losses, int points)
        {
            this.type = type;
            this.wins = wins;
            this.ties = ties;
            this.losses = losses;
            this.points = points;
        }
        public void Encode(Packet pw)
        {
            //GW_Minigamerecord_Decode
            pw.WriteInt((int)type);
            pw.WriteInt(wins);
            pw.WriteInt(ties);
            pw.WriteInt(losses);
            pw.WriteInt(points);
        }
    }
    public class CharacterGameStats
    {
        public GameStats this[MiniGameType index] => allStats[index];

        public GameCharacter mCharacter { get; set; }
        private readonly IDictionary<MiniGameType, GameStats> allStats;

        public CharacterGameStats(GameCharacter pCharacter)
        {
            mCharacter = pCharacter;
            allStats = new Dictionary<MiniGameType, GameStats>()
            {
                { MiniGameType.Omok, new GameStats(MiniGameType.Omok, 0, 0, 0, 0) },
                { MiniGameType.MatchCards, new GameStats(MiniGameType.MatchCards, 0, 0, 0, 0) },
            };
        }

        public void Load()
        {
            using (var reader = Server.Instance.CharacterDatabase.RunQuery("SELECT * FROM gamestats WHERE charid = @charid", "@charid", mCharacter.ID) as MySqlDataReader)
            {
                while (reader.Read())
                {
                    var type = (MiniGameType)reader.GetInt32("type");
                    allStats[type] = new GameStats(
                        type,
                        reader.GetInt32("wins"),
                        reader.GetInt32("ties"),
                        reader.GetInt32("losses"),
                        reader.GetInt32("points")
                    );
                }
            }
        }

        public void Save()
        {
            Server.Instance.CharacterDatabase.RunTransaction(comm =>
            {
                comm.CommandText = "DELETE FROM gamestats WHERE charid = @charid";
                comm.Parameters.AddWithValue("@charid", mCharacter.ID);
                comm.ExecuteNonQuery();

                foreach (var stats in allStats)
                {
                    comm.Parameters.Clear();
                    comm.CommandText = "INSERT INTO gamestats (charid, type, wins, losses, ties, points) VALUES (@charid, @type, @wins, @losses, @ties, @points)";
                    comm.Parameters.AddWithValue("@charid", mCharacter.ID);
                    comm.Parameters.AddWithValue("@type", stats.Key);
                    comm.Parameters.AddWithValue("@wins", stats.Value.wins);
                    comm.Parameters.AddWithValue("@losses", stats.Value.losses);
                    comm.Parameters.AddWithValue("@ties", stats.Value.ties);
                    comm.Parameters.AddWithValue("@points", stats.Value.points);
                    comm.ExecuteNonQuery();
                }
            }, Program.MainForm.LogAppend);
        }
        public void Encode(Packet pw)
        {
            pw.WriteShort((short)allStats.Count);
            foreach (var stats in allStats)
            {
                stats.Value.Encode(pw);
            }
        }
    }
}
