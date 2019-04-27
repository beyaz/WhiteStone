using BOA.Common.Helpers;
using BOA.EntityGeneration.ScriptModel.Creators;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class GeneratorOfTypeClass
    {
        #region Public Properties
        [Inject]
        public ContractBodyDbMembersCreator ContractBodyDbMembersCreator { get; set; }
        #endregion

        #region Public Methods
        public string TransformText(GeneratorData tableInfo)
        {
            var sb = new PaddedStringBuilder();


            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine("using BOA.Types.Kernel.Card.DatabaseIntegration;");

            sb.AppendLine();
            sb.AppendLine("namespace " + tableInfo.NamespaceFullNameOfTypeAssembly);
            sb.AppendLine("{");
            sb.PaddingCount++;

            ContractCommentInfoCreator.Write(sb, tableInfo);

            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {tableInfo.TableName.ToContractName()}Contract : CardContractBase");
            sb.AppendLine("{");
            sb.PaddingCount++;

            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine("// ReSharper disable once EmptyConstructor");
            sb.AppendLine($"public {tableInfo.TableName.ToContractName()}Contract()");
            sb.AppendLine("{");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendAll(ContractBodyDbMembersCreator.Create(tableInfo).PropertyDefinitions);
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("}"); // end of class

            sb.PaddingCount--;
            sb.AppendLine("}"); // end of namespace

            return sb.ToString();
        }
        #endregion
    }
}