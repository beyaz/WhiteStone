using System;
using System.Collections.Generic;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    [Serializable]
    public class WorkingOnScreenListItem
    {
        #region Public Properties
        public bool   IsSelected { get; set; }
        public string Name       { get; set; }
        #endregion
    }

    [Serializable]
    public class Model : ModelBase
    {
        #region Public Properties
        public bool DesignTabIsVisible     { get; set; }
        public bool ScreenInfoTabIsVisible { get; set; }

        public string ScreenSearchKey { get; set; }

        public string                        SolutionFilePath    { get; set; }
        public bool                          StartTabIsVisible   { get; set; }
        public List<WorkingOnScreenListItem> WorkingOnScreenList { get; set; }
        #endregion
    }
}