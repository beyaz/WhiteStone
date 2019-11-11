using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters
{
    /// <summary>
    ///     The business class writer
    /// </summary>
    public class BusinessClassWriter
    {
        

        #region Public Methods
        /// <summary>
        ///     Writes the specified sb.
        /// </summary>
        public void Write(PaddedStringBuilder sb, ICustomSqlInfo data, IProjectCustomSqlInfo projectCustomSqlInfo)
        {
            var key = $"{projectCustomSqlInfo.NamespaceNameOfBusiness}.{data.BusinessClassName}.Execute";

            var template = new BusinessClassTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {nameof(data), data},
                    {nameof(key), key},
                    {nameof(projectCustomSqlInfo.ProfileId),projectCustomSqlInfo.ProfileId }
                }
            };
            template.Initialize();
            var output = template.TransformText();

            sb.AppendAll(output.Trim());
            sb.AppendLine();
        }

        /// <summary>
        ///     Writes the custom SQL class.
        /// </summary>
        public void Write_CustomSqlClass(PaddedStringBuilder sb, IProjectCustomSqlInfo project)
        {

            var template = new CustomSqlClassTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {nameof(project), project}
                }
            };
            template.Initialize();
            var output = template.TransformText();

            sb.AppendAll(output.Trim());
            sb.AppendLine();


            //sb.AppendLine("public static class CustomSql");
            //sb.AppendLine("{");
            //sb.PaddingCount++;

            //sb.AppendLine("public static TOutput Execute<TOutput, T>(ObjectHelper objectHelper, ICustomSqlProxy<TOutput, T> input) where TOutput : GenericResponse<T>");
            //sb.AppendLine("{");
            //sb.PaddingCount++;

            //sb.AppendLine("switch (input.Index)");
            //sb.AppendLine("{");
            //sb.PaddingCount++;

            //foreach (var item in project.CustomSqlInfoList)
            //{
            //    sb.AppendLine($"case {item.SwitchCaseIndex}:");
            //    sb.AppendLine("{");
            //    sb.PaddingCount++;

            //    sb.AppendLine($"return (TOutput) (object) new {item.BusinessClassName}(objectHelper.Context).Execute(({item.ParameterContractName})(object) input);");

            //    sb.PaddingCount--;
            //    sb.AppendLine("}");
            //}

            //sb.PaddingCount--;
            //sb.AppendLine("}");

            //sb.AppendLine();
            //sb.AppendLine("throw new System.InvalidOperationException(input.GetType().FullName);");

            //sb.PaddingCount--;
            //sb.AppendLine("}");

            //sb.PaddingCount--;
            //sb.AppendLine("}");
        }
        #endregion
    }
}