using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using BOAPlugins.FormApplicationGenerator;
using BOAPlugins.FormApplicationGenerator.Logic;
using BOAPlugins.FormApplicationGenerator.UI;
using BOAPlugins.ViewClassDependency;
using WhiteStone;

namespace BOAPlugins.VSIntegration
{
    /// <summary>
    ///     Interaction logic for MainForm.xaml
    /// </summary>
    public partial class MainForm
    {
        #region Constructors
        public MainForm()
        {
            InitializeComponent();
            Loaded += OnLoadCompleted;

            KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    Close();
                }
            };
            Closed += MainForm_Closed;
        }

        void MainForm_Closed(object sender, EventArgs e)
        {
            SM.Get<Configuration>().CheckInCommentDefaultValue = cmpCheckInComment.Text;
            Configuration.SaveToFile();
        }
        #endregion

        #region Public Properties
        public IVisualStudioLayer VisualStudio { get; set; }
        #endregion

        #region Properties
        Command Command
        {
            get
            {
                return new Command
                {
                    VisualStudio = VisualStudio
                };
            }
        }

        Communication Communication
        {
            get { return new Communication(VisualStudio); }
        }
        #endregion

        #region Methods
        static void SafeScope(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        void InitializeFormAssistantDefaultCodes(object sender, RoutedEventArgs e)
        {
            SafeScope(() =>
            {
                FormAssistantProjectInitializer.Initialize(new Model(VisualStudio.GetSolutionFilePath(), "xxx"));

                Close();

                VisualStudio.UpdateStatusbarText("Files are exported.");
            });
        }

        void OnLoadCompleted(object sender, RoutedEventArgs e)
        {
            buttonUpdateMessageCs.Focus();

            if (string.IsNullOrWhiteSpace(VisualStudio.CursorSelectedText))
            {
                buttonViewMethodCallGraph.Visibility = Visibility.Collapsed;
            }
            else
            {
                buttonViewMethodCallGraph.Content = buttonViewMethodCallGraph.Content + ": " + VisualStudio.CursorSelectedText;
            }

            cmpCheckInComment.Text = SM.Get<Configuration>().CheckInCommentDefaultValue;
        }

        void OpenPluginDirectory(object sender, RoutedEventArgs e)
        {
            Close();

            Process.Start(DirectoryHelper.PluginDirectory);
        }

        

        // TODO: remove with sub tree
        void ShowPropertyGenerator(object sender, RoutedEventArgs e)
        {
            Communication.ShowPropertyGenerator();
            Close();
        }

        void UpdateMessageCs(object sender, RoutedEventArgs e)
        {
            SafeScope(() =>
            {
                Command.UpdateMessageCs();
                Close();
            });
        }

        void UpdateMessageTsx(object sender, RoutedEventArgs e)
        {
            SafeScope(() =>
            {
                Command.UpdateMessageTsx();
                Close();
            });
        }
        void RemoveUnusedMessagesInTypescriptCodes(object sender, RoutedEventArgs e)
        {
            SafeScope(() =>
            {
                Command.RemoveUnusedMessagesInTypescriptCodes();
                Close();
            });
        }
        void RemoveUnusedMessagesInCsCodes(object sender, RoutedEventArgs e)
        {
            SafeScope(() =>
            {
                Command.RemoveUnusedMessagesInCsCodes();
                Close();
            });
        }
        


        void UpdateTypeScriptModels(object sender, RoutedEventArgs e)
        {
            SafeScope(() =>
            {
                Command.UpdateTypeScriptModels();
                Close();
            });
        }

        void ViewMethodCallGraph(object sender, RoutedEventArgs e)
        {
            SafeScope(() =>
            {
                Close();

                var input = new Data
                {
                    AssemblySearchDirectoryPath = VisualStudio.GetBinFolderPathOfActiveProject(),
                    SelectedText                = VisualStudio.CursorSelectedText,
                    ActiveProjectName           = VisualStudio.ActiveProjectName
                };

                Communication.Send(input);
            });
        }
        #endregion

        
    }
}