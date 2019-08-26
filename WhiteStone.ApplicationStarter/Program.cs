using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;

namespace WhiteStone.ApplicationStarter
{
    static class Config
    {
        #region Public Properties
        public static string ExportDir       => ConfigurationManager.AppSettings[nameof(ExportDir)];
        public static string ProcessPath     => ConfigurationManager.AppSettings[nameof(ProcessPath)];
        public static string ZipFilePath     => ConfigurationManager.AppSettings[nameof(ZipFilePath)];
        public static string ZipFileTempPath => ZipFilePath + ".tmp";
        public static string ZipFileUrl   => ConfigurationManager.AppSettings[nameof(ZipFileUrl)];
        #endregion
    }

    class Program
    {
        #region Public Methods
        public static void Main(string[] args)
        {
            var data = new EmbeddedZippedAssemblyResolverData
            {
                Assembly                       = typeof(Program).Assembly,
                AppDomain                      = AppDomain.CurrentDomain,
                EmbeddedResourcePathInAssembly = "WhiteStone.ApplicationStarter.References.BOA.TfsAccess.zip"
            };
            EmbeddedZippedAssemblyResolver.Attach(data);

            TryToExtractZipFile();

            if (!File.Exists(Config.ProcessPath))
            {
                Console.WriteLine("Yükleniyor...");

                DownloadFile();

                Console.WriteLine("Yüklendi.");

                File.Move(Config.ZipFileTempPath, Config.ZipFilePath);

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
            TFSAccessForBOA.DownloadFile(Config.ZipFileUrl, Config.ZipFileTempPath);
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