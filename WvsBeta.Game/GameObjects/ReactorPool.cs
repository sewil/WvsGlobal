using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;

namespace WvsBeta.Game.GameObjects
{
    public class ReactorPool
    {
        public Map Field { get; set; }
        public Dictionary<int, FieldReactor> Reactors { get; private set; } = new Dictionary<int, FieldReactor>();

        public ReactorPool(Map field)
        {
            Field = field;
        }
        public void AddReactor(FieldReactor reactor, bool sendPacket = true)
        {
            Reactors.Add(reactor.ID, reactor);
            if (sendPacket)
            {
                reactor.Show();
            }
        }
        public void RemoveReactor(int reactorId, bool sendPacket = true)
        {
            lock (Reactors)
            {
                if (!Reactors.TryGetValue(reactorId, out FieldReactor reactor))
                {
                    Program.MainForm.LogAppend("Reactor " + reactorId + " not found!");
                    return;
                }
                Reactors.Remove(reactorId);
                if (sendPacket)
                {
                    ReactorPacket.DestroyReactor(reactor);
                }
            }
        }
        public void Reset(bool sendPacket = true)
        {
            foreach (var reactor in Reactors.Values)
            {
                reactor.Reset(sendPacket);
            }
        }

        public void ShowReactorsTo(GameCharacter chr)
        {
            Reactors.Values.ForEach(r => r.Show(chr));
        }
        public void TriggerReactor(string reactorName, GameCharacter owner = null)
        {
            if (Reactors.Select(i => i.Value).TryFind(i => i.Name == reactorName, out FieldReactor reactor))
            {
                TriggerReactor(reactor, owner);
            }
            else
            {
                Program.MainForm.LogAppend("Reactor \"" + reactorName + "\" not found!");
            }
        }
        public void TriggerReactor(int reactorId, GameCharacter owner = null)
        {
            if (Reactors.TryGetValue(reactorId, out FieldReactor reactor))
            {
                TriggerReactor(reactor, owner);
            }
            else
            {
                Program.MainForm.LogAppend("Reactor " + reactorId + " not found!");
            }
        }
        public void TriggerReactor(FieldReactor reactor, GameCharacter owner = null)
        {
            reactor.Trigger(owner);
        }

        public void Shuffle()
        {
            lock (Reactors)
            {
                var reactors = Reactors.Values.ToList();
                Reactors.Clear();

                int idxRight = reactors.Count;
                while (idxRight > 1)
                {
                    idxRight--;
                    int idxLeft = Rand32.NextBetween(0, idxRight + 1);
                    FieldReactor valueLeft = reactors[idxLeft];
                    FieldReactor valueRight = reactors[idxRight];
                    Pos leftPos = valueLeft.Position;
                    Pos rightPos = valueRight.Position;
                    valueLeft.Position = rightPos;
                    valueRight.Position = leftPos;
                }
                reactors.ForEach(r => AddReactor(r, false));
            }
        }

        public bool CheckDropTriggers(long pNow, Drop drop)
        {
            if (pNow - drop.CreateTime < 5000 || !Server.Instance.CharacterList.TryGetValue(drop.OwnerID, out GameCharacter owner)) return false;

            var reactors = Reactors.Values.Where(r =>
                r.State.Event?.Type == ReactorEventType.Drop
                && r.State.Event.DropID == drop.Reward.ItemID
                && r.State.Event.DropAmount == drop.Reward.Amount
                && r.EventRectangle?.Contains(drop.AreaPos) == true
            );
            int c = reactors.Count();
            reactors.ForEach(i => i.Trigger(owner));
            return c > 0;
        }
    }
}
