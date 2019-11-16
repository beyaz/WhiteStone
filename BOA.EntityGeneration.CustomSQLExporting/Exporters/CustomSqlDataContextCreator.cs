using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public sealed  class CustomSqlDataContextCreator 
    {
        public IDataContext Create()
        {
            var context = new DataContext();

            var processInfo = new ProcessInfo();

            context.Add(CustomSqlExporter.CustomSqlGenerationOfProfileIdProcess, processInfo);
            context.Add(MsBuildQueue.ProcessInfo, processInfo);

            var configFilePath = Path.GetDirectoryName(typeof(CustomSqlDataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "CustomSQLExporting.json";
            context.Add(CustomSqlExporter.ConfigFile, JsonHelper.Deserialize<ConfigurationContract>(File.ReadAllText(configFilePath)));

            
            context.Add(CustomSqlExporter.Database, new SqlDatabase(context.Get(CustomSqlExporter.ConfigFile).ConnectionString) {CommandTimeout = 1000 * 60 * 60});
            context.Add(MsBuildQueue.MsBuildQueueId, new MsBuildQueue());

            

            AttachEvents(context);
            

            return context;
        }

        void AttachEvents(IDataContext context)
        {

            TypeFileExporter.AttachEvents(context);
            SharedFileExporter.AttachEvents(context);
            BoaRepositoryFileExporter.AttachEvents(context);
            TypesProjectExporter.AttachEvents(context);

            
        }
    }
}