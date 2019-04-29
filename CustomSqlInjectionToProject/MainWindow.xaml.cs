using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection;
using Ninject;

namespace CustomSqlInjectionToProject
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        #endregion

        #region Methods
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var kernel = new BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Kernel())
            {
                Model = kernel.Get<UIController>().CreateModel();
            }
        }
        #endregion

        #region Model
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
                                                                                              "Model", typeof(UIModel), typeof(MainWindow), new PropertyMetadata(default(UIModel)));

        public UIModel Model
        {
            get { return (UIModel) GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
        #endregion

        void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var kernel = new BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Kernel())
            {
                kernel.Get<UIController>().SelectedProfileChange(Model);

                Model = Model.Clone();
            }

        }
    }
}