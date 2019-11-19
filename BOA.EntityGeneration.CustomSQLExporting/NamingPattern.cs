using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using static BOA.EntityGeneration.CustomSQLExporting.ProfileNamingPatternContract;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting
{

   
    static class ProfileNamingPatternInitializer
    {
        #region Public Methods
        public static void Initialize(IDataContext context)
        {
            var config = context.Get(Config);

            var initialValues = new Dictionary<string, string>
            {
                {nameof(ProfileName), context.Get(ProfileName)}
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.ProfileNamingPattern, initialValues);

            context.Add(Data.ProfileNamingPattern, new ProfileNamingPatternContract
            {
                SlnDirectoryPath           = dictionary[nameof(ProfileNamingPatternContract.SlnDirectoryPath)],
                EntityNamespace            = dictionary[nameof(ProfileNamingPatternContract.EntityNamespace)],
                RepositoryNamespace        = dictionary[nameof(ProfileNamingPatternContract.RepositoryNamespace)],
                EntityProjectDirectory     = dictionary[nameof(ProfileNamingPatternContract.EntityProjectDirectory)],
                RepositoryProjectDirectory = dictionary[nameof(ProfileNamingPatternContract.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines    = dictionary[nameof(ProfileNamingPatternContract.BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines           = dictionary[nameof(ProfileNamingPatternContract.EntityUsingLines)].Split('|'),
                EntityAssemblyReferences           = dictionary[nameof(ProfileNamingPatternContract.EntityAssemblyReferences)].Split('|'),
                RepositoryAssemblyReferences = dictionary[nameof(ProfileNamingPatternContract.RepositoryAssemblyReferences)].Split('|'),
                
            });
        }

        
        #endregion
    }

    class ProfileNamingPatternContract
    {
        #region Static Fields
        
        #endregion

        #region Public Properties
        public IReadOnlyList<string> BoaRepositoryUsingLines    { get; set; }
        public string                EntityNamespace            { get; set; }
        public string                EntityProjectDirectory     { get; set; }
        public IReadOnlyList<string> EntityUsingLines           { get; set; }
        public IReadOnlyList<string> EntityAssemblyReferences { get; set; }
        public IReadOnlyList<string> RepositoryAssemblyReferences { get; set; }
        


        public string                RepositoryNamespace        { get; set; }
        public string                RepositoryProjectDirectory { get; set; }
        public string                SlnDirectoryPath           { get; set; }
        #endregion
    }
}