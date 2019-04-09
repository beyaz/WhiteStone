using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class ResourceActionEditor : GroupBox
    {
        Aut_ResourceAction Model => (Aut_ResourceAction)DataContext;

        public ResourceCodeTextBox ResourceCodeTextBox;

        #region Constructors
        public void LoadUI()
        {
            this.LoadJson(@"

{
 Header:'Resource Action',
 Margin:5,
 Content:
 {
    ui:'StackPanel',
	    rows:[
		    {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true,    Margin:5, Text:'{Binding " + nameof(Model.IsVisibleBindingPath) + @"}',     Label:'Is Visible Binding Path' },
            {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true,    Margin:5, Text:'{Binding " + nameof(Model.IsEnableBindingPath) + @"}',      Label:'Is Enable Binding Path' },
            

            {
                ui:'TabControl',
                childs:[
                    {
                        ui: 'TabItem',
                        Header:'Simple',
                        Content:
                        {   
                            ui       : 'OrchestrationIntellisense',
                            Text     :'{Binding " + nameof(Model.OrchestrationMethodName) + @"}',
                            Label    : 'Orchestration Method Name',
                            ToolTip  : 'Upload the form to orchestration side when button clicked.',
                                             
                        }
                    }
                    ,
                    {
                        ui: 'TabItem',
                        Header:'Advanced',
                        Content:
                        {
                            ui      : 'TextBox',
                            Label   : 'Extension Method Name',
                            Text    : '{Binding " + nameof(Model.ExtensionMethodName) + @"}',
                            ToolTip : 'Manuel function yazarak handle etmek istenildiğinde kullanılmalıdır.\nÖrnek:showCustomerXInfo yazılıp extension dosyasında custom olarak implement edilebilir.'
                        }

                    }
                ]

            },

            {
                ui                  : 'ResourceCodeTextBox',   
                SelectedValue       : '{Binding " + Model.AccessPathOf(m => m.OpenFormWithResourceCode) + @"}',
                Name                : 'ResourceCodeTextBox',
                SelectedValuePath   : 'ResourceCode',
                DisplayMemberPath   : 'Name',
                Label               : 'Open Form With Resource Code',
                Margin              : 5
            }
            ,



		    {ui:'RequestIntellisenseTextBox', ShowOnlyClassProperties:true,      Margin:5, Text:'{Binding " + nameof(Model.OpenFormWithResourceCodeDataParameterBindingPath) + @"}',  Label:'Open Form With Data' }

	    ]
 }

}


");


            ResourceCodeTextBox.Text = Model?.OpenFormWithResourceCode;
        }
        #endregion

       

       
    }
}