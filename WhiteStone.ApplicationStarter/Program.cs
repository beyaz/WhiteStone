using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace WhiteStone.ApplicationStarter
{
    static class Config
    {
        #region Public Properties
        public static string ExportDir       => ConfigurationManager.AppSettings[nameof(ExportDir)];
        public static string ProcessPath     => ConfigurationManager.AppSettings[nameof(ProcessPath)];
        public static string ZipFilePath     => ConfigurationManager.AppSettings[nameof(ZipFilePath)];
        public static string ZipFileTempPath => ZipFilePath + ".tmp";
        public static string ZipFileUrl      => ConfigurationManager.AppSettings[nameof(ZipFileUrl)];
        #endregion
    }

    class Program
    {
        #region Public Methods
        public static void Main(string[] args)
        {
            TryToExtractZipFile();

            if (!File.Exists(Config.ProcessPath))
            {
                Console.WriteLine("Yükleniyor...");
                FileHelper.DownloadFile(Config.ZipFileUrl, Config.ZipFileTempPath, true);
                Console.WriteLine("Yüklendi.");

                File.Move(Config.ZipFileTempPath, Config.ZipFilePath);

                TryToExtractZipFile();
            }

            if (args?.Length > 0)
            {
                var arguments = string.Join("|", args);

                Console.WriteLine("Starting process with argument:"+arguments);
                
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
        static void TryToExtractZipFile()
        {
            if (File.Exists(Config.ZipFilePath))
            {
                if (Directory.Exists(Config.ExportDir))
                {
                    Console.WriteLine("Deleting folder: "+Config.ExportDir);
                    Directory.Delete(Config.ExportDir, true);
                    Console.WriteLine("Deleted folder: "+Config.ExportDir);
                }

                Console.WriteLine("Exporting zip file: "+Config.ZipFilePath);
                ZipFile.ExtractToDirectory(Config.ZipFilePath, Config.ExportDir);

                Console.WriteLine("Deleting zip file: "+Config.ZipFilePath);
                File.Delete(Config.ZipFilePath);
                Console.WriteLine("Deleted zip file: "+Config.ZipFilePath);
            }
        }
        #endregion
    }

    static class FileHelper
    {
        #region Public Methods
        public static void CreateDirectoryIfNotExists(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }

        /// <summary>
        ///     Downloads the file.
        /// </summary>
        public static void DownloadFile(string address, string saveFilePath, bool useSsl)
        {
            if (useSsl)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }

            CreateDirectoryIfNotExists(Path.GetDirectoryName(saveFilePath));

            new WebClient().DownloadFile(address, saveFilePath);
        }
        #endregion
    }
}