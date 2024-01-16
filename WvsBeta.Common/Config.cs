using System.IO;
using System;
using System.Linq;

namespace WvsBeta.Common
{
    public class Config
    {
        public readonly ushort MapleVersion;
        public readonly ushort MapleCryptoVersion; // for IV stuff
        public readonly string MaplePatchLocation = "Patcher.exe";
        public readonly byte MapleLocale;
        public readonly int[] EXP;
        public readonly int[] PetExp;
        public readonly byte[] AesUserKey;
        public readonly int PartyMinLevelOffset;
        public readonly double PartyPerUserBonus;
        public readonly double PartyTotalBonus;
        public readonly int MaxDamage;

        private static Config _instance;
        public static Config Instance => _instance ?? new Config();

        private Config()
        {
            ConfigReader reader = new ConfigReader(Path.Combine(Environment.CurrentDirectory, "..", "DataSvr", "Config.img"));
            MapleVersion = reader["MapleVersion"].GetUShort();
            MapleCryptoVersion = reader["MapleCryptoVersion"].GetUShort();
            MaplePatchLocation = reader["MaplePatchLocation"].GetString();
            MapleLocale = reader["MapleLocale"].GetByte();
            EXP = reader["Exp"].Values.Select(i => int.Parse(i)).ToArray();
            PetExp = reader["PetExp"].Values.Select(i => int.Parse(i)).ToArray();
            AesUserKey = reader["AesUserKey"].Values.Select(i => Convert.ToByte(i, 16)).ToArray();
            PartyMinLevelOffset = reader["PartyMinLevelOffset"].GetInt();
            PartyPerUserBonus = reader["PartyPerUserBonus"].GetDouble();
            PartyTotalBonus = reader["PartyTotalBonus"].GetDouble();
            MaxDamage = reader["MaxDamage"].GetInt();

            _instance = this;
        }
    }
}
