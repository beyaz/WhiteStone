using System.Globalization;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    static class Names
    {
        #region Constants
        public const string Parameter       = nameof(Parameter);
        public const string ROW_GUID        = nameof(ROW_GUID);
        public const string UPDATE_DATE     = nameof(UPDATE_DATE);
        public const string VALID_FLAG = nameof(VALID_FLAG);
        public const string UPDATE_TOKEN_ID = nameof(UPDATE_TOKEN_ID);
        public const string UPDATE_USER_ID  = nameof(UPDATE_USER_ID);

        public const string INSERT_DATE = nameof(INSERT_DATE);
        public const string INSERT_USER_ID = nameof(INSERT_USER_ID);
        public const string INSERT_TOKEN_ID = nameof(INSERT_TOKEN_ID);

        public const string ExecutionScope = nameof(ExecutionScope);

        public const string ISupportDmlOperationInfo = nameof(ISupportDmlOperationInfo);
        
        
        #endregion

        #region Public Methods
        public static string ToContractName(this string dbObjectName)
        {
            if (string.IsNullOrEmpty(dbObjectName))
            {
                return dbObjectName;
            }

            if (dbObjectName.Length == 1)
            {
                return dbObjectName.ToUpper();
            }

            var names = dbObjectName.SplitAndClear("_");

            return string.Join(string.Empty, names.Select(name => name.Substring(0, 1).ToUpper(new CultureInfo("EN-US")) + name.Substring(1).ToLowerInvariant()));
        }
        #endregion
    }
}