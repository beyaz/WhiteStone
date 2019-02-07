using System.Windows;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.PropertyEditors
{
    /// <summary>
    ///     Interaction logic for ComponentEditor.xaml
    /// </summary>
    public partial class ComponentEditor
    {
        public Host Host { get; set; }
        #region Constructors
        public ComponentEditor()
        {
            InitializeComponent();
        }
        #endregion

        #region public ComponentInfo Info
        public static readonly DependencyProperty InfoProperty = DependencyProperty.Register("Info", typeof(ComponentInfo), typeof(ComponentEditor), new PropertyMetadata(default(ComponentInfo)));

        public ComponentInfo Info
        {
            get { return (ComponentInfo) GetValue(InfoProperty); }
            set { SetValue(InfoProperty, value); }
        }
        #endregion
    }
}