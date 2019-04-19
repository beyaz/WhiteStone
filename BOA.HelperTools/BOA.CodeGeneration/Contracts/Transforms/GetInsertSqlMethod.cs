using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    class GetInsertSqlMethod
    {
        #region Public Properties
        public TableInfo TableInfo { get; set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationInfo}.GetInsertSql()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return @\"");

            sb.AppendLine($"INSERT INTO [{TableInfo.SchemaName}].[{TableInfo.TableName}]");
            sb.AppendLine("(");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetColumnsWillBeInsert().Select(c => "[" + c.ColumnName + "]")));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine(")");

            sb.AppendLine("VALUES");

            sb.AppendLine("(");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetColumnsWillBeInsert().Select(c => "@" + c.ColumnName)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine(")");

            sb.AppendLine("\";");

            sb.PaddingCount--;
            sb.AppendLine("}");
            sb.PaddingCount++;

            return sb.ToString();
        }
        #endregion
    }
}