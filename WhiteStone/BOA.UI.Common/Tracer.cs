using System;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace BOA.UI.Common
{
    /// <summary>
    ///     The tracer
    /// </summary>
    public class Tracer
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the trace handler.
        /// </summary>
        public Action<string> TraceHandler { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Prepares the message.
        /// </summary>
        public static string PrepareMessage<TMessage>(TMessage traceMessage, object instance, string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            var message = traceMessage + " -> " + JsonConvert.SerializeObject(instance);

            return GetCallerInfo(callerMemberName, callerFilePath, callerLineNumber, message);
        }

        /// <summary>
        ///     Traces the specified trace message.
        /// </summary>
        public void Trace<TMessage>(TMessage traceMessage, object instance, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (TraceHandler == null)
            {
                return;
            }

            var message = PrepareMessage(traceMessage, instance, callerMemberName, callerFilePath, callerLineNumber);
            TraceHandler(message);
        }
        #endregion

        #region Methods
        static string GetCallerInfo(string callerMemberName, string callerFilePath, int? callerLineNumber = null, string message = null)
        {
            return "@Message         : " + message + Environment.NewLine +
                   "@CallerMemberName: " + callerMemberName + Environment.NewLine +
                   "@CallerFilePath  : " + NormalizeCallerFilePath(callerFilePath) + Environment.NewLine +
                   "@CallerLineNumber: " + callerLineNumber;
        }

        static string NormalizeCallerFilePath(string callerFilePath)
        {
            if (string.IsNullOrWhiteSpace(callerFilePath))
            {
                return callerFilePath;
            }

            var src = @"\src\";
            var srcIndex = callerFilePath.IndexOf(src, StringComparison.CurrentCulture);
            if (srcIndex > 0)
            {
                callerFilePath = callerFilePath.Substring(srcIndex + src.Length);
                callerFilePath = callerFilePath.Replace(Path.DirectorySeparatorChar.ToString(), " -> ");
            }
            return callerFilePath;
        }
        #endregion
    }

    /// <summary>
    ///     The trace handler for specific file
    /// </summary>
    public class TraceHandlerForSpecificFile
    {
        #region Fields
        readonly string _filePath;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="TraceHandlerForSpecificFile" /> class.
        /// </summary>
        public TraceHandlerForSpecificFile(string filePath)
        {
            _filePath = filePath;
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Writes the specified message.
        /// </summary>
        public void Write(string message)
        {
            try
            {
                var fs = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.Write);
                var streamWriter = new StreamWriter(fs);
                streamWriter.BaseStream.Seek(0L, SeekOrigin.End);
                streamWriter.WriteLine(message);
                streamWriter.Flush();
                streamWriter.Close();
            }
            catch
            {
                // ignored
            }
        }
        #endregion
    }
}