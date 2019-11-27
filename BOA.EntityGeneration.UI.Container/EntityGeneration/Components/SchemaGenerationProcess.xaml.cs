using System;
using System.Linq;
using System.Threading;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.SchemaToEntityExporting.Exporters;

namespace BOA.EntityGeneration.UI.Container.EntityGeneration.Components
{
    public sealed partial class SchemaGenerationProcess
    {
        #region Constructors
        public SchemaGenerationProcess(string schemaName)
        {
            SchemaName = schemaName;
            Process    = new ProcessContract();

            InitializeComponent();
        }
        #endregion

        #region Public Events
        public event Action ProcessCompletedSuccessfully;
        #endregion

        #region Public Properties
        public ProcessContract Process    { get; set; }
        public string          SchemaName { get; set; }
        #endregion

        #region Public Methods
        public void Start()
        {
            new UIRefresher {Element = this}.Start();

            new Thread(GenerateSchema).Start();
        }
        #endregion

        #region Methods
        void GenerateSchema()
        {
            var schemaExporter = new SchemaExporter();

            schemaExporter.InitializeContext();

            var context = schemaExporter.Context;

            Process = context.processInfo;

            schemaExporter.Context.FileSystem.CheckinComment                          = App.Model.CheckinComment;
            schemaExporter.Context.FileSystem.IntegrateWithTFSAndCheckInAutomatically = context.config.IntegrateWithTFSAndCheckInAutomatically;

            schemaExporter.Export(SchemaName);

            if (context.Errors.Any())
            {
                context.processInfo.Text = string.Join(Environment.NewLine, context.Errors);
                return;
            }

            OnProcessCompletedSuccessfully();
        }

        void OnProcessCompletedSuccessfully()
        {
            ProcessCompletedSuccessfully?.Invoke();
        }
        #endregion
    }
}