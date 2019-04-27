using System.Collections.Generic;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class SchemaExporter
    {
        #region Public Properties
        [Inject]
        public TypeDllCompiler TypeDllCompiler { get; set; }

        [Inject]
        public TypeContractCodeGenerator TypeContractCodeGenerator { get; set; }

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
                sources.Add(TypeContractCodeGenerator.TransformText(generatorData));
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