using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.BoaRepositoryFileExporting;
using BOA.EntityGeneration.CsprojFileExporters;
using BOA.EntityGeneration.DataAccess;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.SharedRepositoryFileExporting;

namespace BOA.EntityGeneration.Exporters
{
    public class EntityGenerationDataContextCreator
    {
        #region Public Properties
        public string ConfigFilePath { get; set; }
        #endregion

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
            context.AttachEvent(TableExportingEvent.TableExportStarted, TableNamingPatternInitializer.Initialize);
            EntityFileExporter.AttachEvents(context);
            SharedFileExporter.AttachEvents(context);
            BoaRepositoryFileExporter.AttachEvents(context);

            

            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, Events.OnSchemaStartedToExport);

            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, EntityCsprojFileExporter.Export);
            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, RepositoryCsprojFileExporter.Export);

            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, MsBuildQueue.Build);
        }

        void InitializeServices(IDataContext context)
        {
            if (ConfigFilePath == null)
            {
                ConfigFilePath = Path.GetDirectoryName(typeof(EntityGenerationDataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
            }

            context.Add(Data.Config, JsonHelper.Deserialize<ConfigContract>(File.ReadAllText(ConfigFilePath)));
            context.Add(Data.Database, new SqlDatabase(context.Get(Data.Config).ConnectionString) {CommandTimeout = 1000 * 60 * 60});
            context.Add(MsBuildQueue.MsBuildQueueId, new MsBuildQueue());


            context.Add(Data.AllSchemaGenerationProcess, new ProcessContract());

            var processContract = new ProcessContract();
            context.Add(Data.SchemaGenerationProcess, processContract);
            context.Add(MsBuildQueue.ProcessInfo, processContract);
        }


        #endregion
    }
}