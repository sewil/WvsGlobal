using reNX.NXProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WvsBeta.Common;

namespace WvsBeta.Game.GameObjects.DataLoading
{
    public class FieldSetData
    {
        public string Name { get; }
        public int MinMembers { get; set; }
        public int MaxMembers { get; set; }
        public short MinLevel { get; }
        public short MaxLevel { get; }
        public short Quest { get; }
        public bool Party { get; }
        public bool Guild { get; }
        public Map[] Maps { get; }
        public IDictionary<Map, bool> Unaffected { get; }
        public int TimeOut { get; }
        public int OpenDelay { get; }
        /// <summary>
        /// Reactor name as key, containing a mapid as key with field set actions
        /// Fast lookup, supports duplicate reactor names for different reactors in different maps
        /// </summary>
        public IDictionary<string, IDictionary<int, IList<FieldSetActionData>>> Actions { get; }
        public IList<FieldSetEventData> Events { get; }
        public bool Shuffle { get; }
        public string Script { get; set; }
        public FieldSetData(NXNode node)
        {
            Name = node.Name;
            var maps = new Dictionary<int, Map>();
            Actions = new Dictionary<string, IDictionary<int, IList<FieldSetActionData>>>();
            Events = new List<FieldSetEventData>();
            Unaffected = new Dictionary<Map, bool>();
            var unaffected = new Dictionary<int, bool>();
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "timeOut":
                        TimeOut = subNode.ValueInt32();
                        break;
                    case "action":
                        {
                            foreach (var actionsNode in subNode)
                            {
                                int mapIdx = int.Parse(actionsNode.Name);
                                int mapID = maps[mapIdx].ID;
                                bool reactorSecondSet = false;
                                foreach (var actionNode in actionsNode)
                                {
                                    var action = new FieldSetActionData(actionNode, reactorSecondSet);
                                    reactorSecondSet = action.ReactorFirstSet && !action.ReactorSecondSet;

                                    if (!Actions.ContainsKey(action.Name))
                                        Actions.Add(action.Name, new Dictionary<int, IList<FieldSetActionData>>());
                                    if (!Actions[action.Name].ContainsKey(mapID))
                                        Actions[action.Name].Add(mapID, new List<FieldSetActionData>());
                                    Actions[action.Name][mapID].Add(action);
                                }
                            }
                        }
                        break;
                    case "event":
                        foreach (var eventNode in subNode)
                        {
                            Events.Add(new FieldSetEventData(eventNode));
                        }
                        break;
                    case "shuffle":
                        Shuffle = subNode.ValueBool();
                        break;
                    case "script":
                        Script = subNode.ValueString();
                        break;
                    case "members":
                        (MinMembers, MaxMembers) = Tools.ParseRange(subNode.ValueString());
                        break;
                    case "level":
                        var (min, max) = Tools.ParseRange(subNode.ValueString());
                        MinLevel = (short)min;
                        MaxLevel = (short)max;
                        break;
                    case "quest":
                        Quest = subNode.ValueInt16();
                        break;
                    case "unaffected":
                        {
                            foreach (var uNode in subNode)
                            {
                                int mapIdx = int.Parse(uNode.Name);
                                bool isUnaffected = uNode.ValueBool();
                                unaffected.Add(mapIdx, isUnaffected);
                            }
                        }
                        break;
                    case "startDelay":
                        OpenDelay = subNode.ValueInt32();
                        break;
                    case "guild":
                        Guild = subNode.ValueBool();
                        break;
                    case "party":
                        Party = subNode.ValueBool();
                        break;
                    default:
                        {
                            if (int.TryParse(subNode.Name, out int mapIdx))
                            {
                                int mapID = subNode.ValueInt32();
                                if (!GameDataProvider.Maps.TryGetValue(mapID, out Map map)) throw new ControlledException("Map " + mapID + " not found in FieldSet " + Name);
                                maps[mapIdx] = map;
                            }
                        }
                        break;
                }
            }
            Maps = new Map[maps.Count];
            maps.ForEach(m => Maps[m.Key] = m.Value);
            Unaffected = unaffected.ToDictionary(i => Maps[i.Key], i => i.Value);
        }
    }
    public enum FieldSetActionType : byte
    {
        SetReactorState = 3,
        SetVar = 4,
        FieldMusic = 5,
        PartyClear = 7
    }
    public class FieldSetActionData
    {
        public string Name { get; }
        public bool ReactorFirstSet { get; private set; }
        public bool ReactorSecondSet { get; }
        private readonly Action<FieldSet> action;
        public FieldSetActionData(NXNode node, bool reactorSecondSet)
        {
            ReactorSecondSet = reactorSecondSet;
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "info":
                        var type = (FieldSetActionType)subNode["type"].ValueByte();
                        var args = new List<string>();
                        foreach (var argNode in subNode)
                        {
                            if (!int.TryParse(argNode.Name, out int _)) continue;
                            args.Add(argNode.ValueString());
                        }
                        action = ParseAction(type, args);
                        break;
                    default:
                        Name = subNode.Name;
                        break;
                }
            }
        }
        public void RunAction(FieldSet fieldSet)
        {
            action?.Invoke(fieldSet);
        }
        private Action<FieldSet> ParseAction(FieldSetActionType type, IList<string> args)
        {
            Action<FieldSet> action = null;
            switch (type)
            {
                case FieldSetActionType.SetVar:
                    action += fs => fs.SetVar(args[0], args[1]);
                    break;
                case FieldSetActionType.FieldMusic:
                    {
                        int mapIdx = int.Parse(args[0]);
                        string music = args[1];
                        action += fs => fs.Maps[mapIdx].EffectMusic(music);
                    }
                    break;
                case FieldSetActionType.SetReactorState:
                    {
                        int mapIdx = int.Parse(args[0]);
                        string reactorName = args[1];
                        sbyte newState = sbyte.Parse(args[2]);
                        ReactorFirstSet = newState >= 0 && !ReactorSecondSet;
                        action += fs =>
                        {
                            if (ReactorSecondSet)
                            {
                                fs.OnEnd.SubscribeOnce(fieldSet => SetReactorState(fieldSet, mapIdx, reactorName, newState));
                            }
                            else
                            {
                                SetReactorState(fs, mapIdx, reactorName, newState);
                            }
                        };
                    }
                    break;
                case FieldSetActionType.PartyClear:
                    {
                        int something = int.Parse(args[0]);
                        int exp = int.Parse(args[1]);
                        var mapIdxs = new int[args.Count - 2];
                        for (int i = 0; i < mapIdxs.Length; i++)
                        {
                            int mapIdx = int.Parse(args[i+2]);
                            mapIdxs[i] = mapIdx;
                        }
                        action += fs => fs.PartyClear(mapIdxs, exp);
                    }
                    break;
            }
            return action;
        }
        private void SetReactorState(FieldSet fs, int mapIdx, string reactorName, sbyte newState)
        {
            var reactorPool = fs.Maps[mapIdx].ReactorPool;
            var reactor = reactorPool.Reactors.Values.FirstOrDefault(i => i.Name == reactorName);
            if (newState == -1) reactor?.Trigger();
            else reactor?.ChangeState(null, (byte)newState);
        }
    }
    public enum FieldSetEventActionType : byte
    {
        SetTimer = 1,
        SetPortalEnabled = 2,
        BroadcastAll = 3,
        EndFieldSet = 4
    }
    public class FieldSetEventData
    {
        public int TimeAfter { get; }
        private readonly Action<FieldSet> action;
        public FieldSetEventData(NXNode node)
        {
            var args = new List<string>();
            FieldSetEventActionType? type = null;
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "timeAfter":
                        TimeAfter = subNode.ValueInt32();
                        break;
                    case "action":
                        type = (FieldSetEventActionType)subNode.ValueByte();
                        break;
                    default:
                        if (int.TryParse(subNode.Name, out var _))
                        {
                            args.Add(subNode.ValueString());
                        }
                        break;
                }
            }
            if (type.HasValue)
            {
                action = ParseAction(type.Value, args);
            }
        }
        private Action<FieldSet> ParseAction(FieldSetEventActionType type, IList<string> args)
        {
            switch (type)
            {
                case FieldSetEventActionType.SetTimer:
                    int timeOut = int.Parse(args[0]);
                    return fs =>
                    {
                        fs.SetTimer(timeOut);
                    };
                case FieldSetEventActionType.SetPortalEnabled:
                    int mapIdx = int.Parse(args[0]);
                    string portalName = args[1];
                    bool isEnabled = args[2] == "1";
                    return fs =>
                    {
                        var map = fs.Maps[mapIdx];
                        map.Portals[portalName].Enabled = isEnabled;
                    };
                case FieldSetEventActionType.BroadcastAll:
                    {
                        int msgType = int.Parse(args[0]);
                        string message = args[1];
                        int npcID = 0;
                        if (msgType == 7)
                        {
                            npcID = int.Parse(args[2]);
                        }
                        return fs =>
                        {
                            foreach (var c in fs.Characters)
                            {
                                if (msgType == 7)
                                {
                                    NpcPacket.SendNPCChatTextSimple(c, npcID, message, false, false);
                                }
                                else
                                {
                                    ChatPacket.SendBroadcastMessageToPlayer(c, message, (BroadcastMessageType)msgType);
                                }
                            }
                        };
                    }
                case FieldSetEventActionType.EndFieldSet:
                    {
                        return fs => fs.End();
                    }
                default: return null;
            }
        }
        public void RunAction(FieldSet fs)
        {
            action?.Invoke(fs);
        }
    }
}
