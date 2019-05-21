using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using WhiteStone.Services;

namespace WhiteStone.Tasks
{
    /// <summary>
    /// The combine files into js file data
    /// </summary>
    [Serializable]
    public class CombineFilesIntoJsFileData
    {
        #region Public Properties
        public bool?  ClearXml     { get; set; }
        public string JsObjectPath { get; set; }
        public string Source       { get; set; }
        public string Destination       { get; set; } 
        #endregion
    }

    public class CombineFilesIntoJsFile
    {
        #region Public Methods
        public static void Run(CombineFilesIntoJsFileData data)
        {
            var strings = data.Source.Split('*');

            var dir = strings[0];

            var pattern = "*" + strings[1];

            var dictionary = new Dictionary<string, string>();
            foreach (var filePath in Directory.GetFileSystemEntries(dir, pattern, SearchOption.AllDirectories))
            {
                var fileContent = File.ReadAllText(filePath);

                var isXmlFile = filePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase);

                if (data.ClearXml == true && isXmlFile)
                {
                    fileContent = XmlHelper.ClearXml(fileContent);
                }

                dictionary[GetDictionaryKey(dir, filePath)] = fileContent;
            }

            FileHelper.WriteAllText(data.Destination, data.JsObjectPath + " = " + new JsonSerializer().Serialize(dictionary));
        }
        #endregion

        #region Methods
        static string GetDictionaryKey(string relativeDir, string filePath)
        {
            var folderName = new DirectoryInfo(relativeDir).Name.Replace(".", "/");

            filePath = filePath.RemoveFromStart(relativeDir);

            filePath = Path.Combine(folderName, filePath);

            filePath = filePath.Replace(Path.DirectorySeparatorChar.ToString(), "/");

            return filePath;
        }
        #endregion
    }
}