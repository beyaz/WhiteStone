using BOA.DataFlow;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters
{
    static class ReadContractMethodWriter
    {
        #region Constants
        const string contractParameterName = "contract";
        #endregion

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb               = context.Get(SharedFileExporter.File);
            var tableInfo        = TableInfo[context];
            var config           = Config[context];
            var typeContractName = context.Get(TableEntityClassNameForMethodParametersInRepositoryFiles);

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Reads one record from reader");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public static void ReadContract(IDataReader reader, {typeContractName} {contractParameterName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var columnInfo in tableInfo.Columns)
            {
                var readerMethodName = columnInfo.SqlReaderMethod.ToString();
                if (columnInfo.SqlReaderMethod == SqlReaderMethods.GetGUIDValue)
                {
                    readerMethodName = "GetGuidValue";
                }

                var contractReadLine = config.ContractReadLine
                                             .Replace("$(Contract)", contractParameterName)
                                             .Replace("$(PropertyName)", columnInfo.ColumnName.ToContractName())
                                             .Replace("$(ColumnName)", columnInfo.ColumnName)
                                             .Replace("$(SqlReaderMethod)", readerMethodName);

                sb.AppendLine(contractReadLine);
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}