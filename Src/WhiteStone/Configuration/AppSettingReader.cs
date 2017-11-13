using System.Configuration;

namespace WhiteStone.Configuration
{
    /// <summary>
    ///     Defines a methods for accessing config reading operations.
    /// </summary>
    public class AppSettingReader : IConfigReader
    {
        /// <summary>
        ///     Gets config value by given key.
        /// </summary>
        /// <param name="key"></param>
        public string this[string key]
        {
            get { return ConfigurationManager.AppSettings[key]; }
        }
    }
}