using System;
using System.Collections.Generic;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The data grid
    /// </summary>
    [Serializable]
    public class BDataGrid:BField
    {
        public LabelInfo TitleInfo { get; set; } = new LabelInfo();

        #region Public Properties
        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        public List<BDataGridColumnInfo> Columns { get; set; } = new List<BDataGridColumnInfo>();

        /// <summary>
        ///     Gets or sets the data source binding path.
        /// </summary>
        public string DataSourceBindingPath { get; set; }


        public  string DataSourceBindingPathInTypeScript => TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + DataSourceBindingPath);
        #endregion
    }
}