using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
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
        
        {ui:'RequestIntellisenseTextBox',  " + nameof(RequestIntellisenseTextBox.ShowOnlyNotNullInt32Properties) + @":true,  Margin:5, Text:'{Binding " + nameof(Model.ActiveTabIndexBindingPath) + @"}', Label:'Active Tab Index Binding Path' },
        {ui:'Button', Text:'Add Tab',Click:'" + nameof(AddTab) + @"'},
        {ui:'Button', Text:'Delete',Click:'" + nameof(Delete) + @"'}
	]
}

");
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }

        public BTabBar Model => (BTabBar) DataContext;
        #endregion

        #region Public Methods
        public void Delete()
        {
            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }
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