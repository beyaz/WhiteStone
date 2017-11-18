using System;

namespace WhiteStone.Tasks
{
    class TaskRunner
    {
        #region Public Methods
        public static void Run(TaskInfo taskInfo)
        {
            var task = (ITask) Activator.CreateInstance(Type.GetType(taskInfo.FullClassName).AssertNotNull());
            task.Keys = taskInfo.Keys;
            task.Run();
        }
        #endregion
    }
}