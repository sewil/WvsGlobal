using System;
using System.Collections.Generic;
using System.Linq;

namespace WvsBeta.Common
{
    public class EventSubscriber<T>
    {
        private readonly IList<Action<T>> actions;
        private readonly IList<Action<T>> oneTimeActions;
        public EventSubscriber()
        {
            actions = new List<Action<T>>();
            oneTimeActions = new List<Action<T>>();
        }

        public void SubscribeOnce(Action<T> action)
        {
            oneTimeActions.Add(action);
        }

        public void Subscribe(Action<T> action)
        {
            actions.Add(action);
        }

        public void Unsubscribe(Action<T> action)
        {
            actions.Remove(action);
        }

        public void Invoke(T obj)
        {
            actions.ForEach(s => s.Invoke(obj));
            foreach (var a in oneTimeActions.ToList())
            {
                a.Invoke(obj);
                oneTimeActions.Remove(a);
            }
        }
    }
}
