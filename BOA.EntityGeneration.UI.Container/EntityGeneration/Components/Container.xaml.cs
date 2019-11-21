using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BOA.EntityGeneration.UI.Container.EntityGeneration.Components
{

    [Serializable]
    public class ContainerModel
    {
        public string SelectedSchemaName { get; set; }
    }

    public partial class Container
    {
        readonly ContainerModel model;

        public Container()
        {
            DataContext = model = new ContainerModel();
            
            InitializeComponent();
        }

        void OnGenerateClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedSchemaName))
            {
                MessageBox.Show("Schema Name seçilmelidir.");
                return;
            }

            var ui = SchemaGenerationProcess.Create(model.SelectedSchemaName);

            ui.Margin = new Thickness(0,10,0,0);

            processContainer.Children.Add(ui);

            ui.Start();
        }
    }
}
