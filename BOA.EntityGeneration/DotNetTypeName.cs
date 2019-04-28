using System;

namespace BOA.EntityGeneration
{
    public static class DotNetTypeName
    {
        #region Constants
        public const string DotNetBool            = "bool";
        public const string DotNetByte            = "byte";
        public const string DotNetByteArray       = "byte[]";
        public const string DotNetDateTime        = "DateTime";
        public const string DotNetDecimal         = "decimal";
        public const string DotNetDouble          = "double";
        public const string DotNetGuid            = "Guid";
        public const string DotNetInt16           = "short";
        public const string DotNetSingle = "float";
        public const string DotNetInt32           = "int";
        public const string DotNetInt64           = "long";
        public const string DotNetNullableDecimal = "decimal?";
        public const string DotNetObject          = "object";
        public const string DotNetStringName      = "string";
        public const string DotNetTimeSpan        = "TimeSpan";
        #endregion


        public static string GetDotNetNullableType(string dotNetType)
        {
            if (dotNetType == null)
            {
                throw new ArgumentNullException(nameof(dotNetType));
            }

            if (dotNetType == DotNetTypeName.DotNetByteArray ||
                dotNetType == DotNetTypeName.DotNetStringName ||
                dotNetType == DotNetTypeName.DotNetObject ||
                dotNetType.EndsWith("?"))
            {
                return dotNetType;
            }

            return dotNetType += "?";
        }
    }
}