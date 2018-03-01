namespace WhiteStone.Services
{
    /// <summary>
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///     Append text in a log file.
        /// </summary>
        void Log(string message, string callerMemberName = null, string callerFilePath = null, int callerLineNumber = 0);
    }
}