namespace System.Collections.Generic
{
    /// <summary>
    ///     The extensions
    /// </summary>
    public static class Extensions
    {
        #region Public Methods
        /// <summary>
        ///     Tries the get value.
        /// </summary>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            var value = default(TValue);
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            return defaultValue;
        }
        #endregion
    }
}