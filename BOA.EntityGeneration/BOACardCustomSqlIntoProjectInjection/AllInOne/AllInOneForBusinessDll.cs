using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Model;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne
{
    public class AllInOneForBusinessDll
    {
        #region Public Properties
        [Inject]
        public BusinessClassWriter BusinessClassWriter { get; set; }

        [Inject]
        public Tracer Tracer { get; set; }
        #endregion

        #region Public Methods
        public string GetCode(ProjectCustomSqlInfo data)
        {
            var sb = new PaddedStringBuilder();

            Write(sb, data);

            return sb.ToString();
        }

        public void Write(PaddedStringBuilder sb, ProjectCustomSqlInfo data)
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

            BusinessClassWriter.Write_CustomSqlClass(sb, data);

            foreach (var item in data.CustomSqlInfoList)
            {
                Tracer.Trace($"Writing parameter class {item.ParameterContractName}");
                sb.AppendLine();
                BusinessClassWriter.Write(sb, item);
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}