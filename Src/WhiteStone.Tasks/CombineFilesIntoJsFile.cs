using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using WhiteStone.Services;

namespace WhiteStone.Tasks
{
    public class CombineFilesIntoJsFile : ITask
    {
        #region Public Properties
        public IDictionary<string, string> Keys { get; set; }
        #endregion

        #region Properties
        string JsObjectPath => Keys[nameof(JsObjectPath)];
        string Source => Keys[nameof(Source)];
        string Target => Keys[nameof(Target)];
        #endregion

        #region Public Methods
        public void Run()
        {
            var strings = Source.Split('*');
            var dir = strings[0];
            var pattern = "*" + strings[1];

            var dictionary = new Dictionary<string, string>();
            foreach (var filePath in Directory.GetFileSystemEntries(dir, pattern))
            {
                dictionary[filePath] = File.ReadAllText(filePath);
            }

            FileHelper.WriteAllText(Target, JsObjectPath + " = " + new JsonSerializer().Serialize(dictionary));
        }
        #endregion
    }
}