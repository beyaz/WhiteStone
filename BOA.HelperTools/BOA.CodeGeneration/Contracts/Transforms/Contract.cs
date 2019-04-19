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
}