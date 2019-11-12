using ___Company___.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    class SharedDalClassWriter
    {
        public static void Write(IDataContext context)
        {
            var sb = context.Get<PaddedStringBuilder>(Context.SharedRepositoryClassOutput);
            var businessClassWriterContext = context.Get<BusinessClassWriterContext>(Context.BusinessClassWriterContext);

            Write(sb,businessClassWriterContext);
        }

        #region Public Methods
        public static void Write(PaddedStringBuilder sb, BusinessClassWriterContext data)
        {
            var namespaceName = string.Format(data.SharedClassConfig.SharedClassNamespaceFormat, data.TableInfo.SchemaName);

            sb.BeginNamespace(namespaceName);

            sb.AppendLine($"public sealed class {data.ClassName}");
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

           sb.EndNamespace();
        }
        #endregion
    }
}