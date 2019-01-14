using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    class BComboBoxInWpf : StackPanel, IHostItem, ISupportSizeInfo, IEventBusListener
    {
        #region Fields
        public TextBox   _bindingPath;
        public TextBlock _label;

        public StackPanel GridContainer;
        #endregion

        #region Constructors
        public BComboBoxInWpf()
        {
            MouseEnter += BInput_MouseEnter;
            MouseLeave += BInput_MouseLeave;

            this.LoadJson(@"
{
    Margin:10,
    Childrens:[
        {
            ui:'Grid',
	        rows:
	        [
		        {view:'TextBlock', Name:'_label',        Text:'{Binding " + nameof(BComboBox.Label) + @",       Mode = OneWay}', MarginBottom:5, IsBold:true},
                {view:'TextBox',   Name:'_bindingPath',  Text:'{Binding " + nameof(BComboBox.SelectedValueBindingPath) + @", Mode = OneWay}' , IsReadOnly:true}        
	        ]
	        
        },
        {ui:'StackPanel',Name:'GridContainer'}

    ]

}


");
        }
        #endregion

        #region Public Properties
        public Host Host                      { get; set; }
        public bool IsEnteredDropLocationMode { get; set; }
        public bool IsInToolbox               { get; set; }

        public BComboBox Model => (BComboBox) DataContext;

        public SizeInfo SizeInfo { get; } = new SizeInfo {IsMedium = true};
        #endregion

        #region Public Methods
        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            if (IsInToolbox)
            {
                return;
            }

            Host.DeAttachToEventBus(GridContainer.Children);

            var bDataGridInfoWpf = Host.CreateBDataGridInfoWpf(Model.DataGrid);

            GridContainer.Children.Clear();
            GridContainer.Children.Add(bDataGridInfoWpf);
        }
        #endregion

        #region Methods
        void BInput_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor                   = Cursors.Hand;
            GridContainer.Visibility = Visibility.Visible;
        }

        void BInput_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor                   = Cursors.Arrow;
            GridContainer.Visibility = Visibility.Collapsed;
        }

        void UpdateBindingPath()
        {
            _bindingPath.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }

        void UpdateLabel()
        {
            _label.GetBindingExpression(TextBlock.TextProperty)?.UpdateTarget();
        }
        #endregion

        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            if (IsInToolbox)
            {
                return;
            }

            OnAttachToEventBus?.Invoke();

            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);
        }

        public void DeAttachToEventBus()
        {
            if (IsInToolbox)
            {
                return;
            }

            OnDeAttachToEventBus?.Invoke();

            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);
        }
        #endregion
    }
}