using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace WhiteStone.ApplicationStarter
{
    static class Config
    {
        #region Public Properties
        public static string ExportDir       => ConfigurationManager.AppSettings[nameof(ExportDir)];
        public static string ProcessPath     => ConfigurationManager.AppSettings[nameof(ProcessPath)];
        public static string ZipFilePath     => ConfigurationManager.AppSettings[nameof(ZipFilePath)];
        public static string ZipFileTempPath => ZipFilePath + ".tmp";
        public static string ZipFileUrl      => ConfigurationManager.AppSettings[nameof(ZipFileUrl)];
        #endregion
    }

    class Program
    {
        #region Public Methods
        public static void Main(string[] args)
        {
            TryToExtractZipFile();

            if (!File.Exists(Config.ProcessPath))
            {
                Console.WriteLine("Yükleniyor...");
                FileHelper.DownloadFile(Config.ZipFileUrl, Config.ZipFileTempPath, true);
                Console.WriteLine("Yüklendi.");

                File.Move(Config.ZipFileTempPath, Config.ZipFilePath);

                TryToExtractZipFile();
            }

            if (args?.Length > 0)
            {
                var arguments = string.Join("|", args);

                Console.WriteLine("Starting process with argument:" + arguments);

                Process.Start(Config.ProcessPath, arguments);
            }
            else
            {
                Console.WriteLine("Starting process...");
                Process.Start(Config.ProcessPath);
            }

            Console.WriteLine("Process is started.");
        }
        #endregion

        #region Methods
        static void TryToExtractZipFile()
        {
            if (File.Exists(Config.ZipFilePath))
            {
                if (Directory.Exists(Config.ExportDir))
                {
                    Console.WriteLine("Deleting folder: " + Config.ExportDir);
                    Directory.Delete(Config.ExportDir, true);
                    Console.WriteLine("Deleted folder: " + Config.ExportDir);
                }

                Console.WriteLine("Exporting zip file: " + Config.ZipFilePath);
                ZipFile.ExtractToDirectory(Config.ZipFilePath, Config.ExportDir);

                Console.WriteLine("Deleting zip file: " + Config.ZipFilePath);
                File.Delete(Config.ZipFilePath);
                Console.WriteLine("Deleted zip file: " + Config.ZipFilePath);
            }
        }
        #endregion
    }

    static class FileHelper
    {
        #region Public Methods
        public static void CreateDirectoryIfNotExists(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
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

            CreateDirectoryIfNotExists(Path.GetDirectoryName(saveFilePath));

            using (var webClient = new WebClient())
            {
                using (var stream = webClient.OpenRead(address))
                {
                    if (stream == null)
                    {
                        throw new ArgumentException(nameof(stream));
                    }

                    stream.ReadTimeout = Timeout.Infinite;

                    using (var fileStream = new FileStream(saveFilePath, FileMode.OpenOrCreate))
                    {
                        stream.TransferStream(fileStream, trace => Console.WriteLine(trace.ShuttleCount));
                    }
                }
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
}