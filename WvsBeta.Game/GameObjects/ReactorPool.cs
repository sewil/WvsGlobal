using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;

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
        public void Show(FieldReactor reactor)
        {
            Reactors[reactor.ID] = reactor;
            ShownReactors[reactor.ID] = reactor;
            reactor.Show();
        }
        public void Clear()
        {
            foreach (byte rid in ShownReactors.Keys)
            {
                DestroyReactor(rid);
            }
        }

        public void ShowReactorsTo(GameCharacter chr)
        {
            ShownReactors.Values.ForEach(r => r.ShowTo(chr));
        }

        public void DestroyReactor(int rid)
        {
            if (ShownReactors.TryGetValue(rid, out FieldReactor r))
            {
                r.Destroy();
                ShownReactors.Remove(rid);
            }
        }

        public void PlayerHitReactor(GameCharacter chr, FieldReactor r)
        {
            r.HitBy(chr);
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
