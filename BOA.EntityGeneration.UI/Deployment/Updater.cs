using System;
using System.IO;
using System.Threading;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.UI.Deployment
{
    static class Config
    {
        #region Constants
        public const int    CurrentVersionNumber = 1;
        public const string VersionInfoFileUrl   = @"https://github.com/beyaz/WhiteStone/blob/master/BOA.EntityGeneration.UI/dist/BOA.EntityGeneration.UI.txt?raw=true";
        public const string ZipFilePath          = @"d:\BOA\EntityGenerator.zip";
        public const string ZipFileTempPath      = @"d:\BOA\EntityGenerator.zip.tmp";
        public const string zipFileUrl           = @"https://github.com/beyaz/WhiteStone/blob/master/BOA.EntityGeneration.UI/dist/BOA.EntityGeneration.UI.zip?raw=true";
        #endregion
    }

    public static class Updater
    {
        #region Public Methods
        public static void StartUpdate()
        {
            new Thread(Fetch).Start();
        }
        #endregion

        #region Methods
        static void Fetch()
        {
            Thread.Sleep(3000);

            try
            {
                var latestVersionNumber = GetLatestVersionNumber();
                if (latestVersionNumber <= Config.CurrentVersionNumber)
                {
                    return;
                }

                Log.Push("Started to download version:" + latestVersionNumber);
                FileHelper.DownloadFile(Config.zipFileUrl, Config.ZipFileTempPath, true);

                File.Move(Config.ZipFileTempPath, Config.ZipFilePath);
                Log.Push("Finished to download version:" + latestVersionNumber);
                File.Delete(Config.ZipFileTempPath);
            }
            catch (Exception e)
            {
                Log.Push(e);
            }
        }

        static int GetLatestVersionNumber()
        {
            var targetPath = Path.GetTempPath() + typeof(Updater).Namespace +".txt";

            FileHelper.DownloadFile(Config.VersionInfoFileUrl, targetPath, true);

            return Convert.ToInt32(FileHelper.ReadFile(targetPath));
        }
        #endregion
    }
}