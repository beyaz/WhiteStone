using System.IO;
using YamlDotNet.Serialization;

namespace BOA.EntityGeneration.UI.Container.Infrastructure
{
    static class YamlHelper
    {
        #region Public Methods
        public static T Deserialize<T>(string serializedContent)
        {
            return new Deserializer().Deserialize<T>(serializedContent);
        }

        public static T DeserializeFromFile<T>(string filePath)
        {
            return Deserialize<T>(File.ReadAllText(filePath));
        }
        #endregion
    }
}