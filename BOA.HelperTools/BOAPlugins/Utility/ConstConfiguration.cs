using System;
using System.IO;

namespace BOAPlugins.Utility
{
    /// <summary>
    ///     The constant configuration
    /// </summary>
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
        /// <summary>
        ///     Gets the boa plugin directory.
        /// </summary>
        public static string BOAPluginDirectory => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar +
                                                   "BOAPlugin" + Path.DirectorySeparatorChar;

        /// <summary>
        ///     Gets the boa plugin directory deep ends.
        /// </summary>
        public static string BOAPluginDirectory_DeepEnds => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar +
                                                            "BOAPlugin" + Path.DirectorySeparatorChar + "DeepEnds" + Path.DirectorySeparatorChar;

        /// <summary>
        ///     Gets the plugin directory.
        /// </summary>
        public static string PluginDirectory => Path.GetDirectoryName(typeof(ConstConfiguration).Assembly.Location) + Path.DirectorySeparatorChar;
        #endregion
    }
}