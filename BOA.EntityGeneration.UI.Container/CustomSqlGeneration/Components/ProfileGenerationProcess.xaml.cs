﻿using System;
using System.Linq;
using System.Threading;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;

namespace BOA.EntityGeneration.UI.Container.CustomSqlGeneration.Components
{
    public sealed partial class ProfileGenerationProcess
    {
        #region Constructors
        public ProfileGenerationProcess(string profileName)
        {
            refresher = new UIRefresher {Element = this};

            ProfileName = profileName;
            Process     = new ProcessContract();

            InitializeComponent();
        }
        #endregion

        #region Public Events
        public event Action ProcessCompletedSuccessfully;
        #endregion

        #region Public Properties
        public ProcessContract Process     { get; set; }
        public string          ProfileName { get; set; }
        #endregion

        readonly UIRefresher refresher;
        #region Public Methods
        public void Start()
        {
            
            refresher.Start();

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
            var exporter = new CustomSqlExporter();

            exporter.InitializeContext();

            var context = exporter.Context;

            Process = context.ProcessInfo;

            exporter.Context.FileSystem.CheckinComment                          = App.Model.CheckinComment;
            exporter.Context.FileSystem.IntegrateWithTFSAndCheckInAutomatically = App.Config.IntegrateWithTFSAndCheckInAutomatically;

            exporter.Export(ProfileName);

            if (context.Errors.Any())
            {
                context.ProcessInfo.Text = string.Join(Environment.NewLine, context.Errors);
                Thread.Sleep(2000);
                refresher.Stop();
                return;
            }

            OnProcessCompletedSuccessfully();
        }
        #endregion
    }
}