using System.IO;

namespace BOAPlugins
{
    public class ConstConfiguration
    {
        #region Constants
        public const string DllDataSourceDirectory = "https://github.com/beyaz/WhiteStone/blob/master/BOA.HelperTools/BOAPlugins.VSIntegration/DeepEnds/";
        #endregion

        #region Public Properties
        public static string ConfigurationJsonFilePath => PluginDirectory + "BOAPlugins.VSIntegration.Configuration.json";
        public static string DeepEndsDirectory         => PluginDirectory + "DeepEnds" + Path.DirectorySeparatorChar;
        public static string PluginDirectory           => Path.GetDirectoryName(typeof(ConstConfiguration).Assembly.Location) + Path.DirectorySeparatorChar;
        #endregion
    }
}