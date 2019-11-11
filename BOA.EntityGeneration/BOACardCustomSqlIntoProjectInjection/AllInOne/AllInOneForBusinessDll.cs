using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne
{
    /// <summary>
    ///     All in one for business DLL
    /// </summary>
    public class AllInOneForBusinessDll
    {
        #region Public Methods
        /// <summary>
        ///     Gets the code.
        /// </summary>
        public string GetCode(IProjectCustomSqlInfo data)
        {
            var sb = new PaddedStringBuilder();

            Write(sb, data);

            return sb.ToString();
        }

        /// <summary>
        ///     Writes the specified sb.
        /// </summary>
        public void Write(PaddedStringBuilder sb, IProjectCustomSqlInfo data)
        {
            sb.AppendLine("using BOA.Base;");
            sb.AppendLine("using BOA.Base.Data;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine($"using {data.NamespaceNameOfType};");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");

            sb.AppendLine();
            sb.AppendLine($"namespace {data.NamespaceNameOfBusiness}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            CustomSqlClassWriter.Write(sb, data);

            foreach (var item in data.CustomSqlInfoList)
            {
                sb.AppendLine();
                BusinessClassWriter.Write(sb, item, data);
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}