using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared
{
    static class SelectByUniqueIndexMethodWriter
    {
        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb        = context.Get(Data.SharedRepositoryFile);
            var tableInfo = context.Get(Data.TableInfo);

            foreach (var indexIdentifier in tableInfo.UniqueIndexInfoList)
            {
                var indexInfo     = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);
                var methodName    = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));
                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public static SqlInfo {methodName}({parameterPart})");
                sb.OpenBracket();

                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(indexInfo.Sql);
                sb.AppendLine();
                sb.AppendLine("\";");
                sb.AppendLine();
                sb.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

                if (indexInfo.SqlParameters.Any())
                {
                    sb.AppendLine();
                    foreach (var columnInfo in indexInfo.SqlParameters)
                    {
                        sb.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                    }
                }

                sb.AppendLine();
                sb.AppendLine("return sqlInfo;");

                sb.CloseBracket();
            }
        }
        #endregion
    }
}