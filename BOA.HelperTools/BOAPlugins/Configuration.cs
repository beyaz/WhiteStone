using System;
using System.IO;
using WhiteStone;
using WhiteStone.Services;

namespace BOAPlugins
{
    [Serializable]
    public class Configuration
    {
        #region Public Properties
        public string CheckInCommentDefaultValue { get; set; }
        #endregion

        #region Properties
        static string      path       => AssemlyUpdater.PluginDirectory + "BOAPlugins.VSIntegration.Configuration.json";
        static ISerializer Serializer => SM.Get<ISerializer>() ?? SM.Set<ISerializer>(new JsonSerializer());
        #endregion

        #region Public Methods
        public static void LoadFromFile()
        {
            var configurationAsString = File.ReadAllText(path);

            var config = Serializer.Deserialize<Configuration>(configurationAsString);

            SM.Set(config);
        }

        public static void SaveToFile()
        {
            File.WriteAllText(path, Serializer.Serialize(SM.Get<Configuration>()));
        }
        #endregion
    }
}