using System;
using Newtonsoft.Json;

namespace WhiteStone.Services
{
    /// <summary>
    ///     Serialization
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        ///     converts given content to requested type
        /// </summary>
        T Deserialize<T>(string serializedContent);
    }

    /// <summary>
    ///     Defines the json serializer.
    /// </summary>
    /// <seealso cref="WhiteStone.Services.ISerializer" />
    public class JsonSerializer : ISerializer
    {
        /// <summary>
        ///     Converts json content to given type
        /// </summary>
        public T Deserialize<T>(string serializedContent)
        {
            return JsonConvert.DeserializeObject<T>(serializedContent);
        }
        /// <summary>
        /// Deserializes the specified serialized content.
        /// </summary>
        public object Deserialize(string serializedContent, Type type)
        {
            return JsonConvert.DeserializeObject(serializedContent,type);
        }
    }
}