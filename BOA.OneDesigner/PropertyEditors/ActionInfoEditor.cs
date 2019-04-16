using System;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    [Serializable]
    public class ActionInfoEditorModel
    {
        #region Public Properties
        public bool       CustomHandlerFunctionExpanderIsExpanded { get; set; }
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
            Host = host;

            var model = new ActionInfoEditorModel
            {
                Info = info
            };

            DataContext = model;

            Content = null;

            LoadUI();
        }
        #endregion

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
                    Label   : 'On Click Custom Handler (Extension Method Name)',
                    Text    : '{Binding " + Model.AccessPathOf(m => m.Info.ExtensionMethodName) + @"}',
                    ToolTip : 'Manuel function yazarak handle etmek istenildiğinde kullanılmalıdır.\nÖrnek:showCustomerXInfo yazılıp extension dosyasında custom olarak implement edilebilir.'
                }
            }
        ]
    }
}

";
            this.LoadJson(template);
        }
        #endregion
    }
}