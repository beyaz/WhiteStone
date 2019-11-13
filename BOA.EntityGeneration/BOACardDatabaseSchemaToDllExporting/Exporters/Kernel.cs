using System.IO;
using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.TfsAccess;
using Ninject;
using static ___Company___.EntityGeneration.DataFlow.DataContext;
using DataContext = ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{


    public class Kernel 
    {
        public static IDataContext CreateDataContext(string configFilePath ,bool isFileAccessWithTfs, string checkinComment)
        {
            var context = new DataContext();
            if (configFilePath == null)
            {
                configFilePath = Path.GetDirectoryName(typeof(Kernel).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
            }

            context.Add(Data.Config, JsonHelper.Deserialize<Config>(File.ReadAllText(configFilePath)));
            context.Add(Data.Database, new SqlDatabase(context.Get(Data.Config).ConnectionString) {CommandTimeout = 1000 * 60 * 60});
            context.Add(Data.MsBuildQueue, new MsBuildQueue());
            context.Add(Data.SchemaGenerationProcess, new ProcessInfo());
            context.Add(Data.CustomSqlGenerationOfProfileIdProcess, new ProcessInfo());


            if (isFileAccessWithTfs)
            {
                context.Add(Data.FileAccess,  new FileSystem
                {
                    Config     = context.Get(Data.Config),
                    FileAccess = new FileAccessWithAutoCheckIn{CheckInComment = checkinComment}
                });
            }
            else
            {
                context.Add(Data.FileAccess,  new FileSystem
                {
                    Config     = context.Get(Data.Config),
                    FileAccess = new TfsAccess.FileAccess()
                });
            }

           
            
            context.Add(Data.AllSchemaGenerationProcess, new ProcessInfo());
            context.Add(Data.SchemaGenerationProcess, new ProcessInfo());

            return context;
        }
        
        #region Constructors
        public Kernel(string configFilePath = null)
        {
            if (configFilePath == null)
            {
                configFilePath = Path.GetDirectoryName(typeof(Kernel).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
            }

            Context.Add(Data.Config, JsonHelper.Deserialize<Config>(File.ReadAllText(configFilePath)));
            Context.Add(Data.Database, new SqlDatabase(Context.Get(Data.Config).ConnectionString) {CommandTimeout = 1000 * 60 * 60});
            Context.Add(Data.MsBuildQueue, new MsBuildQueue());
            Context.Add(Data.SchemaGenerationProcess, new ProcessInfo());
        }
        #endregion
    }
}