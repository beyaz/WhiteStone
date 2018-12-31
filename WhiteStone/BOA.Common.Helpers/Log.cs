using System;
using System.IO;
using System.Runtime.CompilerServices;

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
        #endregion

        #region Methods
        /// <summary>
        ///     Pushes the specified message.
        /// </summary>
        static void PushInternal(string message, string callerMemberName)
        {
            var filePath = Directory + "Log.txt";

            var value = Environment.NewLine + Environment.NewLine + string.Empty.PadLeft(Indent, ' ') + $"{callerMemberName} -> {message}";

            FileHelper.AppendToEndOfFile(filePath, value);
        }
        #endregion
    }
}