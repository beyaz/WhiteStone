using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting
{
    class SharedFileExporter
    {
        #region Static Fields
        internal static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, InitializeOutput);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, WriteUsingList);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, EmptyLine);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, BeginNamespace);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, WriteEmbeddedClasses);

            context.AttachEvent(TableExportingEvent.TableExportStarted, WriteClass);

            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, EndNamespace);
            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, ExportFileToDirectory);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb = context.Get(File);

            sb.BeginNamespace(context.Get(NamingPatternContract.NamingPattern).RepositoryNamespace + ".Shared");
        }

        static void EmptyLine(IDataContext context)
        {
            context.Get(File).AppendLine();
        }

        static void EndNamespace(IDataContext context)
        {
            context.Get(File).EndNamespace();
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var allInOneSourceCode = context.Get(File).ToString();
            var namingPattern      = context.Get(NamingPatternContract.NamingPattern);

            var processInfo = context.Get(SchemaGenerationProcess);

            processInfo.Text = "Exporting Shared repository...";

            FileSystem.WriteAllText(context, namingPattern.RepositoryProjectDirectory + "Shared.cs", allInOneSourceCode);
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void WriteClass(IDataContext context)
        {
            var sb                 = context.Get(File);
            var tableInfo          = TableInfo[context];
            var tableNamingPattern = context.Get(TableNamingPatternContract.TableNamingPattern);

            sb.AppendLine($"public sealed class {tableNamingPattern.SharedRepositoryClassName}");
            sb.OpenBracket();

            if (tableInfo.IsSupportSelectByKey)
            {
                sb.AppendLine();
                DeleteByKeyMethodWriter.Write(context);

                sb.AppendLine();
                SelectByKeyMethodWriter.Write(context);

                UpdateByPrimaryKeyMethodWriter.Write(context);
            }

            SelectByIndexMethodWriter.Write(context);

            SelectAllMethodWriter.Write(context);

            if (tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                SelectAllByValidFlagMethodWriter.Write(context);
            }

            InsertMethodWriter.Write(context);

            ReadContractMethodWriter.Write(context);

            sb.CloseBracket();
        }

        static void WriteEmbeddedClasses(IDataContext context)
        {
            var sb = context.Get(File);

            var path = Path.GetDirectoryName(typeof(SharedFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "SharedRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }

        static void WriteUsingList(IDataContext context)
        {
            var sb = context.Get(File);

            foreach (var line in context.Get(NamingPatternContract.NamingPattern).SharedRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }
        }
        #endregion
    }
}