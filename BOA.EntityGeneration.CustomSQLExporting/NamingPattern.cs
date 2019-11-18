using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;

namespace BOA.EntityGeneration.CustomSQLExporting
{

    static class ProfileNamingPatternInitializer
    {
        public static void Initialize(IDataContext context)
        {
            var config = context.Get(CustomSqlExporter.Config);

            var initialValues = new Dictionary<string, string>
            {
                {nameof(CustomSqlExporter.ProfileName), context.Get(CustomSqlExporter.ProfileName)},
                
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.NamingPattern, initialValues);

            context.Add(ProfileNamingPatternContract.ProfileNamingPattern, new ProfileNamingPatternContract
            {
                SlnDirectoryPath           = dictionary[nameof(ProfileNamingPatternContract.SlnDirectoryPath)],
                EntityNamespace            = dictionary[nameof(ProfileNamingPatternContract.EntityNamespace)],
                RepositoryNamespace        = dictionary[nameof(ProfileNamingPatternContract.RepositoryNamespace)],
                EntityProjectDirectory     = dictionary[nameof(ProfileNamingPatternContract.EntityProjectDirectory)],
                RepositoryProjectDirectory = dictionary[nameof(ProfileNamingPatternContract.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines    = dictionary[nameof(ProfileNamingPatternContract.BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines           = dictionary[nameof(ProfileNamingPatternContract.EntityUsingLines)].Split('|')
            });
        }

        public static void Remove(IDataContext context)
        {
            context.Remove(ProfileNamingPatternContract.ProfileNamingPattern);
        }
    }

    class ProfileNamingPatternContract
    {
        #region Static Fields
        internal static readonly IDataConstant<ProfileNamingPatternContract> ProfileNamingPattern = DataConstant.Create<ProfileNamingPatternContract>(nameof(ProfileNamingPatternContract));
        #endregion

        #region Public Properties
        public IReadOnlyList<string> BoaRepositoryUsingLines    { get; set; }
        public string                EntityNamespace            { get; set; }
        public string                EntityProjectDirectory     { get; set; }
        public IReadOnlyList<string> EntityUsingLines           { get; set; }
        public string                RepositoryNamespace        { get; set; }
        public string                RepositoryProjectDirectory { get; set; }
        public string                SlnDirectoryPath           { get; set; }
        #endregion

        #region Public Methods
        
        #endregion
    }
}