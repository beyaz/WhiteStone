using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using WhiteStone.Services;

namespace WhiteStone.Tasks
{
    public interface ITask
    {
        #region Public Methods
        void Run();
        #endregion
    }

    [Serializable]
    public class GetFileContentFromWebIfNotInTarggetDir : ITask
    {
        #region Public Properties
        public string SourceUrl { get; set; }
        public string TargetDirectory { get; set; }
        #endregion

        #region Public Methods
        public void Run()
        {
            var fileName = Path.GetFileName(SourceUrl);

            var targetFile = Path.Combine(TargetDirectory, fileName.AssertNotNull());
            if (!File.Exists(targetFile))
            {
                var content = new WebClient().DownloadString(SourceUrl);
                File.WriteAllText(targetFile, content);
            }
        }
        #endregion
    }


    class TaskRunner
    {
        #region Static Fields
        static readonly Dictionary<string, Type> Map = new Dictionary<string, Type>();
        #endregion

        static TaskRunner ()
        {
            Register(typeof(GetFileContentFromWebIfNotInTarggetDir));
        }

        static void Register(Type type)
        {
            Map[type.Name] = type;
        }

        public static void Run(string json)
        {
            GetTask(json).Run();
        }

        static ITask GetTask( string json)
        {
            foreach (var key in Map.Keys)
            {
                if (json.Contains(key))
                {
                    return  (ITask)new JsonSerializer().Deserialize(json, Map[key]);
                }
            }

            throw new ArgumentException(json);
        }
    }
}

namespace WhiteStone.Tasks
{
    class Program
    {
       

        #region Methods
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }
            
            var arg0 = args.First();

            TaskRunner.Run(arg0);
        }

       
        #endregion
    }
}