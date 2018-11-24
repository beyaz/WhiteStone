using System;

namespace BOAPlugins.HideSuccessCheck
{
    static class Extensions
    {
        #region Public Methods
        /// <summary>
        ///     Removes value from start of str
        /// </summary>
        public static string RemoveFromStart(this string data, string value)
        {
            if (data == null)
            {
                return null;
            }

            if (data.StartsWith(value, StringComparison.CurrentCulture))
            {
                return data.Substring(value.Length, data.Length - value.Length);
            }

            return data;
        }
        #endregion
    }
}