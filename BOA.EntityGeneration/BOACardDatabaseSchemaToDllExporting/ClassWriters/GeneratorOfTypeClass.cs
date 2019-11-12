using ___Company___.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    /// <summary>
    ///     The generator of type class
    /// </summary>
    static class GeneratorOfTypeClass
    {
        #region Public Methods
        /// <summary>
        ///     Writes the class.
        /// </summary>
        public static void WriteClass()
        {
            var sb        = Context.Get(Data.TypesFileOutput);
            var config    = Context.Get(Data.Config);
            var tableInfo = Context.Get(Data.TableInfo);

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

        /// <summary>
        ///     Writes the using list.
        /// </summary>
        public static void WriteUsingList()
        {
            var sb     = Context.Get(Data.TypesFileOutput);
            var config = Context.Get(Data.Config);

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");

            foreach (var line in config.TypeUsingLines)
            {
                sb.AppendLine(line);
            }
        }
        #endregion
    }
}