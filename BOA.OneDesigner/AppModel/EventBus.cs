using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.AppModel
{
    public sealed class EventBus
    {
        #region Constants
        public const string ComponentDeleted           = nameof(ComponentDeleted);
        public const string DataGridColumnAdded        = nameof(DataGridColumnAdded);
        public const string DataGridColumnRemoved      = nameof(DataGridColumnRemoved);
        public const string OnAfterDropOperation       = nameof(OnAfterDropOperation);
        
        public const string OnDragElementSelected      = nameof(OnDragElementSelected);
        public const string OnDragStarted              = nameof(OnDragStarted);
        public const string RefreshFromDataContext     = nameof(RefreshFromDataContext);
        public const string TabBarPageRemoved          = nameof(TabBarPageRemoved);
        public const string TabBarPageAdded = nameof(TabBarPageAdded);

        public const string WideChanged = nameof(WideChanged);
        public const string SizeChanged = nameof(SizeChanged);
        public const string LabelChanged = nameof(LabelChanged);
        public const string RowCountChanged = nameof(RowCountChanged);
        #endregion

        #region Fields
        readonly ConcurrentDictionary<string, ArrayList> Subscribers = new ConcurrentDictionary<string, ArrayList>();
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

        public string CurrentExecutingEventName { get; set; }

        public void Publish(string eventName)
        {
            CurrentExecutingEventName = eventName;

            Log.Push("Started. " + eventName);
            Log.Indent++;
            new Publisher(Subscribers).Publish(eventName);

            Log.Indent--;
            Log.Push("Finished. " + eventName);

            CurrentExecutingEventName = null;
        }

        public void Subscribe(string eventName, Action action)
        {
            if (Subscribers.ContainsKey(eventName) == false)
            {
                Subscribers[eventName] = ArrayList.Synchronized(new ArrayList());
            }

            var concurrentBag = Subscribers[eventName];
            if (concurrentBag.Contains(action))
            {
                return;
            }

            concurrentBag.Add(action);
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
            readonly ConcurrentBag<Action> CalledActions = new ConcurrentBag<Action>();

            readonly ConcurrentDictionary<string, ArrayList> Subscribers;
            #endregion

            #region Constructors
            public Publisher(ConcurrentDictionary<string, ArrayList> subscribers)
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
                var arrayList = Subscribers[eventName];
                foreach (Action action in arrayList)
                {
                    if (CalledActions.Contains(action))
                    {
                        continue;
                    }

                    CalledActions.Add(action);
                    action();
                    return true;
                }

                return false;
            }
            #endregion
        }
    }
}