using System;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The data grid column
    /// </summary>
    [Serializable]
    public class BDataGridColumnInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the binding path.
        /// </summary>
        public string BindingPath { get; set; }


        public string IsVisibleBindingPath { get; set; }

        public int? Width { get; set; }

        /// <summary>
        ///     Gets or sets the label.
        /// </summary>
        public LabelInfo Label { get; set; }


        public string LabelText => Label.GetDesignerText();
        #endregion
    }
}