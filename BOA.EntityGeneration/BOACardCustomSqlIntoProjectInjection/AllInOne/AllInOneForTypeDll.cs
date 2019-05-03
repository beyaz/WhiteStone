using BOA.Common.Helpers;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    public class AllInOneForTypeDll
    {
        [Inject]
        public TypeClassWriter TypeClassWriter { get; set; }

        public string GetCode( ProjectCustomSqlInfo data)
        {
            var sb = new PaddedStringBuilder();

            Write(sb,data);


            return sb.ToString();

        }

        public void Write(PaddedStringBuilder sb, ProjectCustomSqlInfo data)
        {
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");

            sb.AppendLine();
            sb.AppendLine($"namespace {data.NamespaceNameOfType}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            TypeClassWriter.Write_ICustomSqlProxy(sb);

            foreach (var item in data.CustomSqlInfoList)
            {
                sb.AppendLine();
                TypeClassWriter.Write(sb,item);
            }


            sb.PaddingCount--;
            sb.AppendLine("}");
            
        }
    }
}