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

            Loaded += (s, e) =>
            {
                Refresh();
                GridContainer.Visibility = Visibility.Collapsed;
            };
        }
        #endregion

        #region Public Properties
        public Host Host                      { get; set; }
        public bool IsEnteredDropLocationMode { get; set; }
        public bool IsInToolbox               { get; set; }

        public BComboBox Model => (BComboBox) DataContext;

        public SizeInfo SizeInfo => Model.SizeInfo;
        #endregion

        #region Public Methods
        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            if (IsInToolbox)
            {
                _label.Text = "ComboBox";
                return;
            }

            Host.DeAttachToEventBus(GridContainer.Children);

            Model.DataGrid.ParentIsComboBox = true; // TODO ilerde kaldırılmalı binary serilize olsun diye
            var bDataGridInfoWpf = Host.CreateBDataGridInfoWpf(Model.DataGrid);

            Host.AttachToEventBus(bDataGridInfoWpf, this);

            Host.DragHelper.MakeDraggable(bDataGridInfoWpf);

            GridContainer.Children.Clear();
            GridContainer.Children.Add(bDataGridInfoWpf);
        }
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

        void Show_data_grid_is_this_component_selected()
        {
            if (Host.SelectedElement == this || Host.SelectedElement.FindParent<BComboBoxInWpf>() == this)
            {
                GridContainer.Visibility = Visibility.Visible;
            }
            else
            {
                GridContainer.Visibility = Visibility.Collapsed;
            }
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

            Host.EventBus.Subscribe(EventBus.OnDragElementSelected, Show_data_grid_is_this_component_selected);
            Host.EventBus.Subscribe(EventBus.LabelChanged, UpdateLabel);
            //Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);
        }

        public void DeAttachToEventBus()
        {
            if (IsInToolbox)
            {
                return;
            }

            OnDeAttachToEventBus?.Invoke();

            Host.EventBus.UnSubscribe(EventBus.OnDragElementSelected, Show_data_grid_is_this_component_selected);
            Host.EventBus.UnSubscribe(EventBus.LabelChanged, UpdateLabel);
            //Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);
        }
        #endregion
    }
}