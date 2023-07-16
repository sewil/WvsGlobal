using reNX.NXProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;

namespace WvsBeta.Game.GameObjects.DataLoading
{
    public class FieldSetData
    {
        public string Name { get; }
        public int MinPartyMembers { get; }
        public int MaxPartyMembers { get; }
        public short MinLevel { get; }
        public short MaxLevel { get; }
        public short Quest { get; }
        public Map[] Maps { get; }
        public int TimeOut { get; }
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
                                foreach (var actionNode in actionsNode)
                                {
                                    var action = new FieldSetActionData(actionNode, mapIdx);
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
                    case "minPartyMembers":
                        MinPartyMembers = subNode.ValueInt32();
                        break;
                    case "maxPartyMembers":
                        MaxPartyMembers = subNode.ValueInt32();
                        break;
                    case "minLevel":
                        MinLevel = subNode.ValueInt16();
                        break;
                    case "maxLevel":
                        MaxLevel = subNode.ValueInt16();
                        break;
                    case "quest":
                        Quest = subNode.ValueInt16();
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
        }
    }
    public enum FieldSetActionType : byte
    {
        SetReactorState = 3,
        SetVar = 4,
        FieldMusic = 5,
    }
    public class FieldSetActionData
    {
        public string Name { get; }
        private Action<FieldSet> action;
        private int mapIdx;
        public FieldSetActionData(NXNode node, int mapIdx)
        {
            this.mapIdx = mapIdx;
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
            switch (type)
            {
                case FieldSetActionType.SetVar:
                    return fs => fs.SetVar(args[0], args[1]);
                case FieldSetActionType.FieldMusic:
                    {
                        int mapIdx = int.Parse(args[0]);
                        string music = args[1];
                        return fs => fs.Maps[mapIdx].EffectMusic(music);
                    }
                case FieldSetActionType.SetReactorState:
                    {
                        int mapIdx = int.Parse(args[0]);
                        string reactorName = args[1];
                        int stateInc = int.Parse(args[2]);
                        return fs =>
                        {
                            var reactorPool = fs.Maps[mapIdx].ReactorPool;
                            var reactor = reactorPool.Reactors.Values.FirstOrDefault(i => i.Name == reactorName);
                            byte newState = (byte)((reactor.State.State + stateInc) % reactor.Reactor.States.Count);
                            reactor?.ChangeState(null, newState);
                        };
                    }
                default: return null;
            }
        }
    }
    public enum FieldSetEventActionType : byte
    {
        SetTimer = 1,
        ChangeMapAll = 2,
        BroadcastAll = 3
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
                case FieldSetEventActionType.ChangeMapAll:
                    int mapIdxFrom = int.Parse(args[0]);
                    string portalTo = args[1];
                    int mapIdxTo = int.Parse(args[2]);
                    return fs =>
                    {
                        fs.Maps[mapIdxFrom].Characters.ForEach(c => c.ChangeMap(fs.Maps[mapIdxTo].ID, portalTo));
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
                default: return null;
            }
        }
        public void RunAction(FieldSet fs)
        {
            action?.Invoke(fs);
        }
    }
}
