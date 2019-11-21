using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.Naming.NamingPatternContract;

namespace BOA.EntityGeneration
{
    static class EntityFileExporter
    {
        #region Static Fields
        static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, InitializeOutput);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, WriteUsingList);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, EmptyLine);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, BeginNamespace);

            context.AttachEvent(TableExportingEvent.TableExportStarted, WriteClass);

            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, EndNamespace);
            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, ExportFileToDirectory);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb            = File[context];
            var namingPattern = NamingPattern[context];

            sb.BeginNamespace(namingPattern.EntityNamespace);
        }

        static void EmptyLine(IDataContext context)
        {
            File[context].AppendLine();
        }

        static void EndNamespace(IDataContext context)
        {
            File[context].EndNamespace();
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var sb            = File[context];
            var namingPattern = NamingPattern[context];
            var processInfo   = SchemaGenerationProcess[context];

            processInfo.Text = "Exporting Entity classes.";

            var filePath = namingPattern.EntityProjectDirectory + "All.cs";

            FileSystem.WriteAllText(context, filePath, sb.ToString());
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void WriteClass(IDataContext context)
        {
            var sb                 = File[context];
            var config             = context.Get(Config);
            var tableInfo          = context.Get(TableInfo);
            var tableNamingPattern = context.Get(TableNamingPatternContract.TableNamingPattern);

            ContractCommentInfoCreator.Write(sb, tableInfo);

            var inheritancePart = string.Empty;

            if (config.EntityContractBase != null)
            {
                inheritancePart = ": " + config.EntityContractBase;
            }

            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {tableNamingPattern.EntityClassName} {inheritancePart}");
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

        static void WriteUsingList(IDataContext context)
        {
            var sb            = File[context];
            var namingPattern = NamingPattern[context];

            foreach (var line in namingPattern.EntityUsingLines)
            {
                sb.AppendLine(line);
            }
        }
        #endregion
    }
}