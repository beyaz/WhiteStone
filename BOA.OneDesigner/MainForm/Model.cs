using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    [Serializable]
    public class InitialConfig
    {
        #region Public Properties
        public static InitialConfig Instance          { get; set; }
        public        IReadOnlyList<string>  SolutionFileNames { get; set; }
        #endregion

        #region Properties
        static string filePath => Log.Directory + $"{nameof(InitialConfig)}.txt";
        #endregion

        #region Public Methods
        public static void Save()
        {
            if (Instance != null)
            {
                FileHelper.WriteAllText(filePath, JsonHelper.Serialize(Instance));
            }
        }

        public static void TryLoadFromCache()
        {
            if (File.Exists(filePath) == false)
            {
                return;
            }

            Instance = JsonHelper.Deserialize<InitialConfig>(FileHelper.ReadFile(filePath));
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

        
        #endregion
    }
}