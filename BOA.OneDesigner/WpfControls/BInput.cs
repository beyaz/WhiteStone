using System.Windows.Controls;
using System.Windows.Input;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    public class BInputWpf : Grid
    {
        #region Constructors
        public BInputWpf()
        {
            EventBus.Subscribe(EventBus.OnComponentPropertyChanged, RefreshDataContext);

            MouseEnter += BInput_MouseEnter;
            MouseLeave += BInput_MouseLeave;

            this.LoadJson(@"
{
    Margin:10,
	rows:
	[
		{view:'TextBlock', Text:'{Binding " + nameof(BInput.Label) + @",       Mode = OneWay}', MarginBottom:5, IsBold:true},
        {view:'TextBox',   Text:'{Binding " + nameof(BInput.BindingPath) + @", Mode = OneWay}' , IsReadOnly:true}        
	]
	
}");
        }
        #endregion

        #region Public Properties
        public BInput Data => (BInput) DataContext;
        #endregion

        #region Methods
        void BInput_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        void BInput_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        
        bool IsRefreshingDataContext;

        void RefreshDataContext()
        {
            // OnPropertyChanged(new DependencyPropertyChangedEventArgs(DataContextProperty, DataContext, DataContext));

            if (IsRefreshingDataContext)
            {
                return;
            }

            IsRefreshingDataContext = true;

            var dataContext = DataContext;
            DataContext = null;
            DataContext = dataContext;

            
            IsRefreshingDataContext = false;
        }
        #endregion
    }
}