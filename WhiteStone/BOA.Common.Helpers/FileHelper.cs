using System.IO;
using System.Net;
using WhiteStone.Helpers;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The file helper
    /// </summary>
    public static class FileHelper
    {
        #region Public Methods
        /// <summary>
        ///     Appends to end of file.
        /// </summary>
        public static void AppendToEndOfFile(string filePath, string value)
        {
            var fs = new FileStream(filePath, FileMode.Append);

            var sw = new StreamWriter(fs);
            sw.Write(value);
            sw.Close();
            fs.Close();
        }

        /// <summary>
        ///     Copy the directories
        /// </summary>
        public static void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryName, bool copySubdirectories)
        {
            // Get the subdirectories for the specified directory.
            var directoryInfo = new DirectoryInfo(sourceDirectoryPath);
            var directories   = directoryInfo.GetDirectories();
            if (!directoryInfo.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: "
                                                     + sourceDirectoryPath);
            }

            var parentDirectory = Directory.GetParent(directoryInfo.FullName);
            destinationDirectoryName = Path.Combine(parentDirectory.FullName, destinationDirectoryName);

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destinationDirectoryName))
            {
                Directory.CreateDirectory(destinationDirectoryName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = directoryInfo.GetFiles();

            foreach (var file in files)
            {
                var tempPath = Path.Combine(destinationDirectoryName, file.Name);

                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location using recursive  function. 
            if (copySubdirectories)
            {
                foreach (var item in directories)
                {
                    var tempPath = Path.Combine(destinationDirectoryName, item.Name);
                    CopyDirectory(item.FullName, tempPath, true);
                }
            }
        }

        /// <summary>
        ///     Creates the directory if not exists.
        /// </summary>
        public static void CreateDirectoryIfNotExists(string path)
        {
            var directoryInfo = new FileInfo(path).Directory;
            if (directoryInfo != null)
            {
                directoryInfo.Create();
            }
        }

        /// <summary>
        ///     Downloads the file.
        /// </summary>
        public static void DownloadFile(string address, string saveFilePath, bool useSsl)
        {
            if (useSsl)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }

            CreateDirectoryIfNotExists(saveFilePath);

            try
            {
                new WebClient().DownloadFile(address, saveFilePath);
            }
            catch (WebException e)
            {
                e.Data.Add(nameof(address), address);
                e.Data.Add(nameof(saveFilePath), saveFilePath);
                throw;
            }
        }

        /// <summary>
        ///     Downloads the file.
        /// </summary>
        public static void DownloadFileWithProxy(string address, string saveFilePath, string proxyServerAddress)
        {
            try
            {
                var webClient = new WebClient
                {
                    Proxy = new WebProxy(proxyServerAddress)
                };
                webClient.DownloadFile(address, saveFilePath);
            }
            catch (WebException e)
            {
                e.Data.Add(nameof(address), address);
                e.Data.Add(nameof(saveFilePath), saveFilePath);
                e.Data.Add(nameof(proxyServerAddress), proxyServerAddress);
                throw;
            }
        }

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
        ///     Reads the file.
        /// </summary>
        public static string ReadFile(string path)
        {
            var file = new FileInfo(path);
            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
            {
                return stream.ReadToEndAsString();
            }
        }

        /// <summary>
        ///     Writes all text.
        /// </summary>
        public static void WriteAllText(string path, string data)
        {
            CreateDirectoryIfNotExists(path);

            File.Delete(path);

            var fs = new FileStream(path, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fs);
            sw.Write(data);
            sw.Close();
            fs.Close();
        }
        #endregion
    }
}