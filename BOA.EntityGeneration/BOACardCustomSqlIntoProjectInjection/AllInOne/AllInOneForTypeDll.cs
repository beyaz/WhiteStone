
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne
{
    public static class AllInOneForTypeDll
    {
       

        #region Public Methods
        public static string GetCode(IProjectCustomSqlInfo data)
        {
            var sb = new PaddedStringBuilder();

            Write(sb, data);

            return sb.ToString();
        }

        static void Write(PaddedStringBuilder sb, IProjectCustomSqlInfo data)
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
                TypeClassWriter.Write(sb, item);
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}