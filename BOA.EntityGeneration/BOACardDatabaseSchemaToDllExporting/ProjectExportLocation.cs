namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class ProjectExportLocation
    {
        #region Public Methods
        public string GetExportLocationOfBusinessProject(string schemaName)
        {
            return $@"D:\work\BOA.Kernel\Dev\BOA.Kernel.Card\BOACard\{schemaName}\";
        }

        public string GetExportLocationOfTypeProject(string schemaName)
        {
            return $@"D:\work\BOA.Kernel\Dev\BOA.Kernel.Card\BOACard\{schemaName}\";
        }
        #endregion
    }
}