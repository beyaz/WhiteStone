using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var schemaName = model.SelectedSchemaName;

            if (string.IsNullOrWhiteSpace(schemaName))
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

            if (schemaName == "*")
            {
                foreach (var item in App.Model.SchemaNames.Where(x => x != "*"))
                {
                    schemaGenerationQueue.Enqueue(item);
                }

                // Max three thread
                ConsumeSchemaGenerationQueue();
                ConsumeSchemaGenerationQueue();
                ConsumeSchemaGenerationQueue();
                return;
            }

            StartGeneration(schemaName);
        }

        readonly Queue<string> schemaGenerationQueue = new Queue<string>();

        void ConsumeSchemaGenerationQueue()
        {
            if (!schemaGenerationQueue.Any())
            {
                return;
            }

            StartGeneration(schemaGenerationQueue.Dequeue());
        }


        void StartGeneration(string schemaName)
        {
            var ui = SchemaGenerationProcess.Create(schemaName);
            ui.GenerationProcessCompletedSuccessfully += () =>
            {
                Dispatcher?.Invoke(()=>{processContainer.Children.Remove(ui);});
                Dispatcher?.Invoke(ConsumeSchemaGenerationQueue);
            };

            ui.Margin = new Thickness(0, 10, 0, 0);

            processContainer.Children.Add(ui);

            ui.Start();
        }
        #endregion
    }
}