using System.IO;
using System.Net;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The file helper
    /// </summary>
    public class FileHelper
    {
        #region Public Methods
        /// <summary>
        ///     Downloads the string.
        /// </summary>
        public static string DownloadString(string address)
        {
            try
            {
                return new WebClient().DownloadString(address);
            }
            catch (WebException e)
            {
                e.Data.Add(nameof(address), address);
                throw;
            }
        }

        /// <summary>
        ///     Determines whether [is file locked] [the specified path].
        /// </summary>
        public static bool IsFileLocked(string path)
        {
            FileStream stream = null;
            try
            {
                var file = new FileInfo(path);
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return false;
        }

        /// <summary>
        ///     Writes all text.
        /// </summary>
        public static void WriteAllText(string path, string data)
        {
            var directoryInfo = new FileInfo(path).Directory;
            if (directoryInfo != null)
            {
                directoryInfo.Create();
            }

            var fs = new FileStream(path, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fs);
            sw.Write(data);
            sw.Close();
            fs.Close();
        }
        #endregion
    }
}