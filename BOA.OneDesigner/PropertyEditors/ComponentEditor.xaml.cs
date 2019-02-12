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
            Loaded += (s, e) =>
            {
                sizeEditor.Host = Host;
                _valueBindingPathEditor.
            };
        }
        #endregion

        #region public ComponentInfo Info
        public static readonly DependencyProperty InfoProperty = DependencyProperty.Register("Info", typeof(ComponentInfo), typeof(ComponentEditor), new PropertyMetadata(default(ComponentInfo),OnInfoChanged));

        static void OnInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var componentEditor = d as ComponentEditor;
            if (componentEditor == null)
            {
                return;
            }

            var componentInfo = e.NewValue as ComponentInfo;
            if (componentInfo == null)
            {
                return;
            }

            componentEditor.IsSizeEditorVisible = componentInfo.Type.IsDivider;
        }

        public ComponentInfo Info
        {
            get { return (ComponentInfo) GetValue(InfoProperty); }
            set { SetValue(InfoProperty, value); }
        }
        #endregion



        #region  bool IsSizeEditorVisible
        public static readonly DependencyProperty IsSizeEditorVisibleProperty = DependencyProperty.Register(
                                                        "IsSizeEditorVisible", typeof(bool), typeof(ComponentEditor), new PropertyMetadata(default(bool)));

        public bool IsSizeEditorVisible
        {
            get { return (bool)GetValue(IsSizeEditorVisibleProperty); }
            set { SetValue(IsSizeEditorVisibleProperty, value); }
        } 
        #endregion
    }
}