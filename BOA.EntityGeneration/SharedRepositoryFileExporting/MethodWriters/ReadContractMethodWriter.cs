using BOA.DataFlow;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.SharedRepositoryFileExporting.SharedFileExporter;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters
{
    static class ReadContractMethodWriter
    {
        #region Constants
        const string contractParameterName = "contract";
        #endregion

        #region Public Methods
        public static void Write(Context context)
        {
            var file = File[context];
            var tableInfo        = TableInfo[context];
            var config           = Config[context];
            var typeContractName = TableEntityClassNameForMethodParametersInRepositoryFiles[context];

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Reads one record from reader");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public static void ReadContract(IDataReader reader, {typeContractName} {contractParameterName})");
            file.AppendLine("{");
            file.PaddingCount++;

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

                file.AppendLine(contractReadLine);
            }

            file.PaddingCount--;
            file.AppendLine("}");
        }
        #endregion
    }
}