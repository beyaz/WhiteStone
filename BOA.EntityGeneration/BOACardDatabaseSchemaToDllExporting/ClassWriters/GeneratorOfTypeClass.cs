using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.ScriptModel.Creators;
using Ninject;
using static ___Company___.EntityGeneration.DataFlow.DataContext;
using ___Company___.EntityGeneration.DataFlow;


namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    /// <summary>
    ///     The generator of type class
    /// </summary>
    public class GeneratorOfTypeClass
    {

        #region Public Methods
        /// <summary>
        ///     Writes the using list.
        /// </summary>
        public static void WriteUsingList()
        {
            var sb = Context.Get(Data.TypesFileOutput);
            var config = Context.Get(Data.Config);

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");

            foreach (var line in config.TypeUsingLines)
            {
                sb.AppendLine(line);
            }
        }

        /// <summary>
        ///     Writes the class.
        /// </summary>
        public void WriteClass(PaddedStringBuilder sb, ITableInfo tableInfo)
        {
            var config = Context.Get(Data.Config);

            ContractCommentInfoCreator.Write(sb, tableInfo);

            var inheritancePart = string.Empty;

            if (config.TypeContractBase != null)
            {
                inheritancePart = ": " + config.TypeContractBase;
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