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

                using (var r = new BinaryReader(manifestResourceStream))
                {
                    using (var fs = new FileStream(outDirectory + "\\" + resourceName, FileMode.OpenOrCreate))
                    {
                        using (var w = new BinaryWriter(fs))
                        {
                            w.Write(r.ReadByte());
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Extracts the specified name space.
        /// </summary>
        public static string ReadFile(string nameSpace,  string internalFilePath, string resourceName)
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