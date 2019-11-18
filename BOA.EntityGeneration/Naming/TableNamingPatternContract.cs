using BOA.DataFlow;

namespace BOA.EntityGeneration.Naming
{
    class TableNamingPatternContract
    {
        internal static readonly IDataConstant<TableNamingPatternContract> TableNamingPattern = DataConstant.Create<TableNamingPatternContract>(nameof(TableNamingPatternContract));

        public string EntityClassName           { get; set; }
        public string SharedRepositoryClassName { get; set; }
        public string BoaRepositoryClassName    { get; set; }
        public string SharedRepositoryClassNameInBoaRepositoryFile { get; set; }
    }
}