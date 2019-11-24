using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    class Context : DataFlow.Context
    {
        #region Constructors
        public Context()
        {
            processInfo  = new ProcessContract();
            MsBuildQueue = new MsBuildQueue {Trace = trace => { processInfo.Text = trace; }};
            FileSystem   = new FileSystem();
        }
        #endregion

        #region Public Properties
        public ConfigurationContract config       { get; set; }
        public FileSystem            FileSystem   { get; }
        public MsBuildQueue          MsBuildQueue { get; }
        public ProcessContract       processInfo  { get; }
        public string profileName { get; set; }
        public CustomSqlInfo customSqlInfo { get; set; }
        public CustomSqlNamingPatternContract customSqlNamingPattern { get; set; }
        public SqlDatabase database { get; set; }
        public ProfileNamingPatternContract profileNamingPattern { get; set; }
        #endregion
    }
}