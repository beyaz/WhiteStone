using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared
{
    static class SelectByKeyMethodWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, ISelectByPrimaryKeyInfo data)
        {
            var parameterPart = string.Join(", ", data.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine($"static SqlInfo GetSelectByKeyInfo({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine();
            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(data.Sql);
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();

            sb.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (data.SqlParameters.Any())
            {
                sb.AppendLine();
                foreach (var columnInfo in data.SqlParameters)
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