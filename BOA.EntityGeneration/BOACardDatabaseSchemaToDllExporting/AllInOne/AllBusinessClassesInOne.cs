using System;
using System.IO;
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

        [Inject]
        public Tracer Tracer { get; set; }
        [Inject]
        public Config Config { get; set; }
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
        static void UtilClass(PaddedStringBuilder sb, Config config)
        {

            var path = Path.GetDirectoryName(typeof(AllBusinessClassesInOne).Assembly.Location) + Path.DirectorySeparatorChar + "EmbeddedUtilizationClassForDao.txt";

            var code = FileHelper.ReadFile(path);

            code = code.Replace("{DatabaseEnumName}", config.DatabaseEnumName);
            sb.AppendAll(code.Trim());
            sb.AppendLine();
        }

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

            UtilClass(sb,Config);

            Tracer.SchemaGenerationProcess.Total   = items.Count;
            Tracer.SchemaGenerationProcess.Current = 0;

            foreach (var tableInfo in items)
            {
                Tracer.SchemaGenerationProcess.Text = $"Generating business class: {tableInfo.TableName}";

                Tracer.SchemaGenerationProcess.Current++;

                sb.AppendLine();
                GeneratorOfBusinessClass.WriteClass(sb, tableInfo);

                
            }

            sb.PaddingCount--;
            sb.AppendLine("}"); // end of namespace    
        }
        #endregion
    }
}