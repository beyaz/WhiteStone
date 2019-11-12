using BOA.Common.Helpers;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses
{
    static class SqlInfoClassWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, string embeddedClassesDirectoryPath, string namespaceName)
        {
            var code = FileHelper.ReadFile(embeddedClassesDirectoryPath + "SqlInfo.txt");

            code = code.Replace("____Namespace____", namespaceName);

            sb.AppendAll(code);

            sb.AppendLine();
        }
        #endregion
    }


    static class ObjectHelperSqlUtilClassWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, string embeddedClassesDirectoryPath, string namespaceName)
        {
            var code = FileHelper.ReadFile(embeddedClassesDirectoryPath + "ObjectHelperSqlUtil.txt");

            code = code.Replace("____Namespace____", namespaceName);

            sb.AppendAll(code);

            sb.AppendLine();
        }
        #endregion
    }
}