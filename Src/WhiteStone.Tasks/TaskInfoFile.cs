using System;
using System.Collections.Generic;

namespace WhiteStone.Tasks
{
    [Serializable]
    public class TaskInfoFile
    {
        #region Public Properties
        public IDictionary<string, string> GlobalKeys { get; set; }

        public ICollection<TaskInfo> Tasks { get; set; }
        #endregion
    }
}