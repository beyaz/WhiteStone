using System.IO;
using System.IO.Compression;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The compression helper
    /// </summary>
    public static class CompressionHelper
    {
        #region Public Methods
        /// <summary>
        ///     Compresses the specified data.
        /// </summary>
        public static byte[] Compress(byte[] data)
        {

            if (data == null)
            {
                return null;
            }

            if (data.Length == 0)
            {
                return new byte[0];
            }

            var output = new MemoryStream();
            using (var deflateStream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                deflateStream.Write(data, 0, data.Length);
            }

            return output.ToArray();
        }

        /// <summary>
        ///     Decompresses the specified data.
        /// </summary>
        public static byte[] Decompress(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            if (data.Length == 0)
            {
                return new byte[0];
            }

            var input  = new MemoryStream(data);
            var output = new MemoryStream();
            using (var deflateStream = new DeflateStream(input, CompressionMode.Decompress))
            {
                deflateStream.CopyTo(output);
            }

            return output.ToArray();
        }
        #endregion
    }
}