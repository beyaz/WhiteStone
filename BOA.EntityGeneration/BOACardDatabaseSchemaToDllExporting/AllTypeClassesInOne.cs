using System;
using System.Linq;
using BOA.Common.Helpers;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class Tracer
    {
        public void Trace(string message)
        {
            Console.WriteLine(message);
        }
    }
    public class AllTypeClassesInOne
    {
        [Inject]
        public Tracer Tracer { get; set; }
        #region Public Properties
        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        [Inject]
        public GeneratorOfTypeClass GeneratorOfTypeClass { get; set; }

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

            foreach (var tableInfo in items)
            {
                if (isFirst)
                {
                    GeneratorOfTypeClass.WriteUsingList(sb, tableInfo);
                    sb.AppendLine();
                    sb.AppendLine($"namespace {NamingHelper.GetTypeClassNamespace(schemaName)}");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    isFirst = false;
                }

                Tracer.Trace($"Generating type class for {tableInfo.TableName}");
                sb.AppendLine();
                GeneratorOfTypeClass.WriteClass(sb, tableInfo);
            }

            if (items.Any())
            {
                sb.PaddingCount--;
                sb.AppendLine("}"); // end of namespace    
            }
            
        }
        #endregion
    }
}