using System;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.JsxElementModel
{
    /// <summary>
    ///     The data grid
    /// </summary>
    [Serializable]
    public class BComboBox:BField
    {
        public string Label => LabelInfo.GetDesignerText();

        public SizeInfo SizeInfo { get; set; } = new SizeInfo {IsMedium = true};

        public string IsVisibleBindingPath { get; set; }

        public string IsDisabledBindingPath { get; set; }

        public LabelInfo LabelInfo { get; set; } = new LabelInfo();

        public string ValueMemberPath { get; set; }

        public string DisplayMemberPath { get; set; }

        public string SelectedValueBindingPath { get; set; }

        public BDataGrid DataGrid { get; set; } = new BDataGrid {ParentIsComboBox = true};

        internal string TypeScriptMethodNameOfGetGridColumns { get; set; }


        public string ValueChangedOrchestrationMethod { get; set; }


        public bool IsMultiSelect { get; set; }

    }
}