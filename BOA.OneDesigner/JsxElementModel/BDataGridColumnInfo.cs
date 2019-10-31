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

        /// <summary>
        ///     Gets or sets the date format.
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        ///     Gets or sets the is visible binding path.
        /// </summary>
        public string IsVisibleBindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the label.
        /// </summary>
        public LabelInfo Label { get; set; }

        /// <summary>
        ///     Gets the label text.
        /// </summary>
        public string LabelText => Label.GetDesignerText();

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        public int? Width { get; set; }
        #endregion
    }
}