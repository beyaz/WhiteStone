using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BOA.Common.Helpers;
using BOA.Jaml;

namespace WhiteStone.UI.Container
{
    public class Designer : Grid
    {
        #region Fields
        public Grid ContentGrid;
        #endregion

        #region Constructors
        public Designer()
        {
            const string ui = @"
{
    view:'Grid',
    Margin:7,
    rows:[
		{view:'LabeledTextBox',Text:'{Binding SourceJsonFilePath}', Label:'Source Json File Path',  Height:'auto'},
        {view:'Grid', Name:'ContentGrid', Gravity:1 }
	]
	
}
";
            
            var builder = new Builder
            {
                View        = this,
                DataContext = this
            };

            builder.Config.TryToCreateElement(LabeledTextBox.On);

            builder.SetJson(ui).Build();


            SourceJsonFilePath = Path.GetDirectoryName(this.GetType().Assembly.Location) + Path.DirectorySeparatorChar + "Designer.json";

            FileHelper.WriteAllText(SourceJsonFilePath,"{view:'Button',Content:'Success'}");

            var fileSystemWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName( SourceJsonFilePath)+Path.DirectorySeparatorChar,
                NotifyFilter = NotifyFilters.LastAccess | 
                                         NotifyFilters.LastWrite | 
                                         NotifyFilters.FileName | 
                                         NotifyFilters.DirectoryName,

                EnableRaisingEvents = true
            };

            fileSystemWatcher.Changed += (s, e) =>
            {
                Action action = UpdateResult;
                Dispatcher?.BeginInvoke(DispatcherPriority.Normal, action);
            };

            Process.Start(SourceJsonFilePath);

        }
        
        #endregion

        #region Public Methods
        public static void Start()
        {
            Debug.Assert(Application.Current.MainWindow != null, "Application.Current.MainWindow != null");

            Application.Current.MainWindow.Content = new Designer();

           
        }

        DateTime LastBuildTime = DateTime.Now;

         void UpdateResult()
        {
            try
            {
                var builder = new Builder
                {
                    DataContext = this
                };

                builder.Config.TryToCreateElement(LabeledTextBox.On);

                if (TimeSpan.FromMilliseconds(300) > DateTime.Now -LastBuildTime)
                {
                    return;
                }

                LastBuildTime = DateTime.Now;

                if (FileHelper.IsFileLocked(SourceJsonFilePath))
                {
                    return;
                }

                var json = FileHelper.ReadFile(SourceJsonFilePath);

                var ui = builder.SetJson(json).Build().View;

                ContentGrid.Children.Clear();

                ContentGrid.Children.Add(ui);
            }
            catch (Exception e)
            {
                Log.Push(e);
                App.ShowErrorNotification(e.ToString());
            }
        }
        #endregion

        #region SourceJsonFilePath
        public static readonly DependencyProperty SourceJsonFilePathProperty = DependencyProperty.Register("SourceJsonFilePath", typeof(string), typeof(Designer), new PropertyMetadata(default(string)));

        public string SourceJsonFilePath
        {
            get { return (string) GetValue(SourceJsonFilePathProperty); }
            set { SetValue(SourceJsonFilePathProperty, value); }
        }
        #endregion
    }
}