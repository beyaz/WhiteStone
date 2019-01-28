using System;
using System.Collections.Generic;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.JsxElementModel
{

    [Serializable]
    public class BDataGrid:BField
    {

        public bool ParentIsComboBox { get; set; }

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
}