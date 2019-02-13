using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class ComponentEditorModel
    {
        #region Public Properties
        public ComponentInfo Info                            { get; set; }
        public bool          IsDisabledEditorVisible         { get; set; }
        public bool          IsInfoTextVisible               { get; set; }
        public bool          IsLLabelEditorVisible           { get; set; }
        public bool          IsParamTypeVisible              { get; set; }
        public bool          IsSizeEditorVisible             { get; set; }
        public bool          IsValueBindingPathEditorVisible { get; set; }
        public bool          IsVisibleEditorVisible          { get; set; }
        #endregion
    }

    class ComponentEditor : StackPanel
    {
        #region Fields
        #pragma warning disable 649
        LabelEditor infoTextEditor;
        #pragma warning restore 649
        #endregion

        #region Constructors
        ComponentEditor()
        {
        }
        #endregion

        #region Public Properties
        public Host                 Host  { get; set; }
        public ComponentEditorModel Model => (ComponentEditorModel) DataContext;
        #endregion

        #region Public Methods
        public static ComponentEditor Create(Host host, ComponentInfo info)
        {
            return new ComponentEditor
            {
                Host = host,
                DataContext = new ComponentEditorModel
                {
                    Info                            = info,
                    IsSizeEditorVisible             = info.Type.IsDivider || info.Type.IsBranchComponent || info.Type.IsParameterComponent,
                    IsValueBindingPathEditorVisible = info.Type.IsParameterComponent || info.Type.IsBranchComponent,
                    IsLLabelEditorVisible           = info.Type.IsParameterComponent || info.Type.IsBranchComponent || info.Type.IsInformationText,
                    IsParamTypeVisible              = info.Type.IsParameterComponent,
                    IsInfoTextVisible               = info.Type.IsInformationText,
                    IsVisibleEditorVisible          = info.Type.IsParameterComponent || info.Type.IsBranchComponent,
                    IsDisabledEditorVisible         = info.Type.IsParameterComponent || info.Type.IsBranchComponent
                }
            }.LoadUI();
        }

        public void Delete()
        {
            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }
        #endregion

        #region Methods
        ComponentEditor LoadUI()
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
            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.LabelTextInfo) + @"}'
        }
        ,
        {
            ui          : 'LabelEditor',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsInfoTextVisible) + @"}',
            MarginTop   : 10,
            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.InfoText) + @"}',
            Name        : 'infoTextEditor'
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
        ,
         {
            ui                          : 'RequestIntellisenseTextBox', 
            ShowOnlyBooleanProperties   : true, 
            Margin                      : 5, 
            Text                        : '{Binding " + Model.AccessPathOf(m => m.Info.IsVisibleBindingPath) + @"}', 
            Label                       : 'Is Visible',
            IsVisible                   : '{Binding " + Model.AccessPathOf(m => m.IsVisibleEditorVisible) + @"}'
        }
        ,        
        {
            ui                          : 'RequestIntellisenseTextBox', 
            ShowOnlyBooleanProperties   : true, 
            Margin                      : 5, 
            Text                        : '{Binding " + Model.AccessPathOf(m => m.Info.IsDisabledBindingPath) + @"}', 
            Label                       : 'Is Disabled',
            IsVisible                   : '{Binding " + Model.AccessPathOf(m => m.IsDisabledEditorVisible) + @"}'
        }
        ,        
        {
            ui      : 'Button',
            Text    : 'Delete',
            Click   : '" + nameof(Delete) + @"'
        }

    ]
}

";
            this.LoadJson(template);

            infoTextEditor.Header = "Info Text";


            foreach (var child in Children)
            {
                var frameworkElement = child as FrameworkElement;
                if (frameworkElement!= null)
                {
                    frameworkElement.Margin = new Thickness(5,10,5,0);
                }
            }

            return this;
        }
        #endregion
    }
}