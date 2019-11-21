using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using BOA.DataFlow;

namespace BOA.EntityGeneration.UI.Container.EntityGeneration.Components
{

    [Serializable]
    public class SchemaGenerationProcessModel
    {
        public string SchemaName { get; set; }

        public string ProcessText { get; set; }
        public int ProgressMaximum { get; set; }
        public int ProgressCurrent { get; set; }
    }
    /// <summary>
    /// Interaction logic for SchemaGenerationProcess.xaml
    /// </summary>
    public partial class SchemaGenerationProcess
    {
        SchemaGenerationProcessModel model;

        public static SchemaGenerationProcess Create(string schemaName)
        {
            var model = new SchemaGenerationProcessModel
            {
                SchemaName = schemaName
            };

            var ui= new SchemaGenerationProcess
            {
                DataContext = model
            };

            ui.model = model;

            return ui;
        }

        public SchemaGenerationProcess()
        {
            InitializeComponent();
        }

        public void Start()
        {
            new UIRefresher {Element = this}.Start();

            new Thread(GenerateSchema).Start();
        }

        

        void GenerateSchema()
        {
            // var context = new DataContextCreator().Create();



            model.ProgressMaximum = 19;

            for (var i = 0; i < 20; i++)
            {
                model.ProcessText = "a" + i;
                model.ProgressCurrent = i;

                Thread.Sleep(1000);
            }

        }
    }
}
