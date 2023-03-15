using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    [Script("Pianus")]
    class aqua : IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            FieldSet fset = FieldSet.Instances["Pianus"];
            int users = fset.UserCount;

            if (users < 10)
            {
                target.PlayPortalSE();
                target.ChangeMap(230040420, "");
            }
            else target.Message("The Cave of Pianus is currently full. Please come back later.");
        }
    }
    #region Reactors
    [Script("aquaItem0")]
    class aquaItem0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("aquaItem1")]
    class aquaItem1 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("aquaItem2")]
    class aquaItem2 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    #endregion
}
