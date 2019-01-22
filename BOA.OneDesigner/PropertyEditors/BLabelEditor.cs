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
        public SizeEditor _sizeEditor;
        #endregion

        #region Constructors
        public BLabelEditor()
        {

            
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[

		{ui:'LabelEditor',Name:'_labelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding " + nameof(Model.TextInto) + @"}'},
        {ui:'SizeEditor',Name:'" + nameof(_sizeEditor) + @"',   Header:'Size', MarginTop:10, DataContext:'{Binding " + nameof(Model.SizeInfo) + @"}'},
        {   
            ui       :'CheckBox', 
            Content  :'Is Bold', 
            MarginTop: 10, 
            IsChecked: '{Binding " + nameof(Model.IsBold) + @"}', 
            Checked  : '"+nameof(OnIsBoldChanged)+@"',
            Unchecked: '"+nameof(OnIsBoldChanged)+@"'
        }
        
        

        

	]
}

");

            Loaded += (s, e) =>
            {
                _labelEditor.Host = Host;
                _sizeEditor.Host = Host;
            };
        }
        #endregion

        #region Public Properties
        public Host   Host  { get; set; }
        public BLabel Model => (BLabel) DataContext;
        #endregion

        public void OnIsBoldChanged()
        {
            if (!IsLoaded)
            {
                return;
            }
            Host.EventBus.Publish(EventBus.LabelChanged);
        }

    }
}