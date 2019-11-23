﻿using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using static BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters.MsBuildQueue;
using static BOA.EntityGeneration.CustomSQLExporting.Data;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public sealed class CustomSqlDataContextCreator
    {
        #region Public Methods
        public Context Create()
        {
            var context = new Context();

            InitProcessInfo(context);
            InitializeConfig(context);
            InitializeDatabaseConnection(context);
            context.Add(MsBuildQueueId, new MsBuildQueue());

            AttachEvents(context);

            return context;
        }
        #endregion

        #region Methods
        static void AttachEvents(Context context)
        {
            new EntityFileExporter {Context = context}.AttachEvents();
            new SharedFileExporter {Context = context}.AttachEvents();
            new BoaRepositoryFileExporter {Context = context}.AttachEvents();
            new EntityCsprojFileExporter {Context = context}.AttachEvents();
            new RepositoryCsprojFileExporter {Context = context}.AttachEvents();
        }

        static void InitializeConfig(Context context)
        {
            var configFilePath = Path.GetDirectoryName(typeof(CustomSqlDataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "CustomSQLExporting.json";

            Config[context] = JsonHelper.Deserialize<ConfigurationContract>(File.ReadAllText(configFilePath));
        }

        static void InitializeDatabaseConnection(Context context)
        {
            var connectionString = Config[context].ConnectionString;

            Data.Database[context] = new SqlDatabase(connectionString) {CommandTimeout = 1000 * 60 * 60};
        }

        static void InitProcessInfo(Context context)
        {
            var processContract = new ProcessContract();

            Data.ProcessInfo[context]         = processContract;
            MsBuildQueue.ProcessInfo[context] = processContract;
        }
        #endregion
    }
}