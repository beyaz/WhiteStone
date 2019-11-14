﻿using BOA.DataFlow;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    public class SharedDalClassWriter
    {
        public static void ExportFile(IDataContext context)
        {
            var schemaName            = context.Get(Data.SchemaName);
            var allInOneSourceCode    = context.Get(Data.SharedRepositoryFile).ToString();
            var config                = context.Get(Data.Config);
            var fileAccess            = context.Get(Data.FileAccess);
            var allInOneFilePath      = config.SharedRepositoryAllInOneFilePath.Replace("{SchemaName}", schemaName);

            fileAccess.WriteAllText(allInOneFilePath, allInOneSourceCode);
        }
        public static void Write(IDataContext context)
        {
            var sb = context.Get<PaddedStringBuilder>(Data.SharedRepositoryFile);
            var businessClassWriterContext = context.Get<BusinessClassWriterContext>(Data.BusinessClassWriterContext);

            Write(context,sb,businessClassWriterContext);
        }

        #region Public Methods
        public static void Write(IDataContext context,PaddedStringBuilder sb, BusinessClassWriterContext data)
        {
            var tableInfo = context.Get(Data.TableInfo);

            sb.AppendLine($"sealed class {data.ClassName}");
            sb.OpenBracket();

            if (data.CanWriteDeleteByKeyMethod)
            {
                sb.AppendLine();
                DeleteByKeyMethodWriter.Write(sb, data.DeleteByKeyInfo,data.SharedClassConfig);
            }

            if (data.CanWriteSelectByKeyMethod)
            {
                sb.AppendLine();
                SelectByKeyMethodWriter.Write(sb, data.SelectByPrimaryKeyInfo,data.SharedClassConfig);
            }

            if (tableInfo.IsSupportSelectByUniqueIndex)
            {
                sb.AppendLine();
                SelectByUniqueIndexMethodWriter.Write(context);
            }

            

            sb.CloseBracket();
        }

        public static void WriteUsingList(IDataContext context)
        {
            var sb = context.Get(Data.SharedRepositoryFile);
            var schemaName = context.Get(Data.SchemaName);
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
            context.Get(Data.SharedRepositoryFile).EndNamespace();
        }
        #endregion
    }
}