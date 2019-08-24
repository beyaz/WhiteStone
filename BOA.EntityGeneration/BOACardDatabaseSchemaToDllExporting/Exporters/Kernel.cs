using System;
using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    public class Kernel : StandardKernel
    {
        static Config GetConfig()
        {
            return JsonHelper.Deserialize<Config>(File.ReadAllText("BOA.EntityGeneration.json"));
        }

         static IDatabase CreateConnection(Config config)
        {
            return new SqlDatabase(config.ConnectionString)
            {
                CommandTimeout = 1000 * 60 * 60
            };
        }

        public static IDatabase CreateConnection()
        {
            return new SqlDatabase(GetConfig().ConnectionString)
            {
                CommandTimeout = 1000 * 60 * 60
            };
        }

        #region Constructors
        public Kernel()
        {

            Bind<Config>().ToConstant(GetConfig()).InSingletonScope();
            Bind<IDatabase>().ToConstant(CreateConnection()).InSingletonScope();
            Bind<MsBuildQueue>().To<MsBuildQueue>().InSingletonScope();
            Bind<ScriptModel.Creators.InsertInfoCreator>().To<InsertInfoCreator>();
        }
        #endregion
    }
}