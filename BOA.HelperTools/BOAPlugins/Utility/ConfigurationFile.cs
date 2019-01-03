namespace BOAPlugins.Utility
{
    public class ConfigurationFile : FileBase
    {
        #region Public Methods
        public Configuration Load()
        {
            return base.Load<Configuration>();
        }
        #endregion

        #region Methods
        protected override string GetFilePath()
        {
            if (DirectoryPath == null)
            {
                DirectoryPath = ConstConfiguration.PluginDirectory;
            }

            const string FileName = "BOAPlugins.VSIntegration.Configuration.json";

            return DirectoryPath + FileName;
        }
        #endregion
    }
}