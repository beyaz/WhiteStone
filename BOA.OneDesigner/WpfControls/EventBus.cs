using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BOA.OneDesigner.WpfControls
{
    public static class EventBus
    {
        public const string OnComponentPropertyChanged = nameof(OnBComponentPropertyChanged);

        #region Static Fields
        static readonly ConcurrentDictionary<string, List<Action>> Subscribers = new ConcurrentDictionary<string, List<Action>>();
        #endregion

        #region Public Events
        public static event Action AfterDropOperation;
        public static event Action BComponentPropertyChanged;
        public static event Action DragElementSelected;
        public static event Action DragStarted;
        #endregion

        #region Public Methods
        public static void OnAfterDropOperation()
        {
            AfterDropOperation?.Invoke();
        }

        public static void OnBComponentPropertyChanged()
        {
            BComponentPropertyChanged?.Invoke();
        }

        public static void OnDragElementSelected()
        {
            DragElementSelected?.Invoke();
        }

        public static void OnDragStarted()
        {
            DragStarted?.Invoke();
        }

        public static void Publish(string eventName)
        {
            if (Subscribers.ContainsKey(eventName))
            {
                foreach (var action in Subscribers[eventName])
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