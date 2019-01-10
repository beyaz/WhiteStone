using System.Windows.Controls;
using BOA.Common.Helpers;
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
        #endregion

        #region Constructors
        public BCardEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[  
		{ui:'LabelEditor',Name:'_labelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding " + nameof(BCard.TitleInfo) + @"}'},
        
        
        {ui:'TextBox',Label:'Wide',MarginTop:10, Text:'{Binding " + Model.AccessPathOf(m => m.LayoutProps.Wide) + @", Converter=" + typeof(LayoutPropWideConverter).FullName + @"}', KeyUp:'FirePropertyChanged' },
        {ui:'TextBox',Label:'X',  MarginTop:10,  Text:'{Binding " + Model.AccessPathOf(m => m.LayoutProps.X) + @", Converter=" + typeof(LayoutPropWideConverter).FullName + @"}', KeyUp:'FirePropertyChanged' },

        {ui:'Button', Text:'Delete',Click:'" + nameof(Delete) + @"'}
	]
}

");

            Loaded += (s, e) => { _labelEditor.Host = Host; };
        }
        #endregion

        #region Public Properties
        public Host  Host  { get; set; }
        public BCard Model => (BCard) DataContext;
        #endregion

        #region Public Methods
        public void Delete()
        {
            Model.ShouldBeDelete = true;

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
        #endregion
    }
}