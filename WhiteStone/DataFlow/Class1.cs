using System;
using System.Collections.Generic;
using System.Linq;

namespace BOA.DataFlow
{
   

  

    /// <summary>
    ///     The event
    /// </summary>
    public class Event
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
    public class Property<TValueType> 
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Property" /> class.
        /// </summary>
        internal Property(int id, string name)
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
        public TValueType this[IContext context]
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
    public static class Property
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
        public static Property<TValueType> Create<TValueType>(string name = null)
        {
            return new Property<TValueType>(Id++, name);
        }
        #endregion
    }

    /// <summary>
    ///     The data context
    /// </summary>
    public interface IContext
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
        void Add<T>(Property<T> property, T value);

        /// <summary>
        ///     Attaches the event.
        /// </summary>
        void AttachEvent(Event @event, Action<IContext> action);

        /// <summary>
        ///     Detaches the event.
        /// </summary>
        void DetachEvent(Event @event, Action<IContext> action);

        /// <summary>
        ///     Fires the event.
        /// </summary>
        void FireEvent(Event @event);

        /// <summary>
        ///     Gets the specified data constant.
        /// </summary>
        T Get<T>(Property<T> property);

        /// <summary>
        ///     Removes the specified data constant.
        /// </summary>
        void Remove<T>(Property<T> property);

        /// <summary>
        ///     Tries the get.
        /// </summary>
        T TryGet<T>(Property<T> property);
        #endregion
    }

    public interface IContextContainer
    {
        IContext Context { get; set; }
        T Create<T>() where T : IContextContainer, new();

    }
    public class ContextContainer:IContextContainer
    {
        public IContext Context { get; set; }

        public T Create<T>() where T : IContextContainer, new()
        {
            return new T {Context = this.Context};
        }


        protected void AttachEvent(Event @event, Action action)
        {
            Context.AttachEvent(@event,(c)=>action());
        }

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
        public DataNotFoundException(string message) : base(message)
        {
        }
        #endregion

        #region Public Properties
       
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
        public DataShouldRemoveBeforeSetException(string message) : base(message)
        {
        }
        #endregion

        #region Public Properties
        
        #endregion
    }

    /// <summary>
    ///     The data context
    /// </summary>
    public class Context : IContext
    {
        #region Fields
        /// <summary>
        ///     The dictionary
        /// </summary>
        readonly Dictionary<string, Pair> dictionary = new Dictionary<string, Pair>();

        /// <summary>
        ///     The subscribers
        /// </summary>
        readonly Dictionary<string, List<Action<IContext>>> Subscribers = new Dictionary<string, List<Action<IContext>>>();

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
        public void Add<T>(Property<T> property, T value)
        {
            Pair pair = null;
            if (dictionary.TryGetValue(property.Id, out pair))
            {
                throw new DataShouldRemoveBeforeSetException(property.ToString());
            }

            dictionary[property.Id] = new Pair(property.Id, bracketIndex, value);
        }

        /// <summary>
        ///     Attaches the event.
        /// </summary>
        public void AttachEvent(Event @event, Action<IContext> action)
        {
            List<Action<IContext>> actions = null;

            if (Subscribers.TryGetValue(@event.Name, out actions))
            {
                actions.Add(action);
                return;
            }

            Subscribers[@event.Name] = new List<Action<IContext>> {action};
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
        public void DetachEvent(Event @event, Action<IContext> action)
        {
            List<Action<IContext>> actions = null;

            if (!Subscribers.TryGetValue(@event.Name, out actions))
            {
                return;
            }

            actions.Remove(action);
        }

        /// <summary>
        ///     Fires the event.
        /// </summary>
        public void FireEvent(Event @event)
        {
            List<Action<IContext>> actions = null;

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
        public T Get<T>(Property<T> property)
        {
            Pair pair = null;
            if (dictionary.TryGetValue(property.Id, out pair))
            {
                return (T) pair.value;
            }

            throw new DataNotFoundException(property.ToString());
        }

        public void OpenBracket()
        {
            bracketIndex++;
        }

        /// <summary>
        ///     Removes the specified data constant.
        /// </summary>
        public void Remove<T>(Property<T> property)
        {
            if (dictionary.ContainsKey(property.Id))
            {
                dictionary.Remove(property.Id);
                return;
            }

            throw new DataNotFoundException(property.ToString());
        }

        /// <summary>
        ///     Tries the get.
        /// </summary>
        public T TryGet<T>(Property<T> property)
        {
            Pair pair = null;
            if (dictionary.TryGetValue(property.Id, out pair))
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