using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.MethodWriters
{
    static class ExecuteForListMethodWriter
    {
        #region Public Methods
        public static void Write(PaddedStringBuilder sb, ICustomSqlInfo data)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{data.ResultContractName}>> Execute({data.ParameterContractName} request)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("var sqlInfo = CreateSqlInfo(request);");
            sb.AppendLine();
            sb.AppendLine($"return this.ExecuteCustomSql<{data.ResultContractName}>(CallerMemberPath, sqlInfo, ReadContract, ProfileId, ObjectId);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}