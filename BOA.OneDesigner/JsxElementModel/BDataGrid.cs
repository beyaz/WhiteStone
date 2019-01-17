using System;
using System.Collections.Generic;
using BOA.OneDesigner.Helpers;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.JsxElementModel
{

    [Serializable]
    public class BDataGrid:BField
    {
        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsMedium = true};
        public string SelectedRowDataBindingPath { get; set; }

        public string RowSelectionChangedOrchestrationMethod { get; set; }

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


        public string DataSourceBindingPathInTypeScript => TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + DataSourceBindingPath);
        #endregion
    }

    /// <summary>
    ///     The data grid
    /// </summary>
    [Serializable]
    public class BComboBox:BField
    {
        public new string Label => LabelInfo.GetDesignerText();

        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsMedium = true};

        public string IsVisibleBindingPath { get; set; }

        public string IsDisabledBindingPath { get; set; }

        public LabelInfo LabelInfo { get; set; } = new LabelInfo();

        public string ValueMemberPath { get; set; }

        public string DisplayMemberPath { get; set; }

        public string SelectedValueBindingPath { get; set; }

        public BDataGrid DataGrid { get; set; } = new BDataGrid();

        internal string TypeScriptMethodNameOfGetGridColumns { get; set; }
    }
}