using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.Naming
{
    static class NamingPatternInitializer
    {
        #region Public Methods
        public static void Initialize(Context context)
        {
            var config = context.Get(Data.Config);

            var initialValues = new Dictionary<string, string> {{nameof(Data.SchemaName), context.Get(Data.SchemaName)}};

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.NamingPattern, initialValues);

            context.Add(NamingPatternContract.NamingPattern, new NamingPatternContract
            {
                SlnDirectoryPath           = dictionary[nameof(NamingPatternContract.SlnDirectoryPath)],
                EntityNamespace            = dictionary[nameof(NamingPatternContract.EntityNamespace)],
                RepositoryNamespace        = dictionary[nameof(NamingPatternContract.RepositoryNamespace)],
                EntityProjectDirectory     = dictionary[nameof(NamingPatternContract.EntityProjectDirectory)],
                RepositoryProjectDirectory = dictionary[nameof(NamingPatternContract.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines    = dictionary[nameof(NamingPatternContract.BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines           = dictionary[nameof(NamingPatternContract.EntityUsingLines)].Split('|'),
                SharedRepositoryUsingLines = dictionary[nameof(NamingPatternContract.SharedRepositoryUsingLines)].Split('|')
            });
        }
        #endregion
    }
}