using System.Windows.Controls;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    class BCardEditor:StackPanel
    {
        public BCardEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[  
		{ui:'LabelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding TitleInfo}'}
	]
}

");
        }
    }
}