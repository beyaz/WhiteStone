using System;
using System.Collections.Generic;
using System.Threading;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;
using static BOA.EntityGeneration.DataFlow.Data;

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
            context.Add(ProfileId, profileId);
            context.Add(ProcessedCustomSqlInfoListInProfile, new List<CustomSqlInfo>());

            InitializeProfileInfo(context);
            ProcessCustomSQLsInProfile(context);
            RemoveProfileInfo(context);

            context.Remove(ProcessedCustomSqlInfoListInProfile);
            context.Remove(ProfileId);

            var processInfo = context.Get(CustomSqlGenerationOfProfileIdProcess);
            processInfo.Text = "Finished Successfully.";
            WaitTwoSecondForUserCanSeeSuccessMessage();
        }
        #endregion

        #region Methods
        static void InitializeProfileInfo(IDataContext context)
        {
            var database  = context.Get(Database);
            var profileId = context.Get(ProfileId);
            var config    = context.Get(Config);

            context.Get(CustomSqlGenerationOfProfileIdProcess).Text = "Fetching profile informations...";
            context.Add(CustomSqlNamesInfProfile, ProjectCustomSqlInfoDataAccess.GetCustomSqlNamesInfProfile(database, profileId, config));

            context.FireEvent(OnProfileInfoInitialized);
        }

        static void ProcessCustomSQLsInProfile(IDataContext context)
        {
            var customSqlNamesInfProfile = context.Get(CustomSqlNamesInfProfile);
            var processInfo              = context.Get(CustomSqlGenerationOfProfileIdProcess);

            var config    = context.Get(Config);
            var database  = context.Get(Database);
            var profileId = context.Get(ProfileId);

            processInfo.Total = customSqlNamesInfProfile.Count;

            var switchCaseIndex = 0;
            foreach (var objectId in customSqlNamesInfProfile)
            {
                processInfo.Text    = $"Processing '{objectId}'";
                processInfo.Current = switchCaseIndex;

                var customSqlInfo = ProjectCustomSqlInfoDataAccess.GetCustomSqlInfo(database, profileId, objectId, config, switchCaseIndex++);

                context.Get(ProcessedCustomSqlInfoListInProfile).Add(customSqlInfo);

                context.Add(CustomSqlInfo, customSqlInfo);
                context.FireEvent(OnCustomSqlInfoInitialized);
                context.Remove(CustomSqlInfo);
            }
        }

        static void RemoveProfileInfo(IDataContext context)
        {
            context.FireEvent(OnProfileInfoRemove);
            context.Remove(CustomSqlNamesInfProfile);
        }

        static void WaitTwoSecondForUserCanSeeSuccessMessage()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
        #endregion

        #region Output Strings
        public static readonly IDataConstant<PaddedStringBuilder> SharedDalFile = DataConstant.Create<PaddedStringBuilder>(nameof(SharedDalFile));
        public static readonly IDataConstant<PaddedStringBuilder> BoaDalFile    = DataConstant.Create<PaddedStringBuilder>(nameof(BoaDalFile));
        #endregion

        #region Data
        public static readonly IDataConstant<List<CustomSqlInfo>> ProcessedCustomSqlInfoListInProfile = DataConstant.Create<List<CustomSqlInfo>>();

        public static readonly IDataConstant<string>        ProfileId     = DataConstant.Create<string>(nameof(ProfileId));
        public static readonly IDataConstant<CustomSqlInfo> CustomSqlInfo = DataConstant.Create<CustomSqlInfo>();

        public static readonly IDataConstant<ConfigurationContract> ConfigFile = DataConstant.Create<ConfigurationContract>(nameof(ConfigFile));

        public static readonly IDataConstant<List<string>> CustomSqlNamesInfProfile = DataConstant.Create<List<string>>(nameof(CustomSqlNamesInfProfile));

        public static readonly IDataConstant<IDatabase> Database = DataConstant.Create<IDatabase>();
        public static readonly IDataConstant<ProcessInfo> CustomSqlGenerationOfProfileIdProcess = DataConstant.Create<ProcessInfo>(nameof(CustomSqlGenerationOfProfileIdProcess));
        #endregion
    }
}