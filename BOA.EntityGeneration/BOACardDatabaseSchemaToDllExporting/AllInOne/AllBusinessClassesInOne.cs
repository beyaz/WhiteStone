using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne
{
    public class AllBusinessClassesInOne
    {
        #region Public Properties
        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        [Inject]
        public GeneratorOfBusinessClass GeneratorOfBusinessClass { get; set; }

        [Inject]
        public NamingHelper NamingHelper { get; set; }
        #endregion

        #region Public Methods
        public string GetCode(string schemaName)
        {
            var sb = new PaddedStringBuilder();

            Write(sb, schemaName);

            return sb.ToString();
        }
        #endregion

        #region Methods
        void Write(PaddedStringBuilder sb, string schemaName)
        {

            var items = DataPreparer.Prepare(schemaName);

            if (items.Count == 0)
            {
                throw new NotImplementedException(schemaName);
            }

            GeneratorOfBusinessClass.WriteUsingList(sb, items.First());
            sb.AppendLine();
            sb.AppendLine($"namespace {NamingHelper.GetBusinessClassNamespace(schemaName)}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            ReadNullableFlag(sb);

            foreach (var tableInfo in items)
            {
                Tracer.Trace($"Generating Business class for {tableInfo.TableName}");
                sb.AppendLine();
                GeneratorOfBusinessClass.WriteClass(sb, tableInfo);
            }

            sb.PaddingCount--;
            sb.AppendLine("}"); // end of namespace    
            
        }

        static void ReadNullableFlag(PaddedStringBuilder sb)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine("///     Utility methods");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("static class Util");
            sb.AppendLine("{");
            sb.AppendLine("     /// <summary>");
            sb.AppendLine("	    ///     Read nullable flag");
            sb.AppendLine("	    /// </summary>");
            sb.AppendLine("	    public static bool? ReadNullableFlag(string flag)");
            sb.AppendLine("	    {");
            sb.AppendLine("		    if (flag == null)");
            sb.AppendLine("		    {");
            sb.AppendLine("			    return null;");
            sb.AppendLine("		    }");
            sb.AppendLine();
            sb.AppendLine("		    return flag == \"1\";");
            sb.AppendLine("	    }");
            sb.AppendLine("}");
        }

        [Inject]
        public Tracer Tracer { get; set; }
        #endregion
    }
}