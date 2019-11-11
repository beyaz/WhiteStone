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

            var parameterPart = string.Join(", ", deleteInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine($"static SqlInfo {config.MethodNameOfDeleteByKey}({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine();
            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(deleteInfo.Sql);
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();

            sb.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (deleteInfo.SqlParameters.Any())
            {
                sb.AppendLine();
                foreach (var columnInfo in deleteInfo.SqlParameters)
                {
                    sb.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return sqlInfo;");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}