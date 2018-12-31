using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.WpfControls
{
    public static class EventBus
    {
        #region Constants
        public const string OnAfterDropOperation       = nameof(OnAfterDropOperation);
        public const string OnComponentPropertyChanged = nameof(OnComponentPropertyChanged);
        public const string OnDragElementSelected      = nameof(OnDragElementSelected);
        public const string OnDragStarted              = nameof(OnDragStarted);
        public const string RefreshFromDataContext = nameof(RefreshFromDataContext);
        #endregion

        #region Static Fields
        static readonly ConcurrentDictionary<string, List<Action>> Subscribers = new ConcurrentDictionary<string, List<Action>>();
        #endregion

        #region Public Methods
        public static void Publish(string eventName)
        {
            Log.Push("Publish started.@eventName:"+eventName);
            Log.Indent++;

            if (Subscribers.ContainsKey(eventName))
            {
                var actions = Subscribers[eventName].ToList();

                foreach (var action in actions)
                {
                    if (Subscribers[eventName].Contains(action) == false)
                    {
                        continue;
                    }

                    action();
                }
            }

            Log.Indent--;
            Log.Push("Publish finished.@eventName:"+eventName);
            
        }

        public static void Subscribe(string eventName, Action action)
        {
            if (Subscribers.ContainsKey(eventName))
            {
                Subscribers[eventName].Add(action);
                return;
            }

            Subscribers[eventName] = new List<Action> {action};
        }

        public static void UnSubscribe(string eventName, Action action)
        {
            if (Subscribers.ContainsKey(eventName))
            {
                Subscribers[eventName].Remove(action);
            }
        }
        #endregion
    }
}