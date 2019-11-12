
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;
using static ___Company___.EntityGeneration.DataFlow.DataContext;
using ___Company___.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne
{
    public class AllInOneForTypeDll
    {
        #region Public Properties
        

        [Inject]
        public TypeClassWriter TypeClassWriter { get; set; }
        #endregion

        #region Public Methods
        public string GetCode(IProjectCustomSqlInfo data)
        {
            var sb = new PaddedStringBuilder();

            Write(sb, data);

            return sb.ToString();
        }

        public void Write(PaddedStringBuilder sb, IProjectCustomSqlInfo data)
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