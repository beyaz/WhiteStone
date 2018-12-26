using System;
using System.Collections.Generic;
using BOAPlugins.TypescriptModelGeneration;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    [Serializable]
    public class Model : ModelBase
    {
        #region Public Properties
        public bool DesignTabIsVisible { get; set; }

        public string FormType { get; set; }

        public bool                  FormTypeIsVisible { get; set; }
        public IReadOnlyList<string> FormTypes         { get; set; }

        public IReadOnlyList<string> RequestNames                     { get; set; }
        public bool                  ScreenInfoTabIsVisible           { get; set; }
        public string                RequestName              { get; set; }
        public bool                  RequestNameIsVisible   { get; set; }
        public string                TfsFolderName            { get; set; }
        public bool                  TfsFolderNameIsEnabled { get; set; }
        public string                SolutionFilePath                 { get; set; }
        public SolutionInfo          SolutionInfo                     { get; set; }
        public bool                  StartTabIsVisible                { get; set; }

        public IReadOnlyList<string> TfsFolderNames { get; set; }
        #endregion
    }
}