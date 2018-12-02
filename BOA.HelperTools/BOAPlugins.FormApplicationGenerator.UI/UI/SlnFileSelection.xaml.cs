using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BOAPlugins.Utility.TypescriptModelGeneration;
using Microsoft.Win32;

namespace BOAPlugins.FormApplicationGenerator.UI
{

    class WizardPage:UserControl
    {
        public bool IsOk { get; set; }

        public MainWindowModel Model { get; set; }
    }

    /// <summary>
    ///     Interaction logic for SlnFileSelection.xaml
    /// </summary>
    public partial class SlnFileSelection
    {
        #region Constructors
        public SlnFileSelection()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Properties
        
        #endregion

        #region Methods
        void SelectSlnFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter           = "Sln files (*.sln)|",
                InitialDirectory = @"D:\work\BOA.CardModules\Dev\"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var selectedFile = openFileDialog.FileNames.FirstOrDefault();
                if (selectedFile != null)
                {
                    if (selectedFile.EndsWith(".sln"))
                    {
                        var slnFilePath = openFileDialog.FileNames.First();

                        Model.SolutionInfo = SolutionInfo.CreateFrom(slnFilePath);
                        IsOk              = true;        
                    }
                    
                }
                
            }
        }
        #endregion
    }
}