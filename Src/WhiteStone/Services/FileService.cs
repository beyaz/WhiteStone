using System;
using System.IO;
using WhiteStone.Services;

namespace WhiteStone.Services
{
    /// <summary>
    ///     Defines the file helper.
    /// </summary>
    public class FileService
    {
        /// <summary>
        ///     Gets or sets the tracer.
        /// </summary>
        public Tracer Tracer { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileService" /> class.
        /// </summary>
        public FileService()
        {
            Tracer = new Tracer();
        }

        /// <summary>
        ///     Copies the directory.
        /// </summary>
        public void CopyDirectory(string sourceDirectory, string targetDirectory, bool copySubdirectory)
        {
            if (!Directory.Exists(targetDirectory))
            {
                Tracer.Trace("Creating @targetDir:" + targetDirectory);

                Directory.CreateDirectory(targetDirectory);
            }

            foreach (var file in Directory.GetFiles(sourceDirectory))
            {
                var fileName = Path.GetFileName(file);

                var source = new FileInfo(targetDirectory + fileName);
                var target = new FileInfo(sourceDirectory + fileName);

                if (source.Exists && (!target.Exists || source.CreationTime > target.CreationTime))
                {
                    Tracer.Trace("Started to copy. @FileName: " + fileName);

                    try
                    {
                        File.Copy(sourceDirectory + fileName, targetDirectory + fileName, true);
                    }
                    catch (IOException e)
                    {
                        Tracer.Trace(e.ToString());
                    }

                    Tracer.Trace("FileCopied. FileName: " + fileName);
                }
            }

            if (copySubdirectory)
            {
                foreach (var directory in Directory.GetDirectories(sourceDirectory))
                {
                    var dirName = Path.GetFileName(directory);
                    if (dirName == null)
                    {
                        throw new ArgumentNullException("targetDirectory");
                    }

                    CopyDirectory(directory, Path.Combine(targetDirectory, dirName), true);
                }
            }
        }
    }
}