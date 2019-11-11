using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters
{
    static class CustomSqlClassWriter
    {
        #region Public Methods
        /// <summary>
        ///     Writes the custom SQL class.
        /// </summary>
        public static void Write(PaddedStringBuilder sb, IProjectCustomSqlInfo project)
        {
            sb.AppendLine("public static class CustomSql");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("public static TOutput Execute<TOutput, T>(ObjectHelper objectHelper, ICustomSqlProxy<TOutput, T> input) where TOutput : GenericResponse<T>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("switch (input.Index)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in project.CustomSqlInfoList)
            {
                sb.AppendLine($"case {item.SwitchCaseIndex}:");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"return (TOutput) (object) new {item.BusinessClassName}(objectHelper.Context).Execute(({item.ParameterContractName})(object) input);");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("throw new System.InvalidOperationException(input.GetType().FullName);");

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}