using System.Collections.Generic;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class SchemaExporter
    {
        #region Public Properties
        [Inject]
        public AllBusinessClassesInOne AllBusinessClassesInOne { get; set; }

        [Inject]
        public BusinessDllCompiler BusinessDllCompiler { get; set; }

        [Inject]
        public BusinessProjectExporter BusinessProjectExporter { get; set; }

        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        [Inject]
        public GeneratorOfBusinessClass GeneratorOfBusinessClass { get; set; }

        [Inject]
        public GeneratorOfTypeClass GeneratorOfTypeClass { get; set; }

        [Inject]
        public TypeDllCompiler TypeDllCompiler { get; set; }
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
            var allCodes = AllBusinessClassesInOne.GetCode(schemaName);

            BusinessProjectExporter.Export(schemaName, allCodes);

            BusinessDllCompiler.Compile(schemaName, allCodes);
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