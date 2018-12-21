using System;
using WhiteStone.UI.Container.Mvc;

namespace BOAPlugins.VSIntegration.MainForm
{
    [Serializable]
    public class Model : ModelBase
    {
        #region Public Properties
        public string CursorSelectedText        { get; set; }
        public string MethodCallGraphButtonText { get; set; }
        public bool   MethodCallGraphIsVisible  { get; set; }
        public string SolutionCheckInComment    { get; set; }
        public string SolutionFilePath { get; set; }
        public bool CheckInSolutionIsEnabled { get; set; }
        #endregion
    }
}