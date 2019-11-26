using System;
using System.Linq;
using System.Threading;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.SchemaToEntityExporting.Exporters;

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

        #region Public Events
        public event Action GenerationProcessCompletedSuccessfully;
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
        protected virtual void OnGenerationProcessCompletedSuccessfully()
        {
            GenerationProcessCompletedSuccessfully?.Invoke();
        }

        void GenerateSchema()
        {
            var schemaExporter = new SchemaExporter();
            schemaExporter.InitializeContext();

            var context = schemaExporter.Context;

            schemaExporter.Context.FileSystem.CheckinComment = App.Model.CheckinComment;
            // TODO open for prod schemaExporter.Context.FileSystem.IntegrateWithTFSAndCheckInAutomatically = true;

            model.Process = context.processInfo;

            schemaExporter.Export(model.SchemaName);

            if (context.Errors.Any())
            {
                context.processInfo.Text = string.Join(Environment.NewLine, context.Errors);
                return;
            }

            OnGenerationProcessCompletedSuccessfully();
        }
        #endregion
    }
}