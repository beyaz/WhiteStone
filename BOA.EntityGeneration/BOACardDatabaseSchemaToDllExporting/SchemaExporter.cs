using System.Collections.Generic;
using System.Linq;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class SchemaExporter
    {
        #region Public Properties
        [Inject]
        public BusinessDllCompiler BusinessDllCompiler { get; set; }

        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        [Inject]
        public GeneratorOfBusinessClass GeneratorOfBusinessClass { get; set; }

        [Inject]
        public GeneratorOfTypeClass GeneratorOfTypeClass { get; set; }

        [Inject]
        public TypeDllCompiler TypeDllCompiler { get; set; }

        [Inject]
        public BusinessProjectExporter BusinessProjectExporter { get; set; }
        
        #endregion

        #region Public Methods
        public void Export(string schemaName)
        {
            ExportTypeDll(schemaName);
            ExportBusinessDll(schemaName);
        }
        #endregion

        #region Methods
        void ExportBusinessDll(string schemaName)
        {

            var projectExporterData = new BusinessProjectExporterData();


            var items = DataPreparer.Prepare(schemaName);

            foreach (var tableInfo in items)
            {
                var sourceCode = GeneratorOfBusinessClass.TransformText(tableInfo);

                projectExporterData.Add(tableInfo.TableName.ToContractName(),sourceCode);
                
            }

            BusinessDllCompiler.Compile(schemaName, (from fileData in projectExporterData.Files select fileData.SourceCode).ToArray());
        }

        void ExportTypeDll(string schemaName)
        {
            var sources = new List<string>();

            var items = DataPreparer.Prepare(schemaName);

            foreach (var generatorData in items)
            {
                sources.Add(GeneratorOfTypeClass.TransformText(generatorData));
            }

            TypeDllCompiler.Compile(schemaName, sources.ToArray());
        }
        #endregion
    }
}