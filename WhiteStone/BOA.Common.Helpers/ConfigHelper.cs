﻿using System.Configuration;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The configuration helper
    /// </summary>
    public static class ConfigHelper
    {
        #region Public Methods
        /// <summary>
        ///     Gets from application setting.
        /// </summary>
        public static string GetFromAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        #endregion
    }
}