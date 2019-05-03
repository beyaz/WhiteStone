using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    public class SchemaExporter
    {
        #region Public Properties
        [Inject]
        public AllBusinessClassesInOne AllBusinessClassesInOne { get; set; }

        [Inject]
        public AllTypeClassesInOne AllTypeClassesInOne { get; set; }

        [Inject]
        public BatExporter BatExporter { get; set; }

        

        [Inject]
        public BusinessProjectExporter BusinessProjectExporter { get; set; }

        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        [Inject]
        public GeneratorOfBusinessClass GeneratorOfBusinessClass { get; set; }

        [Inject]
        public GeneratorOfTypeClass GeneratorOfTypeClass { get; set; }

        

        [Inject]
        public TypesProjectExporter TypesProjectExporter { get; set; }
        #endregion

        #region Public Methods
        public void Export(string schemaName)
        {
            ExportTypeDll(schemaName);
            ExportBusinessDll(schemaName);

            BatExporter.Export(schemaName);
        }
        #endregion

        #region Methods
        void ExportBusinessDll(string schemaName)
        {
            var code = AllBusinessClassesInOne.GetCode(schemaName);

            BusinessProjectExporter.Export(schemaName, code);

            // BusinessDllCompiler.Compile(schemaName, code);
        }

        void ExportTypeDll(string schemaName)
        {
            var code = AllTypeClassesInOne.GetCode(schemaName);

            TypesProjectExporter.Export(schemaName, code);

            //TypeDllCompiler.Compile(schemaName, code);
        }
        #endregion
    }
}