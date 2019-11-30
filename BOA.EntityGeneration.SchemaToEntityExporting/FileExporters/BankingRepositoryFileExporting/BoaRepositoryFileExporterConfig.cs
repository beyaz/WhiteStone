using System.Collections.Generic;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting
{
    class BoaRepositoryFileExporterConfig
    {
        #region Public Properties
        public Dictionary<string, string> DefaultValuesForInsertMethod      { get; set; }
        public Dictionary<string, string> DefaultValuesForUpdateByKeyMethod { get; set; }
        public string[] ExcludedColumnNamesWhenInsertOperation { get; set; }
        #endregion
    }
}