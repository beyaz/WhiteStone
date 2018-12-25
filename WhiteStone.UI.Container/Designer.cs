using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.Common;

namespace WhiteStone.UI.Container
{


    public class TestContract
    {
        public bool IsSelected { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

    }


    public class Designer : WindowBase
    {

        public List<TestContract> DataGridComponentItemSource { get; set; } = new List<TestContract>
        {
            new TestContract
            {
                Name = "A",
                Password = "B",
                
            },

            new TestContract
            {
                Name     = "A1",
                Password = "B1",
                IsSelected = true
            }
            ,
            
            new TestContract
            {
            Name     = "A2",
            Password = "B2"
            
        }
        };

        #region Fields
        public Grid ContentGrid;

        DateTime LastBuildTime = DateTime.Now;
        #endregion

        #region Constructors
        public Designer()
        {
            const string ui = @"
{
   Title:'Designer',
   Content:{
       ui:'Grid',
        Margin:7,
        rows:[
		    {ui:'TextBox',Text:'{Binding SourceJsonFilePath}', Label:'Source Json File Path',  Height:'auto'},
            {ui:'Grid', Name:'ContentGrid', Gravity:1 }
	    ]
	    
    }

}
";

            var builder = new Builder
            {
                Caller      = this,
            };

            builder.Load(ui);

            DataContext = this;

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
				{ui:'TextArea',Text:'Success2',gravity:1},
                {
                    ui:'WrapPanel',gravity:2,
                    childs:[
                        {ui:'Tile',Count:'A'},
                        {ui:'Tile',count:'B'},
                        {ui:'DataGrid', Width:300,Height:200, AutoGenerateColumns:true, ItemsSource:'{Binding DataGridComponentItemSource}'}
                    ]
                },
			]
		}
	]
}




");

            FileHelper.ListenForSave(Dispatcher,SourceJsonFilePath,UpdateResult);

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
                

                if (FileHelper.IsFileLocked(SourceJsonFilePath))
                {
                    return;
                }

                LastBuildTime = DateTime.Now;

                ContentGrid.RowDefinitions.Clear();
                ContentGrid.ColumnDefinitions.Clear();
                ContentGrid.Children.Clear();


                var builder = new Builder
                {
                    Caller      = ContentGrid,
                    IsInDesignMode = true
                };

                builder.Load(File.ReadAllText(SourceJsonFilePath));

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