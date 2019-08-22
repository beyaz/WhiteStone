using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
                FileHelper.DownloadFile(Config.ZipFileUrl, Config.ZipFileTempPath, true);

                File.Move(Config.ZipFileTempPath, Config.ZipFilePath);

                TryToExtractZipFile();
            }

            if (args?.Length > 0)
            {
                Process.Start(Config.ProcessPath, string.Join("|", args));
            }
            else
            {
                Process.Start(Config.ProcessPath);
            }
        }
        #endregion

        #region Methods
        static void TryToExtractZipFile()
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