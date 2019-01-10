using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     Interaction logic for BDataGridColumnWpf.xaml
    /// </summary>
    class BTabBarPageWpf : TextBlock, IHostItem,IEventBusListener
    {
        #region Constructors
        public BTabBarPageWpf(BTabBarPage dataContext, Host host, BTabBarWpf bTabBarWpf)
        {
            DataContext = dataContext;
            Host        = host;
            BTabBarWpf  = bTabBarWpf;

           

            Loaded += (s, e) => { UpdateLabel(); };

            MouseEnter += BInput_MouseEnter;
            MouseLeave += BInput_MouseLeave;
        }
        #endregion

        #region Public Properties
        public BTabBarWpf  BTabBarWpf { get; }
        public Host        Host       { get; set; }
        public BTabBarPage Model      => (BTabBarPage) DataContext;
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

        

        void UpdateLabel()
        {
            Text = Model?.Title;
            Margin=new Thickness(2);
            TextDecorations = System.Windows.TextDecorations.Underline;
        }
        #endregion




        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            OnAttachToEventBus?.Invoke();
            Host.EventBus.Subscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
        }

        public void DeAttachToEventBus()
        {
            OnDeAttachToEventBus?.Invoke();
            Host.EventBus.UnSubscribe(EventBus.OnComponentPropertyChanged, UpdateLabel);
        }
        #endregion

    }
}