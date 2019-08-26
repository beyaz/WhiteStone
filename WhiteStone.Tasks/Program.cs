using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using BOA.Common.Helpers;
using WhiteStone.Services;

namespace WhiteStone.Tasks
{
    class Program
    {
        #region Methods
        static void Main(string[] args)
        {
              // args = new[] {@"D:\github\WhiteStone\WhiteStone.Tasks\bin\Debug\DeployTask.json"};
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }

            var tasksFilePath = args.First();

            try
            {
                Console.WriteLine($"Reading file:{tasksFilePath}");

                var jsonContent = File.ReadAllText(tasksFilePath);

                
                var taskParameters = (object[]) JsonHelper.DeserializeWithTypeName(jsonContent);

                foreach (var data in taskParameters)
                {
                    var parameterClass = data.GetType();

                    Console.WriteLine($"Searching task for :{parameterClass}");

                    var targetClass = parameterClass.FullName.RemoveFromEnd("Data");

                    

                    var targetType = parameterClass.Assembly.GetType(targetClass, true);

                    var methodInfo = targetType.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault();
                    if (methodInfo == null)
                    {
                        throw new MissingMemberException();
                    }

                    Console.WriteLine($"Executing task:{targetClass}::{methodInfo.Name}");
                    methodInfo.Invoke(null, new[] {data});

                    Console.WriteLine($"Executing task finished successfully:{targetClass}::{methodInfo.Name}");

                    Thread.Sleep(2000);
                }
            }
            catch (Exception e)
            {
                Log.Push(e);

                Console.WriteLine(e.ToString());

                throw e;
            }
        }
        #endregion
    }
}