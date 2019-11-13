using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    class SharedDalClassWriter
    {
        public static void Write(IDataContext context)
        {
            var sb = context.Get<PaddedStringBuilder>(Data.SharedRepositoryClassOutput);
            var businessClassWriterContext = context.Get<BusinessClassWriterContext>(Data.BusinessClassWriterContext);

            Write(sb,businessClassWriterContext);
        }

        #region Public Methods
        public static void Write(PaddedStringBuilder sb, BusinessClassWriterContext data)
        {
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

            sb.CloseBracket();
        }

        public static void WriteUsingList(IDataContext context)
        {
            var sb = context.Get(Data.SharedRepositoryClassOutput);
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
            context.Get(Data.SharedRepositoryClassOutput).EndNamespace();
        }
        #endregion
    }
}