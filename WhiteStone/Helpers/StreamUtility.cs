using System;
using System.IO;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Helper extension methods for Stream class.
    /// </summary>
    public static class StreamUtility
    {
        /// <summary>
        ///     Reads all part of stream and returns as String
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadToEndAsString(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        ///     Reads all stream and returns as Byte Array
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadAll(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            var array = new byte[stream.Length];
            stream.Read(array, 0, (int) stream.Length);
            return array;
        }

        /// <summary>
        ///     Reads all part of <paramref name="inputStream" /> then writes to <paramref name="outputStream" />
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        public static void ReadAllWriteToOutput(this Stream inputStream, Stream outputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }
            var array = new byte[2048];
            int num;
            do
            {
                num = inputStream.Read(array, 0, array.Length);
                outputStream.Write(array, 0, num);
            } while (num > 0);
        }
    }
}