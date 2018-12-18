using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BOA.Common.Helpers;

namespace WhiteStone.UI.Container
{
    public class Builder : CustomUIMarkupLanguage.UIBuilding.Builder
    {
        #region Constructors
        static Builder()
        {
            RegisterElementCreation(LabeledTextBox.On);
        }
        #endregion
    }

    public class Designer : Grid
    {
        #region Fields
        public Grid ContentGrid;

        DateTime LastBuildTime = DateTime.Now;
        #endregion

        #region Constructors
        public Designer()
        {
            const string ui = @"
{
    Margin:7,
    rows:[
		{view:'LabeledTextBox',Text:'{Binding SourceJsonFilePath}', Label:'Source Json File Path',  Height:'auto'},
        {view:'Grid', Name:'ContentGrid', Gravity:1 }
	]
	
}
";

            var builder = new Builder
            {
                Caller      = this,
                DataContext = this
            };

            builder.Load(ui);

            SourceJsonFilePath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar + "Designer.json";

            FileHelper.WriteAllText(SourceJsonFilePath, @"

{
	view:'grid',	
	rows:[
		{
			view:'grid',gravity:1,
			cols:[
				{view:'Textbox',Text:'Success',gravity:2},
				{view:'GridSplitter'},
				{view:'Textbox',Text:'Success2',gravity:1}
			]
		},
		{view:'GridSplitter'}, 
		{			
			view:'grid',gravity:1,
			cols:[
				{view:'Textbox',Text:'Success',gravity:1},
				{view:'GridSplitter'},
				{ui:'TextArea',Text:'Success2',gravity:2}
			]
		}
	]
}




");

            var fileSystemWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(SourceJsonFilePath) + Path.DirectorySeparatorChar,
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
        #endregion

        #region Methods
        void UpdateResult()
        {
            try
            {
                if (TimeSpan.FromMilliseconds(300) > DateTime.Now - LastBuildTime)
                {
                    return;
                }

                LastBuildTime = DateTime.Now;

                if (FileHelper.IsFileLocked(SourceJsonFilePath))
                {
                    return;
                }

                ContentGrid.RowDefinitions.Clear();
                ContentGrid.ColumnDefinitions.Clear();
                ContentGrid.Children.Clear();

                var builder = new Builder
                {
                    DataContext = this,
                    Caller      = ContentGrid
                };

                builder.Load(FileHelper.ReadFile(SourceJsonFilePath));
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