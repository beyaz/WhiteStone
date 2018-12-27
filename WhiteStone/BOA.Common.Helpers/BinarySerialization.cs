using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The binary serialization
    /// </summary>
    public static class BinarySerialization
    {
        #region Public Methods
        /// <summary>
        ///     Deserializes the specified byte array.
        /// </summary>
        public static object Deserialize(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream(byteArray))
            {
                var binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        ///     Deserializes the specified byte array.
        /// </summary>
        public static T Deserialize<T>(byte[] byteArray)
        {
            return (T) Deserialize(byteArray);
        }

        /// <summary>
        ///     Serializes the specified instance.
        /// </summary>
        public static byte[] Serialize(object instance)
        {
            if (instance == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, instance);
                return memoryStream.ToArray();
            }
        }
        #endregion
    }
}