using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.DataProviders;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.GameObjects.DataLoading;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Game
{
    public class FieldSet
    {
        public static Dictionary<string, FieldSet> Instances { get; } = new Dictionary<string, FieldSet>();
        private readonly Dictionary<string, string> _savedVars = new Dictionary<string, string>();
        public int TimeOut { get; private set; }
        public Map[] Maps => Data.Maps;
        public bool Started { get; set; }
        public bool Shuffle => Data.Shuffle;
        public long EndTime { get; private set; }
        public long TimeRemaining => EndTime - MasterThread.CurrentTime;
        public int MinPartyMembers => Data.MinPartyMembers;
        public int MaxPartyMembers => Data.MaxPartyMembers;
        public int MinLevel => Data.MinLevel;
        public int MaxLevel => Data.MaxLevel;
        public short Quest => Data.Quest;
        public PartyData Party { get; private set; }
        public GameCharacter Leader { get; private set; }
        public int UserCount { get; private set; }
        public IEnumerable<GameCharacter> Characters => Data.Maps.SelectMany(i => i.Characters);
        public event EventHandler OnStart;
        public event EventHandler OnEnd;
        public FieldSetData Data { get; }
        public FieldSet(FieldSetData fsData)
        {
            Data = fsData;
            TimeOut = fsData.TimeOut;
        }

        public virtual void Start(GameCharacter starter)
        {
            Started = true;
            OnStart?.Invoke(this, null);
            EndTime = MasterThread.CurrentTime + (TimeOut * 1000);

            foreach (var map in Maps)
            {
                map.Reset();

                // Shuffle reactors
                if (Shuffle)
                {
                    map.ReactorPool.Shuffle();
                }
                map.OnEnter = RunTimer;
            }

            if (!string.IsNullOrWhiteSpace(Data.Script))
            {
                FieldSetScriptHost.RunScript(this, starter, Data.Script, (err) =>
                {
#if DEBUG
                    starter.Notice("Error running fieldset script + " + err);
#else
                    Program.MainForm.LogAppend("Error running fieldset script " + err);
#endif
                });
            }
        }

        public void SetTimer(int timeOut)
        {
            TimeOut = timeOut;
            EndTime = MasterThread.CurrentTime + (timeOut * 1000);
            Maps.ForEach(m => m.StartTimer(timeOut));
        }

        public virtual void End()
        {
            foreach (var map in Maps)
            {
                map.OnEnter = null;
                foreach (var character in map.Characters.ToList())
                {
                    character.ChangeMap(map.ReturnMap);
                }
                map.KillAllMobs(0);
                map.DropPool.Clear();
                map.EffectObjects.Clear();
                map.Reset();
            }
            Started = false;
            UserCount = 0;
            Party = null;
            OnEnd?.Invoke(this, null);
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
            InventoryFull
        }

        private EnterStatus CheckCanEnter(GameCharacter chr)
        {
            if ((MinLevel > 0 && chr.Level < MinLevel) || (MaxLevel > 0 && chr.Level > MaxLevel) || (Quest > 0 && !chr.QuestRecord.HasQuest(Quest)))
            {
                return EnterStatus.TooWeak;
            }
            return EnterStatus.Success;
        }

        public EnterStatus Enter(GameCharacter chr, int mapIdx, string portalName = null)
        {
            var toEnter = new List<GameCharacter>();
            if (mapIdx < 0 || mapIdx > Maps.Length - 1) return EnterStatus.Invalid;
            if (Started) return EnterStatus.Full;
            toEnter.Add(chr);
            if (MinPartyMembers > 0)
            {
                if (!PartyData.Parties.TryGetValue(chr.PartyID, out PartyData pt)) return EnterStatus.NotInParty;
                if (pt.Leader != chr.ID) return EnterStatus.Invalid;
                Party = pt;
                toEnter = pt.Characters.Where(c => c.Field.ID == chr.Field.ID).ToList(); // needs to be in same map on enter
                Leader = chr;

                if (toEnter.Count != pt.MemberCount || (MaxPartyMembers > 0 && pt.MemberCount > MaxPartyMembers) || pt.MemberCount < MinPartyMembers) return EnterStatus.WrongMemberCount;
            }

            foreach (var c in toEnter)
            {
                var status = CheckCanEnter(c);
                if (status != EnterStatus.Success) return status;
            }

            Start(chr);

            var mapID = Maps[mapIdx].ID;
            toEnter.ForEach(c => c.ChangeMap(mapID, portalName));
            UserCount = toEnter.Count;
            return EnterStatus.Success;
        }

        public static EnterStatus Enter(string name, GameCharacter chr, int mapIdx)
        {
            if (!Instances.TryGetValue(name, out var fs)) return EnterStatus.Invalid;
            return fs.Enter(chr, mapIdx);
        }

        public void TryEndingIt(long currentTime)
        {
            if (!Started) return;

            var timesUp = TimeOut > 0 && EndTime < currentTime;
            var noMorePlayers = Maps.Sum(x => x.Characters.Count) == 0;

            if (timesUp || noMorePlayers)
            {
                End();
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
            if (Started && TimeOut > 0)
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
            foreach (var map in Maps)
            {
                map.SendPacket(packet);
            }
        }
        public void TransferFieldAll(int mapid, string portalname)
        {
            foreach (var map in Maps)
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
        public void IncExpAll(int exp)
        {
            IncExpAll(exp, 0);
        }
        /// <summary>
        /// Party inc exp for all party members.
        /// </summary>
        /// <param name="exp">Amount of exp</param>
        /// <param name="quest">QuestID required to have data set to "1" to increase the exp</param>
        public void IncExpAll(int exp, short quest)
        {
            if (Leader == null || (quest > 0 && Leader.QuestRecord.GetQuestData(quest) != "1")) return;
            Party.Characters.ForEach(m => m.AddEXP(exp, MessageAppearType.ChatGrey));
        }
        #endregion
    }
}
