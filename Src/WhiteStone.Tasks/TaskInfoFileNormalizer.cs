using System.Collections.Generic;

namespace WhiteStone.Tasks
{
    class TaskInfoFileNormalizer
    {
        #region Public Properties
        public TaskInfoFile TaskInfoFile { get; set; }
        #endregion

        #region Public Methods
        public void Normalize()
        {
            foreach (var globalKey in TaskInfoFile.GlobalKeys.Keys)
            {
                var globalValue = TaskInfoFile.GlobalKeys[globalKey];

                foreach (var taskInfo in TaskInfoFile.Tasks)
                {
                    var newKeys = new Dictionary<string, string>();

                    foreach (var key in taskInfo.Keys.Keys)
                    {
                        var newKey = key.Replace(globalKey, globalValue);
                        var newValue = taskInfo.Keys[key]?.Replace(globalKey, globalValue);

                        newKeys[newKey] = newValue;
                    }

                    taskInfo.Keys = newKeys;
                }
            }
        }
        #endregion
    }
}