using System.Collections.Generic;
using System.Threading;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.TfsAccess;
using Ninject;
using WhiteStone.UI.Container.Mvc;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace CustomSqlInjectionToProject.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Static Fields
        static bool IsFinished;
        #endregion

        #region Public Methods
        public void Generate()
        {
            if (Model.CheckInComment.IsNullOrWhiteSpace())
            {
                Model.ViewMessage            = "Tfs Check-in comment girilmelidir.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            if (Model.ProfileId.IsNullOrWhiteSpace())
            {
                Model.ViewMessage            = "ProfileId girilmelidir.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            CheckInCommentAccess.SaveCheckInComment(Model.CheckInComment);

            new Thread(Start).Start();

            Model.StartTimer = true;
        }

        public void GetCapture()
        {
            Model.CustomSqlGenerationOfProfileIdProcess = Context.TryGet(Data.CustomSqlGenerationOfProfileIdProcess) ?? Model.CustomSqlGenerationOfProfileIdProcess;

            if (IsFinished)
            {
                Model.FinishTimer       = true;
                Model.ViewShouldBeClose = true;
            }
        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                ProfileId = Injection.ProfileId,
                CustomSqlGenerationOfProfileIdProcess = new ProcessInfo
                {
                    Text = "Ready"
                },
                CheckInComment = CheckInCommentAccess.GetCheckInComment(),

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
        Kernel CreateKernel()
        {
            var kernel = new Kernel();

            kernel.Bind<FileAccess>().To<FileAccessWithAutoCheckIn>().OnActivation(x => x.CheckInComment = Model.CheckInComment);

            Context.Add(Data.CustomSqlGenerationOfProfileIdProcess, new ProcessInfo());

            return kernel;
        }

        void Start()
        {
            using (var kernel = CreateKernel())
            {
                kernel.Get<ProjectInjector>().Inject(Model.ProfileId.Trim());
                IsFinished = true;
            }
        }
        #endregion
    }
}