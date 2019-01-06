using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BInputEditor : StackPanel, IHostItem
    {
        #region Fields
        public LabelEditor _labelEditor;
        public SizeEditor _sizeEditor;
        #endregion

        #region Constructors
        public BInputEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[
		{ui:'RequestIntellisenseTextBox', Margin:5, Text:'{Binding " + nameof(BInput.BindingPath) + @"}', Label:'Binding Path' },
		{ui:'LabelEditor', Name:'" + nameof(_labelEditor) + @"', DataContext:'{Binding " + nameof(BInput.LabelInfo) + @"}'},
        {ui:'SizeEditor',Name:'" + nameof(_sizeEditor) + @"',   Header:'Size', MarginTop:10, DataContext:'{Binding " + nameof(BInput.SizeInfo) + @"}'},
        {ui:'Button', Text:'Delete',Click:'" + nameof(Delete) + @"'}
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
        public Host Host { get; set; }
        #endregion

        #region Properties
        BInput Model => (BInput) DataContext;
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