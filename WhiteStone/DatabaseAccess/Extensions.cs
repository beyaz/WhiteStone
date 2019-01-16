using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WhiteStone.Helpers;

namespace BOA.DatabaseAccess
{
    /// <summary>
    ///     The extensions
    /// </summary>
    public static class Extensions
    {
        #region Public Methods
        /// <summary>
        ///     Gets the records.
        /// </summary>
        public static List<TContract> GetRecords<TContract>(this Database database, string sql, params object[] parameters) where TContract : class, new()
        {
            database.CommandText = sql;
            for (var i = 0; i < parameters.Length; i+=2)
            {
                var parameterName = parameters[i] as string;
                if (parameterName == null)
                {
                    const string InvalidParameterName = "InvalidParameterName at index ";
                    throw new ArgumentException(InvalidParameterName + i);
                }

                var parameterValue = parameters[i + 1];

                database[parameterName] = parameterValue;
            }

            return database.ExecuteReader().ToList<TContract>();
        }

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