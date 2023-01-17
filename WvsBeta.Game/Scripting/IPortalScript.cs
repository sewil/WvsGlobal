using System.Collections.Generic;

namespace WvsBeta.Game.Scripting
{
    public interface IPortalScript : IGameScript
    {
        void Run(IPortalHost host, GameCharacter target);
    }

    public interface IPortalHost
    {
    }
}
