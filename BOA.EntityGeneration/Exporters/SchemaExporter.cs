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
    /// <summary>
    ///     The schema exporter
    /// </summary>
    class SchemaExporter:ContextContainer
    {

        public SchemaExporter()
        {
            Context = new Context();


            InitializeConfig(Context);
            InitializeDatabase(Context);
            InitializeProcessInfo(Context);
            InitializeBuildQueue(Context);

            AttachEvents(Context);
        }


        static void AttachEvents(Context context)
        {
            context.AttachEvent(TableExportStarted, TableNamingPatternInitializer.Initialize);

            new EntityFileExporter{Context = context}.AttachEvents();
            new SharedFileExporter{Context = context}.AttachEvents();
            new BoaRepositoryFileExporter{Context = context}.AttachEvents();

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
                configFilePath = Path.GetDirectoryName(typeof(SchemaExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
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

        #region Public Methods
        /// <summary>
        ///     Exports the specified schema name.
        /// </summary>
        public  void Export(string schemaName)
        {
            var context = Context;

            context.OpenBracket();

            this.schemaName = schemaName;

            NamingPatternInitializer.Initialize(context);

            context.FireEvent(SchemaExportingEvent.SchemaExportStarted);
            context.FireEvent(SchemaExportingEvent.SchemaExportFinished);

            context.CloseBracket();
        }
        #endregion
    }
}