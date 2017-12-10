using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using WhiteStone.Helpers;
using WhiteStone.Services;

namespace WhiteStone.Tasks
{
    public class CombineFilesIntoJsFile : TaskBase
    {
        #region Properties
        bool? ClearXml => GetKeyAsBoolean(nameof(ClearXml));
        string JsObjectPath => GetKey(nameof(JsObjectPath));
        string Source => GetKey(nameof(Source));
        string Target => GetKey(nameof(Target));
        #endregion

        #region Public Methods
        public override void Run()
        {
            var strings = Source.Split('*');

            var dir = strings[0];

            var pattern = "*" + strings[1];

            var dictionary = new Dictionary<string, string>();
            foreach (var filePath in Directory.GetFileSystemEntries(dir, pattern, SearchOption.AllDirectories))
            {
                var fileContent = File.ReadAllText(filePath);

                var isXmlFile = filePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase);

                if (ClearXml == true && isXmlFile)
                {
                    fileContent = XmlHelper.ClearXml(fileContent);
                }

                dictionary[GetDictionaryKey(dir, filePath)] = fileContent;
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