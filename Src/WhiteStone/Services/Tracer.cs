using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using BOA.Common.Helpers;
using WhiteStone.Helpers;

namespace WhiteStone.Services
{
    /// <summary>
    ///     Defines the tracer.
    /// </summary>
    public class Tracer
    {
        readonly StringBuilder _sb = new StringBuilder();

        /// <summary>
        ///     Gets or sets the trace handler.
        /// </summary>
        public Action<string> TraceHandler { get; set; }

        /// <summary>
        ///     Gets the messages.
        /// </summary>
        public string Messages
        {
            get { return _sb.ToString(); }
        }

        /// <summary>
        ///     Traces the specified message.
        /// </summary>
        public void Trace(string message, object instance = null, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            const char Colon = ':';

            if (instance != null)
            {
                message += Colon + ReflectionHelper.ExportObjectToCSharpCode(instance);
            }

            TraceMain(PrepareMessage(message, callerMemberName, callerFilePath, callerLineNumber));
        }


        void TraceMain(string message)
        {
            if (TraceHandler != null)
            {
                TraceHandler(message);
                return;
            }

            _sb.Append(message);
        }

        static readonly CultureInfo EnglishCulture = new CultureInfo("en-US");

        static string PrepareMessage(string message, string callerMemberName = null, string callerFilePath = null, int callerLineNumber = 0)
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
    }
}