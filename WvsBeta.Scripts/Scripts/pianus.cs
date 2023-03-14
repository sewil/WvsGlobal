using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    [Script("Pianus")]
    class pianus : IPortalScript
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
}
