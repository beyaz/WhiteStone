using System;
using System.Collections.Generic;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    [Serializable]
    public class Model : ModelBase
    {

        public string SelectedRequestName { get; set; }

        public bool SelectedTfsFolderNameIsIsEnabled { get; set; }
        #region Public Properties
        public bool DesignTabIsVisible     { get; set; }

        public bool ScreenInfoTabIsVisible { get; set; }

        public string SelectedTfsFolderName { get; set; }

        public string SolutionFilePath  { get; set; }

        public bool   StartTabIsVisible { get; set; }

        public IReadOnlyList<string> TfsFolderNames { get; set; }

        public IReadOnlyList<string> RequestNames { get; set; }

        public bool SelectedRequestNameIsIsEnabled { get; set; }
        public string SlnFilePath { get; set; }
        #endregion
    }
}