using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BCardEditor : StackPanel, IHostItem
    {
        #region Fields
        public LabelEditor _labelEditor;
        public SizeEditor  _sizeEditor;
        #endregion

        #region Constructors
        public BCardEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[  
		{ui:'LabelEditor',Name:'_labelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding " + nameof(BCard.TitleInfo) + @"}'},
        {ui:'SizeEditor',Name:'_sizeEditor',   Header:'Size', MarginTop:10, DataContext:'{Binding " + nameof(BCard.SizeInfo) + @"}'},
        {ui:'Button', Text:'Delete',Click:'" + nameof(Delete) + @"'}
	]
}

");

            Loaded += (s, e) =>
            {
                _labelEditor.Host = Host;
                _sizeEditor.Host  = Host;
            };
        }
        #endregion

        #region Public Properties
        public Host  Host  { get; set; }
        public BCard Model => (BCard) DataContext;
        #endregion

        #region Public Methods
        public void Delete()
        {
            Model.Container.RemoveItem(Model);

            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }
        #endregion
    }
}