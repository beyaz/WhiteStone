using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace BOA.EntityGeneration.UI.Starter
{
    class Program
    {
        #region Public Methods
        public static void Main()
        {
            const string ZipFilePath = @"D:\BOA\EntityGenerator.zip";
            const string ExportDir   = @"D:\BOA\EntityGenerator\";
            const string ProcessPath = @"D:\BOA\EntityGenerator\WhiteStone.UI.Container.exe";

            if (File.Exists(ZipFilePath))
            {
                if (Directory.Exists(ExportDir))
                {
                    Directory.Delete(ExportDir, true);
                }

                ZipFile.ExtractToDirectory(ZipFilePath, ExportDir);
                File.Delete(ZipFilePath);
            }

            if (!File.Exists(ProcessPath))
            {
                throw new FileNotFoundException(ProcessPath);
            }

            Process.Start(ProcessPath);
        }
        #endregion
    }
}