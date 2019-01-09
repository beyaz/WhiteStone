using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BTabBarPageEditor: StackPanel, IHostItem
    {
        #region Fields
        public LabelEditor _labelEditor;
        #endregion

        #region Constructors
        public BTabBarPageEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[
		{ui:'LabelEditor', Name:'" + nameof(_labelEditor) + @"', DataContext:'{Binding " + nameof(BTabBarPage.TitleInfo) + @"}'},
        {ui:'Button', Text:'Remove Tab',Click:'" + nameof(RemoveTab) + @"'}
	]
}

");

            Loaded += (s, e) => { _labelEditor.Host = Host; };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Properties
        BTabBarPage Model => (BTabBarPage) DataContext;
        #endregion

        #region Public Methods
        public void RemoveTab()
        {
            Model.ShouldRemove = true;

            Host.EventBus.Publish(EventBus.TabBarPageRemoved);
        }
        #endregion
    }
}