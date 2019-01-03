﻿using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BCardEditor:StackPanel,IHostItem
    {
       
        public LabelEditor _labelEditor;

        public BCardEditor()
        {
            
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[  
		{ui:'LabelEditor',Name:'_labelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding "+nameof(BCard.TitleInfo)+@"}'}
	]
}

");

            this.Loaded += (s, e) => { _labelEditor.Host = Host; };


        }

        public Host Host { get; set; }
    }
}