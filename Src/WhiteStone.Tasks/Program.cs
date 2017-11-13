using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using WhiteStone.Services;

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

    [Serializable]
    public class TaskInfo
    {
        #region Public Properties
        public string FullClassName { get; set; }
        public IDictionary<string, string> Keys { get; set; }
        #endregion
    }

    [Serializable]
    public class TaskInfoFile
    {
        #region Public Properties
        public IDictionary<string, string> GlobalKeys { get; set; }

        public ICollection<TaskInfo> Tasks { get; set; }
        #endregion
    }

    public class GetFileContentFromWebIfNotInTarggetDir : ITask
    {
        #region Public Properties
        public IDictionary<string, string> Keys { get; set; }
        #endregion

        #region Properties
        string SourceUrl => Keys[nameof(SourceUrl)];
        string TargetDirectory => Keys[nameof(TargetDirectory)];
        #endregion

        #region Public Methods
        public void Run()
        {
            var fileName = Path.GetFileName(SourceUrl);

            var targetFile = Path.Combine(TargetDirectory, fileName.AssertNotNull());
            if (!File.Exists(targetFile))
            {
                var content = FileHelper.DownloadString(SourceUrl);

                FileHelper.WriteAllText(targetFile, content);
            }
        }
        #endregion
    }

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

namespace WhiteStone.Tasks
{
    class Program
    {
        #region Methods
        static void Main(string[] args)
        {
            // args = new[] {@"D:\github\TicketWebsite\TasksForPrepareOutputs.js"};
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }

            var tasksFilePath = args.First();

            try
            {
                var jsonContent = File.ReadAllText(tasksFilePath);
                var infoFile = new JsonSerializer().Deserialize<TaskInfoFile>(jsonContent);
                foreach (var taskInfo in infoFile.Tasks)
                {
                    TaskRunner.Run(taskInfo);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                Console.WriteLine(tasksFilePath);

                throw;
            }
        }
        #endregion
    }
}