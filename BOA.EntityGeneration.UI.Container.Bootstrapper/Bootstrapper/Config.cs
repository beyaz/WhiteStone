﻿using BOA.EntityGeneration.UI.Container.Infrastructure;

namespace BOA.EntityGeneration.UI.Container.Bootstrapper
{
    class Config
    {
        #region Public Properties
        public string ExportDir   { get; set; }
        public string ProcessPath { get; set; }
        public string ZipFilePath { get; set; }
        public string ZipFileUrl  { get; set; }
        #endregion

        #region Public Methods
        public static Config CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<Config>(nameof(Config) + ".yaml");
        }
        #endregion
    }
}