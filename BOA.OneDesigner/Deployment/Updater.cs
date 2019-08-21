using System;
using System.Threading;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.Deployment
{
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

            const string url = @"https://github.com/beyaz/WhiteStone/blob/master/BOA.OneDesigner/dist/BOA.OneDesigner.zip?raw=true";

            try
            {
                FileHelper.DownloadFile(url, @"d:\BOA\BOA.OneDesigner.zip", true);
            }
            catch (Exception e)
            {
                Log.Push(e);
            }
        }
        #endregion
    }
}