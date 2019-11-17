using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting
{
    class BoaRepositoryFileExporter
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
            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, ClearOutput);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb            = context.Get(File);
            var namingPattern = context.Get(NamingPattern);

            sb.BeginNamespace(namingPattern.RepositoryNamespace);
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
            var allInOneSourceCode = context.Get(File).ToString();
            var namingPattern      = context.Get(NamingPattern);

            FileSystem.WriteAllText(context, namingPattern.RepositoryProjectDirectory + "Boa.cs", allInOneSourceCode);
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void WriteClass(IDataContext context)
        {
            var sb        = context.Get(File);
            var tableInfo = context.Get(TableInfo);
            var tableNamingPattern = context.Get(TableNamingPattern);
            var namingPattern = context.Get( NamingPattern);

            

            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public sealed class {tableNamingPattern.BoaRepositoryClassName} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"const string CallerMemberPrefix = \"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.\";");

            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public {tableNamingPattern.BoaRepositoryClassName}(ExecutionDataContext context) : base(context) {{ }}");

            #region Delete
            if (tableInfo.IsSupportSelectByKey)
            {
                sb.AppendLine();
                DeleteByKeyMethodWriter.Write(context);
            }
            #endregion

            InsertMethodWriter.Write(context);

            #region Update
            if (tableInfo.IsSupportSelectByKey)
            {
                sb.AppendLine();
                UpdateByKeyMethodWriter.Write(context);
            }
            #endregion

            #region SelectByKey
            if (tableInfo.IsSupportSelectByKey)
            {
                sb.AppendLine();
                SelectByKeyMethodWriter.Write(context);
            }
            #endregion

            SelectByUniqueIndexMethodWriter.Write(context);

            var selectAllInfo = SelectAllInfoCreator.Create(tableInfo);

            SelectAllMethodWriter.Write(context);

            if (tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                SelectAllByValidFlagMethodWriter.Write(context);
            }

            sb.CloseBracket();
        }

        static void WriteEmbeddedClasses(IDataContext context)
        {
            var sb = context.Get(File);

            var path = Path.GetDirectoryName(typeof(BoaRepositoryFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BoaRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }

        static void WriteUsingList(IDataContext context)
        {
            var sb = context.Get(File);

            var namingPattern = context.Get(NamingPattern);

            foreach (var line in namingPattern.BoaRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }
        }
        #endregion
    }
}