using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using WhiteStone.Helpers;
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
            foreach (var filePath in Directory.GetFileSystemEntries(dir, pattern,SearchOption.AllDirectories))
            {
                dictionary[GetDictionaryKey(dir, filePath)] = File.ReadAllText(filePath);
            }

            FileHelper.WriteAllText(Target, JsObjectPath + " = " + new JsonSerializer().Serialize(dictionary));
        }
        #endregion

        #region Methods
        static string GetDictionaryKey(string relativeDir, string filePath)
        {
            var folderName = new DirectoryInfo(relativeDir).Name;

            filePath = filePath.RemoveFromStart(relativeDir);

            filePath = Path.Combine(folderName, filePath);

            filePath = filePath.Replace(Path.DirectorySeparatorChar.ToString(), "/");

            return filePath;
        }
        #endregion
    }
}