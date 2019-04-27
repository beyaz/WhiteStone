namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class ProjectExportLocation
    {
        #region Public Methods
        public string GetExportLocationOfBusinessProject(string schemaName)
        {
            return $@"D:\temp\{schemaName}\";
        }

        public string GetExportLocationOfTypeProject(string schemaName)
        {
            return $@"D:\temp\{schemaName}\";
        }
        #endregion
    }
}