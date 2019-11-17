using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
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
        public IReadOnlyList<string> SharedRepositoryUsingLines { get; set; }
        public string                RepositoryNamespace        { get; set; }
        public string                RepositoryProjectDirectory { get; set; }
        public string                SlnDirectoryPath           { get; set; }
        #endregion

        #region Public Methods
        public static void Initialize(IDataContext context)
        {
            var config = context.Get(Data.Config);

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.NamingPattern, (key, value) => key == nameof(Data.SchemaName) ? context.Get(Data.SchemaName) : value);

            context.Add(Id, new NamingPattern
            {
                SlnDirectoryPath           = dictionary[nameof(SlnDirectoryPath)],
                EntityNamespace            = dictionary[nameof(EntityNamespace)],
                RepositoryNamespace        = dictionary[nameof(RepositoryNamespace)],
                EntityProjectDirectory     = dictionary[nameof(EntityProjectDirectory)],
                RepositoryProjectDirectory = dictionary[nameof(RepositoryProjectDirectory)],
                BoaRepositoryUsingLines    = dictionary[nameof(BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines           = dictionary[nameof(EntityUsingLines)].Split('|'),
                SharedRepositoryUsingLines           = dictionary[nameof(SharedRepositoryUsingLines)].Split('|')
            });
        }

        public static void Remove(IDataContext context)
        {
            context.Remove(Id);
        }
        #endregion
    }
}