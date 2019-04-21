using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    class GetUpdateSqlMethod
    {
        #region Public Properties
        public TableInfo TableInfo { get; set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationSave}.GetUpdateSql()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return @\"");

            sb.AppendLine($"UPDATE [{TableInfo.SchemaName}].[{TableInfo.TableName}] SET");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetColumnsWillBeUpdate().Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("WHERE");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetWhereParameters().Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

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