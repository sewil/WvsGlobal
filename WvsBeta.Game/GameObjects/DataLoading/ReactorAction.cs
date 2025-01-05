using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WvsBeta.Common;
using WvsBeta.Common.Enums;

namespace WvsBeta.Game.GameObjects.DataLoading
{
    public interface IReactorAction
    {
        void RunAction(FieldReactor reactor);
    }
    public class DropReactorAction : IReactorAction
    {
        public void RunAction(FieldReactor reactor)
        {
            reactor.Drop();
        }
    }
    public class SpawnMobReactorAction : IReactorAction
    {
        public int MobID { get; }
        public SummonType SummonType { get; }
        public short Amount { get; }
        public Pos Pos { get; }
        public byte MobMeta { get; }

        public SpawnMobReactorAction(string[] args)
        {
            MobID = int.Parse(args[0]);
            SummonType = (SummonType)sbyte.Parse(args[1]);
            Amount = short.Parse(args[2]);
            Pos = null;
            MobMeta = 0;
            if (args.Length >= 6)
            {
                MobMeta = byte.Parse(args[3]);
                short x = short.Parse(args[args.Length - 2]);
                short y = short.Parse(args[args.Length - 1]);
                Pos = new Pos(x, y);
            }
        }
        public void RunAction(FieldReactor reactor)
        {
            reactor.SpawnMob(Pos, MobID, Amount, SummonType, MobMeta);
        }
    }
    public class GoMapReactorAction : IReactorAction
    {
        public bool OnlyOwner { get; }
        public IList<(int, string)> MapIDs { get; }
        private MoveReactorAction MoveAction { get; }
        public GoMapReactorAction(string[] args)
        {
            OnlyOwner = args[0] == "1";
            MapIDs = new List<(int, string)>();
            for (int i = 1; i + 1 < args.Length; i += 2)
            {
                int mapID = int.Parse(args[i]);
                string portalName = args[i + 1];
                MapIDs.Add((mapID, portalName));
            }
            MoveAction = new MoveReactorAction(this);
        }
        public void RunAction(FieldReactor reactor)
        {
            MoveAction.RunAction(reactor);
        }
    }
    public class MoveReactorAction : IReactorAction
    {
        public GoMapReactorAction GoMap { get; }
        public MoveReactorAction(GoMapReactorAction goMap)
        {
            GoMap = goMap;
        }
        public void RunAction(FieldReactor reactor)
        {
            int idx = Rand32.NextBetween(0, GoMap.MapIDs.Count);
            var (mapID, portalName) = GoMap.MapIDs[idx];
            var toMove = new List<GameCharacter>();
            if (GoMap.OnlyOwner) toMove.Add(reactor.Owner);
            else toMove = reactor.Field.Characters.ToList();
            toMove.ForEach(c => c?.ChangeMap(mapID, portalName));
        }
    }
    public class MessageReactorAction : IReactorAction
    {
        public string Message { get; }
        public bool OnlyOwner { get; }
        public MessageReactorAction(string message, bool onlyOwner)
        {
            Message = message;
            OnlyOwner = onlyOwner;
        }
        public void RunAction(FieldReactor reactor)
        {
            if (OnlyOwner) reactor.Owner?.Message(Message);
            else reactor.Field.Message(Message);
        }
    }
    public class SpawnNPCReactorAction : IReactorAction
    {
        public int NpcID { get; }
        public Pos Position { get; }
        public SpawnNPCReactorAction(string[] args)
        {
            NpcID = int.Parse(args[0]);
            short x = short.Parse(args[1]);
            short y = short.Parse(args[2]);
            Position = new Pos(x, y);
        }

        public void RunAction(FieldReactor reactor)
        {
            reactor.SpawnNpc(NpcID, Position);
        }
    }
    public class RunScriptReactorAction : IReactorAction
    {
        public string ScriptName { get; }
        public RunScriptReactorAction(string[] args)
        {
            ScriptName = args[0];
        }
        public void RunAction(FieldReactor reactor)
        {
            reactor.RunScript(ScriptName);
        }
    }
}
