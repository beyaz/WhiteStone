using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BOA.OneDesigner.WpfControls
{
    public static class EventBus
    {
        #region Constants
        public const string OnAfterDropOperation       = nameof(OnAfterDropOperation);
        public const string OnComponentPropertyChanged = nameof(OnComponentPropertyChanged);
        public const string OnDragElementSelected      = nameof(OnDragElementSelected);
        public const string OnDragStarted              = nameof(OnDragStarted);
        #endregion

        #region Static Fields
        static readonly ConcurrentDictionary<string, List<Action>> Subscribers = new ConcurrentDictionary<string, List<Action>>();
        #endregion

        #region Public Methods
        public static void Publish(string eventName)
        {
            if (Subscribers.ContainsKey(eventName))
            {
                var actions = Subscribers[eventName].ToList();

                foreach (var action in actions)
                {
                    action();
                }
            }
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
        #endregion
    }
}