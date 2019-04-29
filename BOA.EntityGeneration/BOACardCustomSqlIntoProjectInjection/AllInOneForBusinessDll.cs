using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    public class AllInOneForBusinessDll
    {

        [Inject]
        public BusinessClassWriter BusinessClassWriter { get; set; }
        [Inject]
        public Tracer Tracer { get; set; }
        

        public string GetCode( ProjectCustomSqlInfo data)
        {
            var sb = new PaddedStringBuilder();

            Write(sb,data);


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

            foreach (var item in data.CustomSqlInfoList)
            {

                Tracer.Trace($"Fetching data for {item.BusinessClassName}");

                sb.AppendLine();
                BusinessClassWriter.Write(sb,item);
            }


            sb.PaddingCount--;
            sb.AppendLine("}");
        }
    }
}