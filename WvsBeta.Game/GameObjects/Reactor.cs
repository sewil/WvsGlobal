using reNX.NXProperties;
using System;
using System.Collections.Generic;
using System.Drawing;
using WvsBeta.Common;
using WvsBeta.Common.Objects;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Game
{
    public enum ReactorEventType
    {
        Hit = 0,
        Drop = 100
    }
    public class ReactorEvent
    {
        public ReactorEventType Type { get; private set; }
        public byte State { get; private set; }
        public Rectangle? Rectangle { get; private set; }
        public int DropID { get; private set; }
        public short DropAmount { get; private set; }
        public ReactorEvent(Reactor reactor, NXNode eNode)
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
                    case "state":
                        State = subNode.ValueByte();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public class FieldReactor
    {
        public readonly int ID;
        public readonly Map Field;
        public readonly Reactor Reactor;
        private byte _state;
        public ReactorState State => Reactor.States[_state];

        public Rectangle? EventRectangle { get; private set; }
        public readonly Pos Position;
        public readonly short FrameDelay;
        public readonly bool FacesLeft;
        public readonly int ReactorTime;
        public readonly string Name;
        public GameCharacter Owner { get; private set; }
        public FieldReactor ShallowCopy => (FieldReactor)MemberwiseClone();
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
            Reactor = DataProvider.Reactors[rID];
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
            if (!Reactor.States.ContainsKey(state)) return;
            _state = state;
            if (State.Event?.Rectangle != null)
            {
                var rect = State.Event.Rectangle.Value;
                rect.Offset(Position);
                EventRectangle = rect;
            }
        }
        public void Show(GameCharacter toChar = null)
        {
            ReactorPacket.ShowReactor(this, toChar);
        }

        public void Reset(bool sendPacket = true)
        {
            destroyAction?.Stop();
            dropAction?.Stop();
            respawnAction?.Stop();
            _state = 0;
            pendingDestroy = false;
            if (sendPacket)
                ReactorPacket.ReactorChangedState(this);
        }
        private bool pendingDestroy;
        private MasterThread.RepeatingAction destroyAction;
        private MasterThread.RepeatingAction dropAction;
        private MasterThread.RepeatingAction respawnAction;
        public void Destroy()
        {
            pendingDestroy = true;
            if (State.Delay > 0)
            {
                ReactorPacket.ReactorChangedState(this);
            }
            destroyAction = MasterThread.RepeatingAction.Start("rdstrp-" + Field.ID + "-" + ID, () => {
                ReactorPacket.DestroyReactor(this);
                if (ReactorTime > -1)
                {
                    respawnAction = MasterThread.RepeatingAction.Start("rrts-" + Field.ID + "-" + ID, time => Field.ReactorPool.Show(ID), ReactorTime * 1000, 0);
                }
                Field.ReactorPool.ShownReactors.Remove(ID);
            }, State.Delay * 1000, 0);
            if (Reactor.Action != null)
            {
                ReactorScriptSession.Run(this, script => {
#if !DEBUG
                    if (Owner != null && Owner.IsGM)
                    {
#endif
                        ChatPacket.SendNoticeMap("Error compiling script: " + script, Field.ID);
#if !DEBUG
                    }
#endif
                });
            }
        }
        public void Trigger(GameCharacter chr)
        {
            if (pendingDestroy) return;
            Owner = chr;
            bool hasAction = Reactor.Action != null;
            byte newState = (byte)((State.Event != null ? State.Event.State : _state + 1) % Reactor.States.Count);
            SetState(newState);

            bool isLast = newState >= Reactor.States.Count - 1;
            if (isLast && hasAction)
            {
                Destroy();
            }
            else
            {
                ReactorPacket.ReactorChangedState(this);
            }
        }
        public bool TriggerDrop(Drop drop, int ownerId)
        {
            var e = State.Event;
            bool trigger = e?.Type == ReactorEventType.Drop && e?.DropID == drop.Reward.ItemID && e.DropAmount == drop.Reward.Amount && EventRectangle?.Contains(drop.Pt2) == true;
            if (trigger)
            {
                if (!Server.Instance.CharacterList.TryGetValue(ownerId, out GameCharacter owner)) return false;
                dropAction = MasterThread.RepeatingAction.Start("rdtr-" + Field.ID + "-" + ID, time =>
                {
                    if (Field.DropPool.Drops.ContainsKey(drop.DropID)) // Check drop still exists
                    {
                        ReactorPacket.ReactorChangedState(this);
                        Field.DropPool.RemoveDrop(drop);
                        Trigger(owner);
                    }
                }, 5000, 0);
            }
            return trigger;
        }
        public IList<Mob> Spawn(short yOffset, params (int mobid, short amount, sbyte summonType, Mob ownerMob)[] mobs)
        {
            var pos = new Pos(Position);
            pos.Offset(0, yOffset);
            var spawned = new List<Mob>();
            var fh = Field.GetFootholdUnderneath(pos.X, pos.Y, out int maxY);
            foreach (var mob in mobs)
            {
                for (int i = 0; i < mob.amount; i++)
                {
                    Field.SpawnMobWithoutRespawning(mob.mobid, pos, (short)(fh?.ID ?? 0), mob.ownerMob, mob.summonType, 0, false, out Mob outMob);
                    spawned.Add(outMob);
                }
            }
            return spawned;
        }
        public void Drop(int mesos, params (int itemId, short amount)[] items)
        {
            int x2 = Position.X - 10 * (items.Length + mesos > 0 ? 1 : 0) + 10;
            short delay = 0;
            foreach (var dropInfo in items)
            {
                BaseItem it = BaseItem.CreateFromItemID(dropInfo.itemId, dropInfo.amount);
                it.GiveStats(ItemVariation.None);

                Field.DropPool.Create(Reward.Create(it), Owner.ID, Owner.PartyID, DropType.Normal, ID, Position, x2, delay, false, 0, false, false);
                x2 += 20;
                delay += 120;
            }

            if (mesos > 0)
            {
                Field.DropPool.Create(Reward.Create(mesos), Owner.ID, Owner.PartyID, DropType.Normal, ID, Position, x2, delay, false, 0, false, false);
            }
        }
    }
    public class ReactorState
    {
        public byte State { get; private set; }
        public Point? Origin { get; private set; }
        public short Z { get; private set; }
        public ReactorEvent Event { get; private set; }
        public int Delay { get; private set; }
        public ReactorState(Reactor reactor, NXNode node)
        {
            State = byte.Parse(node.Name);
            foreach (var subNode in node)
            {
                if (int.TryParse(subNode.Name, out int n))
                {
                    foreach (var ssNode in subNode)
                    {
                        switch (ssNode.Name)
                        {
                            case "origin":
                                Origin = ssNode.ValueOrDie<Point>();
                                break;
                            case "z":
                                Z = ssNode.ValueInt16();
                                break;
                            case "delay":
                                Delay = ssNode.ValueInt32();
                                break;
                            default:
                                break;
                        }
                    }
                    continue;
                }
                switch (subNode.Name)
                {
                    case "event":
                        Event = new ReactorEvent(reactor, subNode["0"]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public class Reactor
    {
        public int ID { get; private set; }
        public string Info { get; private set; }
        public string Action { get; private set; }
        public int Link { get; private set; }
        public IDictionary<byte, ReactorState> States = new Dictionary<byte, ReactorState>();

        public Reactor(NXNode rNode)
        {
            ID = (int)Utils.ConvertNameToID(rNode.Name);

            foreach (var subNode in rNode)
            {
                if (byte.TryParse(subNode.Name, out byte n))
                {
                    States.Add(n, new ReactorState(this, subNode));
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
