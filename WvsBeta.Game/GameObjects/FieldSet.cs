using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.GameObjects.DataLoading;
using WvsBeta.Game.Handlers.Guild;
using WvsBeta.Game.Handlers.GuildQuest;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Game
{
    public class FieldSet
    {
        public static Dictionary<string, FieldSet> Instances { get; } = new Dictionary<string, FieldSet>();
        private readonly Dictionary<string, string> _savedVars = new Dictionary<string, string>();
        public int TimeOut => Data.TimeOut;
        public Map[] Maps => Data.Maps;
        public IList<Map> ActiveMaps { get; }
        public bool Opened { get; private set; }
        public bool Started { get; private set; }
        public bool PendingEnd { get; private set; }
        public bool PendingEnter { get; private set; }
        public bool Shuffle => Data.Shuffle;
        public long StartTime { get; private set; }
        public long EndTime { get; private set; }
        public long TimeRemaining => EndTime - MasterThread.CurrentTime;
        public int MinMembers => Data.MinMembers;
        public int MaxMembers => Data.MaxMembers;
        public short MinLevel => Data.MinLevel;
        public short MaxLevel => Data.MaxLevel;
        public short Quest => Data.Quest;
        public PartyData Party { get; private set; }
        public GuildQuestRegistration GuildRegistration { get; set; }
        public GameCharacter Owner { get; private set; }
        public int UserCount => Characters.Count();
        public IEnumerable<GameCharacter> Characters => ActiveMaps.SelectMany(i => i.Characters);
        public event EventHandler OnOpen;
        public EventSubscriber<FieldSet> OnEnd { get; }
        public FieldSetData Data { get; }
        private IList<FieldSetEventData> pendingEvents;
        private MasterThread.RepeatingAction openAction;
        public FieldSet(FieldSetData fsData)
        {
            OnEnd = new EventSubscriber<FieldSet>();
            Data = fsData;
            foreach (var map in Maps)
            {
                map.SetFieldSet(this);
            }
            ActiveMaps = Maps.Where(i => !fsData.Unaffected.GetValue(i)).ToList();
        }

        public virtual void Start(GameCharacter owner = null)
        {
            if (Started) return;
            Started = true;
            StartTime = MasterThread.CurrentTime;
            pendingEvents = Data.Events.ToList();

            if (TimeOut > 0)
            {
                SetTimer(TimeOut);
            }

            foreach (var map in ActiveMaps)
            {
                // Shuffle reactors
                if (Shuffle)
                {
                    map.ReactorPool.Shuffle();
                }
            }

            if (Data.OpenDelay > 0)
            {
                openAction = MasterThread.RepeatingAction.Start(() => Open(), Data.OpenDelay * 1000, 0);
            }
            else Open(owner);
        }

        private void Open(GameCharacter owner = null)
        {
            Owner = owner ?? Owner;
            if (Owner == null) return;

            if (Data.Guild) GuildRegistration = Owner.GPQRegistration;
            if (Data.Party) Party = Owner.Party;

            if (!string.IsNullOrWhiteSpace(Data.Script))
            {
                FieldSetScriptHost.RunScript(this, Owner, Data.Script, (err) =>
                {
#if DEBUG
                    Owner.Notice("Error running fieldset script + " + err);
#else
                    Program.MainForm.LogAppend("Error running fieldset script " + err);
#endif
                });
            }

            Opened = true;
            OnOpen?.Invoke(this, null);
        }

        private void KickPlayer(GameCharacter chr)
        {
            chr.ChangeMap(chr.Field.ForcedReturn);
        }
        private bool CheckMembers()
        {
            string message = null;
            bool end = false;
            if (Owner == null) end = true;
            else if (!Owner.IsOnline)
            {
                end = true;
                if (Data.Guild) message = "The user that registered has disconnected, so the quest cannot continue. Your Guild Quest will end in 5 seconds.";
            }
            else if (Data.Party && (Party == null || Owner.PartyID != Party.PartyID)) end = true;
            else if (Data.Guild && (GuildRegistration == null || Owner.GuildID != GuildRegistration.GuildID)) end = true; // Master cant quit guild so this should be impossible
            else if (Data.Guild && UserCount < MinMembers)
            {
                end = true;
                message = $"There are less than {MinMembers} members remaining, so the quest cannot continue. Your Guild Quest will end in 5 seconds.";
            }
            else if (Data.Party || Data.Guild)
            {
                foreach (var c in Characters.ToList())
                {
                    if (Data.Party && c.PartyID != Party.PartyID) KickPlayer(c); // Cya
                    if (Data.Guild && c.GuildID != GuildRegistration.GuildID) KickPlayer(c);
                }
            }

            if (end)
            {
                if (message != null) ScheduleEnd(5000, message);
                else End();
            }
            return end;
        }
        private void ScheduleEnd(long delayMS, string message)
        {
            PendingEnd = true;
            if (message != null) Characters.ForEach(c => c.Message(message));
            MasterThread.RepeatingAction.Start(End, delayMS, 0);
        }
        public void SetTimer(int timeOut)
        {
            EndTime = MasterThread.CurrentTime + (timeOut * 1000);
            ActiveMaps.ForEach(m => m.StartTimer(timeOut));
        }

        public virtual void End()
        {
            OnEnd?.Invoke(this);
            foreach (var map in ActiveMaps)
            {
                map.Characters.ToList().ForEach(KickPlayer);
                map.Reset();
            }
            GuildQuestHandler.Unregister(GuildRegistration, true);
            Opened = false;
            Owner = null;
            Started = false;
            PendingEnd = false;
            openAction?.Stop();
            openAction = null;
            _savedVars.Clear();
        }

        public enum EnterStatus
        {
            Invalid = -1,
            Success = 0,
            NotInParty = 1,
            WrongMemberCount = 2,
            TooWeak = 3,
            Full = 4,
        }

        public EnterStatus CheckCanEnter(GameCharacter owner, out IList<GameCharacter> members)
        {
            members = new List<GameCharacter>();
            PartyData pt = null;
            if (Data.Party)
            {
                if (!PartyData.Parties.TryGetValue(owner.PartyID, out pt)) return EnterStatus.NotInParty;
                else if (pt.Leader != owner.ID) return EnterStatus.Invalid;
                members = pt.Characters.Where(i => i.MapID == owner.MapID).ToList();
                if (!Tools.CheckRange(members.Count, MinMembers, MaxMembers) || members.Count != pt.MemberCount) return EnterStatus.WrongMemberCount;
            }
            else if (Data.Guild)
            {
                if (!owner.IsGuildQuestLeader) return EnterStatus.Invalid;
                else return EnterStatus.Success;
            }
            else
            {
                members = new List<GameCharacter> { owner };
            }

            if (!Data.Guild && members.Any(i => !CheckLevel(i)))
            {
                return EnterStatus.TooWeak;
            }
            else if (!Data.Guild && Quest > 0 && members.Any(i => !i.QuestRecord.HasQuest(Quest)))
            {
                return EnterStatus.TooWeak;
            }
            else
            {
                return EnterStatus.Success;
            }
        }

        public bool CheckLevel(GameCharacter chr) => Tools.CheckRange(chr.Level, MinLevel, MaxLevel);

        public EnterStatus Enter(GameCharacter owner, int mapIdx, string portalName = null)
        {
            if (mapIdx < 0 || mapIdx > Maps.Length - 1) return EnterStatus.Invalid;
            else if (Opened) return EnterStatus.Full;

            var status = CheckCanEnter(owner, out var members);
            if (status != EnterStatus.Success) return status;

            PendingEnter = true; // Prevents fs from ending before tele
            // Init maps first before teleporting, shuffle reactors etc.
            if (!Started) Start(owner);

            var mapID = Maps[mapIdx].ID;
            if (Data.Party)
            {
                members.ForEach(c => c.ChangeMap(mapID, portalName));
            }
            else
            {
                owner.ChangeMap(mapID, portalName);
            }

            if (Owner == null) Owner = owner;
            PendingEnter = false;
            return EnterStatus.Success;
        }

        public static EnterStatus Enter(string name, GameCharacter chr, int mapIdx)
        {
            if (!Instances.TryGetValue(name, out var fs)) return EnterStatus.Invalid;
            return fs.Enter(chr, mapIdx);
        }

        public void TryEndingIt(long currentTime)
        {
            if (!Started || PendingEnd || PendingEnter) return;
            
            if (Opened && !CheckMembers())
            {
                var timesUp = TimeOut > 0 && EndTime < currentTime;
                var noMorePlayers = ActiveMaps.Sum(x => x.Characters.Count) == 0;

                if (timesUp || noMorePlayers) End();
            }

            double secondsElapsed = (currentTime - StartTime) / 1000;
            var events = pendingEvents.Where(i => i.TimeAfter <= secondsElapsed);
            foreach (var fsEvent in events.ToList())
            {
                fsEvent.RunAction(this);
                pendingEvents.Remove(fsEvent);
            }
        }

        public static void Update(long currentTime)
        {
            foreach (var keyValuePair in Instances)
            {
                keyValuePair.Value.TryEndingIt(currentTime);
            }
        }

        protected void RunTimer(GameCharacter chr, Map map)
        {
            if (Opened && TimeOut > 0)
            {
                MapPacket.ShowMapTimerForCharacter(chr, (int)(TimeRemaining / 1000));
            }
        }

        public string GetVar(string pName)
        {
            if (_savedVars.ContainsKey(pName)) return _savedVars[pName];
            return "";
        }

        public void SetVar(string pName, string pValue)
        {
            if (!_savedVars.ContainsKey(pName))
                _savedVars.Add(pName, pValue);
            else
                _savedVars[pName] = pValue;
        }
        #region Script helpers
        /// <summary>
        /// Time remaining in seconds
        /// </summary>
        public int GetQuestTime => (int)(TimeRemaining / 1000);
        public void BroadcastMsg(byte type, string text)
        {
            BroadcastMsg((BroadcastMessageType)type, text);
        }
        public void BroadcastMsg(BroadcastMessageType type, string text)
        {
            var packet = ChatPacket.BroadcastMessage(text, type);
            foreach (var map in ActiveMaps)
            {
                map.SendPacket(packet);
            }
        }
        public void TransferFieldAll(int mapid, string portalname)
        {
            foreach (var map in ActiveMaps)
            {
                foreach (var c in map.Characters)
                {
                    c.ChangeMap(mapid, portalname);
                }
            }
        }
        public void TriggerReactor(int mapIdx, string reactorName)
        {
            Maps[mapIdx].ReactorPool.TriggerReactor(reactorName);
        }
        public void PartyClear(int[] mapIdxs, int exp)
        {
            foreach (int mapIdx in mapIdxs)
            {
                Map map = Maps[mapIdx];
                map.EffectPartyClear();
            }
            Characters.ForEach(i => i.AddEXP(exp, MessageAppearType.ChatGrey));
        }
        public void IncExpAll(int exp)
        {
            IncExpAll(exp, 0);
        }
        /// <summary>
        /// Inc exp for all members.
        /// </summary>
        /// <param name="exp">Amount of exp</param>
        /// <param name="quest">QuestID required to have data set to "1" to increase the exp</param>
        public void IncExpAll(int exp, short quest)
        {
            if (quest > 0 && Owner.QuestRecord.GetQuestData(quest) != "1") return;
            Characters.ForEach(m => m.AddEXP(exp, MessageAppearType.ChatGrey));
        }
        #endregion
    }
}
