using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.AppModel
{
    public sealed class EventBus
    {
        #region Constants
        public const string BeforeDragElementSelected  = nameof(BeforeDragElementSelected);
        public const string ComponentDeleted           = nameof(ComponentDeleted);
        public const string DataGridColumnRemoved      = nameof(DataGridColumnRemoved);
        public const string OnAfterDropOperation       = nameof(OnAfterDropOperation);
        public const string OnComponentPropertyChanged = nameof(OnComponentPropertyChanged);
        public const string OnDragElementSelected      = nameof(OnDragElementSelected);
        public const string OnDragStarted              = nameof(OnDragStarted);
        public const string RefreshFromDataContext     = nameof(RefreshFromDataContext);
        public const string TabBarPageRemoved          = nameof(TabBarPageRemoved);
        #endregion

        #region Fields
        readonly ConcurrentDictionary<string, List<Action>> Subscribers = new ConcurrentDictionary<string, List<Action>>();
        #endregion

        public int CountOfListeningEventName(string eventName)
        {
            if (Subscribers.ContainsKey(eventName))
            {
                return Subscribers[eventName].Count;
            }

            return 0;
        }
        public int CountOfListeningEventNames
        {
            get
            {
                var count = 0;
                foreach (var key in Subscribers.Keys)
                {
                    count += CountOfListeningEventName(key);
                }

                return count;
            }
        }


        #region Public Methods
        public void Publish(string eventName)
        {
            Log.Push("Publish started.@eventName:" + eventName);
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
            Log.Push("Publish finished.@eventName:" + eventName);
        }

        public void Subscribe(string eventName, Action action)
        {
            if (Subscribers.ContainsKey(eventName))
            {
                Subscribers[eventName].Add(action);
                return;
            }

            Subscribers[eventName] = new List<Action> {action};
        }

        public void UnSubscribe(string eventName, Action action)
        {
            if (Subscribers.ContainsKey(eventName))
            {
                Subscribers[eventName].Remove(action);

            }
        }
        #endregion
    }
}