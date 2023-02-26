using System;
using System.Collections.Generic;
using WvsBeta.Common;

namespace WvsBeta.Game.GameObjects
{
    public class ReactorPool
    {
        const uint ReactorStart = 200;
        public Map Field { get; set; }
        public bool DropEverlasting { get; set; }
        public Dictionary<int, FieldReactor> Reactors { get; private set; } = new Dictionary<int, FieldReactor>();
        public Dictionary<int, FieldReactor> ShownReactors { get; private set; } = new Dictionary<int, FieldReactor>();

        public ReactorPool(Map field)
        {
            Field = field;
        }
        public void Add(FieldReactor reactor, bool show)
        {
            Reactors.Add(reactor.ID, reactor);
            if (show)
            {
                Show(reactor.ID);
            }
        }
        public FieldReactor Show(int rid)
        {
            if (!Reactors.TryGetValue(rid, out FieldReactor reactor) || ShownReactors.ContainsKey(rid)) return null;
            FieldReactor rclone = reactor.ShallowCopy;
            ShownReactors[reactor.ID] = rclone;
            rclone.Show();
            return rclone;
        }
        public void Clear()
        {
            foreach (var reactor in ShownReactors)
            {
                reactor.Value.Destroy();
            }
        }

        public void ShowReactorsTo(GameCharacter chr)
        {
            ShownReactors.Values.ForEach(r => r.Show(chr));
        }

        public void PlayerHitReactor(GameCharacter chr, FieldReactor r)
        {
            r.Trigger(chr);
        }
        public void PlayerHitReactor(GameCharacter chr, byte rid)
        {
            if (ShownReactors.TryGetValue(rid, out FieldReactor r))
            {
                PlayerHitReactor(chr, r);
            }
        }
    }
}
