using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BOA.DataFlow
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
        ///     Gets the name.
        /// </summary>
        string Name { get; }
        #endregion
    }

    /// <summary>
    ///     The data constant
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface IDataConstant<TValueType> : IDataConstant
    {
        #region Public Indexers
        TValueType this[IDataContext context] { get; set; }
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
    public class DataConstant<TValueType> : IDataConstant<TValueType>
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataConstant{TValueType}" /> class.
        /// </summary>
        internal DataConstant(int id, string name)
        {
            Name = name;
            Id   = id.ToString();
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }
        #endregion

        #region Public Indexers
        public TValueType this[IDataContext context]
        {
            get { return context.Get(this); }
            set { context.Add(this, value); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            if (Name == null)
            {
                return $"Id: {Id} - Type: {typeof(TValueType).FullName}";
            }

            return $"name: {Name} - Type: {typeof(TValueType).FullName}";
        }
        #endregion
    }

    /// <summary>
    ///     The data constant
    /// </summary>
    public static class DataConstant
    {
        #region Static Fields
        /// <summary>
        ///     The identifier
        /// </summary>
        static int Id;
        #endregion

        #region Public Methods
        /// <summary>
        ///     Creates this instance.
        /// </summary>
        public static DataConstant<TValueType> Create<TValueType>(string name = null)
        {
            return new DataConstant<TValueType>(Id++, name);
        }
        #endregion
    }

    /// <summary>
    ///     The data context
    /// </summary>
    public interface IDataContext
    {
        void OpenBracket();
        void CloseBracket();
        #region Public Properties
        /// <summary>
        ///     Gets a value indicating whether this instance is empty.
        /// </summary>
        bool IsEmpty { get; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Adds the specified data constant.
        /// </summary>
        void Add<T>(IDataConstant dataConstant, T value);

        /// <summary>
        ///     Attaches the event.
        /// </summary>
        void AttachEvent(IEvent @event, Action<IDataContext> action);

        /// <summary>
        ///     Detaches the event.
        /// </summary>
        void DetachEvent(IEvent @event, Action<IDataContext> action);

        /// <summary>
        ///     Fires the event.
        /// </summary>
        void FireEvent(IEvent @event);

        /// <summary>
        ///     Gets the specified data constant.
        /// </summary>
        T Get<T>(IDataConstant<T> dataConstant);

        /// <summary>
        ///     Removes the specified data constant.
        /// </summary>
        void Remove(IDataConstant dataConstant);

        /// <summary>
        ///     Tries the get.
        /// </summary>
        T TryGet<T>(IDataConstant<T> dataConstant);
        #endregion
    }

    public interface IContextContainer
    {
        IDataContext Context { get; set; }
        T Create<T>() where T : IContextContainer, new();

    }
    public class ContextContainer:IContextContainer
    {
        public IDataContext Context { get; set; }
        public T Create<T>() where T : IContextContainer, new()
        {
            return new T {Context = this.Context};
        }


        protected void AttachEvent(IEvent @event, Action action)
        {
            Context.AttachEvent(@event,(c)=>c.OpenBracket());
        }

        /// <summary>
        ///     Detaches the event.
        /// </summary>
        // void DetachEvent(IEvent @event, Action<IDataContext> action);
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
        public DataNotFoundException(IDataConstant dataConstant) : base(dataConstant.ToString())
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
        public DataShouldRemoveBeforeSetException(IDataConstant dataConstant) : base(dataConstant.Id)
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
        readonly Dictionary<string, Pair> dictionary = new Dictionary<string, Pair>();

        /// <summary>
        ///     The subscribers
        /// </summary>
        readonly Dictionary<string, List<Action<IDataContext>>> Subscribers = new Dictionary<string, List<Action<IDataContext>>>();

        int bracketIndex;
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return dictionary.Count == 0; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Adds the specified data constant.
        /// </summary>
        public void Add<T>(IDataConstant dataConstant, T value)
        {
            Pair pair = null;
            if (dictionary.TryGetValue(dataConstant.Id, out pair))
            {
                throw new DataShouldRemoveBeforeSetException(dataConstant);
            }

            dictionary[dataConstant.Id] = new Pair(dataConstant.Id, bracketIndex, value);
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

        public void CloseBracket()
        {
            if (bracketIndex<0)
            {
                throw new InvalidOperationException("There is no bracket to close.");
            }

            var removeList = dictionary.Values.Where(p => p.bracketIndex == bracketIndex).Select(p => p.key).ToList();

            foreach (var key in removeList)
            {
                dictionary.Remove(key);
            }

            bracketIndex--;
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
        public T Get<T>(IDataConstant<T> dataConstant)
        {
            Pair pair = null;
            if (dictionary.TryGetValue(dataConstant.Id, out pair))
            {
                return (T) pair.value;
            }

            throw new DataNotFoundException(dataConstant);
        }

        public void OpenBracket()
        {
            bracketIndex++;
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

        /// <summary>
        ///     Tries the get.
        /// </summary>
        public T TryGet<T>(IDataConstant<T> dataConstant)
        {
            Pair pair = null;
            if (dictionary.TryGetValue(dataConstant.Id, out pair))
            {
                return (T) pair.value;
            }

            return default(T);
        }
        #endregion

        class Pair
        {
            #region Fields
            public readonly int    bracketIndex;
            public readonly string key;
            public readonly object value;
            #endregion

            #region Constructors
            public Pair(string key, int bracketIndex, object value)
            {
                this.key          = key;
                this.bracketIndex = bracketIndex;
                this.value        = value;
            }
            #endregion
        }
    }
}