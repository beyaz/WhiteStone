namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class ProjectExportLocation
    {
        #region Public Properties
        public Config Config { get; set; }
        #endregion

        #region Properties
        string Dir => Config.SlnDirectoryPath;
        #endregion

        #region Public Methods
        public string GetExportLocationOfBusinessProject(string schemaName)
        {
            return $@"{Dir}{schemaName}\";
        }

        public string GetExportLocationOfTypeProject(string schemaName)
        {
            return $@"{Dir}{schemaName}\";
        }
        #endregion
    }
}