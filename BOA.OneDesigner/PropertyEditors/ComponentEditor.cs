using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.PropertyEditors
{
    class ComponentEditorModel
    {
        #region Public Properties
        public ComponentInfo Info                                     { get; set; }
        public bool          IsBoldVisible                            { get; set; }
        public bool          IsDisabledEditorVisible                  { get; set; }
        public bool          IsInfoTextVisible                        { get; set; }
        public bool          IsLLabelEditorVisible                    { get; set; }
        public bool          IsMaskVisible                            { get; set; }
        public bool          IsParamTypeVisible                       { get; set; }
        public bool          IsRowCountVisible                        { get; set; }
        public bool          IsSizeEditorVisible                      { get; set; }
        public bool          IsTextIntoVisible                        { get; set; }
        public bool          IsValueBindingPathEditorVisible          { get; set; }
        public bool          IsValueChangedOrchestrationMethodVisible { get; set; }
        public bool          IsVisibleEditorVisible                   { get; set; }
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
                    Info                                     = info,
                    IsSizeEditorVisible                      = info.Type.IsInput || info.Type.IsDivider || info.Type.IsBranchComponent || info.Type.IsParameterComponent || info.Type.IsAccountComponent || info.Type.IsLabel,
                    IsValueBindingPathEditorVisible          = info.Type.IsInput || info.Type.IsParameterComponent || info.Type.IsBranchComponent || info.Type.IsAccountComponent,
                    IsLLabelEditorVisible                    = info.Type.IsInput || info.Type.IsParameterComponent || info.Type.IsBranchComponent || info.Type.IsInformationText || info.Type.IsAccountComponent|| info.Type.IsButton,
                    IsParamTypeVisible                       = info.Type.IsParameterComponent,
                    IsInfoTextVisible                        = info.Type.IsInformationText,
                    IsVisibleEditorVisible                   = info.Type.IsInput || info.Type.IsParameterComponent || info.Type.IsBranchComponent || info.Type.IsAccountComponent,
                    IsDisabledEditorVisible                  = info.Type.IsInput || info.Type.IsParameterComponent || info.Type.IsBranchComponent || info.Type.IsAccountComponent,
                    IsValueChangedOrchestrationMethodVisible = info.Type.IsAccountComponent,
                    IsBoldVisible                            = info.Type.IsLabel,
                    IsTextIntoVisible                        = info.Type.IsLabel
                }
            }.LoadUI();
        }

        public void Delete()
        {
            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }

        public void OnIsBoldChanged()
        {
            Host.EventBus.Publish(EventBus.LabelChanged);
        }

        public void OnRowCountChanged()
        {
            Host.EventBus.Publish(EventBus.RowCountChanged);
        }

        public void OnValueBindingPathChanged()
        {
            var isStringProperty = Host.RequestIntellisenseData.RequestStringPropertyIntellisense.Contains(Model.Info.ValueBindingPath);
            if (isStringProperty)
            {
                Model.IsRowCountVisible = true;
                Model.IsMaskVisible     = true;
            }
            else
            {
                Model.IsRowCountVisible = false;
                Model.IsMaskVisible     = false;
            }

            foreach (var textBox in this.FindChildren<LabeledTextBox>())
            {
                textBox.GetBindingExpression(VisibilityProperty)?.UpdateTarget();
            }
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
            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.ValueBindingPath) + @"}', 
            Label       : 'Binding Path',
            TextChanged : '" + nameof(OnValueBindingPathChanged) + @"'
        }
        ,
        {
            ui          : 'LabelEditor',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsLLabelEditorVisible) + @"}',
            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.LabelTextInfo) + @"}'
        }
        ,
        {
            ui          : 'LabelEditor',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsInfoTextVisible) + @"}',
            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.InfoText) + @"}',
            Name        : 'infoTextEditor'
        }
        ,
        {
            ui          : 'LabelEditor',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsTextIntoVisible) + @"}',
            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.TextInto) + @"}'
        }
        ,
        {
            ui          : 'TextBox',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsParamTypeVisible) + @"}',
            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.ParamType) + @"}', 
            Label       : 'Param Type'
        }
        ,
        {
            ui          : 'SizeEditor',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsSizeEditorVisible) + @"}',
            Header      : 'Size', 
            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.SizeInfo) + @"}'
        }
        ,
         {
            ui                          : 'RequestIntellisenseTextBox', 
            ShowOnlyBooleanProperties   : true, 
            Text                        : '{Binding " + Model.AccessPathOf(m => m.Info.IsVisibleBindingPath) + @"}', 
            Label                       : 'Is Visible',
            IsVisible                   : '{Binding " + Model.AccessPathOf(m => m.IsVisibleEditorVisible) + @"}'
        }
        ,        
        {
            ui                          : 'RequestIntellisenseTextBox', 
            ShowOnlyBooleanProperties   : true, 
            Text                        : '{Binding " + Model.AccessPathOf(m => m.Info.IsDisabledBindingPath) + @"}', 
            Label                       : 'Is Disabled',
            IsVisible                   : '{Binding " + Model.AccessPathOf(m => m.IsDisabledEditorVisible) + @"}'
        }
        ,
        {
            ui                           : 'RequestIntellisenseTextBox',
            IsVisible                    : '{Binding " + Model.AccessPathOf(m => m.IsValueChangedOrchestrationMethodVisible) + @"}',
            ShowOnlyOrchestrationMethods : true, 
            Text                         : '{Binding " + Model.AccessPathOf(m => m.Info.ValueChangedOrchestrationMethod) + @"}', 
            Label                        : 'On Account Number Changed'
        }
        ,
        {   
            ui          : 'CheckBox',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsBoldVisible) + @"}',
            Content     : 'Is Bold', 
            IsChecked   : '{Binding " + Model.AccessPathOf(m => m.Info.IsBold) + @"}', 
            Checked     : '" + nameof(OnIsBoldChanged) + @"',
            Unchecked   : '" + nameof(OnIsBoldChanged) + @"'
        }
        ,
        {   
            ui          : 'LabeledTextBox', 
            Label       : 'Row Count', 
            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.RowCount) + @",Converter=WhiteStone.UI.Container.StringToNullableInt32Converter}',
            TextChanged : '" + nameof(OnRowCountChanged) + @"',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsRowCountVisible) + @"}'
        }
        ,
        {   
            ui          : 'LabeledTextBox', 
            Label       : 'Mask', 
            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.Mask) + @"}',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsMaskVisible) + @"}'                   
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
                if (frameworkElement != null)
                {
                    frameworkElement.Margin = new Thickness(5, 10, 5, 0);
                }
            }

            OnValueBindingPathChanged();

            return this;
        }
        #endregion
    }
}