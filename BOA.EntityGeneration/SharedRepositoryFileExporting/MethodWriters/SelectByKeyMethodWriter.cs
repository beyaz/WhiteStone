using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.SharedRepositoryFileExporting.SharedFileExporter;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters
{
    static class SelectByKeyMethodWriter
    {
        #region Public Methods
        public static void Write(Context context)
        {
            var file = File[context];
            var tableInfo              = TableInfo[context];
            var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);

            var sqlParameters = selectByPrimaryKeyInfo.SqlParameters;
            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine($"public static SqlInfo SelectByKey({parameterPart})");
            file.OpenBracket();

            file.AppendLine("const string sql = @\"");
            file.AppendAll(selectByPrimaryKeyInfo.Sql);
            file.AppendLine();
            file.AppendLine("\";");
            file.AppendLine();

            file.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (sqlParameters.Any())
            {
                file.AppendLine();
                foreach (var columnInfo in sqlParameters)
                {
                    file.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                }
            }

            file.AppendLine();
            file.AppendLine("return sqlInfo;");

            file.CloseBracket();
        }
        #endregion
    }
}