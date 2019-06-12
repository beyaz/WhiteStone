using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BOA.Common.Helpers;
using BOAPlugins.Utility;

namespace BOAPlugins.VSIntegration
{
    class Factory
    {
        #region Static Fields
        static bool _isInitialized;
        #endregion

        #region Properties
        internal static string WhiteStoneBinDirectory => ConstConfiguration.PluginDirectory + "bin" + Path.DirectorySeparatorChar;
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

            var host = new Host();

            SM.Set(host);

            if (Debugger.IsAttached)
            {
                AppDomain.CurrentDomain.AddAssemblySearchDirectory(@"D:\work\BOA.Retired\Dev\BOA.Kernel.DataAccess\BOAPlugins.VSIntegration\bin\Debug\");
            }
            else
            {
                AppDomain.CurrentDomain.AddAssemblySearchDirectory(ConstConfiguration.PluginDirectory);
                AppDomain.CurrentDomain.AddAssemblySearchDirectory(WhiteStoneBinDirectory);
            }
            
            Log.Push("Started to download deepends");
            Task.Run(() => new DownloadHelper().CheckDeepEndsDownloaded());
            Log.Push("Started to download deepends passed.");
        }
        #endregion
    }
}