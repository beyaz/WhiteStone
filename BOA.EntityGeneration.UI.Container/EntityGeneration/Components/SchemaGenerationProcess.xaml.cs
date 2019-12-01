using System;
using System.Linq;
using System.Threading;
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

            new Thread(Run).Start();
        }
        #endregion

        #region Methods
        void Run()
        {
            var exporter = new SchemaExporter();

            exporter.InitializeContext();

            var context = exporter.Context;

            Process = context.ProcessInfo;

            exporter.Context.FileSystem.CheckinComment                          = App.Model.CheckinComment;
            exporter.Context.FileSystem.IntegrateWithTFSAndCheckInAutomatically = App.Config.IntegrateWithTFSAndCheckInAutomatically;

            exporter.Export(SchemaName);

            if (context.ErrorList.Any())
            {
                context.ProcessInfo.Text = string.Join(Environment.NewLine, context.ErrorList);
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