using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BCardEditor:StackPanel
    {


        public BCardEditor()
        {
            
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[  
		{ui:'LabelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding "+nameof(BCard.TitleInfo)+@"}'}
	]
}

");
        }
    }
}