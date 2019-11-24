namespace BOA.EntityGeneration.SchemaToEntityExporting.Models.NamingModel
{
    class TableNamingPatternContract
    {
        #region Public Properties
        public string BoaRepositoryClassName { get; set; }

        public string EntityClassName                              { get; set; }
        public string SharedRepositoryClassName                    { get; set; }
        public string SharedRepositoryClassNameInBoaRepositoryFile { get; set; }
        #endregion
    }
}