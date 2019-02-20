using System;

namespace BOAPlugins.ViewClassDependency
{
    [Serializable]
    public class Data
    {
        #region Public Properties
        public string ActiveProjectName { get; set; }

        public string AssemblySearchDirectoryPath { get; set; }

        public string DgmlFileContent { get; set; }

        public string ErrorMessage       { get; set; }
        public string OutputFileFullPath { get; set; }

        public string SelectedText { get; set; }
        #endregion
    }
}