using System.IO;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.AppModel
{
    class InitialConfigCache
    {
        #region Properties
        static string FilePath => Log.Directory + $"{nameof(InitialConfig)}.json";
        #endregion

        #region Public Methods
        public static void Save()
        {
            FileHelper.WriteAllText(FilePath, JsonHelper.Serialize(SM.Get<InitialConfig>()));
        }

        public static void TryLoadFromCache()
        {
            if (File.Exists(FilePath) == false)
            {
                return;
            }
            
            SM.Set(JsonHelper.Deserialize<InitialConfig>(FileHelper.ReadFile(FilePath)));
        }
        #endregion
    }
}