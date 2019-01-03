using System.IO;
using BOA.Common.Helpers;

namespace BOAPlugins.Utility
{
    public class FileBase
    {
        #region Public Properties
        public string DirectoryPath { get; set; }
        #endregion

        #region Public Methods
        public void Delete()
        {
            if (File.Exists(GetFilePath()))
            {
                File.Delete(GetFilePath());
            }
        }

        public T Load<T>() where T : new()
        {
            var fileNotExists = File.Exists(GetFilePath()) == false;
            if (fileNotExists)
            {
                Save(new T());
            }

            var configurationAsString = File.ReadAllText(GetFilePath());

            return JsonHelper.Deserialize<T>(configurationAsString);
        }

        public void Save<T>(T configuration)
        {
            FileHelper.WriteAllText(GetFilePath(), JsonHelper.Serialize(configuration));
        }
        #endregion

        #region Methods
        protected virtual string GetFilePath()
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