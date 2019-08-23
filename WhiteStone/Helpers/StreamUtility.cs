using System;
using System.IO;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     The stream read trace
    /// </summary>
    public class StreamReadTrace
    {
        #region Public Properties
        /// <summary>
        ///     Gets the current byte count.
        /// </summary>
        public int CurrentByteCount { get; internal set; }

        /// <summary>
        ///     Gets the shuttle count.
        /// </summary>
        public int ShuttleCount { get; internal set; }

        /// <summary>
        ///     Gets the total byte count.
        /// </summary>
        public int TotalByteCount { get; internal set; }
        #endregion
    }

    /// <summary>
    ///     Helper extension methods for Stream class.
    /// </summary>
    public static class StreamUtility
    {
        #region Public Methods
        /// <summary>
        ///     Reads all stream and returns as Byte Array
        /// </summary>
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
        ///     Reads all write to output.
        /// </summary>
        public static void ReadAllWriteToOutput(this Stream inputStream, Stream outputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            if (outputStream == null)
            {
                throw new ArgumentNullException(nameof(outputStream));
            }

            var array = new byte[2048];
            int count;
            do
            {
                count = inputStream.Read(array, 0, array.Length);

                outputStream.Write(array, 0, count);
            } while (count > 0);
        }

        /// <summary>
        ///     Reads all part of stream and returns as String
        /// </summary>
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
        ///     Transfers the stream.
        /// </summary>
        public static void TransferStream(this Stream inputStream, Stream outputStream, Action<StreamReadTrace> onRead)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            if (outputStream == null)
            {
                throw new ArgumentNullException(nameof(outputStream));
            }

            if (onRead == null)
            {
                throw new ArgumentNullException(nameof(onRead));
            }

            var trace = new StreamReadTrace();

            var array = new byte[2048];
            int count;
            do
            {
                count = inputStream.Read(array, 0, array.Length);

                outputStream.Write(array, 0, count);

                trace.ShuttleCount++;
                trace.CurrentByteCount =  count;
                trace.TotalByteCount   += count;

                onRead.Invoke(trace);
            } while (count > 0);
        }
        #endregion
    }
}