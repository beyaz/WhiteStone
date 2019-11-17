using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.DataFlow;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    class SharedFileExporter
    {
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(DataEvent.StartToExportSchema, InitializeOutput);
            context.AttachEvent(DataEvent.StartToExportSchema, WriteUsingList);
            context.AttachEvent(DataEvent.StartToExportSchema, EmptyLine);
            context.AttachEvent(DataEvent.StartToExportSchema, BeginNamespace);
            context.AttachEvent(DataEvent.StartToExportSchema, EndNamespace);

            context.AttachEvent(DataEvent.StartToExportTable, Write);

            context.AttachEvent(DataEvent.FinishingExportingSchema, ExportFileToDirectory);
            context.AttachEvent(DataEvent.FinishingExportingSchema, ClearOutput);
        }

        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }
        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }


        internal static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(File));


        static void EmptyLine(IDataContext context)
        {
            context.Get(File).AppendLine();
        }

        public static void ExportFileToDirectory(IDataContext context)
        {
            var allInOneSourceCode    = context.Get(File).ToString();
            var namingPattern = context.Get(NamingPattern.Id);

            

            FileSystem.WriteAllText(context, namingPattern.RepositoryProjectDirectory+"Shared.cs", allInOneSourceCode);
        }

        static void WriteEmbeddedClasses(IDataContext context)
        {
            var sb = context.Get(File);

            var path = Path.GetDirectoryName(typeof(SharedFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "SharedRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }

        public static void Write(IDataContext context)
        {
            var sb = context.Get(File);
            var tableInfo = context.Get(TableInfo);

            sb.AppendLine($"sealed class {context.Get(SharedRepositoryClassName)}");
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

        #region Public Methods
        

        public static void WriteUsingList(IDataContext context)
        {
            var sb = context.Get(File);

            foreach (var line in context.Get(NamingPattern.Id).SharedRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }

            var config = context.Get(Data.Config);


           sb.AppendLine();

           sb.BeginNamespace(context.Get(NamingPattern.Id).RepositoryNamespace+".Shared");
           

           SharedFileExporter.WriteEmbeddedClasses(context);
        }

        static void BeginNamespace(IDataContext context)
        {
            var sb = context.Get(File);
            
            sb.BeginNamespace(context.Get(NamingPattern.Id).RepositoryNamespace+".Shared");
           

            SharedFileExporter.WriteEmbeddedClasses(context);
        }

      


        public static void EndNamespace(IDataContext context)
        {
            context.Get(File).EndNamespace();
        }
        #endregion
    }
}