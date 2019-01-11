using System;
using System.Collections.Generic;
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
        ///     Gets or sets the label.
        /// </summary>
        public LabelInfo Label { get; set; }


        public string LabelText => Label.GetDesignerText();
        #endregion
    }

    /// <summary>
    ///     The data grid
    /// </summary>
    [Serializable]
    public class BDataGrid:BField
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        public List<BDataGridColumnInfo> Columns { get; set; } = new List<BDataGridColumnInfo>();

        /// <summary>
        ///     Gets or sets the data source binding path.
        /// </summary>
        public string DataSourceBindingPath { get; set; }
        #endregion
    }
}