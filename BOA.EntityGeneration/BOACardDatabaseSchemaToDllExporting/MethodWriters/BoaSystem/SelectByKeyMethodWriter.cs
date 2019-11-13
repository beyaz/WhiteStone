using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class SelectByKeyMethodWriter
    {
        static readonly bool IsOldSystem = true;

        #region Public Methods
        public static void Write(PaddedStringBuilder sb, BusinessClassWriterContext businessClassWriterContext)
        {
            if (IsOldSystem)
            {
                Write_old(sb,businessClassWriterContext);
                return;
            }
            var typeContractName  = businessClassWriterContext.TypeContractName;
            var sharedClassConfig = businessClassWriterContext.SharedClassConfig;
            var schemaName = businessClassWriterContext.TableInfo.SchemaName;
            var tableName = businessClassWriterContext.TableInfo.TableName;

            var parameterPart = string.Join(", ", businessClassWriterContext.SelectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"var sqlInfo = {schemaName}_Core.{tableName.ToContractName()}.{sharedClassConfig.MethodNameOfSelecyByKey}({string.Join(", ", businessClassWriterContext.SelectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

            sb.AppendLine($"return ObjectHelperSqlUtil.ExecuteReaderToContract<{typeContractName}>(this, \"{businessClassWriterContext.BusinessClassNamespace}.{businessClassWriterContext.ClassName}.SelectByKey\", sqlInfo, ReadContract);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        static void Write_old(PaddedStringBuilder sb, BusinessClassWriterContext businessClassWriterContext)
        {
            var selectByPrimaryKeyInfo = businessClassWriterContext.SelectByPrimaryKeyInfo;
            var typeContractName = businessClassWriterContext.TypeContractName;

            var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(selectByPrimaryKeyInfo.Sql);
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();
            sb.AppendLine("var command = this.CreateCommand(sql);");

            if (selectByPrimaryKeyInfo.SqlParameters.Any())
            {
                sb.AppendLine();
                foreach (var columnInfo in selectByPrimaryKeyInfo.SqlParameters)
                {
                    sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                }
            }

            sb.AppendLine();
            sb.AppendLine($"return this.ExecuteReaderForOnlyOneRecord<{typeContractName}>(command, ReadContract);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}