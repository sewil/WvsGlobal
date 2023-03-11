using reNX.NXProperties;
using System;
using System.Collections.Generic;
using System.Drawing;
using WvsBeta.Common;
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
        public Rectangle? Rectangle { get; private set; }
        public int DropID { get; private set; }
        public short DropAmount { get; private set; }
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
            SetState(null, _state, false);
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
            SetState(null, _state, false);
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
        public void SetState(GameCharacter chr, byte state, bool sendPacket = true)
        {
            Owner = chr;
            _state = (byte)(state % Reactor.States.Count);
            bool isLast = _state == Reactor.States.Count - 1;
            if (State.Event?.Rectangle != null)
            {
                var rect = State.Event.Rectangle.Value;
                rect.Offset(Position);
                EventRectangle = rect;
            }
            if (isLast)
            {
                RunScript();
                if (ReactorTime > -1)
                {
                    resetAction = MasterThread.RepeatingAction.Start("rrts-" + Field.ID + "-" + ID, time => Reset(true), ReactorTime * 1000, 0);
                }
            }
            if (sendPacket)
            {
                ReactorPacket.ReactorChangedState(this);
            }
        }
        public void Show(GameCharacter toChar = null)
        {
            ReactorPacket.ShowReactor(this, toChar);
        }

        public void Reset(bool sendPacket = true)
        {
            dropAction?.Stop();
            resetAction?.Stop();
            _state = 0;
            if (sendPacket)
                ReactorPacket.ReactorChangedState(this);
        }
        private MasterThread.RepeatingAction dropAction;
        private MasterThread.RepeatingAction resetAction;
        private void RunScript()
        {
            ReactorScriptSession.Run(this, script => {
#if !DEBUG
                if (Owner != null && Owner.IsGM)
                {
#endif
                    ChatPacket.SendBroadcastMessageToMap(Field.ID, "Error compiling script: " + script, BroadcastMessageType.Notice);
#if !DEBUG
                }
#endif
            });
        }
        public void Trigger(GameCharacter chr)
        {
            SetState(chr, (byte)(_state + 1));
        }
        public bool TriggerDrop(Drop drop, int ownerId)
        {
            var e = State.Event;
            bool trigger = e?.Type == ReactorEventType.Drop && e?.DropID == drop.Reward.ItemID && e.DropAmount == drop.Reward.Amount && EventRectangle?.Contains(drop.AreaPos) == true;
            if (trigger)
            {
                if (!Server.Instance.CharacterList.TryGetValue(ownerId, out GameCharacter owner)) return false;
                dropAction = MasterThread.RepeatingAction.Start("rdtr-" + Field.ID + "-" + ID, time =>
                {
                    if (Field.DropPool.Drops.ContainsKey(drop.DropID)) // Check drop still exists
                    {
                        Field.DropPool.RemoveDrop(drop);
                        Trigger(owner);
                    }
                }, 5000, 0);
            }
            return trigger;
        }
        public IList<NpcLife> SpawnNpc(Pos offset, params int[] npcs)
        {
            var pos = new Pos(Position);
            pos.Offset(offset);
            var fh = Field.GetFootholdUnderneath(pos.X, pos.Y, out int _);
            var npcLives = new List<NpcLife>();
            foreach (int npc in npcs)
            {
                NpcLife npcLife = Field.SpawnNpc(npc, pos, fh);
                if (npcLife != null)
                {
                    npcLives.Add(npcLife);
                }
            }
            return npcLives;
        }
        public IList<Mob> SpawnMob(Pos offset, params (int mobid, short amount, SummonType summonType, Mob ownerMob)[] mobs)
        {
            var pos = new Pos(Position);
            pos.Offset(offset);
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
        public void Drop()
        {
            var rewards = Reward.GetRewards(Owner, Field, Reactor.ID, 'r', false, 1);
            rewards.Shuffle();

            int x2 = Position.X - 10 * rewards.Count + 10;
            short delay = 0;
            foreach (var reward in rewards)
            {
                Field.DropPool.Create(reward, Owner.ID, Owner.PartyID, DropType.Normal, ID, Position, x2, delay, false, false);
                x2 += 20;
                delay += 120;
            }
        }
    }
    public class ReactorState
    {
        public byte State { get; private set; }
        public ReactorEvent Event { get; private set; }
        public ReactorState(NXNode node)
        {
            State = byte.Parse(node.Name);
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "event":
                        Event = new ReactorEvent(subNode["0"]);
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
