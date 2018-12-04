using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BOA.Common.Helpers;
using BOAPlugins.Utility;
using WhiteStone;
using WhiteStone.Services;

namespace BOAPlugins.VSIntegration
{
    class Factory
    {
        #region Static Fields
        static bool _isInitialized;
        #endregion

        #region Properties
        internal static Configuration Configuration => SM.Get<Configuration>();

        internal static string      WhiteStoneBinDirectory => ConstConfiguration.PluginDirectory + "bin" + Path.DirectorySeparatorChar;
        static          ISerializer Serializer             => SM.Get<ISerializer>() ?? SM.Set<ISerializer>(new JsonSerializer());
        #endregion

        #region Public Methods
        public static Communication GetCommunication(IVisualStudioLayer vs)
        {
            return new Communication(vs);
        }

        public static void InitializeApplicationServices()
        {
            if (_isInitialized)
            {
                Log.Push("Already initialized.");
                return;
            }

            Log.Push("Started to initialize.");

            _isInitialized = true;

            if (Debugger.IsAttached)
            {
                AppDomain.CurrentDomain.AddAssemblySearchDirectory(@"D:\work\BOA.Retired\Dev\BOA.Kernel.DataAccess\BOAPlugins.VSIntegration\bin\Debug\");
            }
            else
            {
                AppDomain.CurrentDomain.AddAssemblySearchDirectory(ConstConfiguration.PluginDirectory);
                AppDomain.CurrentDomain.AddAssemblySearchDirectory(WhiteStoneBinDirectory);
            }

            DownloadHelper.EnsureNewtonsoftJson();

            Configuration.LoadFromFile();

            Task.Run(() => DownloadHelper.CheckDeepEndsDownloaded());
        }
        #endregion
    }
}