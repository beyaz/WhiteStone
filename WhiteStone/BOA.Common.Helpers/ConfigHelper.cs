using System;
using System.Configuration;
using System.Linq;

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

        /// <summary>
        ///     Gets the boolean from application setting.
        /// </summary>
        public static bool GetBooleanFromAppSetting(string key)
        {
            return Convert.ToBoolean(GetFromAppSetting(key));
        }

        #endregion
    }
}