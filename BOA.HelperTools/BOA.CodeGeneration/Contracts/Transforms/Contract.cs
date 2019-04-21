using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class Contract : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine("using BOA.Types.Kernel.Card.DatabaseIntegration;");

            sb.AppendLine();
            sb.AppendLine("namespace " + Data.NamespaceFullName);
            sb.AppendLine("{");
            sb.PaddingCount++;

            WriteMainComment(sb);

            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {TableInfo.TableName.ToContractName()}Contract : CardContractBase , {string.Join(", ", GetInterfaces())}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            WriteMainComment(sb);
            sb.AppendLine("// ReSharper disable once EmptyConstructor");
            sb.AppendLine($"public {TableInfo.TableName.ToContractName()}Contract()");
            sb.AppendLine("{");
            sb.AppendLine("}");

            sb.AppendAll(new ContractBodyDbMembers {Columns = TableInfo.Columns}.ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine($"#region {Names.ISupportDmlOperation}");
            sb.AppendLine();
            sb.AppendAll(Create<GetInsertParametersMethod>().ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(Create<GetInsertSqlMethod>().ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(Create<GetUpdateParametersMethod>().ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(Create<GetUpdateSqlMethod>().ToString());
            sb.AppendLine();

            // Delete
            sb.AppendLine();
            sb.AppendAll(Create<GetDeleteParametersMethod>().ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(Create<GetDeleteSqlMethod>().ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(Create<ReadContractMethod>().ToString());
            sb.AppendLine();

            if (Data.IsSupportGetAll)
            {
                sb.AppendLine();
                sb.AppendAll(Create<GetAllSqlMethod>().ToString());
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine($"Databases {Names.ISupportDmlOperation}.GetDatabase()");
            sb.AppendLine("{");
            sb.AppendLine($"    return Databases.{Data.DatabaseEnumName};");
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("#endregion");

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion

        #region Methods
        List<string> GetInterfaces()
        {
            var interfaces = new List<string>();

            if (Data.IsSupportSave)
            {
                interfaces.Add(Names.ISupportDmlOperationSave);
            }

            if (Data.IsSupportSave)
            {
                interfaces.Add(Names.ISupportDmlOperationDelete);
            }

            if (Data.IsSupportGetAll)
            {
                interfaces.Add(Names.ISupportDmlOperationGetAll);
            }

            return interfaces;
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
        #endregion
    }
}