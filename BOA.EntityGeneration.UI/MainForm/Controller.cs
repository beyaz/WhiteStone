using System.Collections.Generic;
using System.Threading;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.TfsAccess;
using WhiteStone.UI.Container.Mvc;

namespace BOA.EntityGeneration.UI.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Static Fields
        static int v = 1;
        #endregion

        #region Public Methods
        public void Generate()
        {
            if (Model.SchemaName.IsNullOrWhiteSpace())
            {
                Model.ViewMessage = "SchemaName girilmelidir.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            Model.StartTimer = true;

            new Thread(Start).Start();
        }

        public void GetCapture()
        {
            Model.ProcessIndicatorValue = v++;
            Model.ProcessIndicatorText = Tracer.LastTrace;
        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                ProcessIndicatorValue = 44,
                ProcessIndicatorText  = "Ready",
                CheckInComment = "2235# - AutoCheckInByEntityGenerator",
                SchemaName = "CRD",


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

        #region Methods
        void Start()
        {
            using (var kernel = new Kernel())
            {
                kernel.Bind<FileAccess>().To<FileAccessWithAutoCheckIn>();
                BOACardDatabaseExporter.Export(kernel, Model.SchemaName);
            }
        }
        #endregion
    }
}