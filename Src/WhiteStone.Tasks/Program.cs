using System;
using System.IO;
using System.Linq;
using WhiteStone.Services;

namespace WhiteStone.Tasks
{
    class Program
    {
        #region Methods
        static void Main(string[] args)
        {
             // args = new[] {@"D:\github\FundManagement\FundManagement\TasksForPrepareOutputs.js"};
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }

            var tasksFilePath = args.First();

            try
            {
                var jsonContent = File.ReadAllText(tasksFilePath);
                var infoFile = new JsonSerializer().Deserialize<TaskInfoFile>(jsonContent);

                new TaskInfoFileNormalizer {TaskInfoFile = infoFile}.Normalize();

                foreach (var taskInfo in infoFile.Tasks)
                {
                    TaskRunner.Run(taskInfo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                Console.Read();
            }
        }
        #endregion
    }
}