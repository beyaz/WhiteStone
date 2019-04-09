using System;
using System.Collections.Generic;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The b data grid
    /// </summary>
    [Serializable]
    public class BDataGrid : BField
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

        /// <summary>
        ///     Gets or sets a value indicating whether [parent is ComboBox].
        /// </summary>
        public bool ParentIsComboBox { get; set; }

        /// <summary>
        ///     Gets or sets the row selection changed orchestration method.
        /// </summary>
        public string RowSelectionChangedOrchestrationMethod { get; set; }

        /// <summary>
        ///     Gets or sets the selected row data binding path.
        /// </summary>
        public string SelectedRowDataBindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the size information.
        /// </summary>
        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsMedium = true};

        /// <summary>
        ///     Gets or sets the title information.
        /// </summary>
        public LabelInfo TitleInfo { get; set; } = new LabelInfo();
        #endregion
    }
}