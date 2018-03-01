using System.Collections.Generic;

namespace WhiteStone.Tasks
{
    public interface ITask
    {
        #region Public Properties
        IDictionary<string, object> Keys { get; set; }
        #endregion

        #region Public Methods
        void Run();
        #endregion
    }

    public abstract class TaskBase : ITask
    {
        #region Public Properties
        public IDictionary<string, object> Keys { get; set; }
        #endregion

        #region Public Methods
        public abstract void Run();
        #endregion

        #region Methods
        protected string GetKey(string key)
        {
            return GetValueByKey(key) as string;
        }

        protected bool? GetKeyAsBoolean(string key)
        {
            return GetValueByKey(key) as bool?;
        }

        protected string[] GetKeyAsStringArray(string key)
        {
            return GetValueByKey(key) as string[];
        }

        object GetValueByKey(string key)
        {
            return Keys.TryGetValue(key, null);
        }
        #endregion
    }
}