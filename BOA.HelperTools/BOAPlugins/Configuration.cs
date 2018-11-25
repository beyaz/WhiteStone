using System;
using System.IO;
using System.Linq;
using System.Net;
using BOA.Common.Helpers;
using WhiteStone;
using WhiteStone.Services;

namespace BOAPlugins
{
    [Serializable]
    public class Configuration
    {
        #region Public Properties
        public static string PluginDirectory              => Path.GetDirectoryName(typeof(Configuration).Assembly.Location) + Path.DirectorySeparatorChar;
        public        string CheckInCommentDefaultValue   { get; set; }
        public        bool   DeepEndsAssembliesDownloaded { get; set; }
        #endregion

        #region Properties
        static string      ConfigurationJsonFilePath => PluginDirectory + "BOAPlugins.VSIntegration.Configuration.json";
        static ISerializer Serializer                => SM.Get<ISerializer>() ?? SM.Set<ISerializer>(new JsonSerializer());
        #endregion

        #region Public Methods
        public static void CheckDeepEndsDownloaded()
        {
            var configuration = SM.Get<Configuration>();
            if (configuration.DeepEndsAssembliesDownloaded)
            {
                return;
            }

            DownloadDeepEnds();

            configuration.DeepEndsAssembliesDownloaded = true;

            SaveToFile();

            LoadFromFile();

            Log.Push(nameof(CheckDeepEndsDownloaded));
        }

        public static void LoadFromFile()
        {
            var configurationAsString = File.ReadAllText(ConfigurationJsonFilePath);

            var config = Serializer.Deserialize<Configuration>(configurationAsString);

            SM.Set(config);
        }

        public static void SaveToFile()
        {
            File.WriteAllText(ConfigurationJsonFilePath, Serializer.Serialize(SM.Get<Configuration>()));
        }
        #endregion

        #region Methods
        internal static void DownloadDeepEnds()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            const string targetDir = "https://github.com/beyaz/WhiteStone/blob/master/BOA.HelperTools/BOAPlugins.VSIntegration/DeepEnds/";

            var sourceDir = @"D:\github\WhiteStone\BOA.HelperTools\BOAPlugins.VSIntegration\DeepEnds\";

            var fileNames = Directory.GetFiles(sourceDir).Select(Path.GetFileName).ToArray();

            foreach (var fileName in fileNames)
            {
                var url          = targetDir + fileName + "?raw=true";
                var saveFilePath = PluginDirectory + "DeepEnds" + Path.DirectorySeparatorChar + fileName;

                if (File.Exists(saveFilePath))
                {
                    continue;
                }

                FileHelper.DownloadFile(url, saveFilePath, true);
            }
        }
        #endregion
    }
}