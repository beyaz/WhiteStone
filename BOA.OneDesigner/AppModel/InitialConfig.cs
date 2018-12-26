using System;
using System.Collections.Generic;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.AppModel
{
    /// <summary>
    ///     The initial configuration
    /// </summary>
    [Serializable]
    public class InitialConfig
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the screen information list.
        /// </summary>
        public List<ScreenInfo> ScreenInfoList { get; set; } 

        /// <summary>
        ///     Gets or sets the TFS folder names.
        /// </summary>
        public IReadOnlyList<string> TfsFolderNames { get; set; }
        #endregion
    }
}