using BOA.Common.Helpers;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses
{
    


    static class ObjectHelperSqlUtilClassWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb,ConfigContract config)
        {
            var path = config.EmbeddedClassesDirectoryPath;

            var code = FileHelper.ReadFile(path + "ObjectHelperSqlUtil.txt");

            sb.AppendAll(code);

            sb.AppendLine();
        }
        #endregion
    }
}