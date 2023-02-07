using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common;
using WvsBeta.Game.GameObjects;

namespace WvsBeta.Game
{
    public class FieldSet
    {
        public static Dictionary<string, FieldSet> Instances { get; } = new Dictionary<string, FieldSet>();
        private readonly Dictionary<string, string> _savedVars = new Dictionary<string, string>();
        public string Name { get; private set; }
        public Map[] Maps { get; private set; }
        public int Timeout { get; private set; }

        public bool Started { get; private set; }
        public long EndTime { get; private set; }
        public long TimeRemaining => EndTime - MasterThread.CurrentTime;

        public int ReturnMap { get; private set; } = -1;
        public string PartyParams { get; private set; }
        public bool EnterAsParty { get; private set; }
        public PartyData Party { get; private set; }
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
                    var map = DataProvider.Maps[subNode.GetInt()];
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
                        case "enterAsParty":
                            EnterAsParty = subNode.GetBool();
                            break;
                        case "ptParams":
                            PartyParams = subNode.GetString();
                            break;
                    }
            }

            Maps = maps;
            Program.MainForm.LogAppend("Loaded fieldset '{0}' with maps {1}", Name, string.Join(", ", Maps.Select(x => x.ID)));
        }

        public void Start()
        {
            Started = true;
            EndTime = MasterThread.CurrentTime + (Timeout * 1000);

            foreach (var map in Maps)
            {
                map.Reset(false); // bool Should be option from config
                map.OnEnter = RunTimer;
            }
            Program.MainForm.LogAppend("Started fieldset '{0}'", Name);
        }

        public void End()
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
                        character.ChangeMap(returnMap);
                    }
                }
            }
            Started = false;
            Party = null;
            _savedVars.Clear();
            Program.MainForm.LogAppend("Ended fieldset '{0}'", Name);
        }

        public enum EnterStatus
        {
            Invalid = -1,
            Success = 0,
            NotInParty = 1,
            WrongMemberCount = 2,
            WrongMemberLevel = 3,
            Full = 4
        }
        public EnterStatus Enter(GameCharacter chr, int mapIdx)
        {
            if (Started) return EnterStatus.Full;
            bool ptstuff = (EnterAsParty || !string.IsNullOrWhiteSpace(PartyParams));
            IList<GameCharacter> members = new List<GameCharacter>();
            if (ptstuff)
            {
                if (!PartyData.Parties.TryGetValue(chr.PartyID, out PartyData pt)) return EnterStatus.NotInParty;
                this.Party = pt;
                members = pt.Characters.Where(c => c.Field.ID == chr.Field.ID).ToList(); // needs to be in same map on enter

                if (!string.IsNullOrWhiteSpace(PartyParams))
                {
                    int[] ptparams = PartyParams.Split(',').Select(s => int.Parse(s)).ToArray();
                    if (members.Count < ptparams[0] || members.Count > ptparams[1])
                    {
                        return EnterStatus.WrongMemberCount;
                    }
                    if (members.Any(m => m.Level < ptparams[2] || m.Level > ptparams[3]))
                    {
                        return EnterStatus.WrongMemberLevel;
                    }
                }
            }
            Start();
            var mapId = Maps[mapIdx].ID;
            if (EnterAsParty)
            { // Move caller last as npc script terminates on move map
                members.Where(c => c.ID != chr.ID).ForEach(m => m.ChangeMap(mapId));
            }
            chr.ChangeMap(mapId);
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

            var timesUp = Timeout > 0 && EndTime < currentTime;
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

        private void RunTimer(GameCharacter chr, Map map)
        {
            if (Started)
                MapPacket.ShowMapTimerForCharacter(chr, (int)(TimeRemaining / 1000));
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
    }
}
