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
        public void TriggerReactor(string reactorName, GameCharacter owner)
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
        public void TriggerReactor(int reactorId, GameCharacter owner)
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
        public void TriggerReactor(FieldReactor reactor, GameCharacter owner)
        {
            reactor.Trigger(owner);
        }
    }
}
