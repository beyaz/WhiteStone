using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses
{
    static class SqlInfoClassWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb)
        {
            var path = Context.Get<Config>(Data.Config).SharedClassConfig.EmbeddedClassesDirectoryPath;

            var code = FileHelper.ReadFile(path + "SqlInfo.txt");

            sb.AppendAll(code);

            sb.AppendLine();
        }
        #endregion
    }


    static class ObjectHelperSqlUtilClassWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb)
        {
            var path = Context.Get<Config>(Data.Config).SharedClassConfig.EmbeddedClassesDirectoryPath;

            var code = FileHelper.ReadFile(path + "ObjectHelperSqlUtil.txt");

            sb.AppendAll(code);

            sb.AppendLine();
        }
        #endregion
    }
}