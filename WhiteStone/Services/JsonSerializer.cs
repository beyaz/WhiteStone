using System;
using Newtonsoft.Json;

namespace WhiteStone.Services
{
    /// <summary>
    ///     Serialization
    /// </summary>
    public interface ISerializer
    {
        #region Public Methods
        /// <summary>
        ///     converts given content to requested type
        /// </summary>
        T Deserialize<T>(string serializedContent);

        /// <summary>
        ///     Deserializes the specified serialized content.
        /// </summary>
        object Deserialize(string serializedContent, Type type);

        /// <summary>
        ///     Serializes the specified object to a JSON string.
        /// </summary>
        string Serialize(object value);
        #endregion
    }

    /// <summary>
    ///     Defines the json serializer.
    /// </summary>
    /// <seealso cref="WhiteStone.Services.ISerializer" />
    public class JsonSerializer : ISerializer
    {
        #region Public Methods
        /// <summary>
        ///     Converts json content to given type
        /// </summary>
        public T Deserialize<T>(string serializedContent)
        {
            return JsonConvert.DeserializeObject<T>(serializedContent);
        }

        /// <summary>
        ///     Deserializes the specified serialized content.
        /// </summary>
        public object Deserialize(string serializedContent, Type type)
        {
            return JsonConvert.DeserializeObject(serializedContent, type);
        }

        /// <summary>
        ///     Serializes the specified object to a JSON string.
        /// </summary>
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
        #endregion
    }
}