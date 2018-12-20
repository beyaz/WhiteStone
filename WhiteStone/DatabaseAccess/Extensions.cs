using System.Linq;
using System.Text.RegularExpressions;

namespace BOA.DatabaseAccess
{
    /// <summary>
    ///     The extensions
    /// </summary>
    public static class Extensions
    {
        #region Public Methods
        /// <summary>
        ///     Runs the script.
        /// </summary>
        public static void RunScript(this IDatabase database, string script)
        {
            var commandTexts = from s in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)
                               where !string.IsNullOrWhiteSpace(s)
                               select s.Trim();

            foreach (var commandString in commandTexts)
            {
                database.CommandText = commandString;
                database.ExecuteNonQuery();
            }
        }
        #endregion
    }
}