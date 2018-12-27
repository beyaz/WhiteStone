using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.UI.Types;
using BOA.UnitTestHelper;

namespace BOA.OneDesigner.AppModel
{
    public interface IDatabase
    {
        #region Public Methods
        List<string>          GetDefaultRequestNames();
        ScreenInfo            GetScreenInfo(string requestName);
        IReadOnlyList<string> GetTfsFolderNames();
        void                  Save(ScreenInfo screenInfo);
        #endregion

        IReadOnlyList<string> GetMessagingGroupNames();
    }

    class CacheHelper : IDatabase
    {
        static BOATestContextDev Dev => new BOATestContextDev();

        public IReadOnlyList<string> GetMessagingGroupNames()
        {
            
            var path = CacheDirectory + nameof(GetMessagingGroupNames) + ".cache";
            if (File.Exists(path))
            {
                return BinarySerialization.Deserialize<List<string>>(File.ReadAllBytes(path));
            }

            var items =  Dev.GetRecords<Pair>("select DISTINCT(Name) as [Key] from BOA.COR.MessagingGroup WITH(NOLOCK)").Select(x=>x.Key).ToList();

            File.WriteAllBytes(path,BinarySerialization.Serialize(items));

            return items;
        }

        #region Properties
        static string CacheDirectory => Log.Directory + "Cache" + Path.DirectorySeparatorChar;
        static string FilePath       => CacheDirectory + $"{nameof(GetTfsFolderNames)}.json";
        #endregion

        #region Public Methods
        public List<string> GetDefaultRequestNames()
        {
            return Directory.GetFiles(CacheDirectory)
                            .Where(x => x != FilePath && !x.Contains(nameof(GetMessagingGroupNames)))
                            .Select(Path.GetFileNameWithoutExtension)
                            .ToList();
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