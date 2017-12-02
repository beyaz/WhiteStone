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

    public abstract class TaskBase: ITask
    {
        public IDictionary<string, object> Keys { get; set; }

        protected string GetKey( string key)
        {
            return Keys[key] as string;
        }

        protected bool? GetKeyAsBoolean(string key)
        {
            return Keys[key] as bool?;
        }

        public abstract void Run();
    }
}