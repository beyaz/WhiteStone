using System.Collections.Generic;
using System.Threading;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Exporters;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;
using WhiteStone.UI.Container.Mvc;
using static BOA.EntityGeneration.CustomSQLExporting.Data;

namespace CustomSqlInjectionToProject.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Static Fields
        static Context context;
        static bool         IsFinished;
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
            Model.CustomSqlGenerationOfProfileIdProcess = context?.TryGet(ProcessInfo) ?? Model.CustomSqlGenerationOfProfileIdProcess;

            if (IsFinished)
            {
                Model.FinishTimer       = true;
                Model.ViewShouldBeClose = true;
            }
        }

        public override void OnViewLoaded()
        {
            var ctx = new CustomSqlDataContextCreator().Create();

            Model = new Model
            {
                ProfileId = Injection.ProfileId,
                CustomSqlGenerationOfProfileIdProcess = new ProcessContract
                {
                    Text = "Ready"
                },
                CheckInComment = CheckInCommentAccess.GetCheckInComment(),
                ProfileIdList  = ctx.GetProfileNames(),

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
            context = new CustomSqlDataContextCreator().Create();

            context.Add(FileSystem.CheckinComment, Model.CheckInComment);
            context.Add(FileSystem.IntegrateWithTFSAndCheckInAutomatically, true);
            context.Add(MsBuildQueue.BuildAfterCodeGenerationIsCompleted, true);

            var profileId = Model.ProfileId.Trim();

            if (profileId == "*")
            {
                foreach (var profileName in context.GetProfileNames())
                {
                    if (profileName == "*")
                    {
                        continue;
                    }

                    //CustomSqlExporter.Export(context, profileName);
                }
            }
            else
            {
                //CustomSqlExporter.Export(context, profileId);
            }

            context.CloseBracket();

            IsFinished = true;

            context = null;
        }
        #endregion
    }
}