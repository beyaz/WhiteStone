using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.DataFlow;
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
            context.AttachEvent(DataEvent.StartToExportSchema, InitializeOutput);
            context.AttachEvent(DataEvent.StartToExportSchema, WriteUsingList);
            context.AttachEvent(DataEvent.StartToExportSchema, EmptyLine);
            context.AttachEvent(DataEvent.StartToExportSchema, BeginNamespace);
            context.AttachEvent(DataEvent.StartToExportSchema, WriteEmbeddedClasses);
            context.AttachEvent(DataEvent.StartToExportSchema, EndNamespace);

            context.AttachEvent(DataEvent.StartToExportTable, WriteClass);

            context.AttachEvent(DataEvent.FinishingExportingSchema, ExportFileToDirectory);
            context.AttachEvent(DataEvent.FinishingExportingSchema, ClearOutput);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb = context.Get(File);

            sb.BeginNamespace(context.Get(Data.NamingPattern).RepositoryNamespace + ".Shared");
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
            context.Get(File).EndNamespace();
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var allInOneSourceCode = context.Get(File).ToString();
            var namingPattern      = context.Get(Data.NamingPattern);

            FileSystem.WriteAllText(context, namingPattern.RepositoryProjectDirectory + "Shared.cs", allInOneSourceCode);
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void WriteClass(IDataContext context)
        {
            var sb        = context.Get(File);
            var tableInfo = context.Get(TableInfo);
            var tableNamingPattern = context.Get(Data.TableNamingPattern);

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

            if (tableInfo.IsSupportSelectByUniqueIndex)
            {
                sb.AppendLine();
                SelectByIndexMethodWriter.Write(context);
            }

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

            foreach (var line in context.Get(Data.NamingPattern).SharedRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }
        }
        #endregion
    }
}