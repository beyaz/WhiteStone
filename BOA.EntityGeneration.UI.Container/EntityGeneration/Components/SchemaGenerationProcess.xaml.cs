using System;
using System.Threading;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace BOA.EntityGeneration.UI.Container.EntityGeneration.Components
{
    [Serializable]
    public class SchemaGenerationProcessModel
    {
        #region Public Properties
        public ProcessContract Process    { get; set; }
        public string          SchemaName { get; set; }
        #endregion
    }

    
    public partial class SchemaGenerationProcess
    {
        #region Fields
        SchemaGenerationProcessModel model;
        #endregion

        #region Constructors
        public SchemaGenerationProcess()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods
        public static SchemaGenerationProcess Create(string schemaName)
        {
            var model = new SchemaGenerationProcessModel
            {
                SchemaName = schemaName,
                Process    = new ProcessContract()
            };

            return new SchemaGenerationProcess
            {
                DataContext = model,
                model       = model
            };
        }

        public void Start()
        {
            new UIRefresher {Element = this}.Start();

            new Thread(GenerateSchema).Start();
        }
        #endregion

        #region Methods
        void GenerateSchema()
        {
            // var context = new DataContextCreator().Create();

            model.Process.Total = 19;

            for (var i = 0; i < 20; i++)
            {
                model.Process.Text    = "a" + i;
                model.Process.Current = i;

                Thread.Sleep(1000);
            }
        }
        #endregion
    }
}