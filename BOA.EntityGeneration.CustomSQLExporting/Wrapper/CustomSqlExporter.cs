﻿using System;
using System.Collections.Generic;
using System.Threading;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    public static class CustomSqlExporter
    {
        #region Static Fields
        public static readonly IEvent OnCustomSqlInfoInitialized = new Event {Name = nameof(OnCustomSqlInfoInitialized)};
        public static readonly IEvent OnProfileInfoInitialized   = new Event {Name = nameof(OnProfileInfoInitialized)};
        public static readonly IEvent OnProfileInfoRemove        = new Event {Name = nameof(OnProfileInfoRemove)};
        #endregion

        #region Public Methods
        public static void Export(IDataContext context, string profileId)
        {
            context.OpenBracket();

            context.Add(ProfileName, profileId);
            ProfileNamingPatternInitializer.Initialize(context);

            InitializeProfileInfo(context);
            ProcessCustomSQLsInProfile(context);
            RemoveProfileInfo(context);

            context.CloseBracket();

            var processInfo = context.Get(ProcessInfo);
            processInfo.Text = "Finished Successfully.";
            WaitTwoSecondForUserCanSeeSuccessMessage();
        }
        #endregion

        #region Methods
        static void InitializeProfileInfo(IDataContext context)
        {
            var database    = context.Get(Database);
            var profileName = context.Get(ProfileName);
            var config      = context.Get(Config);

            context.Get(ProcessInfo).Text = "Fetching profile informations...";
            context.Add(CustomSqlNamesInfProfile, ProjectCustomSqlInfoDataAccess.GetCustomSqlNamesInfProfile(database, profileName, config));

            context.FireEvent(OnProfileInfoInitialized);
        }

        static void ProcessCustomSQLsInProfile(IDataContext context)
        {
            var customSqlNamesInfProfile = context.Get(CustomSqlNamesInfProfile);
            var processInfo              = context.Get(ProcessInfo);

            var config      = context.Get(Config);
            var database    = context.Get(Database);
            var profileName = context.Get(ProfileName);

            processInfo.Total = customSqlNamesInfProfile.Count;

            var switchCaseIndex = 0;
            foreach (var objectId in customSqlNamesInfProfile)
            {
                processInfo.Text    = $"Processing '{objectId}'";
                processInfo.Current = switchCaseIndex;

                var customSqlInfo = ProjectCustomSqlInfoDataAccess.GetCustomSqlInfo(database, profileName, objectId, config, switchCaseIndex++);

                context.OpenBracket();
                context.Add(CustomSqlInfo, customSqlInfo);
                CustomSqlNamingPatternInitializer.Initialize(context);

                context.FireEvent(OnCustomSqlInfoInitialized);

                context.CloseBracket();
            }
        }

        static void RemoveProfileInfo(IDataContext context)
        {
            context.FireEvent(OnProfileInfoRemove);
        }

        static void WaitTwoSecondForUserCanSeeSuccessMessage()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
        #endregion

        #region Output Strings
        #endregion

        #region Data
        public static readonly IDataConstant<string> ProfileName = DataConstant.Create<string>(nameof(ProfileName));

        public static readonly IDataConstant<CustomSqlInfo> CustomSqlInfo = DataConstant.Create<CustomSqlInfo>();

        public static readonly IDataConstant<ConfigurationContract> Config = DataConstant.Create<ConfigurationContract>(nameof(Config));

        public static readonly IDataConstant<List<string>> CustomSqlNamesInfProfile = DataConstant.Create<List<string>>(nameof(CustomSqlNamesInfProfile));

        public static readonly IDataConstant<IDatabase>       Database    = DataConstant.Create<IDatabase>();
        public static readonly IDataConstant<ProcessContract> ProcessInfo = DataConstant.Create<ProcessContract>(nameof(ProcessInfo));
        #endregion
    }
}