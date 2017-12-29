using System.Collections.Generic;
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
        public static void RunScript(IDatabase database, string script)
        {
            foreach (var commandString in SplitScript(script))
            {
                database.CommandText = commandString;
                database.ExecuteNonQuery();
            }
        }
        #endregion

        #region Methods
        internal static IEnumerable<string> SplitScript(string script)
        {
            return from s in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)
                   where !string.IsNullOrWhiteSpace(s)
                   select s.Trim();
        }
        #endregion
    }
}