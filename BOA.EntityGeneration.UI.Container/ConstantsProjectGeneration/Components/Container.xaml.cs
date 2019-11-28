using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BOA.EntityGeneration.UI.Container.ConstantsProjectGeneration.Components
{
    public partial class Container
    {
        public string SelectedName { get; set; }

        #region Constructors
        public Container()
        {
            InitializeComponent();
        }
        #endregion

      

        #region Methods
     

        void OnGenerateClicked(object sender, RoutedEventArgs e)
        {
            

            var checkinComment = App.Model.CheckinComment;

            if (string.IsNullOrWhiteSpace(checkinComment))
            {
                MessageBox.Show("Tfs Check-in comment girilmelidir.");
                return;
            }

           

            StartGeneration();
        }

        void StartGeneration()
        {
            var ui = new GenerationProcess();
            ui.ProcessCompletedSuccessfully += () =>
            {
                Dispatcher?.Invoke(() => { processContainer.Children.Remove(ui); });
            };

            ui.Margin = new Thickness(0, 10, 0, 0);

            processContainer.Children.Add(ui);

            ui.Start();
        }
        #endregion
    }
}