using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using BOA.UI.Types;
using BOA.UnitTestHelper;
using BOAPlugins.Messaging;

namespace BOA.OneDesigner.AppModel
{

    class JsonFile : IDatabase
    {
        #region Properties
        static string            CacheDirectory => Log.Directory + "Cache" + Path.DirectorySeparatorChar;
        static DevelopmentDatabase Dev            => new DevelopmentDatabase();
        static string            FilePath       => CacheDirectory + $"{nameof(GetTfsFolderNames)}.json";
        #endregion

        #region Public Methods
        public List<string> GetDefaultRequestNames()
        {
            return Directory.GetFiles(CacheDirectory)
                            .Select(Path.GetFileNameWithoutExtension)
                            .Where(x => x.StartsWith("BOA."))
                            .ToList();
        }

        public IReadOnlyList<string> GetMessagingGroupNames()
        {
            var path = CacheDirectory + nameof(GetMessagingGroupNames) + ".cache";
            if (File.Exists(path))
            {
                return BinarySerialization.Deserialize<List<string>>(File.ReadAllBytes(path));
            }

            var items = Dev.GetRecords<Pair>("select DISTINCT(Name) as [Key] from BOA.COR.MessagingGroup WITH(NOLOCK)").Select(x => x.Key).ToList();

            File.WriteAllBytes(path, BinarySerialization.Serialize(items));

            return items;
        }

       

        public IList<PropertyInfo> GetPropertyNames(string groupName)
        {
            var path = CacheDirectory + nameof(GetPropertyNames) + ".cache";
            if (File.Exists(path))
            {
                return BinarySerialization.Deserialize<IList<PropertyInfo>>(File.ReadAllBytes(path));
            }
            
            var items = BOAPlugins.Messaging.DataSource.GetPropertyNames(groupName);

            File.WriteAllBytes(path, BinarySerialization.Serialize(items));

            return items;
        }

        public ScreenInfo GetScreenInfo(string requestName)
        {
            var path = GetSaveFilePath(requestName);
            if (File.Exists(path))
            {
                return BinarySerialization.Deserialize<ScreenInfo>(File.ReadAllBytes(path));
            }

            return null;
        }

        public IReadOnlyList<string> GetTfsFolderNames()
        {
            if (File.Exists(FilePath) == false)
            {
                return null;
            }

            return JsonHelper.Deserialize<IReadOnlyList<string>>(FileHelper.ReadFile(FilePath));
        }

        public void Save(ScreenInfo screenInfo)
        {
            var path = GetSaveFilePath(screenInfo.RequestName);

            FileHelper.CreateDirectoryIfNotExists(path);

            File.WriteAllBytes(path, BinarySerialization.Serialize(screenInfo));



            path = GetSaveFilePath(screenInfo.ResourceCode);

            FileHelper.CreateDirectoryIfNotExists(path);

            File.WriteAllBytes(path, BinarySerialization.Serialize(screenInfo));
        }

        public void SaveTfsFolderNames(IReadOnlyList<string> items)
        {
            FileHelper.WriteAllText(FilePath, JsonHelper.Serialize(items));
        }
        #endregion

        #region Methods
        static string GetSaveFilePath(string requestName)
        {
            return CacheDirectory + requestName + ".json";
        }
        #endregion
    }
}