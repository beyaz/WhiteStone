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
        public GeneratorOfTypeClass GeneratorOfTypeClass { get; set; }

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
                sources.Add(GeneratorOfTypeClass.TransformText(generatorData));
            }

            TypeDllCompiler.Compile(schemaName,sources.ToArray());
        }
        #endregion
    }
}