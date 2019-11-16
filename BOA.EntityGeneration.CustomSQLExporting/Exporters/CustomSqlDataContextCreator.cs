﻿using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public sealed class CustomSqlDataContextCreator
    {
        #region Public Methods
        public IDataContext Create()
        {
            var context = new DataContext();

            InitProcessInfo(context);
            InitializeConfig(context);
            InitializeDatabaseConnection(context);
            context.Add(MsBuildQueue.MsBuildQueueId, new MsBuildQueue());

            AttachEvents(context);

            return context;
        }
        #endregion

        #region Methods
        static void AttachEvents(IDataContext context)
        {
            TypeFileExporter.AttachEvents(context);
            SharedFileExporter.AttachEvents(context);
            BoaRepositoryFileExporter.AttachEvents(context);
            TypesProjectExporter.AttachEvents(context);
            RepositoryProjectExporter.AttachEvents(context);
        }

        static void InitializeConfig(IDataContext context)
        {
            var configFilePath = Path.GetDirectoryName(typeof(CustomSqlDataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "CustomSQLExporting.json";
            context.Add(Config, JsonHelper.Deserialize<ConfigurationContract>(File.ReadAllText(configFilePath)));
        }

        static void InitializeDatabaseConnection(DataContext context)
        {
            context.Add(CustomSqlExporter.Database, new SqlDatabase(context.Get(Config).ConnectionString) {CommandTimeout = 1000 * 60 * 60});
        }

        static void InitProcessInfo(IDataContext context)
        {
            var processContract = new ProcessContract();

            context.Add(ProcessInfo, processContract);
            context.Add(MsBuildQueue.ProcessInfo, processContract);
        }
        #endregion
    }
}