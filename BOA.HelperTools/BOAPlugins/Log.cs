using System.Runtime.CompilerServices;
using BOA.Common.Helpers;

namespace BOAPlugins
{
    public class Log
    {
        #region Public Methods
        public static void Push(string message, [CallerMemberName] string callerMemberName = null)
        {
            FileHelper.WriteAllText(Configuration.PluginDirectory + "Log.txt", callerMemberName + " -> " + message);
        }
        #endregion
    }
}