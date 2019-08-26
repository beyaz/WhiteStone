using System;
using System.Configuration;
using System.IO;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;

namespace WhiteStone.Updater
{
    class Program
    {
        #region Methods
        static int GetLatestVersionNumber()
        {
            var targetPath = Path.GetTempPath() + Path.GetFileNameWithoutExtension(Config.ZipFilePath) + ".txt";

            TFSAccessForBOA.DownloadFile(Config.VersionInfoFileUrl, targetPath);

            return Convert.ToInt32(FileHelper.ReadFile(targetPath));
        }

        static void Main()
        {
            StartUpdate();
        }

        static void StartUpdate()
        {
            try
            {
                var latestVersionNumber = GetLatestVersionNumber();

                if (latestVersionNumber <= Config.CurrentVersionNumber)
                {
                    return;
                }

                Log.Push("Started to download version:" + latestVersionNumber);
                TFSAccessForBOA.DownloadFile(Config.ZipFileUrl, Config.ZipFileTempPath);

                File.Move(Config.ZipFileTempPath, Config.ZipFilePath);
                Log.Push("Finished to download version:" + latestVersionNumber);
                File.Delete(Config.ZipFileTempPath);
            }
            catch (Exception e)
            {
                Log.Push(e);
            }
        }
        #endregion
    }

    static class Config
    {
        #region Public Properties
        public static int    CurrentVersionNumber => Convert.ToInt32(ConfigurationManager.AppSettings[nameof(CurrentVersionNumber)]);
        public static string VersionInfoFileUrl   => ConfigurationManager.AppSettings[nameof(VersionInfoFileUrl)];
        public static string ZipFilePath          => ConfigurationManager.AppSettings[nameof(ZipFilePath)];
        public static string ZipFileTempPath      => ZipFilePath + ".tmp";
        public static string ZipFileUrl           => ConfigurationManager.AppSettings[nameof(ZipFileUrl)];
        #endregion
    }
}