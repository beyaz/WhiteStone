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
            sb.AppendLine($"public sealed class {TableInfo.TableName.ToContractName()}Contract : CardContractBase , {string.Join(", ", Data.ContractInterfaces)}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            WriteMainComment(sb);
            sb.AppendLine("// ReSharper disable once EmptyConstructor");
            sb.AppendLine($"public {TableInfo.TableName.ToContractName()}Contract()");
            sb.AppendLine("{");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendAll(new ContractBodyDbMembers {Columns = TableInfo.Columns}.ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(Create<IndexIdentifiers>().ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine($"#region {Names.ISupportDmlOperation}");

            sb.AppendLine();
            sb.AppendLine($"Databases {Names.ISupportDmlOperation}.GetDatabase()");
            sb.AppendLine("{");
            sb.AppendLine($"    return Databases.{Data.DatabaseEnumName};");
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendAll(Create<ReadContractMethod>().ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine("#endregion");



           


            if (Data.IsSupportSave)
            {

                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationSave}");

                sb.AppendLine();
                sb.AppendAll(Create<GetInsertPart>().ToString());
                sb.AppendLine();

                


                sb.AppendLine();
                sb.AppendAll(Create<GetUpdatePart>().ToString());
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            if (Data.IsSupportSelectByKey)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationSelectByKey}");

               
                sb.AppendLine();
                sb.AppendAll(Create<SelectByKeys>().ToString());
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");

            }

            if (Data.IsSupportSelectByUniqueIndex)
            {

                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationSelectByUniqueIndex}");

                sb.AppendLine();
                sb.AppendAll(Create<SelectByUniqueIndex>().ToString());
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");


                
            }

            if (Data.IsSupportSelectByKey)
            {

                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationDelete}");

               


                sb.AppendLine();
                sb.AppendAll(Create<GetDeletePart>().ToString());
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

           

            if (Data.IsSupportGetAll)
            {

                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationDelete}");

                

                sb.AppendLine();
                sb.AppendAll(Create<GetAll>().ToString());
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

           

           

            sb.PaddingCount--;
            sb.AppendLine("}"); // end of class

            sb.PaddingCount--;
            sb.AppendLine("}"); // end of namespace

            return sb.ToString();
        }
        #endregion

        #region Methods
        void WriteMainComment(PaddedStringBuilder sb)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{PaddingForComment}Entity contract for table {TableInfo.SchemaName}.{TableInfo.TableName}");

            foreach (var indexInfo in TableInfo.IndexInfoList)
            {
                sb.AppendLine($"///{PaddingForComment}<para>{indexInfo}</para>");
            }

            sb.AppendLine("/// </summary>");
        }
        #endregion
    }
}