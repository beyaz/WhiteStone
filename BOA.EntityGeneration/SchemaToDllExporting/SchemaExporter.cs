using System.Collections.Generic;
using BOA.EntityGeneration.Generators;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    public class SchemaExporter
    {
        #region Public Properties
        [Inject]
        public TypeDllCompiler TypeDllCompiler { get; set; }

        [Inject]
        public Contract Contract { get; set; }

        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }
        
        #endregion

        #region Public Methods
        public void Export(string schemaName)
        {
            var sources = new List<string>();

            var items = DataPreparer.Prepare(schemaName);

            foreach (var generatorData in items)
            {
                sources.Add(Contract.TransformText(generatorData));
            }

            TypeDllCompiler.Compile(new TypeDllCompilerData
            {
                SchemaName = schemaName,
                Sources            = sources.ToArray()
            });
        }
        #endregion
    }
}