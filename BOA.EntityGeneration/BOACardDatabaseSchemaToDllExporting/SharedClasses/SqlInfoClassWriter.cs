using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses
{
    static class SqlInfoClassWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb,Config config)
        {
            var path = config.SharedClassConfig.EmbeddedClassesDirectoryPath;

            var code = FileHelper.ReadFile(path + "SqlInfo.txt");

            sb.AppendAll(code);

            sb.AppendLine();
        }
        #endregion
    }


    static class ObjectHelperSqlUtilClassWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb,Config config)
        {
            var path = config.SharedClassConfig.EmbeddedClassesDirectoryPath;

            var code = FileHelper.ReadFile(path + "ObjectHelperSqlUtil.txt");

            sb.AppendAll(code);

            sb.AppendLine();
        }
        #endregion
    }
}