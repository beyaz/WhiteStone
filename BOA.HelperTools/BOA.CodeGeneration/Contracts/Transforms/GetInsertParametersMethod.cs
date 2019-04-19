using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    class GetInsertParametersMethod
    {
        public TableInfo TableInfo { get; set; }

        static string GetParameter(ColumnInfo columnInfo)
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("new Parameter");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"Name = \"@{columnInfo.ColumnName}\",");
            sb.AppendLine($"SqlDbType = SqlDbType.{columnInfo.SqlDatabaseTypeName},");
            sb.AppendLine($"Value = {columnInfo.ColumnName.ToContractName()}");

            sb.PaddingCount--;
            sb.AppendLine("}");



            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("internal IReadOnlyList<Parameter> GetInsertParameters()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return new List<Parameter>");
            sb.AppendLine("{");
            sb.PaddingCount++;


            sb.AppendAll(string.Join(","+Environment.NewLine,TableInfo.Columns.Select(GetParameter)));
            sb.AppendLine();


            sb.PaddingCount--;
            sb.AppendLine("}");




            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}