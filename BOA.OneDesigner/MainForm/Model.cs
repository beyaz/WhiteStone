using System;
using System.Collections.Generic;
using BOA.OneDesigner.JsxElementModel;
using BOA.UI.Types;
using BOAPlugins.Messaging;
using BOAPlugins.TypescriptModelGeneration;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    /// <summary>
    ///     The model
    /// </summary>
    [Serializable]
    public class Model : ModelBase
    {

        public bool ScreenInfoGottenFromCache { get; set; }

        #region Public Properties
        /// <summary>
        ///     Gets or sets a value indicating whether [design tab is visible].
        /// </summary>
        public bool DesignIsVisible { get; set; }

        /// <summary>
        ///     Gets or sets the form types.
        /// </summary>
        public IReadOnlyList<string> FormTypes { get; set; }

        /// <summary>
        ///     Gets or sets the messaging group names.
        /// </summary>
        public IReadOnlyList<string> MessagingGroupNames { get; set; }



        



        /// <summary>
        ///     Gets or sets the request names.
        /// </summary>
        public IReadOnlyList<string> RequestNames { get; set; }

        public ScreenInfo ScreenInfo { get; set; }

        public bool SearchIsVisible { get; set; }

        /// <summary>
        ///     Gets or sets the solution file path.
        /// </summary>
        public string SolutionFilePath { get; set; }

        /// <summary>
        ///     Gets or sets the solution information.
        /// </summary>
        public SolutionInfo SolutionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the TFS folder names.
        /// </summary>
        public IReadOnlyList<string> TfsFolderNames { get; set; }

        
        #endregion
    }
}