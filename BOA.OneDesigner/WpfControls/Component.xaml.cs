using System.Windows;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    /// <summary>
    ///     Interaction logic for Component.xaml
    /// </summary>
    public partial class Component
    {
        #region Constructors
        public Component()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region public ComponentInfo Info
        public static readonly DependencyProperty InfoProperty = DependencyProperty.Register("Info", typeof(ComponentInfo), typeof(Component), new PropertyMetadata(default(ComponentInfo)));

        public ComponentInfo Info
        {
            get { return (ComponentInfo) GetValue(InfoProperty); }
            set { SetValue(InfoProperty, value); }
        }
        #endregion

        #region bool IsInToolBox
        public static readonly DependencyProperty IsInToolboxProperty = DependencyProperty.Register(
                                                        "IsInToolbox", typeof(bool), typeof(Component), new PropertyMetadata(default(bool)));

        public bool IsInToolbox
        {
            get { return (bool) GetValue(IsInToolboxProperty); }
            set { SetValue(IsInToolboxProperty, value); }
        }
        #endregion
    }
}