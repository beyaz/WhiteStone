using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    public class SharedDalClassWriter
    {
        public static void ExportFile(IDataContext context)
        {
            var schemaName            = context.Get(SchemaName);
            var allInOneSourceCode    = context.Get(SharedRepositoryFile).ToString();
            var config                = context.Get(Data.Config);
            var fileAccess            = context.Get(FileAccess);
            var allInOneFilePath      = config.SharedRepositoryAllInOneFilePath.Replace("{SchemaName}", schemaName);

            fileAccess.WriteAllText(allInOneFilePath, allInOneSourceCode);
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
            var schemaName = context.Get(SchemaName);
            var config = context.Get(Data.Config);

           sb.UsingNamespace("System");
           sb.UsingNamespace("System.Collections.Generic");
           sb.UsingNamespace("System.Data");
           sb.UsingNamespace("System.Data.SqlClient");

           sb.AppendLine();

           sb.BeginNamespace(NamingHelper.GetSharedRepositoryClassNamespace(schemaName,config));
           SqlInfoClassWriter.Write(sb,config);
        }

        public static void EndNamespace(IDataContext context)
        {
            context.Get(SharedRepositoryFile).EndNamespace();
        }
        #endregion
    }
}