using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.MainForm
{
    [Serializable]
    public class InitialConfig
    {
        #region Public Properties
        public IReadOnlyList<string> TfsFolderNames { get; set; }
        #endregion

       
    }
}