using System;
using System.Data;

namespace BOA.EntityGeneration
{
    /// <summary>
    ///     The extensions
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        ///     Determines whether the specified database type is equal.
        /// </summary>
        public static bool IsEqual(this string dataType, SqlDbType dbType)
        {
            return dbType.ToString().Equals(dataType, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Startses the with.
        /// </summary>
        public static bool StartsWith(this string dataType, SqlDbType dbType, StringComparison comparison)
        {
            return dbType.ToString().Equals(dataType, comparison);
        }
        
    }
}