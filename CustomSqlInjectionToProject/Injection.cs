using System;
using System.Threading;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using BOA.TfsAccess;
using WhiteStone.UI.Container;

namespace CustomSqlInjectionToProject
{
    public static class Injection
    {
        public static string ProfileId;

        #region Methods
        public static void Inject(string[] args)
        {

           


            if (args == null || args.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }

            ProfileId = args[0];

            App.Main();
        }
        #endregion
    }
}