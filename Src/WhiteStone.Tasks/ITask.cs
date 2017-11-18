using System.Collections.Generic;

namespace WhiteStone.Tasks
{
    public interface ITask
    {
        #region Public Properties
        IDictionary<string, string> Keys { get; set; }
        #endregion

        #region Public Methods
        void Run();
        #endregion
    }
}