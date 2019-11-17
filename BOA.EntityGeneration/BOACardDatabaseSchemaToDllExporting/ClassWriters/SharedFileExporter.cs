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
        internal static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(File));

        public static void ExportFile(IDataContext context)
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

        public static void EndNamespace(IDataContext context)
        {
            context.Get(File).EndNamespace();
        }
        #endregion
    }
}