using System.Collections.Generic;
using System.IO;
using BOA.CodeGeneration.Services;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;

namespace BOAPlugins
{
    public static class Util
    {
        #region Public Methods
        public static void WriteFileIfContentNotEqual(string path, string content)
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, content);
                return;
            }

            var existingData = File.ReadAllText(path);

            var isEqual = StringHelper.IsEqualAsData(existingData, content);

            if (!isEqual)
            {
                TFSAccessForBOA.CheckoutFile(path);
                File.WriteAllText(path, content);
            }
        }

        #region Public Methods
        /// <summary>
        ///     Sets the value.
        /// </summary>
        public static void SetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }

            dictionary.Add(key, value);
        }

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
        #endregion
    }
}