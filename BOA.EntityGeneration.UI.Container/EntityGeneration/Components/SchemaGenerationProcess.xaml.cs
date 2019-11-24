using System;
using System.Threading;
using System.Windows;
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
            var schemaExporter = new SchemaExporter();
            schemaExporter.InitializeContext();

            var context = schemaExporter.Context;

            context.OpenBracket();

            context.Add(FileSystem.CheckinComment, App.Model.CheckinComment);
            context.Add(FileSystem.IntegrateWithTFSAndCheckInAutomatically, false);
            context.Add(MsBuildQueue_old.BuildAfterCodeGenerationIsCompleted, true);

            model.Process = DataFlow.Data.ProcessInfo[context];

            

            schemaExporter.Export( model.SchemaName);

            context.CloseBracket();
        }
        #endregion
    }
}