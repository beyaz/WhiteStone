using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{

    class ComponentEditorModel
    {
        public ComponentInfo Info { get; set; }
        public bool IsSizeEditorVisible { get; set; }
    }

    class ComponentEditor2:StackPanel
    {
        public static ComponentEditorModel CreateDataContext(ComponentInfo info)
        {
            return new ComponentEditorModel
            {
                Info                = info,
                IsSizeEditorVisible = info.Type.IsDivider
            };
        }


        public ComponentEditorModel Model => (ComponentEditorModel) DataContext;

        public ComponentEditor2()
        {
            var template = @"
{
    Childs:
    [
        {
            ui         :'SizeEditor',
            IsVisible  :'{Binding " + Model.AccessPathOf(m=>m.IsSizeEditorVisible) + @"}'
            Header     : 'Size', 
            MarginTop  : 10, 
            DataContext: '{Binding " + Model.AccessPathOf(m=>m.Info.SizeInfo) + @"}'
        }
    ]
}

";
            this.LoadJson(template);

        }
    }


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