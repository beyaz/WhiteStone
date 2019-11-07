using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.ScriptModel.Creators;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    public class GeneratorOfTypeClass
    {
        #region Public Properties
        [Inject]
        public Config Config { get; set; }

        [Inject]
        public ContractBodyDbMembersCreator ContractBodyDbMembersCreator { get; set; }

        [Inject]
        public NamingHelper NamingHelper { get; set; }
        #endregion

        #region Public Methods
        public static void WriteUsingList(PaddedStringBuilder sb, ITableInfo tableInfo, Config config)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");

            foreach (var line in config.TypeUsingLines)
            {
                sb.AppendLine(line);
            }
        }

        public void WriteClass(PaddedStringBuilder sb, ITableInfo tableInfo)
        {
            ContractCommentInfoCreator.Write(sb, tableInfo);

            var inheritancePart = string.Empty;

            if (Config.TypeContractBase != null)
            {
                inheritancePart = ": " + Config.TypeContractBase;
            }

            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {tableInfo.TableName.ToContractName()}Contract {inheritancePart}");
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
        }
        #endregion
    }
}