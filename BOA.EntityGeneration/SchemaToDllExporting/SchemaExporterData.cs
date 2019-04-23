using System;
using System.Collections.Generic;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.Generators;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    public class SchemaExporterData
    {
        #region Public Properties
        public string                     CatalogName          { get; set; }
        public IDatabase                  Database             { get; set; }
        public Action<GeneratorData>      OnTableDataCreated   { get; set; }
        public IReadOnlyList<string>      ReferencedAssemblies { get; set; }
        public string                     SchemaName           { get; set; }
        public Func<string, string, bool> TableNameFilter      { get; set; }
        #endregion
    }
}