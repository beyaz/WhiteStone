using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public class CustomSqlDataContextCreator : DataContextCreatorBase
    {
        public IDataContext Create()
        {
            var context = new DataContext();

            context.Add(CustomSqlExporter.CustomSqlGenerationOfProfileIdProcess, new ProcessInfo());
            var configFilePath = Path.GetDirectoryName(typeof(CustomSqlDataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "CustomSQLExporting.json";
            context.Add(CustomSqlExporter.ConfigFile, JsonHelper.Deserialize<ConfigurationContract>(File.ReadAllText(configFilePath)));

            InitializeServices(context);

            AttachEvents(context);
            

            return context;
        }

        protected virtual void AttachEvents(IDataContext context)
        {

            TypeFileExporter.AttachEvents(context);
            SharedFileExporter.AttachEvents(context);
            BoaRepositoryFileExporter.AttachEvents(context);
            TypesProjectExporter.AttachEvents(context);

            
        }
    }
}