using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts.Reactor
{
    #region Reactors
    [Script("boxPaper0")]
    class boxPaper0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.DoDrop(0, (Constants.Items.Zakum.PAPER_DOCUMENT, 1));
        }
    }
    [Script("money100")]
    class money100 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            int meso = Rand32.NextBetween(100, 999);
            target.DoDrop(meso);
        }
    }
    [Script("money10000")]
    class money10000 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            int meso = Rand32.NextBetween(10000,99999);
            target.DoDrop(meso);
        }
    }
    [Script("boxKey0")]
    class boxKey0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.DoDrop(0, (Constants.Items.Zakum.THE_KEY, 1));
        }
    }
    [Script("boxItem0")]
    class boxItem0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.DoDrop(0, (2022001, 1));
        }
    }
    [Script("boxItem1")]
    class boxItem1 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.DoDrop(0, (2000006, 1));
        }
    }
    [Script("boxItem2")]
    class boxItem2 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.DoDrop(0, (2000004, 1));
        }
    }
    [Script("boxItem3")]
    class boxItem3 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.DoDrop(0, (2000005, 1));
        }
    }
    [Script("boxMob0")]
    class boxMob0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.DoSpawn(-1, (9300004, 3, 1));
        }
    }
    [Script("go280010000")]
    class go280010000 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            if (target.Owner == null) return;
            target.Owner.ChangeMap(280010000);
            target.Owner.SendPacket(MessagePacket.RedText("You will go back to the first place by an unknown force."));
        }
    }
    #endregion
}
