using System;
using System.Linq;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;
using MahApps.Metro.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.PropertyEditors
{
    [Serializable]
    public class ActionInfoEditorModel
    {
        #region Public Properties
        public bool       CustomHandlerFunctionExpanderIsExpanded { get; set; }

        public bool AdvancedIsExpanded { get; set; }

        public ActionInfo Info                                    { get; set; }

        public bool OrchestrationCallExpanderIsExpanded { get; set; }
        #endregion
    }

    class ActionInfoEditor : GroupBox
    {
        #region Fields
        public ResourceCodeTextBox _resourceCodeTextBox;
        #endregion

        #region Public Properties
        public Host                  Host  { get; set; }
        public ActionInfoEditorModel Model => (ActionInfoEditorModel) DataContext;
        #endregion

        #region Public Methods
        public void Load(Host host, ActionInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Host = host;

            var model = new ActionInfoEditorModel
            {
                Info = info
            };


           

            

            if (info.OpenFormWithResourceCode.HasValue() || info.YesNoQuestion.HasValue())
            {
                model.AdvancedIsExpanded = true;
            }
            else if (info.ExtensionMethodName.HasValue())
            {
                model.CustomHandlerFunctionExpanderIsExpanded = true;
            }
            else if (info.OrchestrationMethodName.HasValue())
            {
                model.OrchestrationCallExpanderIsExpanded = true;
            }
            


            DataContext = model;

            Content = null;

            LoadUI();
        }
        #endregion
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
        #region Methods
        void LoadUI()
        {
            var template = @"
{
    Content:
    {
        ui  : 'StackPanel',
        Rows:
        [
            {
                ui          : 'Expander',
                IsExpanded  : '{Binding " + Model.AccessPathOf(m => m.OrchestrationCallExpanderIsExpanded) + @",Mode=OneWay}',
                Header      : 'Simple',
                Content     :
                {   
                    ui      : 'OrchestrationIntellisense',
                    Text    : '{Binding " + Model.AccessPathOf(m => m.Info.OrchestrationMethodName) + @"}',
                    Label   : 'Orchestration Call'
                }
            }
            ,
            {
                ui          : 'Expander',
                IsExpanded  : '{Binding " + Model.AccessPathOf(m => m.CustomHandlerFunctionExpanderIsExpanded) + @",Mode=OneWay}',
                Header      : 'Custom Handler',
                Content     :
                {
                    ui      : 'TextBox',
                    Label   : 'Extension Method Name in ...Extension.tsx file',
                    Text    : '{Binding " + Model.AccessPathOf(m => m.Info.ExtensionMethodName) + @"}',
                    ToolTip : 'Manuel function yazarak handle etmek istenildiğinde kullanılmalıdır.\nÖrnek:showCustomerXInfo yazılıp extension dosyasında custom olarak implement edilebilir.'
                }
            }
            ,
            {
                ui          : 'Expander',
                IsExpanded  : '{Binding " + Model.AccessPathOf(m => m.AdvancedIsExpanded) + @",Mode=OneWay}',
                Header      : 'Advanced',
                Content     :
                {
                     ui	    :'StackPanel',
                    Spacing : 10,
		            rows    :
                    [
                        {   
                            ui      : 'OrchestrationIntellisense',
                            Text    : '{Binding " + Model.AccessPathOf(m => m.Info.OrchestrationMethodName) + @"}',
                            Label   : 'Orchestration Call'
                        }
                        ,
                        {
                            ui                          : 'RequestIntellisenseTextBox', 
                            ShowOnlyBooleanProperties   : true, 
                            Text                        : '{Binding " + Model.AccessPathOf(m => m.Info.YesNoQuestionCondition) + @"}', 
                            Label                       : 'Yes - No Question Condition',
                            ToolTip                     : 'Eğer burada verilen binding path deki değer true ise soruyu sor değil ise sorma.'
                        }
                        ,
                        {
                            ui          : 'LabelEditor',
                            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.YesNoQuestionInfo) + @"}',
                            Header      : 'Yes - No Question',
                            ToolTip     : 'Kullanıcıya sorulacak Yes - No sorusun metni.'
                        }
                        ,
                        {   
                            ui          : 'OrchestrationIntellisense',
                            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.YesNoQuestionAfterYesOrchestrationCall) + @"}',
                            Label       : 'Orchestration Call On Yes',
                            ToolTip     : 'Kullanıcı Yes seçeneğini seçer ise gideceği orchestration method ismi.'
                        }
                        ,
                        {
                            ui                          : 'RequestIntellisenseTextBox', 
                            ShowOnlyBooleanProperties   : true, 
                            Text                        : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCodeCondition) + @"}', 
                            Label                       : 'Open Form if this is true',
                            ToolTip                     : 'Eğer burada verilen binding path deki değer true ise formu aç de değil ise açma.'
                        }
                        ,
                        {
                            ui                  : 'ResourceCodeTextBox',
                            Name                : '_resourceCodeTextBox',
                            SelectedValuePath   : 'ResourceCode',
                            DisplayMemberPath   : 'Description',
                            SelectedValue       : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCode) + @"}',
                            Label               : 'Resource Code'
                        }
                        ,
		                {
                            ui                      : 'RequestIntellisenseTextBox', 
                            ShowOnlyClassProperties : true,      
                            Text                    : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCodeDataParameterBindingPath) + @"}',  
                            Label                   : 'Data Parameter'
                        }
                        ,
                        {   
                            ui          : 'CheckBox',
                            Content     : 'Open In Dialog Box', 
                            IsChecked   : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCodeIsInDialogBox) + @"}', 
                            Checked     : '" + nameof(OnIsInDialogBoxChanged) + @"',
                            Unchecked   : '" + nameof(OnIsInDialogBoxChanged) + @"',
                            ToolTip     : 'Seçili ise popup olarak açılır.'
                        }
                        ,
                        {
                            ui          : 'LabelEditor',
                            DataContext : '{Binding " + Model.AccessPathOf(m => m.Info.DialogTitleInfo) + @"}',
                            Header      : 'Title',
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
                        ,
                        {   
                            ui          : 'OrchestrationIntellisense',
                            Text        : '{Binding " + Model.AccessPathOf(m => m.Info.OrchestrationMethodOnDialogResponseIsOK) + @"}',
                            Label       : 'Orchestration Call On Dialog Response Is OK',
                            ToolTip     : 'Dialog response OK olduğu durumda gideceği orch metodu.',
                            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.OpenFormWithResourceCodeIsInDialogBox) + @"}'
                        }
                    ]
                }
            }
        ]
    }
}

";
            this.LoadJson(template);

            if (Model.Info.OpenFormWithResourceCode.HasValue())
            {
                _resourceCodeTextBox.Text = ResourceCodeTextBox.Items.FirstOrDefault(x => x.ResourceCode == Model.Info.OpenFormWithResourceCode)?.Description;
            }
        }
        #endregion
    }
}