using System.IO;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses;
using BOA.EntityGeneration.DataFlow;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    public class SharedDalClassWriter
    {
        public static void ExportFile(IDataContext context)
        {
            var allInOneSourceCode    = context.Get(SharedRepositoryFile).ToString();
            var namingPattern = context.Get(NamingPattern.Id);

            

            FileSystem.WriteAllText(context, namingPattern.RepositoryProjectDirectory+"Shared.cs", allInOneSourceCode);
        }

        static void WriteEmbeddedClasses(IDataContext context)
        {
            var sb = context.Get(SharedRepositoryFile);

            var path = Path.GetDirectoryName(typeof(SharedDalClassWriter).Assembly.Location) + Path.DirectorySeparatorChar + "SharedRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }

        public static void Write(IDataContext context)
        {
            var sb = context.Get(SharedRepositoryFile);
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
            var sb = context.Get(SharedRepositoryFile);

            foreach (var line in context.Get(NamingPattern.Id).SharedRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }

            var config = context.Get(Data.Config);


           sb.AppendLine();

           sb.BeginNamespace(context.Get(NamingPattern.Id).RepositoryNamespace+".Shared");
           

           SharedDalClassWriter.WriteEmbeddedClasses(context);
        }

        public static void EndNamespace(IDataContext context)
        {
            context.Get(SharedRepositoryFile).EndNamespace();
        }
        #endregion
    }
}