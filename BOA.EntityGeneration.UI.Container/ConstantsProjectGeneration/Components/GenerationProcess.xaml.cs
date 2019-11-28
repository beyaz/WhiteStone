﻿using System;
using System.Linq;
using System.Threading;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace BOA.EntityGeneration.UI.Container.ConstantsProjectGeneration.Components
{
    public sealed partial class GenerationProcess
    {
        #region Constructors
        public GenerationProcess()
        {
            Process     = new ProcessContract();

            InitializeComponent();
        }
        #endregion

        #region Public Events
        public event Action ProcessCompletedSuccessfully;
        #endregion

        #region Public Properties
        public ProcessContract Process     { get; set; }
        #endregion

        #region Public Methods
        public void Start()
        {
            new UIRefresher {Element = this}.Start();

            new Thread(Run).Start();
        }
        #endregion

        #region Methods
        void OnProcessCompletedSuccessfully()
        {
            ProcessCompletedSuccessfully?.Invoke();
        }

        void Run()
        {
            var exporter = new BOA.EntityGeneration.ConstantsProjectGeneration.Generator();


            var context = exporter.Context;

            Process = context.ProcessInfo;

            exporter.Context.FileSystem.CheckinComment                          = App.Model.CheckinComment;
            exporter.Context.FileSystem.IntegrateWithTFSAndCheckInAutomatically = context.Config.IntegrateWithTFSAndCheckInAutomatically;

            exporter.Generate();

            if (context.Errors.Any())
            {
                context.ProcessInfo.Text = string.Join(Environment.NewLine, context.Errors);
                return;
            }

            OnProcessCompletedSuccessfully();
        }
        #endregion
    }
}