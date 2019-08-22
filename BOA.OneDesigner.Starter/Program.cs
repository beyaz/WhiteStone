using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace BOA.OneDesigner.Starter
{
    static class Config
    {
        #region Constants
        public const string ExportDir   = @"D:\BOA\BOA.OneDesigner\";
        public const string ProcessPath = @"D:\BOA\BOA.OneDesigner\WhiteStone.UI.Container.exe";
        public const string ZipFilePath = @"D:\BOA\BOA.OneDesigner.zip";
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