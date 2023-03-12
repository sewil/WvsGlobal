namespace WvsBeta.Game.Scripting
{
    public interface IFieldScript : IGameScript
    {
        void Run(Map field, GameCharacter target);
    }
}
