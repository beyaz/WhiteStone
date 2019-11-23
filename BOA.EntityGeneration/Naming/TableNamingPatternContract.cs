using BOA.DataFlow;

namespace BOA.EntityGeneration.Naming
{
    class TableNamingPatternContract
    {
        #region Static Fields
        internal static readonly IProperty<TableNamingPatternContract> TableNamingPattern = Property.Create<TableNamingPatternContract>(nameof(TableNamingPatternContract));
        #endregion

        #region Public Properties
        public string BoaRepositoryClassName { get; set; }

        public string EntityClassName                              { get; set; }
        public string SharedRepositoryClassName                    { get; set; }
        public string SharedRepositoryClassNameInBoaRepositoryFile { get; set; }
        #endregion
    }
}