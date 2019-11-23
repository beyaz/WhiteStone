using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;
using static BOA.EntityGeneration.DataFlow.TableExportingEvent;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting
{
    class SharedFileExporter
    {
        #region Static Fields
        internal static readonly Property<PaddedStringBuilder> File = Property.Create<PaddedStringBuilder>(nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(Context context)
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
        static void BeginNamespace(Context context)
        {
            var file          = File[context];
            var namingPattern = NamingPattern[context];

            file.BeginNamespace(namingPattern.RepositoryNamespace + ".Shared");
        }

        static void EmptyLine(Context context)
        {
            File[context].AppendLine();
        }

        static void EndNamespace(Context context)
        {
            File[context].EndNamespace();
        }

        static void ExportFileToDirectory(Context context)
        {
            var sourceCode    = File[context].ToString();
            var namingPattern = NamingPattern[context];
            var processInfo   = ProcessInfo[context];

            processInfo.Text = "Exporting Shared repository...";

            FileSystem.WriteAllText(context, namingPattern.RepositoryProjectDirectory + "Shared.cs", sourceCode);
        }

        static void InitializeOutput(Context context)
        {
            File[context] = new PaddedStringBuilder();
        }

        static void WriteClass(Context context)
        {
            var file               = File[context];
            var tableInfo          = TableInfo[context];
            var tableNamingPattern = TableNamingPattern[context];

            file.AppendLine($"public sealed class {tableNamingPattern.SharedRepositoryClassName}");
            file.OpenBracket();

            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                DeleteByKeyMethodWriter.Write(context);

                file.AppendLine();
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

            file.CloseBracket();
        }

        static void WriteEmbeddedClasses(Context context)
        {
            var file = File[context];

            var path = Path.GetDirectoryName(typeof(SharedFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "SharedRepositoryFileEmbeddedCodes.txt";

            file.AppendAll(System.IO.File.ReadAllText(path));
            file.AppendLine();
        }

        static void WriteUsingList(Context context)
        {
            var file          = File[context];
            var namingPattern = NamingPattern[context];

            foreach (var line in namingPattern.SharedRepositoryUsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}