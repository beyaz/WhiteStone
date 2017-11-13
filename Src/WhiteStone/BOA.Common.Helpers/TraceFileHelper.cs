using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The trace file helper
    /// </summary>
    public static class TraceFileHelper
    {
        #region Public Methods
        /// <summary>
        ///     Pushes to file.
        /// </summary>
        public static void PushToFile(string filePath, object instance)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Append))
            {
                using (var writer = new StreamWriter(fileStream))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    var ser = JsonSerializer.Create(new JsonSerializerSettings {Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore});
                    ser.Serialize(jsonWriter, instance);
                    jsonWriter.Flush();
                }
            }
        }

        /// <summary>
        ///     Reads the specified file path.
        /// </summary>
        public static IEnumerable<TRecordType> Read<TRecordType>(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open);

            using (var streamReader = new StreamReader(fileStream))
            {
                using (var reader = new JsonTextReader(streamReader))
                {
                    reader.SupportMultipleContent = true;

                    var serializer = new JsonSerializer();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            yield return serializer.Deserialize<TRecordType>(reader);
                        }
                    }
                }
            }
        }
        #endregion
    }
}