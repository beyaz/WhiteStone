using System;
using System.Threading;
using BOA.DataFlow;
using ContextContainer = BOA.EntityGeneration.CustomSQLExporting.Exporters.ContextContainer;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
     class CustomSqlExporter:ContextContainer
    {
        #region Static Fields
        public static readonly IEvent OnCustomSqlInfoInitialized = new Event {Name = nameof(OnCustomSqlInfoInitialized)};
        public static readonly IEvent OnProfileInfoInitialized   = new Event {Name = nameof(OnProfileInfoInitialized)};
        public static readonly IEvent OnProfileInfoRemove        = new Event {Name = nameof(OnProfileInfoRemove)};
        #endregion

        #region Public Methods
        public  void Export( string profileId)
        {
            var context = Context;
            context.OpenBracket();

            context.Add(Data.ProfileName, profileId);
            ProfileNamingPatternInitializer.Initialize(context);

            InitializeProfileInfo(context);
            ProcessCustomSQLsInProfile(context);
            RemoveProfileInfo(context);

            context.CloseBracket();

            var processInfo = context.Get(Data.ProcessInfo);
            processInfo.Text = "Finished Successfully.";
            WaitTwoSecondForUserCanSeeSuccessMessage();
        }
        #endregion

        #region Methods
        static void InitializeProfileInfo(IDataContext context)
        {
            var database    = context.Get(Data.Database);
            var profileName = context.Get(Data.ProfileName);
            var config      = context.Get(Data.Config);

            context.Get(Data.ProcessInfo).Text = "Fetching profile informations...";
            context.Add(Data.CustomSqlNamesInfProfile, ProjectCustomSqlInfoDataAccess.GetCustomSqlNamesInfProfile(database, profileName, config));

            context.FireEvent(OnProfileInfoInitialized);
        }

        static void ProcessCustomSQLsInProfile(IDataContext context)
        {
            var customSqlNamesInfProfile = context.Get(Data.CustomSqlNamesInfProfile);
            var processInfo              = context.Get(Data.ProcessInfo);

            var config      = context.Get(Data.Config);
            var database    = context.Get(Data.Database);
            var profileName = context.Get(Data.ProfileName);

            processInfo.Total = customSqlNamesInfProfile.Count;

            var switchCaseIndex = 0;
            foreach (var objectId in customSqlNamesInfProfile)
            {
                processInfo.Text    = $"Processing '{objectId}'";
                processInfo.Current = switchCaseIndex;

                var customSqlInfo = ProjectCustomSqlInfoDataAccess.GetCustomSqlInfo(database, profileName, objectId, config, switchCaseIndex++);

                context.OpenBracket();
                context.Add(Data.CustomSqlInfo, customSqlInfo);
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
        #endregion
    }
}