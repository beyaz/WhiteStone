using System.Collections.Generic;
using System.Threading;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.TfsAccess;
using Ninject;
using WhiteStone.UI.Container.Mvc;

namespace BOA.EntityGeneration.UI.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Static Fields
        static Tracer Tracer;
        #endregion

        #region Public Methods
        public void Generate()
        {
            if (Model.SchemaName.IsNullOrWhiteSpace())
            {
                Model.ViewMessage            = "SchemaName girilmelidir.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Model.StartTimer = true;

            new Thread(Start).Start();
        }

        public void GetCapture()
        {
            if (Tracer == null)
            {
                return;
            }

            Model.ProcessIndicatorValue = Tracer.CurrentSchemaProcess.PercentageOfCompletion;
            Model.ProcessIndicatorText  = Tracer.CurrentSchemaProcess.Text;
            if (IsFinished)
            {
                Model.FinishTimer = true;
                Model.ProcessIndicatorText = "Finished.";
            }
        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                ProcessIndicatorValue = 44,
                ProcessIndicatorText  = "Ready",
                CheckInComment        = "2235# - AutoCheckInByEntityGenerator",
                SchemaName            = "CRD",

                ActionButtons = new List<ActionButtonInfo>
                {
                    new ActionButtonInfo
                    {
                        ActionName = nameof(Generate),
                        Text       = "Generate"
                    }
                }
            };
        }
        #endregion

        static bool IsFinished;
        #region Methods
        void Start()
        {
            using (var kernel = new Kernel())
            {
                kernel.Bind<FileAccess>().To<FileAccessWithAutoCheckIn>().OnActivation(x => x.CheckInComment = Model.CheckInComment);

                kernel.Bind<Tracer>().To<Tracer>().InSingletonScope();

                Tracer = kernel.Get<Tracer>();

                BOACardDatabaseExporter.Export(kernel, Model.SchemaName);

                IsFinished = true;
            }
        }
        #endregion
    }
}