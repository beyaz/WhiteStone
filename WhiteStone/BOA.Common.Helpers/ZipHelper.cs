using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The zip helper
    /// </summary>
    public static class ZipHelper
    {
        #region Public Methods
        /// <summary>
        ///     Compresses the files.
        /// </summary>
        public static void CompressFiles(string outputFilePath, string[] inputFilePaths)
        {
            CompressFiles(outputFilePath, inputFilePaths, null);
        }

        /// <summary>
        ///     Compresses the files.
        /// </summary>
        public static void CompressFiles(string outputFilePath, string[] inputFilePaths, string password)
        {
            var fsOut     = File.Create(outputFilePath);
            var zipStream = new ZipOutputStream(fsOut);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

            zipStream.Password = password; // optional. Null is the same as not setting. Required if using AES.

            foreach (var inputFilePath in inputFilePaths)
            {
                AddFile(zipStream, inputFilePath);
            }

            zipStream.CloseEntry();

            zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
            zipStream.Close();
        }

        /// <summary>
        ///     Extracts the content from a .zip file inside an specific folder.
        /// </summary>
        public static void ExtractFromZipFile(string zipFilePath, string password, IReadOnlyDictionary<string, string> fileMap)
        {
            ZipFile zipFile = null;
            try
            {
                var fs = File.OpenRead(zipFilePath);

                zipFile = new ZipFile(fs);

                if (!string.IsNullOrEmpty(password))
                {
                    // AES encrypted entries are handled automatically
                    zipFile.Password = password;
                }

                foreach (var pair in fileMap)
                {
                    ExtractFromZipFile(zipFile, pair.Key, pair.Value);
                }
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zipFile.Close();              // Ensure we release resources
                }
            }
        }

        /// <summary>
        ///     Determines whether the specified zip file path has file.
        /// </summary>
        public static bool HasEntry(string zipFilePath, string entryName)
        {
            return HasEntry(zipFilePath, entryName, null);
        }

        /// <summary>
        ///     Determines whether the specified zip file path has file.
        /// </summary>
        public static bool HasEntry(string zipFilePath, string entryName, string password)
        {
            ZipFile zipFile = null;
            try
            {
                var fs = File.OpenRead(zipFilePath);

                zipFile = new ZipFile(fs);

                if (!string.IsNullOrEmpty(password))
                {
                    // AES encrypted entries are handled automatically
                    zipFile.Password = password;
                }

                return zipFile.GetEntry(entryName) != null;
            }
            catch (Exception e)
            {
                Log.Push(e);

                throw;
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zipFile.Close();              // Ensure we release resources
                }
            }
        }

        /// <summary>
        ///     Extracts the content from a .zip file inside an specific folder.
        /// </summary>
        public static void UnZip(string zipFilePath, string outputDirectory)
        {
            ZipFile zipFile = null;
            try
            {
                var fs = File.OpenRead(zipFilePath);

                zipFile = new ZipFile(fs);

                foreach (ZipEntry entry in zipFile)
                {
                    ExtractFromZipFile(zipFile, entry.Name, outputDirectory);
                }
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zipFile.Close();              // Ensure we release resources
                }
            }
        }

        /// <summary>
        ///     Extracts the content from a .zip file inside an specific folder.
        /// </summary>
        public static string UnZip(string zipFilePath)
        {
            var directoryName = Path.GetDirectoryName(zipFilePath);

            var targetExportDirectory = directoryName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(zipFilePath) + Path.DirectorySeparatorChar;

            UnZip(zipFilePath, targetExportDirectory);

            return targetExportDirectory;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Adds the file.
        /// </summary>
        static void AddFile(ZipOutputStream zipStream, string inputFilePath)
        {
            var fi = new FileInfo(inputFilePath);

            var entryName = ZipEntry.CleanName(Path.GetFileName(inputFilePath)); // Removes drive from name and fixes slash direction
            var newEntry = new ZipEntry(entryName)
            {
                DateTime = fi.LastWriteTime,
                Size     = fi.Length
            };

            // Note the zip format stores 2 second granularity

            // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
            // A password on the ZipOutputStream is required if using AES.
            //   newEntry.AESKeySize = 256;

            // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
            // you need to do one of the following: Specify UseZip64.Off, or set the Size.
            // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
            // but the zip will be in Zip64 format which not all utilities can understand.
            //   zipStream.UseZip64 = UseZip64.Off;

            zipStream.PutNextEntry(newEntry);

            // Zip the file in buffered chunks
            // the "using" will close the stream even if an exception occurs
            var buffer = new byte[4096];
            using (var streamReader = File.OpenRead(inputFilePath))
            {
                StreamUtils.Copy(streamReader, zipStream, buffer);
            }
        }

        static void ExtractFromZipFile(ZipFile zipFile, string entryName, string outputFilePath)
        {
            var zipEntry = zipFile.GetEntry(entryName);
            if (zipEntry == null)
            {
                throw new ArgumentNullException(nameof(zipEntry));
            }

            var zipStream = zipFile.GetInputStream(zipEntry);

            var directoryName = Path.GetDirectoryName(outputFilePath);

            if (directoryName?.Length > 0)
            {
                Directory.CreateDirectory(directoryName);
            }

            // 4K is optimum
            var buffer = new byte[4096];
            // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
            // of the file, but does not waste memory.
            // The "using" will close the stream even if an exception occurs.
            using (var streamWriter = File.Create(outputFilePath + entryName))
            {
                StreamUtils.Copy(zipStream, streamWriter, buffer);
            }
        }
        #endregion
    }
}