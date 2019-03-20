using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using CustomUIMarkupLanguage.UIBuilding;
using MahApps.Metro.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.PropertyEditors
{
    class ComponentEditorModel
    {
        #region Public Properties
        public ComponentInfo Info                                      { get; set; }
        public bool          IsBoldVisible                             { get; set; }
        public bool          IsButtonClickedOrchestrationMethodVisible { get; set; }
        public bool          IsDisabledEditorVisible                   { get; set; }
        public bool          IsInDialogBoxIsVisible                    { get; set; }
        public bool          IsInfoTextVisible                         { get; set; }
        public bool          IsLLabelEditorVisible                     { get; set; }
        public bool          IsMaskVisible                             { get; set; }
        public bool          IsMaxLengthVisible                        { get; set; }
        public bool          IsParamTypeVisible                        { get; set; }
        public bool          IsRowCountVisible                         { get; set; }

        public bool   IsSizeEditorVisible                                       { get; set; }
        public bool   IsTextIntoVisible                                         { get; set; }
        public bool   IsValueBindingPathEditorVisible                           { get; set; }
        public bool   IsValueChangedOrchestrationMethodVisible                  { get; set; }
        public bool   IsVisibleEditorVisible                                    { get; set; }
        public bool   OpenFormWithResourceCodeDataParameterBindingPathIsVisible { get; set; }
        public bool   OpenFormWithResourceCodeIsVisible                         { get; set; }
        public string ValueBindingPathLabel                                     { get; set; }
        #endregion
    }

    class ComponentEditor : StackPanel
    {
        #region Fields
        public ResourceCodeTextBox2 _resourceCodeTextBox;
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
            var dataContext = new ComponentEditorModel
            {
                Info                                                      = info,
                IsSizeEditorVisible                                       = info.Type.IsInput || info.Type.IsDivider || info.Type.IsBranchComponent || info.Type.IsParameterComponent || info.Type.IsAccountComponent || info.Type.IsLabel || info.Type.IsInformationText,
                IsValueBindingPathEditorVisible                           = info.Type.IsInput || info.Type.IsParameterComponent || info.Type.IsBranchComponent || info.Type.IsAccountComponent || info.Type.IsCreditCardComponent,
                IsLLabelEditorVisible                                     = info.Type.IsInput || info.Type.IsParameterComponent || info.Type.IsBranchComponent || info.Type.IsInformationText || info.Type.IsAccountComponent,
                IsParamTypeVisible                                        = info.Type.IsParameterComponent,
                IsInfoTextVisible                                         = info.Type.IsInformationText,
                IsVisibleEditorVisible                                    = info.Type.IsInput || info.Type.IsParameterComponent || info.Type.IsBranchComponent || info.Type.IsAccountComponent || info.Type.IsButton,
                IsDisabledEditorVisible                                   = info.Type.IsInput || info.Type.IsParameterComponent || info.Type.IsBranchComponent || info.Type.IsAccountComponent || info.Type.IsButton,
                IsBoldVisible                                             = info.Type.IsLabel,
                IsTextIntoVisible                                         = info.Type.IsLabel || info.Type.IsButton,
                IsButtonClickedOrchestrationMethodVisible                 = info.Type.IsButton,
                OpenFormWithResourceCodeIsVisible                         = info.Type.IsButton,
                OpenFormWithResourceCodeDataParameterBindingPathIsVisible = info.Type.IsButton,
                ValueBindingPathLabel                                     = "Value Binding Path"
            };

            if (info.Type.IsCreditCardComponent)
            {
                dataContext.ValueBindingPathLabel = "Açık Kart No";
            }

            var componentEditor = new ComponentEditor
            {
                Host        = host,
                DataContext = dataContext
            };
            componentEditor.LoadUI();

            return componentEditor;
        }

        public void Delete()
        {
            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }

        public void OnIsBoldChanged()
        {
            Host.EventBus.Publish(EventBus.LabelChanged);
        }

        public void OnIsInDialogBoxChanged()
        {
            if (Model.Info.CssOfDialog.IsNullOrWhiteSpace())
            {
                Model.Info.CssOfDialog = "{width:'60%' , height:'50%' }";
            }

            foreach (var element in this.FindChildren<OrchestrationIntellisense>())
            {
                element.GetBindingExpression(VisibilityProperty)?.UpdateTarget();
            }

            foreach (var element in this.FindChildren<LabeledTextBox>())
            {
                element.GetBindingExpression(LabeledTextBox.TextProperty)?.UpdateTarget();
            }
        }

        public void OnRowCountChanged()
        {
            Host.EventBus.Publish(EventBus.RowCountChanged);
        }

        public void OnValueBindingPathChanged()
        {
            if (Model.Info.Type.IsCreditCardComponent == false)
            {
                var isStringProperty = Host.RequestIntellisenseData.RequestStringPropertyIntellisense.Contains(Model.Info.ValueBindingPath);
                if (isStringProperty)
                {
                    Model.IsMaxLengthVisible = true;
                    Model.IsRowCountVisible  = true;
                    Model.IsMaskVisible      = true;
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

            Model.IsValueChangedOrchestrationMethodVisible = CanShowValueChangedEventInput();

            foreach (var element in this.FindChildren<OrchestrationIntellisense>())
            {
                element.GetBindingExpression(VisibilityProperty)?.UpdateTarget();
            }
        }
        #endregion

        #region Methods
        bool CanShowValueChangedEventInput()
        {
            if (Model.Info.Type.IsAccountComponent)
            {
                return true;
            }

            if (Model.Info.Type.IsCreditCardComponent)
            {
                return true;
            }

            var isBooleanProperty = Host.RequestIntellisenseData.RequestBooleanPropertyIntellisense.Contains(Model.Info.ValueBindingPath);
            if (isBooleanProperty)
            {
                return true;
            }

            return false;
        }

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
            Label       : '{Binding " + Model.AccessPathOf(m => m.ValueBindingPathLabel) + @"}', 
            TextChanged : '" + nameof(OnValueBindingPathChanged) + @"'
        }
        ,
        {
            ui          : 'RequestIntellisenseTextBox',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsAccountComponent) + @"}',
            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.AccountSuffixBindingPath) + @"}', 
            Label       : 'Account Suffix Binding Path'
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
            ui      : 'GroupBox',
            Header  : 'Visual',
            Content :
            {
                ui      : 'StackPanel',
                Childs  : 
                [
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
                        ui          : 'LabeledTextBox', 
                        Label       : 'Max Length', 
                        Text        : '{Binding " + Model.AccessPathOf(m => m.Info.MaxLength) + @",Converter=WhiteStone.UI.Container.StringToNullableInt32Converter}',
                        IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsMaxLengthVisible) + @"}'                   
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
                        ui          : 'CheckBox',
                        IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsBoldVisible) + @"}',
                        Content     : 'Is Bold', 
                        IsChecked   : '{Binding " + Model.AccessPathOf(m => m.Info.IsBold) + @"}', 
                        Checked     : '" + nameof(OnIsBoldChanged) + @"',
                        Unchecked   : '" + nameof(OnIsBoldChanged) + @"'
                    } 
                ]
            }
        }
        ,
        {
            ui      : 'GroupBox',
            Header  : 'Events',
            Content :
            {
                ui: 'StackPanel',
                Childs:
                [
                    {
                        ui                           : 'OrchestrationIntellisense',
                        IsVisible                    : '{Binding " + Model.AccessPathOf(m => m.IsValueChangedOrchestrationMethodVisible) + @"}',
                        Text                         : '{Binding " + Model.AccessPathOf(m => m.Info.ValueChangedOrchestrationMethod) + @"}', 
                        Label                        : 'On Value Changed'
                    }               
                    ,
                    {   
                        ui                          : 'OrchestrationIntellisense',
                        Text                        : '{Binding " + Model.AccessPathOf(m => m.Info.ButtonClickedOrchestrationMethod) + @"}',
                        Label                       : 'On Value Changed (Goto Orch)',
                        IsVisible                   : '{Binding " + Model.AccessPathOf(m => m.IsButtonClickedOrchestrationMethodVisible) + @"}'                   
                    }
                    ,
                    {
                        ui                  : 'ResourceCodeTextBox',
Name:'_resourceCodeTextBox',
                        SelectedValuePath   : 'ResourceCode',
                        DisplayMemberPath   : 'Description',
                        SelectedValue       : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCode) + @"}',
                        Label               : 'Open Form With Resource Code',
                        IsVisible           : '{Binding " + Model.AccessPathOf(m => m.OpenFormWithResourceCodeIsVisible) + @"}'
                    }
                    ,
		            {
                        ui                      : 'RequestIntellisenseTextBox', 
                        ShowOnlyClassProperties : true,      
                        Text                    : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCodeDataParameterBindingPath) + @"}',  
                        Label                   : 'Open Form With Data',
                        IsVisible               : '{Binding " + Model.AccessPathOf(m => m.OpenFormWithResourceCodeDataParameterBindingPathIsVisible) + @"}'
                    }
                    ,
                    {   
                        ui          : 'CheckBox',
                        IsVisible   : '{Binding " + Model.AccessPathOf(m => m.OpenFormWithResourceCodeIsVisible) + @"}',
                        Content     : 'Open In Dialog Box', 
                        IsChecked   : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCodeIsInDialogBox) + @"}', 
                        Checked     : '" + nameof(OnIsInDialogBoxChanged) + @"',
                        Unchecked   : '" + nameof(OnIsInDialogBoxChanged) + @"',
                        ToolTip     : 'Seçili ise popup olarak açılır.'
                    }
                    ,
                    {   
                        ui          : 'OrchestrationIntellisense',
                        Text        : '{Binding " + Model.AccessPathOf(m => m.Info.OrchestrationMethodOnDialogResponseIsOK) + @"}',
                        Label       : 'On dialog response is OK',
                        ToolTip     : 'Dialog response OK olduğu durumda gideceği orch metodu.',
                        IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCodeIsInDialogBox) + @"}'                   
                    }
                    ,
                    {   
                        ui          : 'TextBox',
                        Text        : '{Binding " + Model.AccessPathOf(m => m.Info.CssOfDialog) + @"}',
                        Label       : 'Dialog Style',
                        ToolTip     : " + "\"Açılacak popup'ın css bilgisi. Örn:{width:'50%' , height:'50%'}\"" + @",
                        IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCodeIsInDialogBox) + @"}'                   
                    }
                ]
            }
        }        
        ,
        {
            ui      : 'Button',
            Text    : 'Delete',
            Margin  : 20,
            Click   : '" + nameof(Delete) + @"'
        }

    ]
}

";
            this.LoadJson(template);

            _resourceCodeTextBox.Text = Model?.Info?.OpenFormWithResourceCode;

            infoTextEditor.Header = "Info Text";

            foreach (var child in Children)
            {
                var frameworkElement = child as FrameworkElement;
                if (frameworkElement != null)
                {
                    var zeroMargin = new Thickness(0);

                    if (frameworkElement.Margin == zeroMargin)
                    {
                        frameworkElement.Margin = new Thickness(5, 15, 5, 0);
                    }
                }
            }

            OnValueBindingPathChanged();

            return this;
        }
        #endregion
    }
}