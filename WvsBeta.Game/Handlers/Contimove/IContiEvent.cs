using WvsBeta.Common;

namespace WvsBeta.Game.Handlers.Contimove
{
    public interface IContiEvent
    {
        bool EventInProgress { get; }
        void ResetEvent();
        bool HandleMove(long systemTime);
    }
}
