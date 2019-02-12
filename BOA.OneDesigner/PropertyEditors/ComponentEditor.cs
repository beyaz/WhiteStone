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
            ui          :'SizeEditor',
            IsVisible   :'{Binding " + Model.AccessPathOf(m => m.IsSizeEditorVisible) + @"}',
            Header      : 'Size', 
            MarginTop   : 10,
            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.SizeInfo) + @"}'
        }
        ,
        {
            ui          : 'RequestIntellisenseTextBox',
            IsVisible   :'{Binding " + Model.AccessPathOf(m => m.IsValueBindingPathEditorVisible) + @"}',
            MarginTop   : 10,
            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.ValueBindingPath) + @"}', 
            Label       : 'Binding Path' 
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
                    IsValueBindingPathEditorVisible = info.Type.IsBranchComponent
                }
            };
        }
        #endregion
    }
}