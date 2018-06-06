using System;
using Newtonsoft.Json;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The json helper
    /// </summary>
    public static class JsonHelper
    {
        #region Public Methods
        /// <summary>
        ///     Deserializes the specified serialized content.
        /// </summary>
        public static T Deserialize<T>(string serializedContent)
        {
            return JsonConvert.DeserializeObject<T>(serializedContent);
        }

        /// <summary>
        ///     Deserializes the specified serialized content.
        /// </summary>
        public static object Deserialize(string serializedContent, Type type)
        {
            return JsonConvert.DeserializeObject(serializedContent, type);
        }

        /// <summary>
        ///     Serializes the specified value.
        /// </summary>
        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
        #endregion
    }
}