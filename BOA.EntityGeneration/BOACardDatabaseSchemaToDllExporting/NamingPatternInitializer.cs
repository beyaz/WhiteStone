using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    static class NamingPatternInitializer
    {
        #region Public Methods
        public static void Initialize(IDataContext context)
        {
            var config = context.Get(Data.Config);

            var initialValues = new Dictionary<string, string> {{nameof(Data.SchemaName), context.Get(Data.SchemaName)}};

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.NamingPattern, initialValues);

            context.Add(Data.NamingPattern, new NamingPattern
            {
                SlnDirectoryPath           = dictionary[nameof(NamingPattern.SlnDirectoryPath)],
                EntityNamespace            = dictionary[nameof(NamingPattern.EntityNamespace)],
                RepositoryNamespace        = dictionary[nameof(NamingPattern.RepositoryNamespace)],
                EntityProjectDirectory     = dictionary[nameof(NamingPattern.EntityProjectDirectory)],
                RepositoryProjectDirectory = dictionary[nameof(NamingPattern.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines    = dictionary[nameof(NamingPattern.BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines           = dictionary[nameof(NamingPattern.EntityUsingLines)].Split('|'),
                SharedRepositoryUsingLines = dictionary[nameof(NamingPattern.SharedRepositoryUsingLines)].Split('|')
            });
        }

        public static void Remove(IDataContext context)
        {
            context.Remove(Data.NamingPattern);
        }
        #endregion
    }
}