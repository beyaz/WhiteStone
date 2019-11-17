using System.Data;
using BOA.DataFlow;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters
{
    static class ReadContractMethodWriter
    {
        const string contractParameterName = "contract";

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb         = context.Get(SharedFileExporter.File);
            var tableInfo  = context.Get(TableInfo);
            var config = context.Get(Config);
            var typeContractName = context.Get(TableEntityClassNameForMethodParametersInRepositoryFiles);
           
            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Reads one record from reader");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"static void ReadContract(IDataReader reader, {typeContractName} {contractParameterName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var columnInfo in tableInfo.Columns)
            {
                var contractReadLine = config.ContractReadLine
                                             .Replace("$(Contract)", contractParameterName)
                                             .Replace("$(PropertyName)", columnInfo.ColumnName.ToContractName())
                                             .Replace("$(ColumnName)", columnInfo.ColumnName)
                                             .Replace("$(SqlReaderMethod)", columnInfo.SqlReaderMethod.ToString());

                sb.AppendLine(contractReadLine);
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}