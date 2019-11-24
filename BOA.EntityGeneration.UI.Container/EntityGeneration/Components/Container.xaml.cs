using System;
using System.Windows;

namespace BOA.EntityGeneration.UI.Container.EntityGeneration.Components
{
    [Serializable]
    public class ContainerModel
    {
        #region Public Properties
        public string SelectedSchemaName { get; set; }
        #endregion
    }

    public partial class Container
    {
        #region Fields
        readonly ContainerModel model;
        #endregion

        #region Constructors
        public Container()
        {
            DataContext = model = new ContainerModel();

            InitializeComponent();
        }
        #endregion

        #region Methods
        void OnGenerateClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedSchemaName))
            {
                MessageBox.Show("Schema Name seçilmelidir.");
                return;
            }

            var checkinComment = App.Model.CheckinComment;

            if (string.IsNullOrWhiteSpace(checkinComment))
            {
                MessageBox.Show("Tfs Check-in comment girilmelidir.");
                return;
            }

            var ui = SchemaGenerationProcess.Create(model.SelectedSchemaName);

            ui.Margin = new Thickness(0, 10, 0, 0);

            processContainer.Children.Add(ui);

            ui.Start();
        }
        #endregion
    }
}