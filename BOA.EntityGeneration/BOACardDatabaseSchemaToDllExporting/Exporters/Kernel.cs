using System.IO;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    public class Kernel : StandardKernel
    {
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