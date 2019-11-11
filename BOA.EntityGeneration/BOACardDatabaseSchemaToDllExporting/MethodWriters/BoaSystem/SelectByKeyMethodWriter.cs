﻿using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem
{
    static class SelectByKeyMethodWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, BusinessClassWriterContext businessClassWriterContext, Config config)
        {
            var typeContractName = businessClassWriterContext.TypeContractName;

            var parameterPart = string.Join(", ", businessClassWriterContext.SelectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"var sqlInfo = GetSelectByKeyInfo({string.Join(", ", businessClassWriterContext.SelectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))})");

            sb.AppendLine($"return this.ExecuteReaderToContract(\"{businessClassWriterContext.BusinessClassNamespace}.{businessClassWriterContext.ClassName}.SelectByKey\", sqlInfo, readContract);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}