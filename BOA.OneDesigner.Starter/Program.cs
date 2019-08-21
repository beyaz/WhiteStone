using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace BOA.OneDesigner.Starter
{
    class Program
    {
        #region Public Methods
        public static void Main()
        {
            const string ZipFilePath = @"D:\BOA\BOA.OneDesigner.zip";
            const string ExportDir   = @"D:\BOA\BOA.OneDesigner\";
            const string ProcessPath = @"D:\BOA\BOA.OneDesigner\WhiteStone.UI.Container.exe";

            if (File.Exists(ZipFilePath))
            {
                Directory.Delete(ExportDir,true);
                ZipFile.ExtractToDirectory(ZipFilePath, ExportDir);
                // File.Delete(ZipFilePath);
            }

            Process.Start(ProcessPath);
        }
        #endregion
    }
}