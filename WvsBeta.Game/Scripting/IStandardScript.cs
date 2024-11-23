using System.Collections.Generic;

namespace WvsBeta.Game.Scripting
{
    public interface IStandardScript : IGameScript
    {
        void PetMenu(INpcHost self, GameCharacter target, string ask);
    }
}
