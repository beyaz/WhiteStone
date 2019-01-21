using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     Interaction logic for BDataGridColumnWpf.xaml
    /// </summary>
    public partial class BDataGridColumnWpf : IHostItem,IEventBusListener
    {
        #region Constructors
        public BDataGridColumnWpf(BDataGridColumnInfo dataContext, Host host, BDataGridInfoWpf bDataGridInfoWpf)
        {
            DataContext      = dataContext;
            Host             = host;
            BDataGridInfoWpf = bDataGridInfoWpf;

            InitializeComponent();
            BorderThickness = new Thickness(1);
            BorderBrush     = Brushes.DarkBlue;

         

            MouseEnter += BInput_MouseEnter;
            MouseLeave += BInput_MouseLeave;
        }
        #endregion

        #region Public Properties
        public BDataGridInfoWpf    BDataGridInfoWpf { get; }
        public Host                Host             { get; set; }
        public BDataGridColumnInfo Model            => (BDataGridColumnInfo) DataContext;
        #endregion

        #region Methods
       

        void BInput_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor      = Cursors.Hand;
            BorderBrush = Brushes.GreenYellow;
        }

        void BInput_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor      = Cursors.Arrow;
            BorderBrush = Brushes.DarkBlue;
        }

        

        void UpdateBindingPath()
        {
            if (Host.SelectedElement != this)
            {
                return;
            }

            _bindingPath.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }

        void UpdateLabel()
        {
            if (Host.SelectedElement != this)
            {
                return;
            }
            _label.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }
        #endregion



        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            OnAttachToEventBus?.Invoke();

            Host.EventBus.Subscribe(EventBus.LabelChanged, UpdateLabel);
            //Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);
        }

        public void DeAttachToEventBus()
        {
            OnDeAttachToEventBus?.Invoke();

            Host.EventBus.UnSubscribe(EventBus.LabelChanged, UpdateLabel);
            //Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateBindingPath);
        }
        #endregion
    }
}