using reNX.NXProperties;
using System;
using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Common.Enums;

namespace WvsBeta.Game.GameObjects.DataLoading
{
    public class ReactorActionData
    {
        public string Name { get; }
        public IList<ReactorActionDataAction> Actions;
        public ReactorActionData(NXNode node)
        {
            Name = node.Name;
            Actions = new List<ReactorActionDataAction>();
            foreach (var subNode in node)
            {
                Actions.Add(new ReactorActionDataAction(subNode));
            }
        }
    }
    public enum ReactorActionType : byte
    {
        GoMap = 0,
        SpawnMob = 1,
        Drop = 2,
        SpawnNPC = 6,
        RunScript = 10
    }
    public class ReactorActionDataAction
    {
        private readonly Action<FieldReactor> action;
        public ReactorActionDataAction(NXNode node)
        {
            ReactorActionType type = 0;
            var args = new List<string>();
            string message = null;
            foreach (var subNode in node)
            {
                if (subNode.Name == "message")
                {
                    message = subNode.ValueString();
                }
                else if (subNode.Name == "type")
                {
                    type = (ReactorActionType)subNode.ValueByte();
                }
                else if (int.TryParse(subNode.Name, out var _))
                {
                    args.Add(subNode.ValueString());
                }
            }
            action = ParseAction(type, message, args);
        }
        public Action<FieldReactor> ParseAction(ReactorActionType type, string message, IList<string> args)
        {
            Action<FieldReactor> action = null;
            bool messageField = true;
            switch (type)
            {
                case ReactorActionType.GoMap:
                    {
                        bool moveAll = bool.Parse(args[0]);
                        if (!moveAll) messageField = false;
                        var mapIDs = new List<(int, string)>();
                        for (int i = 1; i + 1 < args.Count; i += 2)
                        {
                            int mapID = int.Parse(args[i]);
                            string portalName = args[i + 1];
                            mapIDs.Add((mapID, portalName));
                        }
                        action += reactor =>
                        {
                            int idx = Rand32.NextBetween(0, mapIDs.Count);
                            var (mapID, portalName) = mapIDs[idx];
                            var toMove = new List<GameCharacter>();
                            if (moveAll) toMove = reactor.Field.Characters;
                            else toMove.Add(reactor.Owner);
                            toMove.ForEach(c => c?.ChangeMap(mapID, portalName));
                        };
                    }
                    break;
                case ReactorActionType.SpawnMob:
                    {
                        int mobID = int.Parse(args[0]);
                        var summonType = (SummonType)sbyte.Parse(args[1]);
                        short amount = short.Parse(args[2]);
                        Pos pos = null;
                        byte mobMeta = 0;
                        if (args.Count >= 4) mobMeta = byte.Parse(args[3]);
                        if (args.Count >= 6)
                        {
                            short x = short.Parse(args[4]);
                            short y = short.Parse(args[5]);
                            pos = new Pos(x, y);
                        }
                        action += (r) => r.SpawnMob(pos, mobID, amount, summonType, mobMeta);
                    }
                    break;
                case ReactorActionType.Drop:
                    {
                        action += r => r.Drop();
                        break;
                    }
                case ReactorActionType.SpawnNPC:
                    {
                        int npcID = int.Parse(args[0]);
                        short x = short.Parse(args[1]);
                        short y = short.Parse(args[2]);
                        var pos = new Pos(x, y);
                        action += r => r.SpawnNpc(npcID, pos);
                    }
                    break;
                case ReactorActionType.RunScript:
                    {
                        string scriptName = args[0];
                        action += r => r.RunScript(scriptName);
                    }
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrWhiteSpace(message))
            {
                action += reactor =>
                {
                    if (messageField) reactor.Field.Message(message);
                    else reactor.Owner?.Message(message);
                };
            }
            return action;
        }
        public void RunAction(FieldReactor reactor)
        {
            action?.Invoke(reactor);
        }
    }
}
