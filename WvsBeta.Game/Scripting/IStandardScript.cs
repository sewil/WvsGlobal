using System.Collections.Generic;

namespace WvsBeta.Game.Scripting
{
    public interface IStandardScript : IGameScript
    {
        IDictionary<string, string> ScriptNameMap { get; }
        void PetMenu(INpcHost self, GameCharacter target, string ask);
    }
}
