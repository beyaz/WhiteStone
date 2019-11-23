namespace BOA.EntityGeneration.CustomSQLExporting
{
    class CustomSqlNamingPatternContract
    {
        #region Public Properties
        public string InputClassName                   { get; set; }
        public string ReferencedEntityAccessPath       { get; set; }
        public string ReferencedEntityAssemblyPath     { get; set; }
        public string ReferencedEntityReaderMethodPath { get; set; }
        public string ReferencedRepositoryAssemblyPath { get; set; }
        public string RepositoryClassName              { get; set; }
        public string ResultClassName                  { get; set; }
        #endregion
    }
}