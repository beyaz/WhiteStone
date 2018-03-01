using System.Globalization;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Utility methods for globalizations
    /// </summary>
    public static class GlobalizationUtility
    {
        /// <summary>
        ///     Gets culture of "en-US"
        /// </summary>
        public static CultureInfo EnglishCulture
        {
            get { return new CultureInfo("en-US"); }
        }

        /// <summary>
        ///     Gets culture of "tr-TR"
        /// </summary>
        public static CultureInfo TurkishCulture
        {
            get { return new CultureInfo("tr-TR"); }
        }
    }
}