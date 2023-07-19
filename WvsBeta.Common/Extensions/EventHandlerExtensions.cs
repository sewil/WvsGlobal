using System;

namespace WvsBeta.Common.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void ClearInvocations<T>(this EventHandler<T> e)
        {
            var invocationList = e?.GetInvocationList();
            if (invocationList != null)
            {
                foreach (Delegate d in invocationList)
                {
                    e -= (EventHandler<T>)d;
                }
            }
        }
        public static void ClearInvocations(this EventHandler e)
        {
            var invocationList = e?.GetInvocationList();
            if (invocationList != null)
            {
                foreach (Delegate d in invocationList)
                {
                    e -= (EventHandler)d;
                }
            }
        }

        public static void SubscribeOnce(this EventHandler e, Action action)
        {

        }
    }
}
