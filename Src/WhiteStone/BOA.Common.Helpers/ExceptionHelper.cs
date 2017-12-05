using System;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The exception helper
    /// </summary>
    public static class ExceptionHelper
    {
        #region Public Methods
        /// <summary>
        ///     Adds the data.
        /// </summary>
        public static T AddData<T>(this T exception, string key, object value) where T : Exception
        {
            exception.Data[key] = value;
            return exception;
        }
        #endregion
    }
}