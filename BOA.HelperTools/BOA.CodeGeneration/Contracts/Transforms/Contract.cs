using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class Contract
    {
        public TableInfo TableInfo { get; set; }

        string NamespaceFullName => $"BOA.Types.Kernel.Card.{TableInfo.SchemaName}";

        bool IsSupportGetAll => TableInfo.SchemaName == "PRM";

        bool IsSupportSave => TableInfo.PrimaryKeyColumns.Any();

        List<string> GetInterfaces()
        {
            var interfaces = new List<string>();

            if (IsSupportSave)
            {
                interfaces.Add(Names.ISupportDmlOperationSave);
            }

            if (IsSupportSave)
            {
                interfaces.Add(Names.ISupportDmlOperationDelete);
            }

            if (IsSupportGetAll)
            {
                interfaces.Add(Names.ISupportDmlOperationGetAll);
            }

            return interfaces;
        }

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

           

            WriteMainComment(sb);

            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {TableInfo.TableName.ToContractName()}Contract : CardContractBase , {string.Join(", ",GetInterfaces())}" );
            sb.AppendLine("{");
            sb.PaddingCount++;


            WriteMainComment(sb);
            sb.AppendLine("// ReSharper disable once EmptyConstructor");
            sb.AppendLine($"public {TableInfo.TableName.ToContractName()}Contract()");
            sb.AppendLine("{");
            sb.AppendLine("}");

            sb.AppendAll(new ContractBodyDbMembers{Columns = TableInfo.Columns}.ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine($"#region {Names.ISupportDmlOperation}");
            sb.AppendLine();
            sb.AppendAll(new GetInsertParametersMethod{TableInfo = TableInfo}.ToString());
            sb.AppendLine();
            
            sb.AppendLine();
            sb.AppendAll(new GetInsertSqlMethod{TableInfo = TableInfo}.ToString());
            sb.AppendLine();


            sb.AppendLine();
            sb.AppendAll(new GetUpdateParametersMethod{TableInfo = TableInfo}.ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(new GetUpdateSqlMethod{TableInfo = TableInfo}.ToString());
            sb.AppendLine();


            // Delete
            sb.AppendLine();
            sb.AppendAll(new GetDeleteParametersMethod{TableInfo = TableInfo}.ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(new GetDeleteSqlMethod{TableInfo = TableInfo}.ToString());
            sb.AppendLine();


            sb.AppendLine();
            sb.AppendAll(new ReadContractMethod{TableInfo = TableInfo}.ToString());
            sb.AppendLine();


            if (IsSupportGetAll)
            {
                sb.AppendLine();
                sb.AppendAll(new GetAllSqlMethod{TableInfo = TableInfo}.ToString());
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine($"Databases {Names.ISupportDmlOperation}.GetDatabase()");
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

        void WriteMainComment(PaddedStringBuilder sb)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Entity contract for table {TableInfo.SchemaName}.{TableInfo.TableName}");

            foreach (var indexInfo in TableInfo.IndexInfoList)
            {
                sb.AppendLine($"///     <para>{indexInfo}</para>");
            }

            sb.AppendLine("/// </summary>");
        }
    }
}