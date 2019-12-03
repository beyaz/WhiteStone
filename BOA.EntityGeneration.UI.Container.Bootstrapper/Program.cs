using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using BOA.CodeGeneration.Util;
using BOA.EntityGeneration.UI.Container.Bootstrapper.Infrastructure;

namespace BOA.EntityGeneration.UI.Container.Bootstrapper
{
    class Program
    {
        #region Static Fields
        static readonly Config Config = Config.CreateFromFile();
        #endregion

        #region Properties
        static string ZipFileTempPath => Config.ZipFilePath + ".tmp";
        #endregion

        #region Public Methods
        public static void Main(string[] args)
        {
            EmbeddedZippedAssemblyResolver.Attach(new EmbeddedZippedAssemblyResolverData
            {
                Assembly                       = typeof(Program).Assembly,
                AppDomain                      = AppDomain.CurrentDomain,
                EmbeddedResourcePathInAssembly = "BOA.EntityGeneration.UI.Container.Bootstrapper.References.BOA.TfsAccess.zip"
            });
            EmbeddedZippedAssemblyResolver.Attach(new EmbeddedZippedAssemblyResolverData
            {
                Assembly                       = typeof(Program).Assembly,
                AppDomain                      = AppDomain.CurrentDomain,
                EmbeddedResourcePathInAssembly = "BOA.EntityGeneration.UI.Container.Bootstrapper.References.WhiteStone.dll"
            });

            TryToExtractZipFile();

            if (!File.Exists(Config.ProcessPath))
            {
                Console.WriteLine("Yükleniyor...");

                DownloadFile();

                Console.WriteLine("Yüklendi.");

                File.Move(ZipFileTempPath, Config.ZipFilePath);

                TryToExtractZipFile();
            }

            if (args?.Length > 0)
            {
                var arguments = string.Join("|", args);

                Console.WriteLine("Starting process with argument:" + arguments);

                Process.Start(Config.ProcessPath, arguments);
            }
            else
            {
                Console.WriteLine("Starting process...");
                Process.Start(Config.ProcessPath);
            }

            Console.WriteLine("Process is started.");
        }
        #endregion

        #region Methods
        static void DownloadFile()
        {
            TFSAccessForBOA.DownloadFile(Config.ZipFileUrl, ZipFileTempPath);
        }

        static void TryToExtractZipFile()
        {
            if (File.Exists(Config.ZipFilePath))
            {
                if (Directory.Exists(Config.ExportDir))
                {
                    Console.WriteLine("Deleting folder: " + Config.ExportDir);
                    Directory.Delete(Config.ExportDir, true);
                    Console.WriteLine("Deleted folder: " + Config.ExportDir);
                }

                Console.WriteLine("Exporting zip file: " + Config.ZipFilePath);
                ZipFile.ExtractToDirectory(Config.ZipFilePath, Config.ExportDir);

                Console.WriteLine("Deleting zip file: " + Config.ZipFilePath);
                File.Delete(Config.ZipFilePath);
                Console.WriteLine("Deleted zip file: " + Config.ZipFilePath);
            }
        }
        #endregion
    }
}