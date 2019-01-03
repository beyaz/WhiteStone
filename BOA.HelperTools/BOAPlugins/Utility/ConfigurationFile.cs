using System.IO;
using BOA.Common.Helpers;

namespace BOAPlugins.Utility
{
    public class ConfigurationFile
    {
        public static void LoadFromFile()
        {
            var configurationAsString = File.ReadAllText(ConstConfiguration.ConfigurationJsonFilePath);

            SM.Set(JsonHelper.Deserialize<Configuration>(configurationAsString));
        }

        public static void SaveToFile()
        {
            File.WriteAllText(ConstConfiguration.ConfigurationJsonFilePath, JsonHelper.Serialize(SM.Get<Configuration>()));
        }

        public void Save(Configuration configuration)
        {
            throw new System.NotImplementedException();
        }

        public Configuration Load()
        {
            throw new System.NotImplementedException();
        }
    }
}