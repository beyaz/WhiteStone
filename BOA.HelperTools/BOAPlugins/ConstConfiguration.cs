using System.IO;

namespace BOAPlugins
{
    public class ConstConfiguration
    {
        #region Constants
        /// <summary>
        ///     Gets the client bin.
        /// </summary>
        public const string BoaClientBin = @"d:\boa\client\bin\";

        /// <summary>
        ///     Gets the server bin.
        /// </summary>
        public const string BoaServerBin = @"d:\boa\server\bin\";
        #endregion

        #region Public Properties
        public static string ConfigurationJsonFilePath => PluginDirectory + "BOAPlugins.VSIntegration.Configuration.json";
        public static string PluginDirectory           => Path.GetDirectoryName(typeof(ConstConfiguration).Assembly.Location) + Path.DirectorySeparatorChar;
        #endregion
    }
}