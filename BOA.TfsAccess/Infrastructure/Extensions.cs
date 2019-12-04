using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA
{

    static class FileHelper
    {
        public static void CreateDirectoryIfNotExists(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }
        public static void WriteAllText(string path, string data)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            CreateDirectoryIfNotExists(Path.GetDirectoryName(path));

            File.Delete(path);

            var fs = new FileStream(path, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fs);
            sw.Write(data);
            sw.Close();
            fs.Close();
        }
    }
    static class Extensions
    {
        /// <summary>
        ///     Determines whether this instance has value.
        /// </summary>
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
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
        ///     Removes value from start of str
        /// </summary>
        public static string RemoveFromStart(this string data, string value)
        {
            return RemoveFromStart(data, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Removes value from start of str
        /// </summary>
        public static string RemoveFromStart(this string data, string value, StringComparison comparison)
        {
            if (data == null)
            {
                return null;
            }

            if (data.StartsWith(value, comparison))
            {
                return data.Substring(value.Length, data.Length - value.Length);
            }

            return data;
        }
    }
}
