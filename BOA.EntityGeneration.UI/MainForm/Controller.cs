﻿using System.Collections.Generic;
using System.Threading;
using ___Company___.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.UI.Deployment;
using WhiteStone.UI.Container.Mvc;

namespace BOA.EntityGeneration.UI.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Static Fields
        static bool   IsFinished;
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

            if (Model.SchemaName.IsNullOrWhiteSpace())
            {
                Model.ViewMessage            = "SchemaName girilmelidir.";
                Model.ViewMessageTypeIsError = true;
                return;
            }

            CheckInCommentAccess.SaveCheckInComment(Model.CheckInComment);

            if (Model.SchemaName.Trim() == "*")
            {
                Model.AllSchemaGenerationProcessIsVisible = true;
                new Thread(StartAll).Start();
            }
            else
            {
                new Thread(Start).Start();
            }

            Model.StartTimer = true;
        }

        public void GetCapture()
        {
            
            Model.SchemaGenerationProcess    = context?.TryGet(Data.SchemaGenerationProcess) ?? Model.SchemaGenerationProcess;

            Model.AllSchemaGenerationProcess = context?.TryGet(Data.AllSchemaGenerationProcess) ?? Model.AllSchemaGenerationProcess;

            if (IsFinished)
            {
                Model.FinishTimer = true;

                Model.SchemaGenerationProcess.Text        = "Finished.";
                Model.AllSchemaGenerationProcessIsVisible = false;
            }
        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                SchemaGenerationProcess = new ProcessInfo
                {
                    Text = "Ready"
                },
                CheckInComment = CheckInCommentAccess.GetCheckInComment(),
                SchemaName     = "CRD",

                ActionButtons = new List<ActionButtonInfo>
                {
                    new ActionButtonInfo
                    {
                        ActionName = nameof(Generate),
                        Text       = "Generate"
                    }
                }
            };

            Updater.StartUpdate();
        }
        #endregion

        #region Methods

        IDataContext context;
        void Start()
        {
            context = new DataContextCreator {CheckinComment = Model.CheckInComment, IsFileAccessWithTfs = true}.Create();

            BOACardDatabaseExporter.Export(context, Model.SchemaName);

            IsFinished = true;
        }
        

        void StartAll()
        {
            context = new DataContextCreator {CheckinComment = Model.CheckInComment, IsFileAccessWithTfs = true}.Create();
            BOACardDatabaseExporter.Export(context);
        }
        #endregion
    }
}