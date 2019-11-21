using System.Collections.Generic;
using BOA.DataFlow;

namespace BOA.EntityGeneration.Naming
{
    class NamingPatternContract
    {
        #region Static Fields
        internal static readonly IDataConstant<NamingPatternContract> NamingPattern = DataConstant.Create<NamingPatternContract>(nameof(NamingPatternContract));
        #endregion

        #region Public Properties
        public IReadOnlyList<string> BoaRepositoryUsingLines    { get; set; }
        public string                EntityNamespace            { get; set; }
        public string                EntityProjectDirectory     { get; set; }
        public IReadOnlyList<string> EntityUsingLines           { get; set; }
        public string                RepositoryNamespace        { get; set; }
        public string                RepositoryProjectDirectory { get; set; }
        public IReadOnlyList<string> SharedRepositoryUsingLines { get; set; }
        public string                SlnDirectoryPath           { get; set; }
        #endregion
    }
}