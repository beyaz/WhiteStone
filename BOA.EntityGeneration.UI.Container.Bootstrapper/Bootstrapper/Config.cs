using BOA.EntityGeneration.UI.Container.Bootstrapper.Infrastructure;

namespace BOA.EntityGeneration.UI.Container.Bootstrapper
{
    class Config
    {
        #region Public Properties
        public string ZipFilePath          { get; set; }
        public string ZipFileUrl           { get; set; }
        public string ExportDir { get; set; }
        public string ProcessPath { get; set; }
        #endregion

        

        #region Public Methods
        public static Config CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<Config>(nameof(Config) + ".yaml");
        }
        #endregion
    }
}