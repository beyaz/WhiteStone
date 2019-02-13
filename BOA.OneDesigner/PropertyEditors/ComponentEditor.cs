using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class ComponentEditorModel
    {
        #region Public Properties
        public ComponentInfo Info                            { get; set; }
        public bool          IsLLabelEditorVisible           { get; set; }
        public bool          IsParamTypeVisible              { get; set; }
        public bool          IsSizeEditorVisible             { get; set; }
        public bool          IsValueBindingPathEditorVisible { get; set; }
        #endregion
    }

    class ComponentEditor : StackPanel
    {
        #region Constructors
        public ComponentEditor()
        {
            var template = @"
{
    Childs:
    [
        {
            ui          : 'RequestIntellisenseTextBox',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsValueBindingPathEditorVisible) + @"}',
            MarginTop   : 10,
            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.ValueBindingPath) + @"}', 
            Label       : 'Binding Path' 
        }
        ,
        {
            ui          : 'LabelEditor',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsLLabelEditorVisible) + @"}',
            MarginTop   : 10,
            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.LabelTextInfo) + @"}', 
            Label       : 'Binding Path' 
        }
        ,
        {
            ui          : 'TextBox',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsParamTypeVisible) + @"}',
            MarginTop   : 10,
            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.ParamType) + @"}', 
            Label       : 'Param Type'
        }
        ,
        {
            ui          : 'SizeEditor',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsSizeEditorVisible) + @"}',
            Header      : 'Size', 
            MarginTop   : 10,
            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.SizeInfo) + @"}'
        }
    ]
}

";
            this.LoadJson(template);
        }
        #endregion

        #region Public Properties
        public ComponentEditorModel Model => (ComponentEditorModel) DataContext;
        #endregion

        #region Public Methods
        public static ComponentEditor Create(ComponentInfo info)
        {
            return new ComponentEditor
            {
                DataContext = new ComponentEditorModel
                {
                    Info                            = info,
                    IsSizeEditorVisible             = info.Type.IsDivider || info.Type.IsBranchComponent,
                    IsValueBindingPathEditorVisible = info.Type.IsBranchComponent,
                    IsLLabelEditorVisible           = info.Type.IsParameterComponent || info.Type.IsBranchComponent,
                    IsParamTypeVisible              = info.Type.IsParameterComponent
                }
            };
        }
        #endregion
    }
}