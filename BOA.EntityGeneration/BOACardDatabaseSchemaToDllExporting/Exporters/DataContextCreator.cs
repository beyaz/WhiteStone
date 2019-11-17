using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.SharedRepositoryFileExporting;
using static BOA.EntityGeneration.DataFlow.DataEvent;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    public class DataContextCreatorBase
    {
        #region Public Properties
        public string ConfigFilePath { get; set; }
        #endregion

        #region Public Methods
        public void InitializeServices(IDataContext context)
        {
            if (ConfigFilePath == null)
            {
                ConfigFilePath = Path.GetDirectoryName(typeof(DataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
            }

            context.Add(Data.Config, JsonHelper.Deserialize<ConfigContract>(File.ReadAllText(ConfigFilePath)));
            context.Add(Data.Database, new SqlDatabase(context.Get(Data.Config).ConnectionString) {CommandTimeout = 1000 * 60 * 60});
            context.Add(MsBuildQueue.MsBuildQueueId, new MsBuildQueue());

            context.Add(Data.AllSchemaGenerationProcess, new ProcessContract());
            context.Add(Data.SchemaGenerationProcess, new ProcessContract());
            
        }
        #endregion
    }

    public class DataContextCreator : DataContextCreatorBase
    {
        #region Public Methods
        public IDataContext Create()
        {
            var context = new DataContext();

            InitializeServices(context);

            AttachEvents(context);

            return context;
        }
        #endregion

        #region Methods
        protected virtual void AttachEvents(IDataContext context)
        {

            EntityFileExporter.AttachEvents(context);
            SharedFileExporter.AttachEvents(context);

            context.AttachEvent(StartToExportTable, TableNamingPatternInitializer.Initialize);

            
            context.AttachEvent(StartToExportTable, GeneratorOfBusinessClass.WriteClass);

            context.AttachEvent(StartToExportTable, TableNamingPatternInitializer.Remove);

            
            context.AttachEvent(StartToExportSchema, GeneratorOfBusinessClass.WriteUsingList);
            
            context.AttachEvent(StartToExportSchema, AllBusinessClassesInOne.BeginNamespace);
            context.AttachEvent(StartToExportSchema, Events.OnSchemaStartedToExport);
            
            context.AttachEvent(StartToExportSchema, GeneratorOfBusinessClass.EndNamespace);

            context.AttachEvent(StartToExportSchema, TypesProjectExporter.ExportTypeDll);
            context.AttachEvent(StartToExportSchema, BusinessProjectExporter.Export);


            context.AttachEvent(StartToExportSchema, MsBuildQueue.Build);
        }
        #endregion
    }
}