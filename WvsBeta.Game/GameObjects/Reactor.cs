using reNX.NXProperties;
using System;
using System.Collections.Generic;
using System.Drawing;
using WvsBeta.Common;
using WvsBeta.Common.WzObjects;
using WvsBeta.Game.Scripting;
using static WvsBeta.MasterThread;
using WvsBeta.Common.Extensions;
using System.Text.RegularExpressions;
using static WvsBeta.Common.Strings;
using log4net;
using System.Collections;
using System.Linq;

namespace WvsBeta.Game
{
    public enum ReactorEventType
    {
        Hit = 0,
        Drop = 100
    }
    public class ReactorEvent
    {
        public ReactorEventType Type { get; }
        public Rectangle? Rectangle { get; }
        public int DropID { get; }
        public short DropAmount { get; }
        public ReactorEvent(NXNode eNode)
        {
            if (eNode.ContainsChild("lt"))
            {
                var lt = eNode["lt"].ValueOrDie<Point>();
                var rb = eNode["rb"].ValueOrDie<Point>();
                Rectangle = new Rectangle(lt.X, lt.Y, rb.X - lt.X, rb.Y - lt.Y);
            }
            foreach (var subNode in eNode)
            {
                if (int.TryParse(subNode.Name, out int n))
                {
                    if (n == 0)
                    {
                        DropID = subNode.ValueInt32();
                    }
                    else if (n == 1)
                    {
                        DropAmount = subNode.ValueInt16();
                    }
                    continue;
                }
                switch (subNode.Name)
                {
                    case "type":
                        Type = (ReactorEventType)subNode.ValueByte();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public class FieldReactor
    {
        public int ID { get; }
        public Map Field { get; }
        public Reactor Reactor { get; }
        private byte _state;
        public ReactorState State => Reactor.States[_state];

        public Rectangle? EventRectangle { get; private set; }
        private Pos _position;
        public Pos Position
        {
            get => _position;
            set
            {
                _position = value;
                SetEventRectangle();
            }
        }
        public short FrameDelay { get; }
        public bool FacesLeft { get; }
        public int ReactorTime { get; }
        public string Name { get; }
        public GameCharacter Owner { get; private set; }
        public bool Shown { get; private set; } = true;
        public Mob OwnerMob { get; private set; }
        public FieldReactor(byte id, Map map, Reactor reactor, byte state, short x, short y, bool facesLeft)
        {
            ID = id;
            Field = map;
            Reactor = reactor;
            Position = new Pos(x, y);
            FacesLeft = facesLeft;
            _state = state;
            ReactorTime = -1;
            SetState(_state);
        }
        public FieldReactor(Map map, NXNode node)
        {
            ID = (int)Utils.ConvertNameToID(node.Name);
            Field = map;
            int rID = node["id"].ValueInt32();
            Reactor = GameDataProvider.Reactors[rID];
            Position = new Pos(node["x"].ValueInt16(), node["y"].ValueInt16());
            FacesLeft = node["f"].ValueBool();
            ReactorTime = node["reactorTime"].ValueInt32();
            SetState(_state);
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "name":
                        Name = subNode.ValueString();
                        break;
                    default:
                        break;
                }
            }
        }
        private void SetState(byte state)
        {
            _state = (byte)(state % Reactor.States.Count);
            SetEventRectangle();
        }
        private void SetEventRectangle()
        {
            if (State.Event?.Rectangle != null)
            {
                var rect = State.Event.Rectangle.Value;
                rect.Offset(Position);
                EventRectangle = rect;
            }
        }
        public void ChangeState(GameCharacter chr, byte state, bool sendPacket = true, short delay = 0)
        {
            Owner = chr;
            SetState(state);
            bool isLast = state == Reactor.States.Count - 1;
            if (sendPacket)
            {
                if (isLast) ReactorPacket.DestroyReactor(this);
                else ReactorPacket.ReactorChangedState(this);
            }
            QueueStateChange(Guid.NewGuid(), state, delay);
        }

        class StateChange
        {
            public Guid Guid { get; }
            public short Delay { get; }
            public RepeatingAction RepeatingAction { get; set; }
            public StateChange(Guid guid, short delay)
            {
                Guid = guid;
                Delay = delay;
            }
        }
        private void QueueStateChange(Guid guid, byte state, short delay)
        {
            bool isLast = state == Reactor.States.Count - 1;
            var stateChange = new StateChange(guid, delay);
            stateChanges.Add(stateChange.Guid, stateChange);

            short totalDelay = (short)stateChanges.Values.Sum(i => i.Delay);
            stateChange.RepeatingAction = RepeatingAction.Start(() =>
            {
                stateChanges.Remove(guid);
                if (isLast) DestroyReactor();
                else if (Reactor.States.Count == 1) RunAction();
            }, totalDelay, 0);
        }

        private void DestroyReactor()
        {
            Program.MainForm.LogDebug("Destroyed reactor " + ID + " (" + Reactor.ID + ") at " + Position.ToString() + " in map " + Field.ID + ". " + (ReactorTime > 0 ? "Respawn in " + ReactorTime + " seconds" : ""));
            RunAction();

            if (ReactorTime > 0)
            {
                Shown = false;
                resetAction = RepeatingAction.Start(() =>
                {
                    Program.MainForm.LogDebug("Reactor " + ID + " (" + Reactor.ID + ") respawned at " + Position.ToString() + " in map " + Field.ID);
                    Reset();
                }, ReactorTime * 1000, 0);
            }
        }

        public void Show(GameCharacter toChar = null)
        {
            if (!Shown) return;
            ReactorPacket.ShowReactor(this, toChar);
        }

        public void Reset(bool sendPacket = true)
        {
            dropAction?.Stop();
            resetAction?.Stop();
            stateChanges.Values.ForEach(q => q.RepeatingAction.Stop());
            stateChanges.Clear();
            _state = 0;
            Owner = null;
            OwnerMob = null;
            if (Shown)
            {
                if (sendPacket) ReactorPacket.ReactorChangedState(this);
            }
            else
            {
                Shown = true;
                if (sendPacket) Show();
            }
        }
        private MasterThread.RepeatingAction dropAction;
        private MasterThread.RepeatingAction resetAction;
        private readonly IDictionary<Guid, StateChange> stateChanges = new Dictionary<Guid, StateChange>();
        private void RunAction()
        {
            RunFieldSetActions();
            if (Reactor.Action != null)
            {
                if (GameDataProvider.ReactorActions.TryGetValue(Reactor.Action, out var rAction))
                {
                    rAction.Actions.ForEach(a => a.RunAction(this));
                }
                else
                {
                    RunScript();
                }
            }
        }
        private void RunFieldSetActions()
        {
            var fs = Field.FieldSet;
            if (fs != null && Name != null && fs.Data.Actions.TryGetValue(Name, out var fsMapActions) && fsMapActions.TryGetValue(Field.ID, out var fsActions))
            {
                if (!fs.Started) fs.Start(Owner);
                fsActions.ForEach(v => v.RunAction(fs));
            }
        }
        public void RunScript(string scriptName = null)
        {
            ReactorScriptSession.Run(this, script =>
            {
#if !DEBUG
                if (Owner?.IsGM == false) return;
#endif
                Owner.Notice("Error compiling script: " + script);
            }, scriptName);
        }
        public void Trigger(GameCharacter owner = null, bool sendPacket = true, short delay = 0)
        {
            ChangeState(owner, (byte)(_state + 1), sendPacket, delay);
        }
        public NpcLife SpawnNpc(int npcID, Pos pos)
        {
            if (pos == null) pos = Position;
            var fh = Field.GetFootholdUnderneath(pos.X, pos.Y, out int _);
            var npcLives = new List<NpcLife>();
            NpcLife npcLife = Field.SpawnTempNpc(npcID, pos, fh);
            return npcLife;
        }
        public IList<Mob> SpawnMob(Pos pos, int mobID, short amount, SummonType summonType, byte mobMeta)
        {
            if (pos == null) pos = Position;
            var spawned = new List<Mob>();
            var fh = Field.GetFootholdUnderneath(pos.X, pos.Y, out int _);
            for (int i = 0; i < amount; i++)
            {
                Field.SpawnMobWithoutRespawning(mobID, pos, (short)(fh?.ID ?? 0), OwnerMob, summonType, 0, false, out Mob outMob);
                spawned.Add(outMob);
                if (mobMeta == 2 && OwnerMob == null) OwnerMob = outMob;
            }
            return spawned;
        }
        public void Drop()
        {
            var rewards = Reward.GetRewards(Owner, Field, Reactor.ID, 'r', false, 1);
            rewards.Shuffle();

            int x2 = Position.X - 10 * rewards.Count + 10;
            short delay = 0;
            foreach (var reward in rewards)
            {
                Field.DropPool.Create(reward, Owner.ID, Owner.PartyID, DropType.Normal, 0, Position, x2, delay, false, false);
                x2 += 20;
                delay += 120;
            }
        }
        public void Warp()
        {
            var maps = Reactor.InfoMaps;
            if (maps.Count == 0 || Owner == null) return;
            else
            {
                int idx = Rand32.NextBetween(0, Reactor.InfoMaps.Count - 1);
                Map map = maps[idx];
                Owner.ChangeMap(map);
            }
        }
    }
    public class ReactorState
    {
        public sbyte State { get; }
        public ReactorEvent Event { get; }
        public WzAnimation Hit { get; }
        public ReactorState(NXNode node)
        {
            State = sbyte.Parse(node.Name);
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "event":
                        Event = new ReactorEvent(subNode["0"]);
                        break;
                    case "hit":
                        Hit = new WzAnimation(subNode);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public class Reactor
    {
        public int ID { get; }
        public string Info { get; }
        public IList<Map> InfoMaps { get; } = new List<Map>();
        public string Action { get; }
        public int Link { get; }
        public IDictionary<byte, ReactorState> States = new Dictionary<byte, ReactorState>();

        public Reactor(NXNode rNode)
        {
            ID = (int)Utils.ConvertNameToID(rNode.Name);

            foreach (var subNode in rNode)
            {
                if (byte.TryParse(subNode.Name, out byte n))
                {
                    States.Add(n, new ReactorState(subNode));
                    continue;
                }
                switch (subNode.Name)
                {
                    case "info":
                        {
                            Info = subNode["info"].ValueString();
                            if (subNode.ContainsChild("link"))
                            {
                                int link = subNode["link"].ValueInt32();
                                Link = link;
                            }
                            break;
                        }
                    case "action":
                        Action = subNode.ValueString();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
