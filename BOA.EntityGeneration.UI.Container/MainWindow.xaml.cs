using System.Threading;
using System.Timers;
using System.Windows;
using Timer = System.Timers.Timer;

namespace BOA.EntityGeneration.UI.Container
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        Timer timer = new Timer();

        void StartTimer()
        {
            timer         =  new Timer(100);
            timer.Elapsed += OnTimedEvent;
            timer.Start();
            
        }

        void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Dispatcher?.Invoke(() =>
            {
                var temp = Model;
                Model = null;
                Model = temp;
            });
        }

        void FireAction(string controllerPublicMethodName)
        {

            if (Model.StartTimer)
            {
                Model.StartTimer = false;
                StartTimer();
            }

            if (Model.FinishTimer)
            {
                timer.Stop();
            }
        }

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            Model = Data.Model[App.Context];

        }
        #endregion

        #region Model
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(MainWindowModel), typeof(MainWindow), new PropertyMetadata(default(MainWindowModel)));

        public MainWindowModel Model
        {
            get { return (MainWindowModel) GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
        #endregion

        void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Model.StartTimer = true;
            new Thread(A).Start();
            FireAction("Mod");
        }

        void A()
        {
            for (int i = 0; i < 20; i++)
            {
                Data.Model[App.Context].SchemaGenerationProcessText = "aa + "+ i;
                Thread.Sleep(1000);    
            }
            
        }
    }
}