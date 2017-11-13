using System;
using System.IO;

namespace WhiteStone.Services.FileLogging
{
    /// <summary>
    ///     Defines the desktop application file logger.
    /// </summary>
    public class FileLoggerCreator
    {
        /// <summary>
        ///     Creates this instance.
        /// </summary>
        public virtual FileLogger Create()
        {
            var path = typeof(FileLoggerCreator).Assembly.Location;
            if (path == null)
            {
                throw new ArgumentException(nameof(path));
            }

            path = Directory.GetParent(path).FullName +
                   Path.DirectorySeparatorChar +
                   "Log.txt";

            File.Delete(path);
            var logger = new FileLogger(path);

            return logger;
        }
    }
}