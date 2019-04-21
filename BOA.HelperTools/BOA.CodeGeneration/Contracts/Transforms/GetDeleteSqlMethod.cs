using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    class GetDeleteSqlMethod
    {
        #region Public Properties
        public TableInfo TableInfo { get; set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationDelete}.GetDeleteSql()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return @\"");
            sb.PaddingCount++;
            sb.AppendLine($"DELETE FROM [{TableInfo.SchemaName}].[{TableInfo.TableName}] WHERE {string.Join(" , ", TableInfo.GetWhereParameters().Select(c => $"[{c.ColumnName}] = @{c.ColumnName}"))}");
            sb.PaddingCount--;
            sb.AppendLine("\";");

            sb.PaddingCount--;
            sb.AppendLine("}");
            sb.PaddingCount++;

            return sb.ToString();
        }
        #endregion
    }
}