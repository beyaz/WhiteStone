using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BTabBarEditor : StackPanel, IHostItem
    {
        #region Constructors
        public BTabBarEditor()
        {
            this.LoadJson(@"
{ 
    Margin:10,
	Childs:[
        {ui:'Button', Text:'Add Tab',Click:'" + nameof(AddTab) + @"'}
	]
}

");
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }

        public BTabBar Model => (BTabBar) DataContext;
        #endregion

        #region Methods
        void AddTab()
        {
            Model.Items.Add(new BTabBarPage
            {
                TitleInfo = LabelInfoHelper.CreateNewLabelInfo("Page " + Model.Items.Count)
            });

            Host.EventBus.Publish(EventBus.OnComponentPropertyChanged);
        }
        #endregion
    }
}