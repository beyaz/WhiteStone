using System.Collections.Generic;
using System.Threading;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Exporters;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;
using BOA.EntityGeneration.DataFlow;
using WhiteStone.UI.Container.Mvc;

namespace CustomSqlInjectionToProject.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Static Fields
        static IDataContext context;
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
            Model.CustomSqlGenerationOfProfileIdProcess = context?.TryGet(CustomSqlExporter.CustomSqlGenerationOfProfileIdProcess) ?? Model.CustomSqlGenerationOfProfileIdProcess;

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
                CustomSqlGenerationOfProfileIdProcess = new ProcessInfo
                {
                    Text = "Ready"
                },
                CheckInComment = CheckInCommentAccess.GetCheckInComment(),
                ProfileIdList =  ctx.GetProfileNames(),

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

            context.Add(FileSystem.CheckinComment,Model.CheckInComment);
            context.Add(FileSystem.IntegrateWithTFSAndCheckInAutomatically,true);

            CustomSqlExporter.Export(context, Model.ProfileId.Trim());

            context.Remove(FileSystem.CheckinComment);
            context.Remove(FileSystem.IntegrateWithTFSAndCheckInAutomatically);

            IsFinished = true;

            context = null;
        }
        #endregion
    }
}