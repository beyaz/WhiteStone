﻿using System.Collections.Generic;
using System.Threading;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
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
            Model.CustomSqlGenerationOfProfileIdProcess = context?.TryGet(Data.CustomSqlGenerationOfProfileIdProcess) ?? Model.CustomSqlGenerationOfProfileIdProcess;

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
        void Start()
        {
            context = new CustomSqlDataContextCreator {IsFileAccessWithTfs = true, CheckinComment = Model.CheckInComment}.Create();
            CustomSqlExporter.Export(context, Model.ProfileId.Trim());
            IsFinished = true;

            context = null;
        }
        #endregion
    }
}