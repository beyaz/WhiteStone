using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace WhiteStone.Services.FileLogging
{
    /// <summary>
    ///     Append text existing file.
    /// </summary>
    public class FileLogger : ILogger
    {
        readonly string _file;

        /// <summary>
        ///     Append text existing file.
        /// </summary>
        public FileLogger(string file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            _file = file;
        }

        void Write(string message)
        {
            try
            {
                var fs = new FileStream(_file, FileMode.Append);
                var sw = new StreamWriter(fs);
                sw.Write(message);
                sw.Close();
                fs.Close();
            }
            catch (IOException)
            {
                // ignored
            }
        }

        static readonly CultureInfo EnglishCulture = new CultureInfo("en-US");

        /// <summary>
        ///     Prepares the message.
        /// </summary>
        public static string PrepareMessage(string message, string callerMemberName = null, string callerFilePath = null, int callerLineNumber = 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine("------------------------------------------------------------------------------------------");
            sb.AppendLine("@Message         : " + message);
            sb.AppendLine("@Time            : " + DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss", EnglishCulture));
            sb.AppendLine("@CallerFilePath  : " + callerFilePath);
            sb.AppendLine("@CallerMemberName: " + callerMemberName);
            sb.AppendLine("@CallerLineNumber: " + callerLineNumber);
            sb.AppendLine("------------------------------------------------------------------------------------------");
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        ///     Logs the specified message.
        /// </summary>
        public void Log(string message, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            Write(PrepareMessage(message, callerMemberName, callerFilePath, callerLineNumber));
        }
    }
}