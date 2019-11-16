using System;
using System.Data;
using BOA.Common.Helpers;

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

        public static void BeginNamespace(this PaddedStringBuilder sb,string namespaceName)
        {
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.PaddingCount++;
        }
        public static void OpenBracket(this PaddedStringBuilder sb)
        {
            sb.AppendLine("{");
            sb.PaddingCount++;
        }
        public static void CloseBracket(this PaddedStringBuilder sb)
        {
            sb.PaddingCount--;
            sb.AppendLine("}");
            
        }
        public static void EndNamespace(this PaddedStringBuilder sb)
        {
            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        public static void UsingNamespace(this PaddedStringBuilder sb, string namespaceName)
        {
            sb.AppendLine($"using {namespaceName};");
        }
    }
}