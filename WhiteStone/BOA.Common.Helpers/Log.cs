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
        #region Public Methods
        /// <summary>
        ///     Determines whether the specified message is null.
        /// </summary>
        public static void IsNull(string message, [CallerMemberName] string callerMemberName = null)
        {
            PushInternal(message + " is null.", callerMemberName);
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
            var filePath = Path.GetDirectoryName(typeof(Log).Assembly.Location) + Path.DirectorySeparatorChar + "Log.txt";

            var value = Environment.NewLine + $"{callerMemberName} -> {message}";

            FileHelper.AppendToEndOfFile(filePath, value);
        }
        #endregion
    }
}