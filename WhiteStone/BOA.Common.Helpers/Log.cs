using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The log
    /// </summary>
    public class Log
    {
        #region Public Properties
        /// <summary>
        ///     Gets the directory.
        /// </summary>
        public static string Directory => Path.GetDirectoryName(typeof(Log).Assembly.Location) + Path.DirectorySeparatorChar;

        /// <summary>
        ///     Gets or sets the indent.
        /// </summary>
        public static int Indent { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Determines whether the specified message is null.
        /// </summary>
        public static void IsNull(string message, [CallerMemberName] string callerMemberName = null)
        {
            PushInternal(message + " is null.", callerMemberName);
        }

        /// <summary>
        ///     Pushes the specified exception.
        /// </summary>
        public static void Push(Exception exception, [CallerMemberName] string callerMemberName = null)
        {
            PushInternal(exception.ToString(), callerMemberName);
        }

        /// <summary>
        ///     Pushes the specified message.
        /// </summary>
        public static void Push(string message, [CallerMemberName] string callerMemberName = null)
        {
            PushInternal(message, callerMemberName);
        }

        /// <summary>
        ///     Pushes the specified instance.
        /// </summary>
        public static void Push<T>(T instance, [CallerMemberName] string callerMemberName = null)
        {
            PushInternal(JsonHelper.Serialize(instance), callerMemberName);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Pushes the specified message.
        /// </summary>
        static void PushInternal(string message, string callerMemberName)
        {
            const string fileName = "Log.txt";

            try
            {
                var filePath = Directory + fileName;

                var indent = string.Empty.PadLeft(Indent, ' ');

                var sb = new StringBuilder();

                sb.AppendLine();
                sb.AppendLine("-------------------------------------------------------------------------");
                sb.AppendLine(indent + $"Time    : {DateTime.Now:yyyy.MM.dd hh:mm:ss}");
                sb.AppendLine(indent + $"Caller  : {callerMemberName}");
                sb.AppendLine(indent + $"Message : {message}");
                sb.AppendLine("-------------------------------------------------------------------------");
                sb.AppendLine();

                FileHelper.RemoveReadOnlyFlag(filePath);

                FileHelper.AppendToEndOfFile(filePath, sb.ToString());
            }
            catch (Exception)
            {
                // ignored
            }
        }
        #endregion
    }
}