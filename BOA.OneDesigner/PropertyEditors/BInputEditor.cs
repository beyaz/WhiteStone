using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BInputEditor : StackPanel
    {
        #region Constructors
        public BInputEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[
		{ui:'RequestIntellisenseTextBox', Margin:5, Text:'{Binding " + nameof(BInput.BindingPath) + @"}', Label:'Binding Path' },
		{ui:'LabelEditor', DataContext:'{Binding " + nameof(BInput.LabelInfo) + @"}'}
	]
}

");
        }
        #endregion
    }
}