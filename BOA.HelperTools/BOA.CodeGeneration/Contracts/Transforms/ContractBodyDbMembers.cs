using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{

    static class Names
    {
        public const string Parameter = "Parameter";

        public static string ToContractName(this string dbObjectName)
        {
            if (string.IsNullOrEmpty(dbObjectName))
            {
                return dbObjectName;
            }

            if (dbObjectName.Length == 1)
            {
                return dbObjectName.ToUpper();
            }

            var names = dbObjectName.SplitAndClear("_");

            return string.Join(string.Empty, names.Select(name => name.Substring(0, 1).ToUpper(new System.Globalization.CultureInfo("EN-US")) + name.Substring(1).ToLowerInvariant()));

            
        }

    }

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


    public class Contract
    {
        public TableInfo TableInfo { get; set; }

        string NamespaceFullName => $"BOA.Types.Kernel.Card.{TableInfo.SchemaName}";

        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("namespace " + NamespaceFullName);
            sb.AppendLine("{");
            sb.PaddingCount++;

           

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Entity contract for table {TableInfo.SchemaName}.{TableInfo.TableName}");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public class {TableInfo.TableName.ToContractName()}Contract : CardContractBase" );
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(new ContractBodyDbMembers{Columns = TableInfo.Columns}.ToString());
            sb.AppendLine();


            sb.AppendLine();
            sb.AppendAll(new GetInsertParametersMethod{TableInfo = TableInfo}.ToString());
            sb.AppendLine();
            


            sb.PaddingCount--;
            sb.AppendLine("}");


            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
    }

    class ContractBodyDbMembers
    {
        #region Public Properties
        public IReadOnlyCollection<ColumnInfo> Columns { get; set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("#region Database Columns");
            sb.AppendLine();
            foreach (var columnInfo in Columns)
            {
                sb.AppendLine();
                Write(sb, columnInfo);
            }
            sb.AppendLine();
            sb.AppendLine("#endregion");

            return sb.ToString();
        }
        #endregion

        #region Methods
        static void Write(StringBuilder sb, ColumnInfo data)
        {
            const string PaddingForComment = "     ";

            var comment = data.Comment;

            if (comment.HasValue())
            {
                sb.AppendLine("/// <summary>");
                var commentList    = comment.Split(Environment.NewLine.ToCharArray());
                var isFirstComment = true;
                foreach (var item in commentList)
                {
                    if (isFirstComment)
                    {
                        isFirstComment = false;
                        sb.AppendLine("///" + PaddingForComment + "" + item);
                    }
                    else
                    {
                        sb.AppendLine("///" + PaddingForComment + "<para> " + item + " </para>");
                    }
                }

                sb.AppendLine(@"/// </summary>");
            }

            sb.AppendLine("public " + data.DotNetType + " " + data.ColumnName.ToContractName() + " { get; set; }");
        }
        #endregion
    }
}