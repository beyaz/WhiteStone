using System.Collections.Generic;
using BOA.EntityGeneration.Generators;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    public class SchemaExporter
    {
        #region Public Properties
        [Inject]
        public Compiler Compiler { get; set; }

        [Inject]
        public Contract Contract { get; set; }

        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        [Inject]
        public ContractDataCreator ContractDataCreator { get; set; }
        #endregion

        #region Public Methods
        public void Export(string schemaName)
        {
            var sources = new List<string>();

            var items = DataPreparer.Prepare(schemaName);

            foreach (var generatorData in items)
            {
                var contractData = ContractDataCreator.Create(generatorData);

                sources.Add(Contract.TransformText(contractData));
            }

            Compiler.Compile(new CompilerData
            {
                OutputAssemblyName = schemaName,
                Sources            = sources.ToArray(),
                ReferencedAssemblies = new List<string>
                {
                    @"d:\boa\server\bin\BOA.Types.Kernel.Card.dll",
                    @"d:\boa\server\bin\BOA.Common.dll"
                }
            });
        }
        #endregion
    }
}