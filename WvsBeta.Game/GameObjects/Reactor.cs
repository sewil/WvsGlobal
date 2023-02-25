using reNX.NXProperties;
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
        public Rectangle Rectangle { get; private set; }
        public int DropID { get; private set; }
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
        public void Trigger(FieldReactor reactor)
        {
            reactor.State = State;
        }
    }
    public class FieldReactor
    {
        public int ID { get; private set; }
        public Map Field { get; private set; }
        public Reactor Reactor { get; private set; }
        public byte State { get; set; }
        public ReactorState ReactorState => Reactor.States[State];
        public Pos Position { get; private set; }
        public short FrameDelay { get; set; }
        public short Z => ReactorState.Z;
        public bool FacesLeft { get; private set; }
        public readonly int ReactorTime;
        public readonly string Name;
        public GameCharacter Owner { get; private set; }
        public FieldReactor ShallowCopy()
        {
            return (FieldReactor)MemberwiseClone();
        }
        public FieldReactor(byte id, Map map, Reactor reactor, byte state, short x, short y, bool facesLeft)
        {
            ID = id;
            Field = map;
            Reactor = reactor;
            Position = new Pos(x, y);
            State = state;
            FacesLeft = facesLeft;
        }
        public FieldReactor(Map map, NXNode node)
        {
            ID = (int)Utils.ConvertNameToID(node.Name);
            Field = map;
            int rID = node["id"].ValueInt32();
            Reactor = DataProvider.Reactors[rID];
            Position = new Pos(node["x"].ValueInt16(), node["y"].ValueInt16());
            State = 0;
            FacesLeft = node["f"].ValueBool();
            ReactorTime = node["reactorTime"].ValueInt32();
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

        public void Show()
        {
            ReactorPacket.ShowReactor(this);
        }

        public void ShowTo(GameCharacter chr)
        {
            ReactorPacket.ShowReactor(this, true, chr);
        }

        public void Destroy()
        {
            ReactorPacket.DestroyReactor(this);
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

        public void HitBy(GameCharacter chr)
        {
            Owner = chr;
            ReactorState.Event?.Trigger(this);
            if (State < Reactor.States.Count - 1)
            {
                ReactorPacket.ReactorChangedState(this);
            }
            else
            {
                Destroy();
                Field.ReactorPool.ShownReactors.Remove(ID);
            }
        }
        public void DoSpawn(short yOffset, params (int mobid, short amount, sbyte summonType)[] mobs)
        {
            var pos = new Pos(Position.X, (short)(Position.Y + yOffset));
            foreach (var mob in mobs)
            {
                for (int i = 0; i < mob.amount; i++)
                {
                    Field.SpawnMob(mob.mobid, null, pos, 0, null, mob.summonType, 0);
                }
            }
        }
        public void DoDrop(int mesos, params (int itemId, short amount)[] items)
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
        public Point Origin { get; private set; }
        public short Z { get; private set; }
        public ReactorEvent Event { get; set; }
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
