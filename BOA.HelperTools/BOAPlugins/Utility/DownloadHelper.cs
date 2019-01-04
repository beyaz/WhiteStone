using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOAPlugins.VSIntegration;

namespace BOAPlugins.Utility
{
    public class DownloadHelper
    {
        #region Public Methods
        public static void CheckDeepEndsDownloaded(Host host)
        {
            var serverFiles = new[]
            {
                "Newtonsoft.Json.dll",
                "ClangSharp.dll",
                "DeepEnds.Console.exe",
                "DeepEnds.Console.exe.config",
                "DeepEnds.Core.dll",
                "DeepEnds.Cpp.dll",
                "DeepEnds.CSharp.dll",
                "DeepEnds.Decompile.dll",
                "DeepEnds.DGML.dll",
                "DeepEnds.DoxygenXml.dll",
                "DeepEnds.Reporting.dll",
                "DeepEnds.VBasic.dll",
                "libclang.dll",
                "Microsoft.CodeAnalysis.CSharp.dll",
                "Microsoft.CodeAnalysis.dll",
                "Microsoft.CodeAnalysis.VisualBasic.dll",
                "Microsoft.VisualStudio.Diagnostics.PerformanceProvider.dll",
                "Microsoft.VisualStudio.GraphModel.dll",
                "Microsoft.VisualStudio.OLE.Interop.dll",
                "Mono.Cecil.dll",
                "System.Collections.Immutable.dll",
                "System.Reflection.Metadata.dll"
            };

            DownloadDeepEnds(ConstConfiguration.BOAPluginDirectory_DeepEnds, serverFiles);
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

            const string DllDataSourceDirectory = "https://github.com/beyaz/WhiteStone/blob/master/BOA.HelperTools/BOAPlugins.VSIntegration/DeepEnds/";

            var url = DllDataSourceDirectory + fileName + "?raw=true";

            FileHelper.DownloadFile(url, saveFilePath, true);
        }
        #endregion
    }
}