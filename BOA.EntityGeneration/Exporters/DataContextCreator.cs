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
    public  class EntityGenerationDataContextCreator
    {
        #region Public Methods
        public static Context Create()
        {
            var context = new Context();

            InitializeServices(context);

            AttachEvents(context);

            return context;
        }
        #endregion

        #region Methods
        static void AttachEvents(Context context)
        {
            context.AttachEvent(TableExportStarted, TableNamingPatternInitializer.Initialize);

            new EntityFileExporter{Context = context}.AttachEvents();
            SharedFileExporter.AttachEvents(context);
            BoaRepositoryFileExporter.AttachEvents(context);

            context.AttachEvent(SchemaExportStarted, Events.OnSchemaStartedToExport);

            context.AttachEvent(SchemaExportFinished, EntityCsprojFileExporter.Export);
            context.AttachEvent(SchemaExportFinished, RepositoryCsprojFileExporter.Export);

            context.AttachEvent(SchemaExportStarted, MsBuildQueue.Build);
        }

        static void InitializeBuildQueue(Context context)
        {
            context.Add(MsBuildQueue.MsBuildQueueId, new MsBuildQueue());
        }

        static void InitializeConfig(Context context, string configFilePath = null)
        {
            if (configFilePath == null)
            {
                configFilePath = Path.GetDirectoryName(typeof(EntityGenerationDataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
            }

            context.Add(Config, JsonHelper.Deserialize<ConfigContract>(File.ReadAllText(configFilePath)));
        }

        static void InitializeDatabase(Context context)
        {
            var config = Config[context];

            context.Add(Data.Database, new SqlDatabase(config.ConnectionString) {CommandTimeout = 1000 * 60 * 60});
        }

        static void InitializeProcessInfo(Context context)
        {
            var processContract = new ProcessContract();
            context.Add(ProcessInfo, processContract);
            context.Add(MsBuildQueue.ProcessInfo, processContract);
        }

        static void InitializeServices(Context context)
        {
            InitializeConfig(context);
            InitializeDatabase(context);
            InitializeProcessInfo(context);
            InitializeBuildQueue(context);
        }
        #endregion
    }
}