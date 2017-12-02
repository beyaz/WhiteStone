using System;
using System.Collections.Generic;

namespace WhiteStone.Tasks
{
    [Serializable]
    public class TaskInfo
    {
        #region Public Properties
        public string FullClassName { get; set; }
        public IDictionary<string, object> Keys { get; set; }

        

        #endregion
    }
}