using System;
using System.Threading;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.Exporters;

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
            var context = EntityGenerationDataContextCreator.Create();

            context.OpenBracket();

            context.Add(FileSystem.CheckinComment, App.CheckInComment);
            context.Add(FileSystem.IntegrateWithTFSAndCheckInAutomatically, false);
            context.Add(MsBuildQueue.BuildAfterCodeGenerationIsCompleted, true);

            model.Process = DataFlow.Data.ProcessInfo[context];

            SchemaExporter.Export(context, model.SchemaName);

            context.CloseBracket();
        }
        #endregion
    }
}