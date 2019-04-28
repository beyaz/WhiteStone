using System;
using System.Data;

namespace BOA.EntityGeneration
{
    public static class Extensions
    {

        public static bool IsEqual(this string dataType, SqlDbType dbType)
        {
            return dbType.ToString().Equals(dataType, StringComparison.OrdinalIgnoreCase);
        }
    }
}