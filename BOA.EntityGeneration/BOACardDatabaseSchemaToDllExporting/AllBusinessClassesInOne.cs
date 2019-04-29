using System;
using System.Linq;
using BOA.Common.Helpers;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
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
            var isFirst = true;

            var items = DataPreparer.Prepare(schemaName);

            if (items.Count == 0)
            {
                throw new NotImplementedException(schemaName);
            }

            foreach (var tableInfo in items)
            {
                if (isFirst)
                {
                    GeneratorOfBusinessClass.WriteUsingList(sb, tableInfo);
                    sb.AppendLine();
                    sb.AppendLine($"namespace {NamingHelper.GetBusinessClassNamespace(schemaName)}");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    isFirst = false;
                }
                Tracer.Trace($"Generating Business class for {tableInfo.TableName}");
                sb.AppendLine();
                GeneratorOfBusinessClass.WriteClass(sb, tableInfo);
            }

            sb.PaddingCount--;
            sb.AppendLine("}"); // end of namespace    
            
        }

        [Inject]
        public Tracer Tracer { get; set; }
        #endregion
    }
}