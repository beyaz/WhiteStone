﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.CustomSQLExporting.Exporters;
using BOA.EntityGeneration.CustomSQLExporting.Exporters.BoaRepositoryExporting;
using BOA.EntityGeneration.CustomSQLExporting.Exporters.SharedFileExporting;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    class CustomSqlExporter : ContextContainer
    {

        #region Public Methods
        public void Export(string profileId)
        {
            Context.ProfileName          = profileId;
            Context.ProfileNamingPattern = CreateProfileNamingPattern();

            ProcessInfo.Text = "Fetching profile informations...";

            var customSqlNamesInfProfile = ProjectCustomSqlInfoDataAccess.GetCustomSqlNamesInfProfile(Database, ProfileName, Config);

            Context.OnProfileInfoInitialized();

            ProcessInfo.Total = customSqlNamesInfProfile.Count;

            var switchCaseIndex = 0;
            foreach (var objectId in customSqlNamesInfProfile)
            {
                ProcessInfo.Text    = $"Processing '{objectId}'";
                ProcessInfo.Current = switchCaseIndex;

                Context.CustomSqlInfo = ProjectCustomSqlInfoDataAccess.GetCustomSqlInfo(Database, ProfileName, objectId, Config, switchCaseIndex++);

                InitializeCustomSqlNamingPattern();

                Context.OnCustomSqlInfoInitialized();
            }

            Context.OnProfileInfoRemove();

            ProcessInfo.Text = "Finished Successfully.";

            WaitTwoSecondForUserCanSeeSuccessMessage();
        }

        public IReadOnlyList<string> GetProfileNames()
        {
            var profileIdList = new List<string>();

            Database.CommandText = Config.SQL_GetProfileIdList;
            var reader = Database.ExecuteReader();
            while (reader.Read())
            {
                profileIdList.Add(reader["ProfileId"].ToString());
            }

            reader.Close();

            profileIdList.Add("*");

            return profileIdList;
        }

        public void InitializeContext()
        {
            Context = new Context();
            InitializeConfig();
            InitializeDatabaseConnection();

            // attach events
            Create<EntityFileExporter>().AttachEvents();
            Create<SharedFileExporter>().AttachEvents();
            Create<BoaRepositoryFileExporter>().AttachEvents();
            Create<EntityCsprojFileExporter>().AttachEvents();
            Create<RepositoryCsprojFileExporter>().AttachEvents();

            Context.ProfileInfoRemove += MsBuildQueue.Build;
        }
        #endregion

        #region Methods
        static void WaitTwoSecondForUserCanSeeSuccessMessage()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        ProfileNamingPatternContract CreateProfileNamingPattern()
        {
            var initialValues = new Dictionary<string, string>
            {
                {nameof(ProfileName), ProfileName}
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(Config.ProfileNamingPattern, initialValues);

            return new ProfileNamingPatternContract
            {
                SlnDirectoryPath             = dictionary[nameof(ProfileNamingPatternContract.SlnDirectoryPath)],
                EntityNamespace              = dictionary[nameof(ProfileNamingPatternContract.EntityNamespace)],
                RepositoryNamespace          = dictionary[nameof(ProfileNamingPatternContract.RepositoryNamespace)],
                EntityProjectDirectory       = dictionary[nameof(ProfileNamingPatternContract.EntityProjectDirectory)],
                RepositoryProjectDirectory   = dictionary[nameof(ProfileNamingPatternContract.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines      = dictionary[nameof(ProfileNamingPatternContract.BoaRepositoryUsingLines)].SplitAndClear("|"),
                EntityUsingLines             = dictionary[nameof(ProfileNamingPatternContract.EntityUsingLines)].SplitAndClear("|"),
                EntityAssemblyReferences     = dictionary[nameof(ProfileNamingPatternContract.EntityAssemblyReferences)].SplitAndClear("|").ToList(),
                RepositoryAssemblyReferences = dictionary[nameof(ProfileNamingPatternContract.RepositoryAssemblyReferences)].SplitAndClear("|").ToList()
            };
        }

        void InitializeConfig()
        {
            Context.Config = CustomSQLExportingConfig.CreateFromFile();
        }

        void InitializeCustomSqlNamingPattern()
        {
            var initialValues = new Dictionary<string, string>
            {
                {nameof(ProfileName), ProfileName},
                {"CamelCasedCustomSqlName", CustomSqlInfo.Name.ToContractName()}
            };

            var entityReferencedResultColumn = CustomSqlInfo.ResultColumns.FirstOrDefault(x => x.IsReferenceToEntity);

            if (entityReferencedResultColumn != null)
            {
                initialValues[nameof(CustomSqlInfo.SchemaName)] = CustomSqlInfo.SchemaName;
                initialValues["CamelCasedResultName"]           = entityReferencedResultColumn.Name.ToContractName();
            }

            var dictionary = ConfigurationDictionaryCompiler.Compile(Config.CustomSqlNamingPattern, initialValues);

            Context.CustomSqlNamingPattern = new CustomSqlNamingPatternContract
            {
                ResultClassName                  = dictionary[nameof(CustomSqlNamingPatternContract.ResultClassName)],
                RepositoryClassName              = dictionary[nameof(CustomSqlNamingPatternContract.RepositoryClassName)],
                InputClassName                   = dictionary[nameof(CustomSqlNamingPatternContract.InputClassName)],
                ReferencedEntityAccessPath       = dictionary[nameof(CustomSqlNamingPatternContract.ReferencedEntityAccessPath)],
                ReferencedEntityAssemblyPath     = dictionary[nameof(CustomSqlNamingPatternContract.ReferencedEntityAssemblyPath)],
                ReferencedEntityReaderMethodPath = dictionary[nameof(CustomSqlNamingPatternContract.ReferencedEntityReaderMethodPath)],
                ReferencedRepositoryAssemblyPath = dictionary[nameof(CustomSqlNamingPatternContract.ReferencedRepositoryAssemblyPath)]
            };
        }

        void InitializeDatabaseConnection()
        {
            Context.Database = new SqlDatabase(Config.ConnectionString) {CommandTimeout = 1000 * 60 * 60};
        }
        #endregion
    }
}