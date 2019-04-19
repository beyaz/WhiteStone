using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class Contract
    {
        public TableInfo TableInfo { get; set; }

        string NamespaceFullName => $"BOA.Types.Kernel.Card.{TableInfo.SchemaName}";

        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine("using BOA.Types.Kernel.Card.DatabaseIntegration;");

            sb.AppendLine();
            sb.AppendLine("namespace " + NamespaceFullName);
            sb.AppendLine("{");
            sb.PaddingCount++;

           

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Entity contract for table {TableInfo.SchemaName}.{TableInfo.TableName}");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public class {TableInfo.TableName.ToContractName()}Contract : CardContractBase , {Names.ISupportDmlOperationInfo}" );
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(new ContractBodyDbMembers{Columns = TableInfo.Columns}.ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine($"#region {Names.ISupportDmlOperationInfo}");
            sb.AppendLine();
            sb.AppendAll(new GetInsertParametersMethod{TableInfo = TableInfo}.ToString());
            sb.AppendLine();
            
            sb.AppendLine();
            sb.AppendAll(new InsertSql{TableInfo = TableInfo}.ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine($"Databases {Names.ISupportDmlOperationInfo}.GetDatabase()");
            sb.AppendLine("{");
            sb.AppendLine($"    return Databases.{TableInfo.CatalogName};");
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("#endregion");

            sb.PaddingCount--;
            sb.AppendLine("}");


            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}