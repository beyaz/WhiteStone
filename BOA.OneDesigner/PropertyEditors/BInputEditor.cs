using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BInputEditor : StackPanel,IHostItem
    {
        public LabelEditor _labelEditor;
        #region Constructors
        public BInputEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[
		{ui:'RequestIntellisenseTextBox', Margin:5, Text:'{Binding " + nameof(BInput.BindingPath) + @"}', Label:'Binding Path' },
		{ui:'LabelEditor', Name:'"+nameof(_labelEditor)+@"', DataContext:'{Binding " + nameof(BInput.LabelInfo) + @"}'}
	]
}

");

            this.Loaded += (s, e) => { _labelEditor.Host = Host; };
        }
        #endregion

        public Host Host { get; set; }
    }
}