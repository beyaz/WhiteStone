using BOAPlugins.Utility;

namespace BOAPlugins.VSIntegration
{
    public class Host
    {
        #region Public Properties
        public ConfigurationFile ConfigurationFile { get; set; } = new ConfigurationFile();
        public CheckInInformationFile CheckInInformationFile { get; set; } = new CheckInInformationFile();
        #endregion
    }
}