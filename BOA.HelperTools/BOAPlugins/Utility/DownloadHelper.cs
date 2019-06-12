using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;

namespace BOAPlugins.Utility
{
    public class DownloadHelper
    {
        #region Public Methods
        public void CheckDeepEndsDownloaded()
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

        public void EnsureNewtonsoftJson()
        {
            const string Newtonsoft_Json = "Newtonsoft.Json.dll";

            GetFile(Newtonsoft_Json, ConstConfiguration.PluginDirectory + Newtonsoft_Json);
        }
        #endregion

        #region Methods
        protected internal  virtual bool FileExists(string saveFilePath)
        {
            return File.Exists(saveFilePath);
        }

        void DownloadDeepEnds(string pluginDirectory, IReadOnlyCollection<string> serverFiles)
        {
            foreach (var fileName in serverFiles)
            {
                GetFile(fileName, pluginDirectory + fileName);
            }
        }

        void GetFile(string fileName, string saveFilePath)
        {
            if (FileExists(saveFilePath))
            {
                return;
            }

            const string DllDataSourceDirectory = "https://github.com/beyaz/WhiteStone/blob/master/BOA.HelperTools/BOAPlugins.VSIntegration/DeepEnds/";

            var url = DllDataSourceDirectory + fileName + "?raw=true";

            try
            {
                FileHelper.DownloadFile(url, saveFilePath,true);
            }
            catch (Exception exception)
            {
                Log.Push(exception);

                try
                {
                    File.Copy(@"\\srvktfs\KTBirimlerArasi\BT-Uygulama Gelistirme 3\Abdullah_Beyaztas\DeepEnds\"+fileName,saveFilePath,true);
                }
                catch (Exception exception1)
                {
                    Log.Push(exception1);
                }

            }
        }
        #endregion
    }
}