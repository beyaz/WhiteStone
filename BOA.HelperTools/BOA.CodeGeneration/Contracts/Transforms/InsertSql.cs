using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    class InsertSql
    {
        public TableInfo TableInfo { get; set; }

        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"INSERT INTO [{TableInfo.SchemaName}].[{TableInfo.TableName}]");
            sb.AppendLine("(");
            sb.PaddingCount++;

            sb.AppendAll(string.Join(","+Environment.NewLine,TableInfo.Columns.Select(c=>"["+c.ColumnName+"]")));
            sb.AppendLine();
            
            sb.PaddingCount--;
            sb.AppendLine(")");

            sb.AppendLine("VALUES");

            sb.AppendLine("(");
            sb.PaddingCount++;

            sb.AppendAll(string.Join(","+Environment.NewLine,TableInfo.Columns.Select(c=>"@"+c.ColumnName)));
            sb.AppendLine();
            
            sb.PaddingCount--;
            sb.AppendLine(")");

            return sb.ToString();
        }
    }
}