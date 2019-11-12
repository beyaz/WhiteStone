using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared
{
    static class DeleteByKeyMethodWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, IDeleteInfo deleteInfo, SharedClassConfig config)
        {
            var sqlParameters = deleteInfo.SqlParameters;

            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine($"public static SqlInfo {config.MethodNameOfDeleteByKey}({parameterPart})");
           sb.OpenBracket();

            sb.AppendLine($"const string sql = \"{deleteInfo.Sql}\";");
            sb.AppendLine();

            sb.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (sqlParameters.Any())
            {
                sb.AppendLine();
                foreach (var columnInfo in sqlParameters)
                {
                    sb.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return sqlInfo;");

            sb.CloseBracket();
        }
        #endregion
    }
}