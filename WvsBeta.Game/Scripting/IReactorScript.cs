namespace WvsBeta.Game.Scripting
{
    public interface IReactorScript : IGameScript
    {
        void Run(IReactorHost host, FieldReactor target);
    }

    public interface IReactorHost
    {
    }
}
