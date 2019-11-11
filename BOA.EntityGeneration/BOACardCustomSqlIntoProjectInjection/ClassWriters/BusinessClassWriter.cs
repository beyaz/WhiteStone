using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.MethodWriters;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters
{
    static class BusinessClassWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, ICustomSqlInfo data, IProjectCustomSqlInfo projectCustomSqlInfo)
        {
            var key = $"{projectCustomSqlInfo.NamespaceNameOfBusiness}.{data.BusinessClassName}.Execute";

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public sealed class {data.BusinessClassName} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"const string CallerMemberPath = \"{key}\";");
            sb.AppendLine($"const string ProfileId        = \"{projectCustomSqlInfo.ProfileId}\";");
            sb.AppendLine($"const string ObjectId         = \"{data.Name}\";");
            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public {data.BusinessClassName}(ExecutionDataContext context) : base(context) {{}}");
            sb.AppendLine();

            if (data.SqlResultIsCollection)
            {
                ExecuteForListMethodWriter.Write(sb, data);
            }
            else
            {
                ExecuteForSingleContractMethodWriter.Write(sb, data);
            }

            sb.AppendLine();
            ReadContractMethodWriter.Write(sb, data);
            sb.AppendLine();
            CreateSqlInfoMethodWriter.Write(sb, data);

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}