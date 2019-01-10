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

        #region Public Properties
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
        #endregion

        #region Public Methods
        public int CountOfListeningEventName(string eventName)
        {
            if (Subscribers.ContainsKey(eventName))
            {
                return Subscribers[eventName].Count;
            }

            return 0;
        }

        public void Publish(string eventName)
        {
            Log.Push("Started. "+ eventName);
            Log.Indent++;
            new Publisher(Subscribers).Publish(eventName);

            Log.Indent--;
            Log.Push("Finished. "+ eventName);
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

        class Publisher
        {
            #region Fields
            readonly List<Action> CalledActions = new List<Action>();

            readonly ConcurrentDictionary<string, List<Action>> Subscribers;
            #endregion

            #region Constructors
            public Publisher(ConcurrentDictionary<string, List<Action>> subscribers)
            {
                Subscribers = subscribers;
            }
            #endregion

            #region Public Methods
            public void Publish(string eventName)
            {
                if (!Subscribers.ContainsKey(eventName))
                {
                    return;
                }

                while (TryCall(eventName))
                {
                }
            }
            #endregion

            #region Methods
            bool TryCall(string eventName)
            {
                var action = Subscribers[eventName].FirstOrDefault(x => CalledActions.Contains(x) == false);
                if (action == null)
                {
                    return false;
                }

                CalledActions.Add(action);
                action();
                return true;
            }
            #endregion
        }
    }
}