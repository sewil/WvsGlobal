using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using WvsBeta.Common;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Database;
using WvsBeta.Game.Events.GMEvents;
using WvsBeta.Game.Handlers.Contimove;
using WvsBeta.Game.Handlers.Guild;
using WvsBeta.Game.Handlers.GuildQuest;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Game
{
    public class Server
    {
        public static bool Tespia { get; private set; }
        public static Server Instance { get; private set; }

        public Rand32 Randomizer { get; set; } = new Rand32();
        public LoopingID MiniRoomIDs { get; set; } = new LoopingID();
        public LoopingID PartyIDs { get; set; } = new LoopingID();

        public double RateMobEXP = 1.0d;
        public double RateMesoAmount = 1.0d;
        public double RateDropChance = 1.0d;

        public byte ID { get; private set; }
        public bool InMigration { get; set; }
        public bool IsNewServerInMigration { get; set; }
        public bool CenterMigration { get; set; }
        public string Name { get; set; }
        public string WorldName { get; set; }
        public byte WorldID { get; set; }

        public int CenterPort { get; set; }
        public IPAddress CenterIP { get; set; }

        public ushort Port { get; set; }
        public IPAddress PublicIP { get; set; }
        public IPAddress PrivateIP { get; set; }

        public CenterSession CenterConnection { get; set; }

        public int GetOnlineId() => RedisBackend.GetOnlineId(WorldID, ID);

        public bool Initialized { get; private set; }

        private GameAcceptor GameAcceptor { get; set; }
        public MySQL_Connection CharacterDatabase { get; private set; }

        public Dictionary<int, Tuple<Packet, long>> CCIngPlayerList { get; } = new Dictionary<int, Tuple<Packet, long>>();
        public ConcurrentDictionary<string, Player> PlayerList { get; } = new ConcurrentDictionary<string, Player>();
        public Dictionary<int, GameCharacter> CharacterList { get; } = new Dictionary<int, GameCharacter>();
        public Dictionary<int, GuildData> Guilds { get; } = new Dictionary<int, GuildData>();
        public IDictionary<byte, List<GuildQuestRegistration>> GuildQuestRegistrations = new Dictionary<byte,List<GuildQuestRegistration>>();
        public List<GuildQuestRegistration> GuildQuestChannelRegistrations => GuildQuestRegistrations.GetValue(ID);
        public HashSet<GameCharacter> StaffCharacters { get; } = new HashSet<GameCharacter>();
        public IDictionary<int, PetItem> SpawnedPets = new Dictionary<int, PetItem>();

        public Dictionary<int, (string reason, string name, byte level, GameCharacter.BanReasons banReason, long time)> DelayedBanRecords { get; } = new Dictionary<int, (string, string, byte, GameCharacter.BanReasons, long)>();

        public DiscordReporter BanDiscordReporter { get; private set; }
        public DiscordReporter ServerTraceDiscordReporter { get; private set; }
        public DiscordReporter MutebanDiscordReporter { get; private set; }

        public Dictionary<string, IGameScript> AvailableScripts { get; } = new Dictionary<string, IGameScript>();
        public Dictionary<string, Dictionary<string, object>> ScriptVars { get; } = new Dictionary<string, Dictionary<string, object>>();

        public string ScrollingHeader { get; private set; }

        public void SetScrollingHeader(string newText)
        {
            ScrollingHeader = newText;
            Program.MainForm.LogAppend("Updating scrolling header to: {0}", ScrollingHeader);
            ChatPacket.SendBroadcastMessageToChannel(ScrollingHeader, BroadcastMessageType.Header);
        }

        public void LogToLogfile(string what)
        {
            Program.LogFile.WriteLine(what);
        }

        public void AddDelayedBanRecord(GameCharacter chr, string reason, GameCharacter.BanReasons banReason, int extraDelay)
        {
            // Only enqueue when we haven't recorded it yet, otherwise you would
            // be able to extend the A/B delay.
            if (DelayedBanRecords.ContainsKey(chr.UserID)) return;


            GameCharacter.HackLog.Info(new GameCharacter.PermaBanLogRecord
            {
                reason = reason
            });
            var seconds = Rand32.NextBetween(3, 10) + extraDelay;
            DelayedBanRecords[chr.UserID] = (reason, chr.Name, chr.Level, banReason, MasterThread.CurrentTime + (seconds * 1000));

            var str = $"Enqueued delayed permban for userid {chr.UserID}, charname {chr.Name}, level {chr.Level}, reason ({banReason}) {reason}, map {chr.MapID} in {seconds} seconds...";
            BanDiscordReporter.Enqueue(str);

            ChatPacket.SendBroadcastMessageToGMs(
                str,
                BroadcastMessageType.Notice
            );
        }

        public void CheckMaps(long pNow)
        {
            GameDataProvider.Maps.ForEach(x => x.Value.MapTimer(pNow));
        }

        public void CheckPets(long ctime)
        {
            foreach (var petKVP in SpawnedPets.ToList())
            {
                GameCharacter chr = GetCharacter(petKVP.Key);

                var pet = petKVP.Value;
                if (chr == null || pet == null || !GameDataProvider.Pets.TryGetValue(pet.ItemID, out PetData petData))
                {
                    SpawnedPets.Remove(petKVP.Key);
                    continue;
                }

                long timeSinceLastHunger = ctime - chr.PetLastHunger;
                if (timeSinceLastHunger < 180000) continue;
                chr.PetLastHunger = ctime;

                byte newFullness = (byte)Math.Max(0, pet.Fullness - petData.Hungry);
                pet.Fullness = newFullness;

                if (newFullness <= 5)
                {
                    Pet.IncreaseCloseness(chr, pet, -1);
                    PetsPacket.RemovePet(chr, PetRemoveReason.Hungry, true);
                }
                else Pet.UpdatePet(chr, pet);
            }
        }

        public void AddPlayer(Player player)
        {
            string hash;
            do
            {
                hash = Cryptos.GetNewSessionHash();
            } while (PlayerList.ContainsKey(hash));
            PlayerList.TryAdd(hash, player);
            player.SessionHash = hash;
        }

        public void RemovePlayer(string hash)
        {
            for (var i = 0; i < 3; i++)
            {
                if (PlayerList.TryRemove(hash, out Player derp)) return;
            }
            Program.MainForm.LogAppend("Unable to remove player with hash {0}", hash);
        }

        public GameCharacter GetCharacter(int ID)
        {
            return CharacterList.TryGetValue(ID, out GameCharacter ret) ? ret : null;
        }


        public GameCharacter GetCharacter(string name)
        {
            name = name.ToLowerInvariant();
            return (from kvp in CharacterList where kvp.Value != null && kvp.Value.Name.ToLowerInvariant() == name select kvp.Value).FirstOrDefault();
        }

        public bool IsPlayer(string hash)
        {
            return PlayerList.ContainsKey(hash);
        }

        public Player GetPlayer(string hash)
        {
            return PlayerList.TryGetValue(hash, out Player ret) ? ret : null;
        }

        private Server(string name)
        {
            Name = name;
            ID = 0xFF;
        }
        public static void Init(string configFile)
        {
            Instance = new Server(configFile);
            Instance.Load();
        }
        public void AssignServer(byte channelId)
        {
            ID = channelId;
            GuildQuestRegistrations.Add(ID, new List<GuildQuestRegistration>());
        }

        public string GetConfigPath(string filename) =>
            Path.Combine(ConfigReader.DataSvrPath, filename);

        void Load()
        {
            Initialized = false;
            LoadConfig(new ConfigReader(GetConfigPath(Name + ".img")));
            LoadDBConfig(GetConfigPath("Database.img"));

            ConnectToCenter();

            Initialized = true;

            MasterThread.RepeatingAction.Start("RemoveNotConnectingPlayers",
                curTime =>
                {
                    var tmp = CCIngPlayerList.ToArray();
                    foreach (var elem in tmp)
                    {
                        if ((elem.Value.Item2 - curTime) > 10000)
                        {
                            CCIngPlayerList.Remove(elem.Key);
                        }
                    }
                },
                0,
                5000
            );

            MasterThread.RepeatingAction.Start("Delayed Ban Processor",
                curTime =>
                {
                    var tmp = DelayedBanRecords.ToList();
                    foreach (var keyValuePair in tmp)
                    {
                        var value = keyValuePair.Value;
                        var userid = keyValuePair.Key;
                        if (value.time <= curTime)
                        {
                            CenterConnection.KickUser(userid);
                            CharacterDatabase.PermaBan(userid, (byte)value.banReason, "AB-" + Name, value.reason);

                            var (maxMachineBanCount, maxUniqueBanCount, maxIpBanCount) = CharacterDatabase.GetUserBanRecordLimit(userid);
                            var (machineBanCount, uniqueBanCount, ipBanCount) = CharacterDatabase.GetUserBanRecord(userid);

                            var str = $"Delayed permaban for userid {userid}, charname {value.name}, level {value.level}, reason {value.reason}. Ban counts: {machineBanCount}/{uniqueBanCount}/{ipBanCount} of {maxMachineBanCount}/{maxUniqueBanCount}/{maxIpBanCount}.";
                            if (uniqueBanCount >= maxUniqueBanCount ||
                                ipBanCount >= maxIpBanCount)
                            {
                                str += " Reached limits, so new accounts are useless.";
                            }

                            BanDiscordReporter.Enqueue(str);

                            ChatPacket.SendBroadcastMessageToGMs(
                                str,
                                BroadcastMessageType.Notice
                            );

                            DelayedBanRecords.Remove(userid);
                        }
                    }
                },
                0,
                1000
            );

            CharacterDatabase.SetupPinger(MasterThread.Instance);

            ContinentMan.Init();

            DiscordReporter.Username = Program.IMGFilename;
            BanDiscordReporter = new DiscordReporter(DiscordReporter.BanLogURL);

            ServerTraceDiscordReporter = new DiscordReporter(DiscordReporter.ServerTraceURL);

            MutebanDiscordReporter = new DiscordReporter(
                "discord muteban discord report url"
            );
        }

        public void ConnectToCenter()
        {
            if (CenterConnection?.Disconnected == false) return;
            CenterConnection = new CenterSession();
        }

        private void LoadDBConfig(string configFile)
        {
            var lines = File.ReadLines(configFile)
                .Select(l => l.Split(' '))
                .Select(p => p.Length == 2 ? "" : p[2])
                .ToList();
            string Username = lines[0];
            string Password = lines[1];
            string Database = lines[2];
            string Host = lines[3];

            CharacterDatabase = new MySQL_Connection(MasterThread.Instance, Username, Password, Database, Host);
            Inventory.Connection = CharacterDatabase;
            CharacterCashItems.Connection = CharacterDatabase;
        }

        private void LoadConfig(ConfigReader reader)
        {
            Port = reader["port"].GetUShort();
            WorldID = reader["gameWorldId"].GetByte();

            CharacterCashItems.WorldID = WorldID;
            log4net.GlobalContext.Properties["WorldID"] = WorldID;

            PublicIP = IPAddress.Parse(reader["PublicIP"].GetString());
            PrivateIP = IPAddress.Parse(reader["PrivateIP"].GetString());

            CenterIP = IPAddress.Parse(reader["center"]["ip"].GetString());
            CenterPort = reader["center"]["port"].GetUShort();
            WorldName = reader["center"]["worldName"].GetString();

            Tespia = reader["tespia"]?.GetBool() ?? false;

            string tmpHeader = reader["scrollingHeader"]?.GetString() ?? "";
            if (tmpHeader == "EMPTY")
            {
                tmpHeader = "";
            }

            ScrollingHeader = tmpHeader;

            RedisBackend.Init(reader);
        }

        public void StartListening()
        {
            Program.MainForm.LogAppend($"Starting to listen on port {Port}");
            GameAcceptor = new GameAcceptor();
        }

        public void StopListening()
        {
            Program.MainForm.LogAppend($"Stopped listening on port {Port}");
            GameAcceptor?.Stop();
            GameAcceptor = null;
        }

        /// <summary>
        /// Broadcast packet to all other channels
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="packetHandler">Packet handler for this channel (to prevent unnecessary sending)</param>
        public void BroadcastPacket(Packet packet, Action<Packet> packetHandler = null)
        {
            if (CenterConnection == null) return;
            var pw = new Packet(ISClientMessages.BroadcastPacketToGameServersExcept);
            pw.WriteInt(ID);

            var pr = new Packet(packet.ToArray());
            pw.WriteBytes(pr.ReadLeftoverBytes());
            CenterConnection.SendPacket(pw);
            if (packetHandler != null)
            {
                pr.Position = 1;
                packetHandler(pr);
            }
        }
    }
}
