using System.Collections.Generic;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile
{
    class AllSchemaInOneClassRepositoryFileExporterConfig
    {
        #region Public Properties
        public Dictionary<string, string> DefaultValuesForInsertMethod      { get; set; }
        public Dictionary<string, string> DefaultValuesForUpdateByKeyMethod { get; set; }
        public Dictionary<string, string> NamingPattern                     { get; set; }
        public string[] ExcludedColumnNamesWhenInsertOperation { get; set; }
        #endregion
    }
}