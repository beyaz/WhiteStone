
namespace BOA.EntityGeneration.Common
{

    public static class DotNetName
    {

    }
}

namespace BOA.CodeGeneration.Common
{
    public static class Names
    {
        #region Constants
        public const string BiggerThan         = "BiggerThan";
        public const string BiggerThanOrEquals = "BiggerThanOrEqual";
        public const string ChannelId          = "ChannelId";
        public const string ChannelIdValue     = "Context.ApplicationContext.Authentication.Channel";

        public const string Contains              = "Contains";

        public const string DotNetBool            = "bool";
        public const string DotNetByte            = "byte";
        public const string DotNetByteArray       = "byte[]";
        public const string DotNetDateTime        = "DateTime";
        public const string DotNetDecimal         = "decimal";
        public const string DotNetDouble          = "double";
        public const string DotNetGuid            = "Guid";
        public const string DotNetInt16           = "short";
        public const string DotNetInt32           = "int";
        public const string DotNetInt64           = "long";
        public const string DotNetNullableDecimal = "decimal?";
        public const string DotNetObject          = "object";
        public const string DotNetStringName      = "string";
        public const string DotNetTimeSpan        = "TimeSpan";
        public const string EndsWith              = "EndsWith";

        public const string HostIP        = "HostIP";
        public const string HostIPValue   = "Context.ApplicationContext.Authentication.IPAddress";
        public const string HostName      = "HostName";
        public const string HostNameValue = "Context.ApplicationContext.Authentication.MachineName";

        public const string IsNull           = "IsNull";
        public const string LessThan         = "LessThan";
        public const string LessThanOrEquals = "LessThanOrEqual";
        public const string NotEqual         = "NotEqual";

        public const string StartsWith            = "StartsWith";
        public const string SystemDate            = "SystemDate";
        public const string SystemDateValue       = "DateTime.Now";
        public const string UpdateHostIP          = "UpdateHostIP";
        public const string UpdateHostIPValue     = "Context.ApplicationContext.Authentication.IPAddress";
        public const string UpdateHostName        = "UpdateHostName";
        public const string UpdateHostNameValue   = "Context.ApplicationContext.Authentication.MachineName";
        public const string UpdateSystemDate      = "UpdateSystemDate";
        public const string UpdateSystemDateValue = "DateTime.Now";

        public const string UpdateUserName      = "UpdateUserName";
        public const string UpdateUserNameValue = "Context.ApplicationContext.Authentication.UserName";
        public const string UserName            = "UserName";
        public const string UserNameValue       = "Context.ApplicationContext.Authentication.UserName";
        #endregion

        #region Static Fields
        internal static readonly string[] GenericUpdateInformationColumns =
        {
            UpdateUserName,
            UpdateHostName,
            UpdateHostIP,
            UpdateSystemDate
        };

        internal static readonly string[] GenericColumns =
        {
            UserName,
            HostName,
            HostIP,
            SystemDate,
            UpdateUserName,
            UpdateHostName,
            UpdateHostIP,
            UpdateSystemDate,
            ChannelId
        };
        #endregion

        #region Methods
        internal static string NormalizeColumnNameForReversedKeywords(this string columnName)
        {
            if (columnName == "Key")
            {
                return "[" + columnName + "]";
            }

            return columnName;
        }
        #endregion
    }
}