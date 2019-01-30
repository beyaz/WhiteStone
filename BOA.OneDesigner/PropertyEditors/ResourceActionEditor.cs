using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class ResourceActionEditor : GroupBox
    {
        Aut_ResourceAction Model => (Aut_ResourceAction)DataContext;

        #region Constructors
        public ResourceActionEditor()
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
            {ui:'RequestIntellisenseTextBox', ShowOnlyOrchestrationMethods:true, Margin:5, Text:'{Binding " + nameof(Model.OrchestrationMethodName) + @"}',  Label:'Orchestration Method Name' },

            {ui:'ResourceCodeTextBox',                                           Margin:5, Text:'{Binding " + nameof(Model.OpenFormWithResourceCode) + @"}', Label:'Open Form With Resource Code'},
		    {ui:'RequestIntellisenseTextBox', ShowOnlyClassProperties:true,      Margin:5, Text:'{Binding " + nameof(Model.OpenFormWithResourceCodeDataParameterBindingPath) + @"}',  Label:'Open Form With Data' }

	    ]
 }

}


");
        }
        #endregion

       

       
    }
}