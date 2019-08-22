using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace WhiteStone.ApplicationStarter
{
    static class Config
    {
        #region Public Properties
        public static string ExportDir   => ConfigurationManager.AppSettings[nameof(ExportDir)];
        public static string ProcessPath => ConfigurationManager.AppSettings[nameof(ProcessPath)];
        public static string ZipFilePath => ConfigurationManager.AppSettings[nameof(ZipFilePath)];
        #endregion
    }

    class Program
    {
        #region Public Methods
        public static void Main()
        {
            if (File.Exists(Config.ZipFilePath))
            {
                if (Directory.Exists(Config.ExportDir))
                {
                    Directory.Delete(Config.ExportDir, true);
                }

                ZipFile.ExtractToDirectory(Config.ZipFilePath, Config.ExportDir);
                File.Delete(Config.ZipFilePath);
            }

            if (!File.Exists(Config.ProcessPath))
            {
                throw new FileNotFoundException(Config.ProcessPath);
            }

            Process.Start(Config.ProcessPath);
        }
        #endregion
    }
}