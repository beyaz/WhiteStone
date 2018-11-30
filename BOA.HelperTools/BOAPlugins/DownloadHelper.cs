using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using WhiteStone;

namespace BOAPlugins
{
    public class DownloadHelper
    {
        #region Public Methods
        public static void CheckDeepEndsDownloaded()
        {
            var configuration = SM.Get<Configuration>();
            if (configuration.DeepEndsAssembliesDownloaded)
            {
                return;
            }

            DownloadDeepEnds(ConstConfiguration.DeepEndsDirectory, configuration.ServerFiles);

            configuration.DeepEndsAssembliesDownloaded = true;

            Configuration.SaveToFile();

            Configuration.LoadFromFile();

            Log.Push(nameof(CheckDeepEndsDownloaded));
        }

        public static void EnsureNewtonsoftJson()
        {
            const string Newtonsoft_Json = "Newtonsoft.Json.dll";

            GetFile(Newtonsoft_Json, ConstConfiguration.PluginDirectory + Newtonsoft_Json);
        }
        #endregion

        #region Methods
        internal static void DownloadDeepEnds(string pluginDirectory, IReadOnlyCollection<string> serverFiles)
        {
            foreach (var fileName in serverFiles)
            {
                GetFile(fileName, pluginDirectory + fileName);
            }
        }

        static void GetFile(string fileName, string saveFilePath)
        {
            if (File.Exists(saveFilePath))
            {
                return;
            }

            var url = ConstConfiguration.DllDataSourceDirectory + fileName + "?raw=true";

            FileHelper.DownloadFile(url, saveFilePath, true);
        }
        #endregion
    }
}