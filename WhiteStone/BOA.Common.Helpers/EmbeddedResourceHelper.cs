using System;
using System.IO;
using System.Reflection;
using System.Resources;
using WhiteStone.Helpers;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The embedded resource helper
    /// </summary>
    public static class EmbeddedResourceHelper
    {
        #region Public Methods
        /// <summary>
        ///     Extracts the specified name space.
        /// </summary>
        public static void Extract(string nameSpace, string outDirectory, string internalFilePath, string resourceName)
        {
            var assembly = Assembly.GetCallingAssembly();

            var filePath = nameSpace + "." + internalFilePath + "." + resourceName;

            using (var manifestResourceStream = assembly.GetManifestResourceStream(filePath))
            {
                if (manifestResourceStream == null)
                {
                    throw new MissingManifestResourceException(filePath);
                }

                var targetFilePath = $"{outDirectory}{Path.DirectorySeparatorChar}{resourceName}";

                if (File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);    
                }

                var targetDirectory = Path.GetDirectoryName(targetFilePath);
                if (targetDirectory == null)
                {
                    throw new ArgumentNullException(nameof(targetDirectory));
                }

                if (File.Exists(targetDirectory))
                {
                    File.Delete(targetDirectory);
                }

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                using (var fs = new FileStream(targetFilePath, FileMode.OpenOrCreate))
                {
                    manifestResourceStream.ReadAllWriteToOutput(fs);
                }
            }
        }


        /// <summary>
        ///     Extracts the specified name space.
        /// </summary>
        public static string ReadFile(string nameSpace, string internalFilePath, string resourceName)
        {
            var assembly = Assembly.GetCallingAssembly();

            var filePath = nameSpace + "." + internalFilePath + "." + resourceName;

            using (var manifestResourceStream = assembly.GetManifestResourceStream(filePath))
            {
                if (manifestResourceStream == null)
                {
                    throw new MissingManifestResourceException(filePath);
                }

                return manifestResourceStream.ReadToEndAsString();
            }
        }
        #endregion
    }
}