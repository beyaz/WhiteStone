using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class ResourceActionEditor : GroupBox
    {
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
		    {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true, Margin:5, Text:'{Binding " + nameof(Aut_ResourceAction.IsEnabledBindingPath) + @"}', Label:'Is Enabled Binding Path' }
		    
	    ]
 }

}


");
        }
        #endregion

       

       
    }
}