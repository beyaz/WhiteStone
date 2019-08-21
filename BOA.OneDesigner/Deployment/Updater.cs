using System;
using System.IO;
using System.Threading;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.Deployment
{
    public static class Updater
    {
        #region Constants
        const string versionInfoFileUrl = @"https://github.com/beyaz/WhiteStone/blob/master/BOA.OneDesigner/dist/BOA.OneDesigner.txt?raw=true";
        const string zipFileUrl         = @"https://github.com/beyaz/WhiteStone/blob/master/BOA.OneDesigner/dist/BOA.OneDesigner.zip?raw=true";
        #endregion

        static int CurrentVersionNumber = 0;

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
                if (latestVersionNumber <= CurrentVersionNumber)
                {
                    return;
                }

                Log.Push("Started to download version:"+latestVersionNumber);
                FileHelper.DownloadFile(zipFileUrl, @"d:\BOA\BOA.OneDesigner.zip", true);
                Log.Push("Finished to download version:"+latestVersionNumber);
            }
            catch (Exception e)
            {
                Log.Push(e);
            }
        }

        static int GetLatestVersionNumber()
        {
            var targetPath = Path.GetTempPath() + "BOA.OneDesigner.LatestVersion.txt";

            FileHelper.DownloadFile(versionInfoFileUrl, targetPath, true);

            return Convert.ToInt32(FileHelper.ReadFile(targetPath));
        }
        #endregion
    }
}