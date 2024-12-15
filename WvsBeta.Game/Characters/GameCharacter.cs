using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using log4net;
using MySql.Data.MySqlClient;
using WvsBeta.Common;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Events;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.GameObjects.MiniRooms;
using WvsBeta.Game.Handlers.Guild;
using WvsBeta.Game.Handlers.GuildQuest;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public partial class GameCharacter : Character, IFieldObj
    {
        private static ILog _characterLog = LogManager.GetLogger("CharacterLog");

        // WorldServer Event (EXP rate)
        public static int ms_nIncExpRate_WSE = 100;

        // Regular EXP rate
        public static int ms_nIncEXPRate => (int)(Server.Instance.RateMobEXP * 100);

        // When married
        public static int ms_nIncExpRate_Wedding = 100;

        // User-specific exp rate
        public double m_dIncExpRate = 1.0;
        // No exp rate ticket
        public double m_dIncDropRate = 1.0;
        public double m_dIncDropRate_Ticket = 1.0;

        public static int ms_nPartyBonusEventRate = 0;

        public short MapChair { get; set; } = -1;
        private DateTime LastSavepoint;
        private long LastPlayTimeSave;

        Map _field;
        public Map Field {
            get { return _field; }
            set
            {
                _field = value;
                CharacterStat.MapID = _field.ID;
            }
        }

        public short HP
        {
            get => CharacterStat.HP;
            set
            {
                CharacterStat.HP = value;
                PartyHPUpdate();
            }
        }

        public byte PortalCount { get; set; }

        public bool GMHideEnabled { get; private set; }
        public bool Donator { get; private set; }
        public bool BetaPlayer { get; private set; }
        public MiniRoom Room { get; set; }
        public byte RoomSlotId { get; set; }
        public bool UsingTimer { get; set; }

        public new GameInventory Inventory => (GameInventory)base.Inventory;
        public new GameCharacterSkills Skills => (GameCharacterSkills)base.Skills;
        public CharacterBuffs Buffs { get; private set; }
        public new GameCharacterPrimaryStats PrimaryStats { get => (GameCharacterPrimaryStats)base.PrimaryStats; set => base.PrimaryStats = value; }
        public Rand32 CalcDamageRandomizer { get; private set; }
        public Rand32 RndActionRandomizer { get; private set; }
        public CharacterSummons Summons { get; private set; }
        public CharacterStorage Storage { get; private set; }
        public CharacterQuests Quests { get; private set; }
        public CharacterGameStats GameStats { get; private set; }
        public long PetLastInteraction { get; set; }
        public long PetLastHunger { get; set; }
        public EventTeam Team { get; set; }
        public EventSubscriber<GameCharacter> OnLeaveParty { get; } = new EventSubscriber<GameCharacter>();
        public EventSubscriber<GameCharacter> OnLeaveGuild { get; } = new EventSubscriber<GameCharacter>();

        public PartyData Party
        {
            get
            {
                if (!PartyData.Parties.ContainsKey(PartyID)) return null;
                else return PartyData.Parties[PartyID];
            }
        }

        public List<int> Wishlist { get; private set; }

        public NpcChatSession NpcSession { get; set; } = null;
        public int ShopNPCID { get; set; } = 0;
        public int TrunkNPCID { get; set; } = 0;

        public Player Player { get; set; }

        public bool Undercover { get; set; }
        public string ImitatorName { get; private set; }
        public DateTime MutedUntil { get; set; }
        public byte MuteReason { get; set; }

        public long LastChat { get; set; }

        public bool IsInNPCChat => ShopNPCID != 0 || TrunkNPCID != 0 || NpcSession != null;
        public bool IsInMiniRoom => Room != null;
        public bool CanAttachAdditionalProcess
        {
            get
            {
                var ret = !IsInNPCChat && !IsInMiniRoom;

                if (ret == false)
                {
                    HackLog.Warn($"CanAttachAdditionalProcess: {ShopNPCID}, {TrunkNPCID}, {NpcSession}, {Room}");
                }
                return ret;
            }
        }

        public int DoorMapId = Constants.InvalidMap;
        public long tLastDoor = 0;


        public GameCharacter(int CharacterID)
        {
            CharacterStat.ID = CharacterID;
        }

        public bool IsAFK => (MasterThread.CurrentTime - LastMove) > 120000 &&
                             (MasterThread.CurrentTime - LastChat) > 120000;

        public void SendPacket(byte[] pw)
        {
            Player?.Socket?.SendData(pw);
        }

        public void SendPacket(Packet pw)
        {
            Player?.Socket?.SendPacket(pw);
        }

        public void EncodeForRemote(Packet packet)
        {
            packet.WriteString(Name);

            (Guild ?? GuildData.Default).EncodeForRemote(packet);

            BuffPacket.EncodeForRemote(this, packet);
            new AvatarLook(this, false).Encode(packet);

            packet.WriteInt(Inventory.ActiveItemID);
            packet.WriteInt(Inventory.ChocoCount);
            packet.WriteShort(Position.X);
            packet.WriteShort(Position.Y);
            packet.WriteByte(Stance);
            packet.WriteShort(Foothold);

            packet.WriteBool(IsGM && !Undercover);

            var petItem = GetSpawnedPet();
            packet.WriteBool(petItem != null);
            if (petItem != null)
            {
                packet.WriteInt(petItem.ItemID);
                packet.WriteString(petItem.Name);
                packet.WriteLong(petItem.CashId);
                var ml = petItem.MovableLife;
                packet.WriteShort(ml.Position.X);
                packet.WriteShort(ml.Position.Y);
                packet.WriteByte(ml.Stance);
                packet.WriteShort(ml.Foothold);
            }

            // Mini Game & Player Shops
            string miniGame = null;
            packet.WriteBool(miniGame != null); // Hardcoded end of minigame & player shops until implemented
            if (miniGame != null)
            {
                packet.WriteInt(0);
                packet.WriteString("");
                packet.WriteByte(0);
                packet.WriteByte(0);
                packet.WriteByte(0);
                packet.WriteByte(0);
                packet.WriteByte(0);
            }

            // Couple rings
            var coupleRing = Inventory.GetEquippedCoupleRing();
            packet.WriteBool(coupleRing != null);
            if (coupleRing != null)
            {
                coupleRing.EncodeRing(packet);
            }
        }

        public void HandleDeath()
        {
            HackLog.Info("Player will be moved back to town/return map");
            ModifyHP(50, false);

            // Remove all buffs
            PrimaryStats.Reset(true);

            // There's only 1 map that has this. Its the pharmacy map in kerning
            if (Field.ReturnMap == Constants.InvalidMap)
            {
                ChangeMap(Field.ID);
            }
            else
            {
                ChangeMap(Field.ReturnMap);
            }
        }


        public void SetIncExpRate()
        {
            var currentDateTime = MasterThread.CurrentDate;
            SetIncExpRate(currentDateTime.Day, currentDateTime.Hour);
        }

        public void SetIncExpRate(int day, int hour)
        {
            const int Exp_Normal = 100;
            const int Exp_Premium = 100;
            const int Drop_Normal = 100;
            const int Drop_Premium = 100;

            bool isPremium = false;
            double expRate = 1.0;
            double dropRate = 1.0;

            // TODO: check inventories

            if (ms_nIncEXPRate != 100)
            {
                // Check player range, we don't care lol
                expRate = ms_nIncEXPRate * expRate * 0.01;
            }

            if (isPremium)
            {
                expRate *= 1.2;
            }

            // Check inventories for droprate tickets

            if (isPremium)
            {
                expRate *= Exp_Premium * 0.01;
                dropRate *= Drop_Premium * 0.01;
            }
            else
            {
                expRate *= Exp_Normal * 0.01;
                dropRate *= Drop_Normal * 0.01;
            }

            m_dIncDropRate_Ticket = 1.0;

            m_dIncDropRate = dropRate;
            m_dIncExpRate = expRate;

            Trace.WriteLine($"Rates: EXP {m_dIncExpRate}, Drop {m_dIncDropRate}, Drop ticket {m_dIncDropRate_Ticket}");
        }

        public bool IsShownTo(IFieldObj Object)
        {
            if (GMHideEnabled)
            {
                var player = Object as GameCharacter;
                if (player != null && player.IsGM) return true;
                return false;
            }

            return true;
        }

        public void CleanupInstances()
        {
            Room?.RemovePlayer(this, MiniRoomLeaveReason.Cancel);
            ShopNPCID = 0;
            TrunkNPCID = 0;
            NpcSession?.SoftStop();
        }

        public void TryActivateHide()
        {
            if (!IsGM || GMHideEnabled || true) return;

            var hideSkill = Constants.Gm.Skills.Hide;
            // Make sure that the user has the skill
            if (Skills.GetSkillLevel(hideSkill) == 0)
                Skills.AddSkillPoint(hideSkill);

            if (!Undercover)
            {
                Buffs.AddBuff(hideSkill, 1);
                SetGMHide(true, true);
            }
        }

        public void Save()
        {
            if (ImitatorName != null) return;

            _characterLog.Debug("Saving character...");
            Server.Instance.CharacterDatabase.RunTransaction(comm =>
            {
                var saveQuery = new StringBuilder();

                saveQuery.Append("UPDATE characters SET ");
                saveQuery.Append("skin = '" + CharacterStat.Skin + "', ");
                saveQuery.Append("hair = '" + CharacterStat.Hair + "', ");
                saveQuery.Append("gender = '" + (byte)CharacterStat.Gender + "', ");
                saveQuery.Append("eyes = '" + CharacterStat.Face + "', ");
                saveQuery.Append("map = '" + CharacterStat.MapID + "', ");
                saveQuery.Append("pos = '" + PortalID + "', ");
                saveQuery.Append("level = '" + CharacterStat.Level + "', ");
                saveQuery.Append("job = '" + CharacterStat.Job + "', ");
                saveQuery.Append("chp = '" + CharacterStat.HP + "', ");
                saveQuery.Append("cmp = '" + CharacterStat.MP + "', ");
                saveQuery.Append("mhp = '" + CharacterStat.MaxHP + "', ");
                saveQuery.Append("mmp = '" + CharacterStat.MaxMP + "', ");
                saveQuery.Append("`int` = '" + CharacterStat.Int + "', ");
                saveQuery.Append("dex = '" + CharacterStat.Dex + "', ");
                saveQuery.Append("str = '" + CharacterStat.Str + "', ");
                saveQuery.Append("luk = '" + CharacterStat.Luk + "', ");
                saveQuery.Append("ap = '" + CharacterStat.AP + "', ");
                saveQuery.Append("sp = '" + CharacterStat.SP + "', ");
                saveQuery.Append("fame = '" + CharacterStat.Fame + "', ");
                saveQuery.Append("exp = '" + CharacterStat.EXP + "', ");
                saveQuery.Append($"pet_cash_id = 0x{CharacterStat.PetCashId:X16},");
                // saveQuery.Append($"playtime = playtime + 0x{(MasterThread.CurrentTime - LastPlayTimeSave):X16}, ");
                saveQuery.Append("last_savepoint = '" + LastSavepoint.ToString("yyyy-MM-dd HH:mm:ss") + "' ");
                saveQuery.Append("WHERE ID = " + CharacterStat.ID);

                comm.CommandText = saveQuery.ToString();
                comm.ExecuteNonQuery();
            }, Program.MainForm.LogAppend);

            LastPlayTimeSave = MasterThread.CurrentTime;

            Server.Instance.CharacterDatabase.RunTransaction(comm =>
            {
                comm.CommandText = "DELETE FROM character_wishlist WHERE charid = " + CharacterStat.ID;
                comm.ExecuteNonQuery();

                if (Wishlist.Count > 0)
                {
                    var wishlistQuery = new StringBuilder();

                    wishlistQuery.Append("INSERT INTO character_wishlist VALUES ");
                    wishlistQuery.Append(string.Join(", ", Wishlist.Select(serial => "(" + CharacterStat.ID + ", " + serial + ")")));

                    comm.CommandText = wishlistQuery.ToString();
                    comm.ExecuteNonQuery();
                }
            }, Program.MainForm.LogAppend);

            Inventory.SaveInventory();
            Inventory.SaveCashItems(null);
            Skills.SaveSkills();
            Storage.Save();
            Quests.SaveQuests();
            GameStats.Save();
            if (GuildID > 0) GuildDbHandler.SaveMember(GuildID, ID, GuildMember.Rank);

            _characterLog.Debug("Saving finished!");
        }

        public void PartyHPUpdate()
        {
            if (PartyID == 0) return;

            Field
                .GetInParty(PartyID)
                .Where(p => p.CharacterStat.ID != CharacterStat.ID)
                .ForEach(p => p.SendPacket(PartyPacket.SendHpUpdate(HP, PrimaryStats.TotalMaxHP, CharacterStat.ID)));

        }

        public void FullPartyHPUpdate()
        {
            if (PartyID == 0) return;

            Field
                .GetInParty(PartyID)
                .Where(p => p.CharacterStat.ID != CharacterStat.ID)
                .Select(p => Tuple.Create(this, p))
                .ForEach(pair =>
                {
                    pair.Item1.SendPacket(PartyPacket.SendHpUpdate(pair.Item2.HP, pair.Item2.PrimaryStats.TotalMaxHP, pair.Item2.CharacterStat.ID));
                    pair.Item2.SendPacket(PartyPacket.SendHpUpdate(pair.Item1.HP, pair.Item1.PrimaryStats.TotalMaxHP, pair.Item1.CharacterStat.ID));
                });
        }

        public enum LoadFailReasons
        {
            None,
            UnknownCharacter,
            NotFromPreviousIP,
            UserAlreadyOnline,
            TransitionTimeout
        }

        public LoadFailReasons Load(string IP)
        {
            int? imitateID = RedisBackend.Instance.GetImitateID(ID);
            int originalID = ID;
            bool isImitating = imitateID.HasValue;
            if (isImitating)
            {
                ID = imitateID.Value;
                _characterLog.Debug($"Loading character {ID} from IP {IP}... (IMITATION from ID {originalID})");
            }
            else _characterLog.Debug($"Loading character {CharacterStat.ID} from IP {IP}...");

            using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery(@"
                SELECT
                    characters.*,
                    users.gm, users.last_ip, users.online
                FROM characters
                LEFT JOIN users ON users.id = characters.userid
                WHERE characters.id = @id
                ",
                "@id", originalID
            ))
            {
                if (!data.Read())
                {
                    _characterLog.Debug("Loading failed: unknown character.");
                    return LoadFailReasons.UnknownCharacter;
                }

                if (data.GetString("last_ip") != IP && !isImitating)
                {
#if DEBUG
                    Program.MainForm.LogAppend("Allowed player " + this.ID +
                                               " to log in from different IP because source is running in debug mode!");
#else
                    _characterLog.Debug("Loading failed: not from previous IP.");
                    return LoadFailReasons.NotFromPreviousIP;
#endif
                }
                UserID = data.GetInt32("userid"); // For cashitem loading
                Name = data.GetString("name");
                GMLevel = (GMLevel)data.GetByte("gm");

                if (isImitating) ImitatorName = Name;
                else ImitatorName = null;
            }

            int tmpUserID = UserID;

            using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery(@"
                SELECT c.*, users.gm, users.last_ip, users.online, users.quiet_ban_expire, users.quiet_ban_reason, coalesce(g.guild_id, 0) AS guild_id
                FROM characters c
                LEFT JOIN users ON users.id = c.userid
                LEFT JOIN guild_members g ON c.id = g.character_id
                WHERE c.id = @id",
                "@id", ID
            ))
            {
                if (!data.Read())
                {
                    _characterLog.Debug("Loading failed: unknown character.");
                    if (isImitating)
                    {
                        // Reset!
                        RedisBackend.Instance.SetImitateID(originalID, 0);
                    }
                    return LoadFailReasons.UnknownCharacter;
                }

                UserID = data.GetInt32("userid"); // For cashitem loading
                MutedUntil = data.GetDateTime("quiet_ban_expire");
                MuteReason = data.GetByte("quiet_ban_reason");
                LastSavepoint = data.GetDateTime("last_savepoint");
                GuildID = data.GetInt32("guild_id");
                LastPlayTimeSave = MasterThread.CurrentTime;

                var _mapId = data.GetInt32("map");

                Map field;
                if (!GameDataProvider.Maps.TryGetValue(_mapId, out field))
                {
                    Program.MainForm.LogAppend(
                        "The map of {0} is not valid (nonexistant)! Map was {1}. Returning to 0", CharacterStat.ID, _mapId);
                    field = GameDataProvider.Maps[0];
                    PortalID = 0;
                }
                Field = field;

                // Push back player when there's a forced return value
                // Forced return is already set on log out
                //if (field.ForcedReturn != Constants.InvalidMap)
                //{
                //    _mapId = field.ForcedReturn;
                //    if (!DataProvider.Maps.TryGetValue(_mapId, out field))
                //    {
                //        Program.MainForm.LogAppend(
                //            "The map of {0} is not valid (nonexistant)! Map was {1}. Returning to 0", CharacterStat.ID, _mapId);
                //        // Note: using Field here
                //        Field = DataProvider.Maps[0];
                //    }
                //    else
                //    {
                //        Field = DataProvider.Maps[_mapId];
                //    }
                //    PortalID = 0;
                //}
                //else
                //{
                PortalID = (byte)data.GetInt16("pos");
                //}

                // Select portal to spawn on.
                {
                    Portal portal = Field.SpawnPoints.Find(x => x.ID == PortalID);
                    if (portal == null) portal = Field.GetRandomStartPoint();
                    Position = new Pos(portal.X, portal.Y);
                }
                Stance = 0;
                Foothold = 0;

                CalcDamageRandomizer = new Rand32();
                RndActionRandomizer = new Rand32();

                PrimaryStats = new GameCharacterPrimaryStats(this);

                CharacterStat.LoadFromReader(data);
                BuddyListCapacity = (byte)data.GetInt32("buddylist_size");

                // Make sure we don't update too many times
                lastSaveStep = CalculateSaveStep();
            }

            base.Inventory = new GameInventory(this);
            Inventory.LoadInventory();

            UserID = tmpUserID;

            base.Skills = new GameCharacterSkills(this);
            Skills.LoadSkills();

            Storage = new CharacterStorage(this);
            Storage.Load();

            Buffs = new CharacterBuffs(this);

            Summons = new CharacterSummons(this);

            Quests = new CharacterQuests(this);
            Quests.LoadQuests();

            GameStats = new CharacterGameStats(this);
            GameStats.Load();

            if (GuildID > 0 && Guild == null) GuildHandler.SendLoadGuild(this, GuildID);

            Wishlist = new List<int>();
            using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery("SELECT serial FROM character_wishlist WHERE charid = " + CharacterStat.ID))
            {
                while (data.Read())
                {
                    Wishlist.Add(data.GetInt32(0));
                }
            }

            // Loading done, switch back ID
            ID = originalID;

            InitDamageLog();

            SetIncExpRate();

            var muteTimeSpan = RedisBackend.Instance.GetCharacterMuteTime(CharacterStat.ID);
            if (muteTimeSpan.HasValue)
                HacklogMuted = MasterThread.CurrentDate.Add(muteTimeSpan.Value);
            else
                HacklogMuted = DateTime.MinValue;

            Undercover = RedisBackend.Instance.IsUndercover(CharacterStat.ID);

            RedisBackend.Instance.SetPlayerOnline(
                UserID,
                Server.Instance.GetOnlineId(),
                IP
            );

            _characterLog.Debug("Loaded!");
            return LoadFailReasons.None;
        }


        public void SetupLogging()
        {
            ThreadContext.Properties["UserID"] = UserID;
            ThreadContext.Properties["CharacterID"] = CharacterStat.ID;
            ThreadContext.Properties["CharacterName"] = CharacterStat.Name;
            ThreadContext.Properties["MapID"] = CharacterStat.MapID;
        }

        public static void RemoveLogging()
        {
            ThreadContext.Properties.Remove("UserID");
            ThreadContext.Properties.Remove("CharacterID");
            ThreadContext.Properties.Remove("CharacterName");
            ThreadContext.Properties.Remove("MapID");
        }
        #region Script helpers
        public bool IsPartyBoss => Party?.Leader == ID;
        public CharacterQuests QuestRecord => Quests;
        public short AP => CharacterStat.AP;
        public short SP => CharacterStat.SP;
        public short POP => CharacterStat.Fame;
        public short MHP => CharacterStat.MaxHP;
        public short STR => CharacterStat.Str;
        public short DEX => CharacterStat.Dex;
        public short INT => CharacterStat.Int;
        public short LUK => CharacterStat.Luk;
        public bool IsClosedBetaTester => false;
        public string SCharacterName => Name;
        public void PlayPortalSE()
        {
            PlayerEffectPacket.SendPortalSoundEffect(this);
        }
        public void SendSound(string path)
        {
            SendPacket(FieldEffectPacket.EffectSound(path));
        }
        public void Message(string text)
        {
            SendPacket(MessagePacket.RedText(text));
        }
        public void Notice(string text, BroadcastMessageType type = BroadcastMessageType.Notice)
        {
            ChatPacket.SendBroadcastMessageToPlayer(this, text, type);
        }
        public void IncEXP(uint amount, int isSelf)
        {
            AddEXP(amount, MessageAppearType.ChatGrey, isSelf == 1);
        }
        public void IncHP(double value, int isSelf)
        {
            ModifyHP((short)value);
        }
        public void IncAP(short ap, int isSelf)
        {
            AddAP(ap, isSelf == 1);
        }
        public void IncSP(short sp, int isSelf)
        {
            AddSP(sp);
        }
        public void IncMMP(short value, int isSelf)
        {
            ModifyMaxMP(value);
        }
        public void IncMHP(short value, int isSelf)
        {
            ModifyMaxHP(value);
        }
        public void IncPetTame(short inc)
        {
            var pet = GetSpawnedPet();
            if (pet == null) return;
            Pet.IncreaseCloseness(this, pet, inc);
        }
        public byte IncMoney(int inc, int isQuest)
        {
            return (byte)(IncMoney(inc, isQuest == 1 ? MessageAppearType.ChatGrey : MessageAppearType.SideWhite) ? 1 : 0);
        }
        public bool IncMoney(int inc, MessageAppearType appearType = MessageAppearType.None)
        {
            return Inventory.MassExchange(inc, null, appearType);
        }
        public void IncPOP(short inc, int isSelf)
        {
            AddFame(inc);
        }

        public int IncFriendMax(byte inc, int mesos)
        {
            if (!Inventory.CanExchange(mesos) || !IncreaseBuddySlots(inc)) return 0;
            Inventory.Exchange(mesos);
            return 1;
        }

        public void LearnSkill(int skillid)
        {
            this.Skills.AddSkillPoint(skillid);
        }

        public AvatarSelectState ChangeAvatar(int coupon, int avatarID)
        {
            if (0 <= avatarID && avatarID <= 4) // Skin
            {
                if (!Inventory.MassExchange(0, (coupon, -1))) return AvatarSelectState.MissingCoupon;
                SetSkin((byte)avatarID);
                return AvatarSelectState.Success;
            }
            else if (avatarID % (20000 + ((byte)Gender * 1000)) < 1000) // Face
            {
                if (!Inventory.MassExchange(0, (coupon, -1))) return AvatarSelectState.MissingCoupon;
                SetFace(avatarID);
                return AvatarSelectState.Success;
            }
            else if (avatarID % (30000 + ((byte)Gender * 1000)) < 1000) // Hair & hair color
            {
                if (!Inventory.MassExchange(0, (coupon, -1))) return AvatarSelectState.MissingCoupon;
                SetHair(avatarID);
                return AvatarSelectState.Success;
            }
            return AvatarSelectState.SysError;
        }
        #endregion
        #region Guild
        public enum GuildCreateStatus
        {
            Success = 0,
            TooLowLv = 1,
            NotInParty = 2,
            NotPartyLeader = 3,
            NotEnoughMembers = 4,
            PartyTraitor = 5,
            NotEnoughMesos = 6
        }
        public int GuildID { get; set; }
        public GuildData Guild => Server.Instance.Guilds.ContainsKey(GuildID) ? Server.Instance.Guilds[GuildID] : null;
        public GuildMember GuildMember => Guild?.Members.ContainsKey(ID) == true ? Guild.Members[ID] : null;
        public bool IsGuildMaster => GuildMember?.Rank == GuildRank.Master;
        public bool IsGuildSubMaster => GuildMember?.Rank == GuildRank.JrMaster;
        public bool CanChangeRank => GuildMember?.Rank <= GuildRank.JrMaster;
        public bool IsGuildMember => GuildMember != null;
        public int GetGuildCountMax => Guild?.Capacity ?? 0;
        public bool IsGuildMarkExist => Guild?.Emblem.Background != 0;
        public GuildCreateStatus IsCreateGuildPossible(int mesos)
        {
            if (Level < 10) return GuildCreateStatus.TooLowLv;
            else if (PartyID == 0) return GuildCreateStatus.NotInParty;
            else if (!IsPartyBoss) return GuildCreateStatus.NotPartyLeader;
#if DEBUG
            else if (Party.Characters.Where(c => c.Field.ID == Field.ID).Count() < 2) return GuildCreateStatus.NotEnoughMembers;
#else
            else if (Party.Characters.Where(c => c.Field.ID == Field.ID).Count() < 6) return GuildCreateStatus.NotEnoughMembers;
#endif
            else if (Party.Characters.Any(c => c.IsGuildMember)) return GuildCreateStatus.PartyTraitor;
            else if (!Inventory.CanExchange(-mesos)) return GuildCreateStatus.NotEnoughMesos;
            return GuildCreateStatus.Success;
        }
        public void SetGuildMark(int mesos)
        {
            _guildPendingFee = mesos;
            GuildHandler.HandleCreateGuildEmblem(this);
        }
        public void RemoveGuildMark(int mesos)
        {
            _guildPendingFee = mesos;
            GuildHandler.HandleRemoveGuildEmblem(this);
        }
        public int _guildPendingFee;
        public string _guildNamePending;
        public List<bool> _guildContractPending;
        public void CreateNewGuild(int mesos)
        {
            _guildPendingFee = mesos;
            GuildHandler.SendCreateGuildEnterName(this);
        }

        public int IncGuildCountMax(int slots, int mesos)
        {
            return GuildHandler.SendExpandGuild(this, slots, mesos);
        }
        public bool RemoveGuild(int mesos)
        {
            return GuildHandler.HandleDisbandGuild(this, mesos, GuildID);
        }
        public bool IsGuildQuestLeader => GPQRegistration?.Leader == ID;
        public GuildQuestRegistration GPQRegistration => Guild?.GPQRegistration;
        public int CanEnterGuildQuest
        {
            get
            {
                if (GPQRegistration == null) return 0;
                else if (GPQRegistration.ChannelID != Server.Instance.ID) return 0;
                else if (GPQRegistration.QueueIndex != 0) return 0;
                else if (!FieldSet.Instances["Guild1"].Started && !IsGuildQuestLeader) return 0;
                else if (FieldSet.Instances["Guild1"].Opened) return 0;
                else return IsGuildQuestLeader ? 1 : 2;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action">0 = guild count (-n means your guild is registered on other channel n), 1 = register, 2 = cancel registration, 3 = waitlist</param>
        public int DealWithGuildQuest(int action)
        {
            var registrations = Server.Instance.GuildQuestRegistrations;
            byte channelId = Server.Instance.ID;

            if (action == 0)
            {
                if (GPQRegistration != null)
                {
                    return -(GPQRegistration.ChannelID + 1);
                }
                else
                {
                    return registrations[channelId].Count;
                }
            }
            else if (action == 1)
            {
                return GuildQuestHandler.Register(this);
            }
            else if (action == 2)
            {
                return GuildQuestHandler.Unregister(GPQRegistration, false);
            }
            else if (action == 3)
            {
                if (GPQRegistration == null) return -1;
                else
                {
                    return registrations[GPQRegistration.ChannelID].FindIndex(i => i.GuildID == GuildID);
                }
            }
            return 0;
        }
        public void IncGuildPoint(int gp)
        {
            // TODO
        }
        #endregion
    }
}