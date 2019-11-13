using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;

namespace ___Company___.EntityGeneration.DataFlow
{
    /// <summary>
    ///     The data context
    /// </summary>
    public class DataContext : ___Company___.DataFlow.DataContext
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        public DataContext()
        {
            AttachEvent(DataEvent.StartToExportTable, GeneratorOfTypeClass.WriteClass);
            AttachEvent(DataEvent.StartToExportTable, GeneratorOfBusinessClass.CreateBusinessClassWriterContext);
            AttachEvent(DataEvent.StartToExportTable, GeneratorOfBusinessClass.WriteClass);
            AttachEvent(DataEvent.StartToExportTable, SharedDalClassWriter.Write);
            AttachEvent(DataEvent.StartToExportTable, GeneratorOfBusinessClass.RemoveBusinessClassWriterContext);

            AttachEvent(DataEvent.StartToExportSchema, SharedDalClassWriter.WriteUsingList);
            AttachEvent(DataEvent.StartToExportSchema, GeneratorOfTypeClass.WriteUsingList);
            AttachEvent(DataEvent.StartToExportSchema, GeneratorOfBusinessClass.WriteUsingList);
            AttachEvent(DataEvent.StartToExportSchema, GeneratorOfTypeClass.BeginNamespace);
            AttachEvent(DataEvent.StartToExportSchema, AllBusinessClassesInOne.BeginNamespace);
            AttachEvent(DataEvent.StartToExportSchema, Events.OnSchemaStartedToExport);
            AttachEvent(DataEvent.StartToExportSchema, SharedDalClassWriter.EndNamespace);
            AttachEvent(DataEvent.StartToExportSchema, GeneratorOfTypeClass.EndNamespace);
            AttachEvent(DataEvent.StartToExportSchema, GeneratorOfBusinessClass.EndNamespace);

            AttachEvent(DataEvent.StartToExportSchema, TypesProjectExporter.ExportTypeDll);
            AttachEvent(DataEvent.StartToExportSchema, BusinessProjectExporter.Export);
            AttachEvent(DataEvent.StartToExportSchema, SharedDalClassWriter.ExportFile);
            
            AttachEvent(DataEvent.StartToExportSchema, MsBuildQueue.Build);
        }
        #endregion
    }
}