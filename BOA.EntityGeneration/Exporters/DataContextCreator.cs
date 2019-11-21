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
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;
using static BOA.EntityGeneration.DataFlow.TableExportingEvent;

namespace BOA.EntityGeneration.Exporters
{
    public sealed class EntityGenerationDataContextCreator
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
        static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(TableExportStarted, TableNamingPatternInitializer.Initialize);

            EntityFileExporter.AttachEvents(context);
            SharedFileExporter.AttachEvents(context);
            BoaRepositoryFileExporter.AttachEvents(context);

            context.AttachEvent(SchemaExportStarted, Events.OnSchemaStartedToExport);

            context.AttachEvent(SchemaExportFinished, EntityCsprojFileExporter.Export);
            context.AttachEvent(SchemaExportFinished, RepositoryCsprojFileExporter.Export);

            context.AttachEvent(SchemaExportStarted, MsBuildQueue.Build);
        }

        void InitializeServices(IDataContext context)
        {
            if (ConfigFilePath == null)
            {
                ConfigFilePath = Path.GetDirectoryName(typeof(EntityGenerationDataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
            }

            context.Add(Config, JsonHelper.Deserialize<ConfigContract>(File.ReadAllText(ConfigFilePath)));
            context.Add(Data.Database, new SqlDatabase(context.Get(Config).ConnectionString) {CommandTimeout = 1000 * 60 * 60});
            context.Add(MsBuildQueue.MsBuildQueueId, new MsBuildQueue());


            var processContract = new ProcessContract();
            context.Add(ProcessInfo, processContract);
            context.Add(MsBuildQueue.ProcessInfo, processContract);
        }
        #endregion
    }
}