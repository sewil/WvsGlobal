using System.IO;
using System;
using System.Linq;

namespace WvsBeta.Common
{
    public class Config
    {
        public ushort MapleVersion { get; }
        public ushort MapleCryptoVersion { get; }
        public string MaplePatchLocation { get; }
        public byte MapleLocale { get; }
        public int PingInterval { get; }
        public bool MemoryCRCEnabled { get; }
        public string ClientBinaryName { get; }
        public bool ShandaEnabled { get; }
        public bool AesEnabled { get; }
        public byte[] AesUserKey { get; }
        public byte[] IvShiftKey { get; }
        public int[] EXP { get; }
        public int[] PetExp { get; }
        public int PartyMinLevelOffset { get; }
        public double PartyPerUserBonus { get; }
        public double PartyTotalBonus { get; }
        public int MaxDamage { get; }

        private static Config _instance;
        public static Config Instance => _instance ?? new Config();

        private Config()
        {
            ConfigReader reader = new ConfigReader(Path.Combine(Environment.CurrentDirectory, "..", "DataSvr", "Config.img"));
            MapleVersion = reader["MapleVersion"].GetUShort();
            MapleCryptoVersion = reader["MapleCryptoVersion"].GetUShort();
            MaplePatchLocation = reader["MaplePatchLocation"].GetString();
            MapleLocale = reader["MapleLocale"].GetByte();
            PingInterval = reader["PingInterval"].GetInt();
            MemoryCRCEnabled = reader["MemoryCRCEnabled"].GetBool();
            ClientBinaryName = reader["ClientBinaryName"].GetString();
            ShandaEnabled = reader["ShandaEnabled"].GetBool();
            AesEnabled = reader["AesEnabled"].GetBool();
            AesUserKey = reader["AesUserKey"].Values.Select(i => Convert.ToByte(i, 16)).ToArray();
            IvShiftKey = reader["IvShiftKey"].Values.Select(i => Convert.ToByte(i, 16)).ToArray();
            EXP = reader["Exp"].Values.Select(i => int.Parse(i)).ToArray();
            PetExp = reader["PetExp"].Values.Select(i => int.Parse(i)).ToArray();
            PartyMinLevelOffset = reader["PartyMinLevelOffset"].GetInt();
            PartyPerUserBonus = reader["PartyPerUserBonus"].GetDouble();
            PartyTotalBonus = reader["PartyTotalBonus"].GetDouble();
            MaxDamage = reader["MaxDamage"].GetInt();

            _instance = this;
        }
    }
}
