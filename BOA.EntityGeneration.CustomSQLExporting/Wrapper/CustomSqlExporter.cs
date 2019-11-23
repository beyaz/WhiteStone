using System;
using System.Threading;
using BOA.DataFlow;
using ContextContainer = BOA.EntityGeneration.CustomSQLExporting.Exporters.ContextContainer;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    class CustomSqlExporter : ContextContainer
    {
        #region Static Fields
        public static readonly Event OnCustomSqlInfoInitialized = new Event {Name = nameof(OnCustomSqlInfoInitialized)};
        public static readonly Event OnProfileInfoInitialized   = new Event {Name = nameof(OnProfileInfoInitialized)};
        public static readonly Event OnProfileInfoRemove        = new Event {Name = nameof(OnProfileInfoRemove)};
        #endregion

        #region Public Methods
        public void Export(string profileId)
        {
            var context = Context;
            context.OpenBracket();

            context.Add(Data.ProfileName, profileId);
            ProfileNamingPatternInitializer.Initialize(context);

            InitializeProfileInfo();
            ProcessCustomSQLsInProfile();
            RemoveProfileInfo();

            context.CloseBracket();

            processInfo.Text = "Finished Successfully.";
            WaitTwoSecondForUserCanSeeSuccessMessage();
        }
        #endregion

        #region Methods
        void InitializeProfileInfo()
        {
            var database    = Context.Get(Data.Database);
            var profileName = Context.Get(Data.ProfileName);
            var config      = Context.Get(Data.Config);

            processInfo.Text = "Fetching profile informations...";
            Context.Add(Data.CustomSqlNamesInfProfile, ProjectCustomSqlInfoDataAccess.GetCustomSqlNamesInfProfile(database, profileName, config));

            Context.FireEvent(OnProfileInfoInitialized);
        }

        void ProcessCustomSQLsInProfile()
        {
            var customSqlNamesInfProfile = Context.Get(Data.CustomSqlNamesInfProfile);

            var config      = Context.Get(Data.Config);
            var database    = Context.Get(Data.Database);
            var profileName = Context.Get(Data.ProfileName);

            processInfo.Total = customSqlNamesInfProfile.Count;

            var switchCaseIndex = 0;
            foreach (var objectId in customSqlNamesInfProfile)
            {
                processInfo.Text    = $"Processing '{objectId}'";
                processInfo.Current = switchCaseIndex;

                customSqlInfo = ProjectCustomSqlInfoDataAccess.GetCustomSqlInfo(database, profileName, objectId, config, switchCaseIndex++);

                Context.OpenBracket();
                Context.Add(Data.CustomSqlInfo, customSqlInfo);
                CustomSqlNamingPatternInitializer.Initialize(Context);

                Context.FireEvent(OnCustomSqlInfoInitialized);

                Context.CloseBracket();
            }
        }

        void RemoveProfileInfo()
        {
            Context.FireEvent(OnProfileInfoRemove);
        }

        void WaitTwoSecondForUserCanSeeSuccessMessage()
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