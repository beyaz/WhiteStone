﻿using System.Diagnostics;
using System.IO;

namespace CustomSqlInjectionToProject.Deployment
{
    public static class Updater
    {
        #region Public Methods
        public static void StartUpdate()
        {
            Process.Start($"{Path.GetDirectoryName(typeof(Updater).Assembly.Location)}{Path.DirectorySeparatorChar}WhiteStone.Updater.exe");
        }
        #endregion
    }
}