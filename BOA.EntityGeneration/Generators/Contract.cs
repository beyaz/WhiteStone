﻿using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.ScriptModelCreation;
using Ninject;

namespace BOA.EntityGeneration.Generators
{
    public class Contract : GeneratorBase
    {
        [Inject]
        public GetAll GetAll { get; set; }

        #region Public Methods
        public string TransformText(GeneratorData data)
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

            ContractCommentInfoCreator.Write(sb, TableInfo);

            sb.AppendLine("[Serializable]");

            sb.AppendLine($"public sealed class {TableInfo.TableName.ToContractName()}Contract : CardContractBase , {string.Join(", ", Data.ContractInterfaces)}");
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
            sb.AppendAll(Create<IndexIdentifiers>().ToString());
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
            sb.AppendLine($"return Databases.{Data.DatabaseEnumName};");
            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.AppendLine();
            sb.AppendAll(Create<ReadContractMethod>().ToString());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine("#endregion");

            if (Data.IsSupportInsert)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationInsert}");

                sb.AppendLine();
                sb.AppendAll(Create<GetInsertPart>().ToString());
                sb.AppendLine();

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            if (Data.IsSupportUpdate)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationUpdate}");

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

            if (Data.IsSupportSelectByIndex)
            {
                sb.AppendLine();
                sb.AppendLine($"#region {Names.ISupportDmlOperationSelectByIndex}");

                sb.AppendLine();
                sb.AppendAll(Create<SelectByIndex>().ToString());
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