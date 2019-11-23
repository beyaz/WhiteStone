using System;
using System.Collections.Generic;
using System.Threading;
using BOA.Common.Helpers;
using BOA.DataFlow;
using static BOA.EntityGeneration.CustomSQLExporting.Data;
using ContextContainer = BOA.EntityGeneration.CustomSQLExporting.Exporters.ContextContainer;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    class CustomSqlExporter : ContextContainer
    {
        #region Static Fields
        public static readonly Event OnCustomSqlInfoInitialized = Event.Create(nameof(OnCustomSqlInfoInitialized));
        public static readonly Event OnProfileInfoInitialized   = Event.Create(nameof(OnProfileInfoInitialized));
        public static readonly Event OnProfileInfoRemove        = Event.Create(nameof(OnProfileInfoRemove));
        #endregion

        #region Public Methods
        public void Export(string profileId)
        {
            var context = Context;

            context.OpenBracket();

            profileName = profileId;

            InitializeProfileNamingPattern();

            InitializeProfileInfo();
            ProcessCustomSQLsInProfile();
            RemoveProfileInfo();

            context.CloseBracket();

            processInfo.Text = "Finished Successfully.";
            WaitTwoSecondForUserCanSeeSuccessMessage();
        }
        #endregion


        void InitializeProfileNamingPattern()
        {

            var initialValues = new Dictionary<string, string>
            {
                {nameof(Data.ProfileName), profileName}
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.ProfileNamingPattern, initialValues);

            profileNamingPattern = new ProfileNamingPatternContract
            {
                SlnDirectoryPath             = dictionary[nameof(ProfileNamingPatternContract.SlnDirectoryPath)],
                EntityNamespace              = dictionary[nameof(ProfileNamingPatternContract.EntityNamespace)],
                RepositoryNamespace          = dictionary[nameof(ProfileNamingPatternContract.RepositoryNamespace)],
                EntityProjectDirectory       = dictionary[nameof(ProfileNamingPatternContract.EntityProjectDirectory)],
                RepositoryProjectDirectory   = dictionary[nameof(ProfileNamingPatternContract.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines      = dictionary[nameof(ProfileNamingPatternContract.BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines             = dictionary[nameof(ProfileNamingPatternContract.EntityUsingLines)].Split('|'),
                EntityAssemblyReferences     = dictionary[nameof(ProfileNamingPatternContract.EntityAssemblyReferences)].Split('|'),
                RepositoryAssemblyReferences = dictionary[nameof(ProfileNamingPatternContract.RepositoryAssemblyReferences)].Split('|')
            };
        }
        #region Methods
        static void WaitTwoSecondForUserCanSeeSuccessMessage()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        void InitializeProfileInfo()
        {

            processInfo.Text = "Fetching profile informations...";
            Context.Add(CustomSqlNamesInfProfile, ProjectCustomSqlInfoDataAccess.GetCustomSqlNamesInfProfile(database, profileName, config));

            Context.FireEvent(OnProfileInfoInitialized);
        }

        void ProcessCustomSQLsInProfile()
        {
            var customSqlNamesInfProfile = Context.Get(CustomSqlNamesInfProfile);


            processInfo.Total = customSqlNamesInfProfile.Count;

            var switchCaseIndex = 0;
            foreach (var objectId in customSqlNamesInfProfile)
            {
                processInfo.Text    = $"Processing '{objectId}'";
                processInfo.Current = switchCaseIndex;

                customSqlInfo = ProjectCustomSqlInfoDataAccess.GetCustomSqlInfo(database, profileName, objectId, config, switchCaseIndex++);

                Context.OpenBracket();
                Context.Add(CustomSqlInfo, customSqlInfo);
                CustomSqlNamingPatternInitializer.Initialize(Context);

                Context.FireEvent(OnCustomSqlInfoInitialized);

                Context.CloseBracket();
            }
        }

        void RemoveProfileInfo()
        {
            Context.FireEvent(OnProfileInfoRemove);
        }
        #endregion

       
    }
}