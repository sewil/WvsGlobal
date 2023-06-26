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
        public int[] Maps { get; }
        public int TimeOut { get; }
        public IDictionary<int, IList<FieldSetActionData>> Actions { get; }
        public IList<FieldSetEventData> Events { get; }
        public bool Shuffle { get; }
        public string Script { get; set; }
        public FieldSetData(NXNode node)
        {
            Name = node.Name;
            var maps = new List<int>();
            Actions = new Dictionary<int, IList<FieldSetActionData>>();
            Events = new List<FieldSetEventData>();
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "timeOut":
                        TimeOut = subNode.ValueInt32();
                        break;
                    case "action":
                        foreach (var actionsNode in subNode)
                        {
                            int actionMapIdx = int.Parse(actionsNode.Name);
                            var actions = new List<FieldSetActionData>();
                            foreach (var actionNode in actionsNode)
                            {
                                actions.Add(new FieldSetActionData(actionNode, actionMapIdx));
                            }
                            Actions.Add(actionMapIdx, actions);
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
                    default:
                        if (int.TryParse(subNode.Name, out int mapIdx))
                            maps[mapIdx] = subNode.ValueInt32();
                        break;
                }
            }
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
                        byte newState = byte.Parse(args[2]);
                        return fs =>
                        {
                            var reactorPool = fs.Maps[mapIdx].ReactorPool;
                            var reactor = reactorPool.Reactors.Values.FirstOrDefault(i => i.Name == reactorName);
                            reactor?.SetState(null, newState);
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
                    int mapIdxTo = int.Parse(args[1]);
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
