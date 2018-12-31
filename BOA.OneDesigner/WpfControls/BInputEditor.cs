﻿using System.Windows.Controls;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    class BInputEditor:StackPanel
    {
        public BInputEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[
		{ui:'RequestIntellisenseTextBox', Text:'{Binding BindingPath}', Label:'Binding Path' },
		{ui:'LabelEditor', MarginTop:10, DataContext:'{Binding LabelInfo}'}
	]
}

");
        }
    }
}