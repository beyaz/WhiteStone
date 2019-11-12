﻿using System;
using System.Collections.Generic;

namespace ___Company___.DataFlow
{
    /// <summary>
    ///     The data constant
    /// </summary>
    public interface IDataConstant
    {
        #region Public Properties
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        Type Type { get; }
        #endregion
    }

    /// <summary>
    ///     The event
    /// </summary>
    public interface IEvent
    {
        #region Public Properties
        /// <summary>
        ///     Gets the name.
        /// </summary>
        string Name { get; }
        #endregion
    }

    /// <summary>
    ///     The event
    /// </summary>
    public class Event : IEvent
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }

    /// <summary>
    ///     The data constant
    /// </summary>
    public class DataConstant : IDataConstant
    {
        #region Public Properties
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public Type Type { get; set; }
        #endregion
    }

    /// <summary>
    ///     The data context
    /// </summary>
    public interface IDataContext
    {
        #region Public Methods
        /// <summary>
        ///     Gets the specified data constant.
        /// </summary>
        T Get<T>(IDataConstant dataConstant);
        #endregion
    }

    /// <summary>
    ///     The data not found exception
    /// </summary>
    public class DataNotFoundException : InvalidOperationException
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataNotFoundException" /> class.
        /// </summary>
        public DataNotFoundException(IDataConstant dataConstant)
        {
            DataConstant = dataConstant;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the data constant.
        /// </summary>
        public IDataConstant DataConstant { get; }
        #endregion
    }

    /// <summary>
    ///     The data should remove before set exception
    /// </summary>
    public class DataShouldRemoveBeforeSetException : InvalidOperationException
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataShouldRemoveBeforeSetException" /> class.
        /// </summary>
        public DataShouldRemoveBeforeSetException(IDataConstant dataConstant)
        {
            DataConstant = dataConstant;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the data constant.
        /// </summary>
        public IDataConstant DataConstant { get; }
        #endregion
    }

    /// <summary>
    ///     The data context
    /// </summary>
    public class DataContext : IDataContext
    {
        #region Fields
        /// <summary>
        ///     The dictionary
        /// </summary>
        readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

        /// <summary>
        ///     The subscribers
        /// </summary>
        readonly Dictionary<string, List<Action<IDataContext>>> Subscribers = new Dictionary<string, List<Action<IDataContext>>>();
        #endregion

        #region Public Methods
        /// <summary>
        ///     Adds the specified data constant.
        /// </summary>
        public void Add<T>(IDataConstant dataConstant, T value)
        {
            object val = null;
            if (dictionary.TryGetValue(dataConstant.Id, out val))
            {
                throw new DataShouldRemoveBeforeSetException(dataConstant);
            }

            dictionary[dataConstant.Id] = value;
        }

        /// <summary>
        ///     Attaches the event.
        /// </summary>
        public void AttachEvent(IEvent @event, Action<IDataContext> action)
        {
            List<Action<IDataContext>> actions = null;

            if (Subscribers.TryGetValue(@event.Name, out actions))
            {
                actions.Add(action);
                return;
            }

            Subscribers[@event.Name] = new List<Action<IDataContext>> {action};
        }

        /// <summary>
        ///     Detaches the event.
        /// </summary>
        public void DetachEvent(IEvent @event, Action<IDataContext> action)
        {
            List<Action<IDataContext>> actions = null;

            if (!Subscribers.TryGetValue(@event.Name, out actions))
            {
                return;
            }

            actions.Remove(action);
        }

        /// <summary>
        ///     Fires the event.
        /// </summary>
        public void FireEvent(IEvent @event)
        {
            List<Action<IDataContext>> actions = null;

            if (!Subscribers.TryGetValue(@event.Name, out actions))
            {
                return;
            }

            foreach (var action in actions)
            {
                action(this);
            }
        }

        /// <summary>
        ///     Gets the specified data constant.
        /// </summary>
        public T Get<T>(IDataConstant dataConstant)
        {
            object value = null;
            if (dictionary.TryGetValue(dataConstant.Id, out value))
            {
                return (T) value;
            }

            throw new DataNotFoundException(dataConstant);
        }

        /// <summary>
        ///     Removes the specified data constant.
        /// </summary>
        public void Remove(IDataConstant dataConstant)
        {
            if (dictionary.ContainsKey(dataConstant.Id))
            {
                dictionary.Remove(dataConstant.Id);
                return;
            }

            throw new DataNotFoundException(dataConstant);
        }
        #endregion
    }
}