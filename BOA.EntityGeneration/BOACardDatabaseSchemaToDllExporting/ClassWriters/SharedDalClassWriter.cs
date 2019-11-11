using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.Shared;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    class SharedDalClassWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, BusinessClassWriterContext data)
        {
            sb.AppendLine($"public sealed class {data.ClassName}");
            sb.AppendLine("{");
            sb.PaddingCount++;

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

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}