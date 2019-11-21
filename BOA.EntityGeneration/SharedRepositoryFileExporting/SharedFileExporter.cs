using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;
using static BOA.EntityGeneration.DataFlow.TableExportingEvent;
using static BOA.EntityGeneration.Naming.NamingPatternContract;

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
            context.AttachEvent(SchemaExportStarted, InitializeOutput);
            context.AttachEvent(SchemaExportStarted, WriteUsingList);
            context.AttachEvent(SchemaExportStarted, EmptyLine);
            context.AttachEvent(SchemaExportStarted, BeginNamespace);
            context.AttachEvent(SchemaExportStarted, WriteEmbeddedClasses);

            context.AttachEvent(TableExportStarted, WriteClass);

            context.AttachEvent(SchemaExportFinished, EndNamespace);
            context.AttachEvent(SchemaExportFinished, ExportFileToDirectory);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var file = File[context];

            file.BeginNamespace(NamingPattern[context].RepositoryNamespace + ".Shared");
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
            var sourceCode = File[context].ToString();
            var namingPattern      = NamingPattern[context];

            var processInfo = SchemaGenerationProcess[context];

            processInfo.Text = "Exporting Shared repository...";

            FileSystem.WriteAllText(context, namingPattern.RepositoryProjectDirectory + "Shared.cs", sourceCode);
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void WriteClass(IDataContext context)
        {
            var sb                 = File[context];
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
            var sb = File[context];

            var path = Path.GetDirectoryName(typeof(SharedFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "SharedRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }

        static void WriteUsingList(IDataContext context)
        {
            var sb = File[context];

            foreach (var line in NamingPattern[context].SharedRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }
        }
        #endregion
    }
}