using System;
using System.Configuration;
using BOA.Common.Helpers;

namespace WhiteStone.Updater
{
    static class Config
    {
        #region Public Properties
        public static int    CurrentVersionNumber => Convert.ToInt32(ConfigurationManager.AppSettings[nameof(CurrentVersionNumber)]);
        public static string VersionInfoFileUrl   => ZipFileUrl.RemoveFromEnd(".zip") + ".txt";
        public static string ZipFilePath          => ConfigurationManager.AppSettings[nameof(ZipFilePath)];
        public static string ZipFileTempPath      => ZipFilePath + ".tmp";
        public static string ZipFileUrl           => ConfigurationManager.AppSettings[nameof(ZipFileUrl)];
        #endregion
    }
}