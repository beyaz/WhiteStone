using System.Data;
using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared
{
    static class ReadContractMethodWriter
    {
        const string contractParameterName = "contract";

        #region Public Methods
        public static void Write(IDataContext context)
        {
            var sb         = context.Get(SharedRepositoryFile);
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
                var readerLine = config.ReadLineDefault;

                if (columnInfo.SqlDbType == SqlDbType.Char &&
                    columnInfo.DotNetType == DotNetTypeName.DotNetBool)
                {
                    readerLine = config.ReadLineCharToBool;
                }
                else if (columnInfo.SqlDbType == SqlDbType.Char &&
                         columnInfo.DotNetType == DotNetTypeName.DotNetBoolNullable)
                {
                    readerLine = config.ReadLineCharToBoolNullable;
                }

                readerLine = readerLine
                             .Replace("{Contract}", contractParameterName)
                             .Replace("{PropertyName}", columnInfo.ColumnName.ToContractName())
                             .Replace("{ColumnName}", columnInfo.ColumnName)
                             .Replace("{SqlReaderMethod}", columnInfo.SqlReaderMethod.ToString());

                sb.AppendLine(readerLine);
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}