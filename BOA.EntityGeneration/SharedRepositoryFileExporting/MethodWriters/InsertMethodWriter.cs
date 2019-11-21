using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using BOA.EntityGeneration.Util;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters
{
    static class InsertMethodWriter
    {
        const string contractParameterName = "contract";

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb                     = context.Get(SharedFileExporter.File);
            var tableInfo              = TableInfo[context];
            var typeContractName = context.Get(TableEntityClassNameForMethodParametersInRepositoryFiles);

            var insertInfo = new InsertInfoCreator().Create(tableInfo);

            var sqlParameters = insertInfo.SqlParameters;
            

            sb.AppendLine($"public static SqlInfo Insert({typeContractName} {contractParameterName})");
            sb.OpenBracket();

            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(insertInfo.Sql);
            sb.AppendLine();
            if (tableInfo.HasIdentityColumn)
            {
                sb.AppendLine("SELECT CAST(SCOPE_IDENTITY() AS INT)");
            }
            sb.AppendLine("\";");
            sb.AppendLine();

            sb.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (sqlParameters.Any())
            {
                sb.AppendLine();
                foreach (var columnInfo in sqlParameters)
                {
                    sb.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlInsert(columnInfo)});");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return sqlInfo;");

            sb.CloseBracket();
        }
        #endregion
    }
}