using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    static class EntityFileExporter
    {
        #region Static Fields
        static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(DataEvent.StartToExportSchema, InitializeOutput);
            context.AttachEvent(DataEvent.StartToExportSchema, WriteUsingList);
            context.AttachEvent(DataEvent.StartToExportSchema, EmptyLine);
            context.AttachEvent(DataEvent.StartToExportSchema, BeginNamespace);
            context.AttachEvent(DataEvent.StartToExportSchema, EndNamespace);

            context.AttachEvent(DataEvent.StartToExportTable, WriteClass);

            context.AttachEvent(DataEvent.FinishingExportingSchema, ExportFileToDirectory);
            context.AttachEvent(DataEvent.FinishingExportingSchema, ClearOutput);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb            = context.Get(File);
            var namingPattern = context.Get(Data.NamingPattern);

            sb.BeginNamespace(namingPattern.EntityNamespace);
        }

        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }

        static void EmptyLine(IDataContext context)
        {
            context.Get(File).AppendLine();
        }

        static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(File);
            sb.EndNamespace();
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var sb            = context.Get(File);
            var namingPattern = context.Get(Data.NamingPattern);
            var processInfo   = context.Get(SchemaGenerationProcess);

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
            var sb        = context.Get(File);
            var config    = context.Get(Config);
            var tableInfo = context.Get(TableInfo);
            var tableNamingPattern = context.Get(Data.TableNamingPattern);

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
            var sb            = context.Get(File);
            var namingPattern = context.Get(Data.NamingPattern);

            foreach (var line in namingPattern.EntityUsingLines)
            {
                sb.AppendLine(line);
            }
        }
        #endregion
    }
}