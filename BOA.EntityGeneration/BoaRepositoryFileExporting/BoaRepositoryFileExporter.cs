using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;
using static BOA.EntityGeneration.DataFlow.TableExportingEvent;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting
{
    class BoaRepositoryFileExporter
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

            file.BeginNamespace(namingPattern.RepositoryNamespace);
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

            processInfo.Text = "Exporting Boa repository...";

            FileSystem.WriteAllText(context, namingPattern.RepositoryProjectDirectory + "Boa.cs", sourceCode);
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

            ContractCommentInfoCreator.Write(file, tableInfo);
            file.AppendLine($"public sealed class {tableNamingPattern.BoaRepositoryClassName} : ObjectHelper");
            file.AppendLine("{");
            file.PaddingCount++;

            ContractCommentInfoCreator.Write(file, tableInfo);
            file.AppendLine($"public {tableNamingPattern.BoaRepositoryClassName}(ExecutionDataContext context) : base(context) {{ }}");

            #region Delete
            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                DeleteByKeyMethodWriter.Write(context);
            }
            #endregion

            InsertMethodWriter.Write(context);

            #region Update
            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                UpdateByKeyMethodWriter.Write(context);
            }
            #endregion

            #region SelectByKey
            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                SelectByKeyMethodWriter.Write(context);
            }
            #endregion

            SelectByIndexMethodWriter.Write(context);

            SelectAllMethodWriter.Write(context);

            if (tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                SelectAllByValidFlagMethodWriter.Write(context);
            }

            file.CloseBracket();
        }

        static void WriteEmbeddedClasses(Context context)
        {
            var file = File[context];

            var path = Path.GetDirectoryName(typeof(BoaRepositoryFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BoaRepositoryFileEmbeddedCodes.txt";

            file.AppendAll(System.IO.File.ReadAllText(path));
            file.AppendLine();
        }

        static void WriteUsingList(Context context)
        {
            var file = File[context];

            var namingPattern = NamingPattern[context];

            foreach (var line in namingPattern.BoaRepositoryUsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}