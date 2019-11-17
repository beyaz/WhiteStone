using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    class NamingPattern
    {
        #region Static Fields
        internal static readonly IDataConstant<NamingPattern> Id = DataConstant.Create<NamingPattern>(nameof(NamingPattern));
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
        public static void Initialize(IDataContext context)
        {
            var config = context.Get(CustomSqlExporter.Config);

            var initialValues = new Dictionary<string, string>
            {
                {nameof(CustomSqlExporter.ProfileName), context.Get(CustomSqlExporter.ProfileName)},
                
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.NamingPattern, initialValues);

            context.Add(Id, new NamingPattern
            {
                SlnDirectoryPath           = dictionary[nameof(SlnDirectoryPath)],
                EntityNamespace            = dictionary[nameof(EntityNamespace)],
                RepositoryNamespace        = dictionary[nameof(RepositoryNamespace)],
                EntityProjectDirectory     = dictionary[nameof(EntityProjectDirectory)],
                RepositoryProjectDirectory = dictionary[nameof(RepositoryProjectDirectory)],
                BoaRepositoryUsingLines    = dictionary[nameof(BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines           = dictionary[nameof(EntityUsingLines)].Split('|')
            });
        }

        public static void Remove(IDataContext context)
        {
            context.Remove(Id);
        }
        #endregion
    }
}