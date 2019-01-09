using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BTabBarEditor : StackPanel, IHostItem
    {

        public BTabBar Model => (BTabBar) DataContext;

        public void Delete()
        {
            Model.Container.RemoveItem(Model);

            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }

        public void FirePropertyChanged()
        {
            if (Host == null)
            {
                return;
                // TODO:??
                // throw Error.InvalidOperation();
            }

            Host.EventBus.Publish(EventBus.OnComponentPropertyChanged);
        }

        public Host Host { get; set; }

        #region Fields
        public LabelEditor _labelEditor;
        #endregion


        public BTabBarEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[  
		{ui:'LabelEditor',Name:'_labelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding " + nameof(BCard.TitleInfo) + @"}'},
        
        
        

        {ui:'Button', Text:'Delete',Click:'" + nameof(Delete) + @"'}
	]
}

");

            Loaded += (s, e) => { _labelEditor.Host = Host; };
        }
    }
}