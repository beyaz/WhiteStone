using BOA.Services;

namespace BOAPlugins.Utility
{
    public class Host
    {
        #region Public Properties
        public JsonFile<CheckInInformation> CheckInInformationFile { get; set; } = new JsonFile<CheckInInformation>(ConstConfiguration.BOAPluginDirectory + "CheckInInformation.json");
        public JsonFile<Configuration>      ConfigurationFile      { get; set; } = new JsonFile<Configuration>(ConstConfiguration.PluginDirectory + "BOAPlugins.VSIntegration.Configuration.json");
        #endregion
    }
}