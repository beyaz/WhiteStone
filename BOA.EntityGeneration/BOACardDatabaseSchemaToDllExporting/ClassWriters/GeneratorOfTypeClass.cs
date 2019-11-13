using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    /// <summary>
    ///     The generator of type class
    /// </summary>
    static class GeneratorOfTypeClass
    {
        #region Public Methods
        /// <summary>
        ///     Begins the namespace.
        /// </summary>
        public static void BeginNamespace(IDataContext context)
        {
            var sb         = context.Get(Data.TypeClassesOutput);
            var schemaName = context.Get(Data.SchemaName);
            var config = context.Get(Data.Config);

            sb.BeginNamespace(NamingHelper.GetTypeClassNamespace(schemaName,config));
        }

        /// <summary>
        ///     Ends the namespace.
        /// </summary>
        public static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(Data.TypeClassesOutput);
            sb.EndNamespace();
        }

        /// <summary>
        ///     Writes the class.
        /// </summary>
        public static void WriteClass(IDataContext context)
        {
            var sb        = context.Get(Data.TypeClassesOutput);
            var config    = context.Get(Data.Config);
            var tableInfo = context.Get(Data.TableInfo);

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
        public static void WriteUsingList(IDataContext context)
        {
            var sb     = context.Get(Data.TypeClassesOutput);
            var config = context.Get(Data.Config);

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