using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.ScriptModelCreation;
using Ninject;

namespace BOA.EntityGeneration.Generators
{
    public class Contract
    {
        #region Public Properties
        [Inject]
        public ContractBodyDbMembersCreator ContractBodyDbMembersCreator { get; set; }

        [Inject]
        public GetAll GetAll { get; set; }

        [Inject]
        public GetDeletePart GetDeletePart { get; set; }

        [Inject]
        public GetInsertPart GetInsertPart { get; set; }

        [Inject]
        public GetUpdatePart GetUpdatePart { get; set; }

        [Inject]
        public IndexIdentifiers IndexIdentifiers { get; set; }

        [Inject]
        public ReadContractMethod ReadContractMethod { get; set; }

        [Inject]
        public SelectByIndex SelectByIndex { get; set; }

        [Inject]
        public SelectByKeys SelectByKeys { get; set; }

        [Inject]
        public SelectByUniqueIndex SelectByUniqueIndex { get; set; }
        #endregion

        #region Public Methods
        public string TransformText(ContractData data)
        {
            var sb = new PaddedStringBuilder();

            var TableInfo = data.TableInfo;

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine("using BOA.Types.Kernel.Card.DatabaseIntegration;");

            sb.AppendLine();
            sb.AppendLine("namespace " + data.NamespaceFullNameOfTypeAssembly);
            sb.AppendLine("{");
            sb.PaddingCount++;

            ContractCommentInfoCreator.Write(sb, TableInfo);

            sb.AppendLine("[Serializable]");

            sb.AppendLine($"public sealed class {TableInfo.TableName.ToContractName()}Contract : CardContractBase , {string.Join(", ", data.ContractInterfaces)}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            ContractCommentInfoCreator.Write(sb, TableInfo);
            sb.AppendLine("// ReSharper disable once EmptyConstructor");
            sb.AppendLine($"public {TableInfo.TableName.ToContractName()}Contract()");
            sb.AppendLine("{");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendAll(ContractBodyDbMembersCreator.Create(TableInfo).PropertyDefinitions);
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(IndexIdentifiers.TransformText(data));
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine($"#region {Names.ISupportDmlOperation}");

            #region Database
            sb.AppendLine();
            sb.AppendLine($"Databases {Names.ISupportDmlOperation}.Database");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region get
            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;
            sb.AppendLine($"return Databases.{data.DatabaseEnumName};");
            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.AppendLine();
            sb.AppendAll(ReadContractMethod.TransformText(data));
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine("#endregion");

            if (data.IsSupportInsert)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationInsert}");

                sb.AppendLine();
                sb.AppendAll(GetInsertPart.TransformText(data));
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            if (data.IsSupportSelectByKey)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationUpdate}");

                sb.AppendLine();
                sb.AppendAll(GetUpdatePart.TransformText(data));
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            if (data.IsSupportSelectByKey)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationSelectByKey}");

                sb.AppendLine();
                sb.AppendAll(SelectByKeys.TransformText(data));
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            if (data.IsSupportSelectByUniqueIndex)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationSelectByUniqueIndex}");

                sb.AppendLine();
                sb.AppendAll(SelectByUniqueIndex.TransformText(data));
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            if (data.IsSupportSelectByIndex)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationSelectByIndex}");

                sb.AppendLine();
                sb.AppendAll(SelectByIndex.TransformText(data));
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            if (data.IsSupportSelectByKey)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationDelete}");

                sb.AppendLine();
                sb.AppendAll(GetDeletePart.TransformText(data));
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            if (data.IsSupportGetAll)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationDelete}");

                sb.AppendLine();
                sb.AppendAll(GetAll.TransformText(data));

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
    }
}