using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using WhiteStone;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    [Serializable]
    public class InitialConfig
    {
        #region Public Properties
        public        IReadOnlyList<string>  SolutionFileNames { get; set; }
        #endregion

       
    }

    class InitialConfigCache
    {
        #region Properties
        static string FilePath => Log.Directory + $"{nameof(InitialConfig)}.txt";
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

    [Serializable]
    public class WorkingOnScreenListItem
    {
        #region Public Properties
        public bool   IsSelected { get; set; }
        public string Name       { get; set; }
        #endregion
    }

    [Serializable]
    public class Model : ModelBase
    {
        #region Public Properties
        public bool DesignTabIsVisible     { get; set; }
        public bool ScreenInfoTabIsVisible { get; set; }

        public string ScreenSearchKey { get; set; }

        public string                        SolutionFilePath    { get; set; }
        public bool                          StartTabIsVisible   { get; set; }
        public List<WorkingOnScreenListItem> WorkingOnScreenList { get; set; }


        public IReadOnlyList<string> SolutionFileNames { get; set; }

        
        #endregion
    }
}