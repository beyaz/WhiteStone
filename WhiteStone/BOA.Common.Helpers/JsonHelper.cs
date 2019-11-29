using System;
using Newtonsoft.Json;

namespace BOA.Common.Helpers
{
    //public static class YamlHelper
    //{
    //    public static T Deserialize<T>(string serializedContent)
    //    {
    //        return new YamlDotNet.Serialization.Deserializer().Deserialize<T>(serializedContent);
    //    }
    //}


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
        ///     Deserializes the name of the with type.
        /// </summary>
        public static object DeserializeWithTypeName(string serializedContent)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            return JsonConvert.DeserializeObject(serializedContent, settings);
        }

        /// <summary>
        ///     Serializes the specified value.
        /// </summary>
        public static string Serialize(object value)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting        = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(value, settings);
        }

        /// <summary>
        ///     Serializes the name of the with type.
        /// </summary>
        public static string SerializeWithTypeName(object value)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting        = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling  = TypeNameHandling.All
            };

            return JsonConvert.SerializeObject(value, settings);
        }
        #endregion
    }
}