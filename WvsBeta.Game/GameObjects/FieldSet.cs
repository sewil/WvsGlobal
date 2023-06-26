using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Game.Events.GMEvents;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.Packets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WvsBeta.Game
{
    public class FieldSet
    {
        public static Dictionary<string, FieldSet> Instances { get; } = new Dictionary<string, FieldSet>();
        private readonly Dictionary<string, string> _savedVars = new Dictionary<string, string>();
        public string Name { get; private set; }
        public Map[] Maps { get; private set; }
        public int Timeout { get; private set; }

        public bool Started { get; set; }
        public long EndTime { get; private set; }
        public long TimeRemaining => EndTime - MasterThread.CurrentTime;

        public int ReturnMap { get; private set; } = -1;
        public string ReturnPortal { get; private set; }
        public string PartyParams { get; private set; }
        public short ReqQuest { get; private set; }
        public bool EnterAsParty { get; private set; }
        public PartyData Party { get; private set; }
        public GameCharacter Leader { get; private set; }
        public bool CleanupMobs { get; private set; }
        public bool CleanupDrops { get; private set; }
        public bool CleanupEffectObjects { get; private set; }
        public int[] CleanupNpcs { get; private set; }
        public bool ResetReactors { get; }
        public bool ShuffleReactors { get; }
        public int Capacity { get; }
        public int UserCount { get; private set; }
        public IEnumerable<GameCharacter> Characters => Maps.SelectMany(i => i.Characters);
        public event EventHandler OnEnd;
        public event EventHandler<long> OnTimerUpdate;
        public FieldSet(ConfigReader.Node fsNode)
        {
            Name = fsNode.Name;
            Instances[Name] = this;

            var maps = new Map[0];

            foreach (var subNode in fsNode.SubNodes)
            {
                if (int.TryParse(subNode.Name, out var idx))
                {
                    if (idx >= maps.Length)
                        Array.Resize(ref maps, idx + 1);
                    var map = GameDataProvider.Maps[subNode.GetInt()];
                    maps[idx] = map;
                    map.ParentFieldSet = this;
                }
                else switch (subNode.Name)
                    {
                        case "timeOut":
                            Timeout = subNode.GetInt();
                            break;
                        case "returnMap":
                            ReturnMap = subNode.GetInt();
                            break;
                        case "returnPortal":
                            ReturnPortal = subNode.GetString();
                            break;
                        case "enterAsParty":
                            EnterAsParty = subNode.GetBool();
                            break;
                        case "ptParams":
                            PartyParams = subNode.GetString();
                            break;
                        case "cleanupMobs":
                            CleanupMobs = subNode.GetBool();
                            break;
                        case "cleanupDrops":
                            CleanupDrops = subNode.GetBool();
                            break;
                        case "cleanupEffectObjects":
                            CleanupEffectObjects = subNode.GetBool();
                            break;
                        case "cleanupNpcs":
                            CleanupNpcs = subNode.GetString().Split(',').Select(i => int.Parse(i)).ToArray();
                            break;
                        case "resetReactors":
                            ResetReactors = subNode.GetBool();
                            break;
                        case "shuffleReactors":
                            ShuffleReactors = subNode.GetBool();
                            break;
                        case "reqQuest":
                            ReqQuest = subNode.GetShort();
                            break;
                        case "capacity":
                            Capacity = subNode.GetInt();
                            break;
                    }
            }

            Maps = maps;
            Program.MainForm.LogAppend("Loaded fieldset '{0}' with maps {1}", Name, string.Join(", ", Maps.Select(x => x.ID)));
        }

        public static void Load()
        {
            var reader = new ConfigReader(Server.Instance.GetConfigPath("FieldSet.img"));
            foreach (var node in reader.RootNode)
            {
                if (node.Name.StartsWith("Event"))
                {
                    EventFieldSet.Create(node);
                    continue;
                }
                new FieldSet(node);
            }
        }
        public void ResetOnTimerUpdate()
        {
            OnTimerUpdate?.ClearInvocations();
            OnTimerUpdate = null;
        }

        public virtual void Start()
        {
            Started = true;
            EndTime = MasterThread.CurrentTime + (Timeout * 1000);

            foreach (var map in Maps)
            {
                // Reset portals
                foreach (var keyValuePair in map.Portals)
                {
                    var portalType = keyValuePair.Value.Type;
                    keyValuePair.Value.Enabled = !(portalType == 4 || portalType == 5);
                }

                // Shuffle reactors
                if (ShuffleReactors)
                {
                    map.ReactorPool.Shuffle();
                }
                map.OnEnter = RunTimer;
            }
            Program.MainForm.LogAppend("Started fieldset '{0}'", Name);
        }

        public void SetTimer(int timeOut)
        {
            Timeout = timeOut;
            EndTime = MasterThread.CurrentTime + (timeOut * 1000);
            Maps.ForEach(m => m.Characters.ForEach(c => RunTimer(c, m)));
        }

        public virtual void End()
        {
            foreach (var map in Maps)
            {
                map.OnEnter = null;
                map.OnTimerEnd?.Invoke(map);
                if (ReturnMap > -1)
                {
                    int returnMap = ReturnMap == 0 ? map.ForcedReturn : ReturnMap;
                    foreach (var character in map.Characters.ToList())
                    {
                        character.ChangeMap(returnMap, ReturnPortal);
                    }
                }
                if (CleanupMobs)
                {
                    map.KillAllMobs(0);
                }
                if (CleanupDrops)
                {
                    map.DropPool.Clear();
                }
                if (CleanupEffectObjects)
                {
                    map.EffectObjects.Clear();
                }
                if (ResetReactors)
                {
                    map.ReactorPool.Reset(false);
                }
                if (CleanupNpcs != null)
                {
                    foreach (var npcLife in CleanupNpcs)
                    {
                        map.RemoveNpcLife(npcLife);
                    }
                }
            }
            Started = false;
            UserCount = 0;
            Party = null;
            OnEnd?.Invoke(this, null);
            OnEnd?.ClearInvocations();
            OnEnd = null;
            ResetOnTimerUpdate();
            _savedVars.Clear();
            Program.MainForm.LogAppend("Ended fieldset '{0}'", Name);
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

        public virtual EnterStatus EnterCheck(GameCharacter chr, int mapIdx, out IList<GameCharacter> partyMembers)
        {
            partyMembers = new List<GameCharacter>();
            if (mapIdx < 0 || mapIdx > Maps.Length - 1) return EnterStatus.Invalid;
            if ((Capacity > 0 && UserCount >= Capacity) || Started) return EnterStatus.Full;

            bool checkPt = (EnterAsParty || !string.IsNullOrWhiteSpace(PartyParams));
            if (checkPt)
            {
                if (!PartyData.Parties.TryGetValue(chr.PartyID, out PartyData pt)) return EnterStatus.NotInParty;
                if (pt.Leader != chr.ID) return EnterStatus.Invalid;
                this.Party = pt;
                partyMembers = pt.Characters.Where(c => c.Field.ID == chr.Field.ID).ToList(); // needs to be in same map on enter
                Leader = chr;

                if (!string.IsNullOrWhiteSpace(PartyParams))
                {
                    int[] ptparams = PartyParams.Split(',').Select(s => int.Parse(s)).ToArray();
                    int minrequired = Math.Max(pt.MemberCount, ptparams[0]);
                    if (partyMembers.Count < minrequired || partyMembers.Count > ptparams[1])
                    {
                        return EnterStatus.WrongMemberCount;
                    }
                    if (partyMembers.Any(m => m.Level < ptparams[2] || m.Level > ptparams[3]))
                    {
                        return EnterStatus.TooWeak;
                    }
                }
            }
            if (ReqQuest > 0)
            {
                bool weak = checkPt ? partyMembers.Any(m => !m.Quests.HasQuest(ReqQuest)) : !chr.Quests.HasQuest(ReqQuest);
                if (weak)
                    return EnterStatus.TooWeak;
            }
            return EnterStatus.Success;
        }

        public EnterStatus Enter(GameCharacter chr, int mapIdx, string portalName = null, bool start = true)
        {
            var status = EnterCheck(chr, mapIdx, out IList<GameCharacter> partyMembers);
            if (status != EnterStatus.Success) return status;
            FinishEnter(chr, mapIdx, partyMembers, portalName, start);
            return status;
        }

        protected void FinishEnter(GameCharacter chr, int mapIdx, IList<GameCharacter> partyMembers, string portalName = null, bool start = true)
        {
            if (start)
            {
                Start();
            }
            var mapId = Maps[mapIdx].ID;
            if (EnterAsParty)
            { // Move caller last as npc script terminates on move map
                partyMembers.Where(c => c.ID != chr.ID).ForEach(m =>
                {
                    m.ChangeMap(mapId, portalName);
                    UserCount++;
                });
            }
            chr.ChangeMap(mapId, portalName);
            if (!chr.IsAdmin) UserCount++;
        }

        public static EnterStatus Enter(string name, GameCharacter chr, int mapIdx)
        {
            if (!Instances.TryGetValue(name, out var fs)) return EnterStatus.Invalid;
            return fs.Enter(chr, mapIdx);
        }

        public void TryEndingIt(long currentTime)
        {
            if (!Started) return;

            var timesUp = Timeout > 0 && EndTime < currentTime;
            var noMorePlayers = Maps.Sum(x => x.Characters.Count) == 0;
            OnTimerUpdate?.Invoke(this, TimeRemaining);

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
            if (Started && Timeout > 0)
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
