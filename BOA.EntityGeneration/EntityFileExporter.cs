using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;
using static BOA.EntityGeneration.DataFlow.TableExportingEvent;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

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
            context.AttachEvent(SchemaExportStarted, InitializeOutput);
            context.AttachEvent(SchemaExportStarted, WriteUsingList);
            context.AttachEvent(SchemaExportStarted, EmptyLine);
            context.AttachEvent(SchemaExportStarted, BeginNamespace);

            context.AttachEvent(TableExportStarted, WriteClass);

            context.AttachEvent(SchemaExportFinished, EndNamespace);
            context.AttachEvent(SchemaExportFinished, ExportFileToDirectory);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var file          = File[context];
            var namingPattern = NamingPattern[context];

            file.BeginNamespace(namingPattern.EntityNamespace);
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
            var file          = File[context];
            var namingPattern = NamingPattern[context];
            var processInfo   = SchemaGenerationProcess[context];

            processInfo.Text = "Exporting Entity classes.";

            var filePath = namingPattern.EntityProjectDirectory + "All.cs";

            FileSystem.WriteAllText(context, filePath, file.ToString());
        }

        static void InitializeOutput(IDataContext context)
        {
            File[context] = new PaddedStringBuilder();
        }

        static void WriteClass(IDataContext context)
        {
            var file               = File[context];
            var config             = Config[context];
            var tableInfo          = TableInfo[context];
            var tableNamingPattern = TableNamingPattern[context];

            ContractCommentInfoCreator.Write(file, tableInfo);

            var inheritancePart = string.Empty;

            if (config.EntityContractBase != null)
            {
                inheritancePart = ": " + config.EntityContractBase;
            }

            file.AppendLine("[Serializable]");
            file.AppendLine($"public sealed class {tableNamingPattern.EntityClassName} {inheritancePart}");
            file.OpenBracket();

            ContractCommentInfoCreator.Write(file, tableInfo);
            file.AppendLine("// ReSharper disable once EmptyConstructor");
            file.AppendLine($"public {tableInfo.TableName.ToContractName()}Contract()");
            file.OpenBracket();
            file.CloseBracket();
            file.AppendLine();

            file.AppendAll(ContractBodyDbMembersCreator.Create(tableInfo).PropertyDefinitions);
            file.AppendLine();

            file.CloseBracket(); // end of class
        }

        static void WriteUsingList(IDataContext context)
        {
            var file          = File[context];
            var namingPattern = NamingPattern[context];

            foreach (var line in namingPattern.EntityUsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}