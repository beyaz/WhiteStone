using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;
using static ___Company___.EntityGeneration.DataFlow.DataContext;




namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    /// <summary>
    ///     The schema exporter
    /// </summary>
    public class SchemaExporter
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets all business classes in one.
        /// </summary>
        [Inject]
        public AllBusinessClassesInOne AllBusinessClassesInOne { get; set; }

        /// <summary>
        ///     Gets or sets all type classes in one.
        /// </summary>
        [Inject]
        public AllTypeClassesInOne AllTypeClassesInOne { get; set; }

        /// <summary>
        ///     Gets or sets the business project exporter.
        /// </summary>
        [Inject]
        public BusinessProjectExporter BusinessProjectExporter { get; set; }

        /// <summary>
        ///     Gets or sets the configuration.
        /// </summary>
        [Inject]
        public Config Config { get; set; }

        /// <summary>
        ///     Gets or sets the data preparer.
        /// </summary>
        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        /// <summary>
        ///     Gets or sets the generator of business class.
        /// </summary>
        [Inject]
        public GeneratorOfBusinessClass GeneratorOfBusinessClass { get; set; }

      



        /// <summary>
        ///     Gets or sets the types project exporter.
        /// </summary>
        [Inject]
        public TypesProjectExporter TypesProjectExporter { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Exports the specified schema name.
        /// </summary>
        public void Export(string schemaName)
        {
            
            Context.Add(Data.SchemaName,schemaName);
            Context.Add(Data.SharedRepositoryClassOutput,new PaddedStringBuilder());
            

            SharedDalClassWriter.WriteUsingList();

            ExportTypeDll(schemaName);

            ExportBusinessDll(schemaName);

            Context.Remove(Data.SharedRepositoryClassOutput);
            Context.Remove(Data.SchemaName);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Exports the business DLL.
        /// </summary>
        void ExportBusinessDll(string schemaName)
        {
            var code = AllBusinessClassesInOne.GetCode(schemaName);

            BusinessProjectExporter.ExportAllInOneFile(schemaName, code);

            if (Config.EnableFullProjectExport)
            {
                BusinessProjectExporter.Export(schemaName, code);
            }
        }

        /// <summary>
        ///     Exports the type DLL.
        /// </summary>
        void ExportTypeDll(string schemaName)
        {
            var code = AllTypeClassesInOne.GetCode(schemaName);

            TypesProjectExporter.ExportAllInOneFile(schemaName, code);

            if (Config.EnableFullProjectExport)
            {
                TypesProjectExporter.Export(schemaName, code);
            }
        }
        #endregion
    }
}