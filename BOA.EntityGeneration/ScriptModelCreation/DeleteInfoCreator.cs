using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.ScriptModelCreation
{
    public static class DeleteInfoCreator
    {
        #region Public Methods
        public static DeleteInfo Create(TableInfo tableInfo)
        {
            var parameters = tableInfo.PrimaryKeyColumns;

            return new DeleteInfo
            {
                Sql           = $"DELETE FROM [{tableInfo.SchemaName}].[{tableInfo.TableName}] WHERE {string.Join(" , ", parameters.Select(c => $"[{c.ColumnName}] = @{c.ColumnName}"))}",
                SqlParameters = parameters
            };
        }
        #endregion
    }


    public static class ContractCommentInfoCreator
    {
        #region Public Methods
        public static ContractCommentInfo Create(TableInfo tableInfo)
        {
            var sb = new PaddedStringBuilder();

            Write(sb,tableInfo);


            return new ContractCommentInfo
            {
                Comment           = sb.ToString()
            };
        }

        public static void Write(PaddedStringBuilder sb, TableInfo tableInfo)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Entity contract for table {tableInfo.SchemaName}.{tableInfo.TableName}");

            foreach (var indexInfo in tableInfo.IndexInfoList)
            {
                sb.AppendLine($"///{Padding.ForComment}<para>{indexInfo}</para>");
            }

            sb.AppendLine("/// </summary>");
        }
        #endregion
    }
}