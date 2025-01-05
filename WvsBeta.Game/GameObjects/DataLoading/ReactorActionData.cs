using reNX.NXProperties;
using System.Collections.Generic;
using WvsBeta.Common;

namespace WvsBeta.Game.GameObjects.DataLoading
{
    public class ReactorActionData
    {
        public string Name { get; }
        public IList<ReactorActionSetData> ActionSets;
        public ReactorActionData(NXNode node)
        {
            Name = node.Name;
            ActionSets = new List<ReactorActionSetData>();
            foreach (var subNode in node)
            {
                ActionSets.Add(new ReactorActionSetData(subNode));
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
    public class ReactorActionSetData
    {
        public IList<IReactorAction> Actions { get; }
        public ReactorActionSetData(NXNode node)
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
            Actions = ParseActions(type, message, args.ToArray());
        }
        private IList<IReactorAction> ParseActions(ReactorActionType type, string message, string[] args)
        {
            IList<IReactorAction> actions = new List<IReactorAction>();
            GoMapReactorAction goMap = null;
            switch (type)
            {
                case ReactorActionType.GoMap:
                    goMap = new GoMapReactorAction(args);
                    break;
                case ReactorActionType.SpawnMob:
                    actions.Add(new SpawnMobReactorAction(args));
                    break;
                case ReactorActionType.Drop:
                    actions.Add(new DropReactorAction());
                    break;
                case ReactorActionType.SpawnNPC:
                    actions.Add(new SpawnNPCReactorAction(args));
                    break;
                case ReactorActionType.RunScript:
                    actions.Add(new RunScriptReactorAction(args));
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrWhiteSpace(message))
            {
                actions.Add(new MessageReactorAction(message, goMap?.OnlyOwner ?? false));
            }
            if (goMap != null) actions.Add(goMap);
            return actions;
        }
        public void RunAction(FieldReactor reactor)
        {
            Actions.ForEach(action => action.RunAction(reactor));
        }
    }
}
