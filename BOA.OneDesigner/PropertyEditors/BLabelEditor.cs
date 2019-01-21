using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BLabelEditor : StackPanel, IHostItem
    {
        #region Fields
        public LabelEditor _labelEditor;
        #endregion

        #region Constructors
        public BLabelEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[

		{ui:'LabelEditor',Name:'_labelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding " + nameof(Model.TextInto) + @"}'}
        
        

        

	]
}

");

            Loaded += (s, e) => { _labelEditor.Host = Host; };
        }
        #endregion

        #region Public Properties
        public Host   Host  { get; set; }
        public BLabel Model => (BLabel) DataContext;
        #endregion
    }
}