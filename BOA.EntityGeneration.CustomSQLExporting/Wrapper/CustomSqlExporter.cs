using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Exporters;
using BOA.EntityGeneration.CustomSQLExporting.Models;
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

        #region Constructors
       
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
        }
        #endregion

        #region Public Methods
        public void Export(string profileId)
        {

            Context.profileName          = profileId;
            Context.profileNamingPattern = CreateProfileNamingPattern();

            processInfo.Text = "Fetching profile informations...";

            var customSqlNamesInfProfile = ProjectCustomSqlInfoDataAccess.GetCustomSqlNamesInfProfile(database, ProfileName, config);

            Context.FireEvent(OnProfileInfoInitialized);

            processInfo.Total = customSqlNamesInfProfile.Count;

            var switchCaseIndex = 0;
            foreach (var objectId in customSqlNamesInfProfile)
            {
                processInfo.Text    = $"Processing '{objectId}'";
                processInfo.Current = switchCaseIndex;


                Context.customSqlInfo = ProjectCustomSqlInfoDataAccess.GetCustomSqlInfo(database, ProfileName, objectId, config, switchCaseIndex++);

                InitializeCustomSqlNamingPattern();

                Context.FireEvent(OnCustomSqlInfoInitialized);

            }

            Context.FireEvent(OnProfileInfoRemove);


            processInfo.Text = "Finished Successfully.";

            WaitTwoSecondForUserCanSeeSuccessMessage();
        }

        public IReadOnlyList<string> GetProfileNames()
        {
            var profileIdList = new List<string>();

            database.CommandText = config.SQL_GetProfileIdList;
            var reader = database.ExecuteReader();
            while (reader.Read())
            {
                profileIdList.Add(reader["ProfileId"].ToString());
            }

            reader.Close();

            profileIdList.Add("*");

            return profileIdList;
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

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.ProfileNamingPattern, initialValues);

            return new ProfileNamingPatternContract
            {
                SlnDirectoryPath             = dictionary[nameof(ProfileNamingPatternContract.SlnDirectoryPath)],
                EntityNamespace              = dictionary[nameof(ProfileNamingPatternContract.EntityNamespace)],
                RepositoryNamespace          = dictionary[nameof(ProfileNamingPatternContract.RepositoryNamespace)],
                EntityProjectDirectory       = dictionary[nameof(ProfileNamingPatternContract.EntityProjectDirectory)],
                RepositoryProjectDirectory   = dictionary[nameof(ProfileNamingPatternContract.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines      = dictionary[nameof(ProfileNamingPatternContract.BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines             = dictionary[nameof(ProfileNamingPatternContract.EntityUsingLines)].Split('|'),
                EntityAssemblyReferences     = dictionary[nameof(ProfileNamingPatternContract.EntityAssemblyReferences)].Split('|').ToList(),
                RepositoryAssemblyReferences = dictionary[nameof(ProfileNamingPatternContract.RepositoryAssemblyReferences)].Split('|').ToList()
            };
        }

        public string ConfigFilePath { get; set; } = Path.GetDirectoryName(typeof(CustomSqlExporter).Assembly.Location) + Path.DirectorySeparatorChar + "CustomSQLExporting.json";
        void InitializeConfig()
        {
            Context.config = JsonHelper.Deserialize<ConfigurationContract>(File.ReadAllText(ConfigFilePath));
        }

        void InitializeCustomSqlNamingPattern()
        {
            var initialValues = new Dictionary<string, string>
            {
                {nameof(ProfileName), ProfileName},
                {"CamelCasedCustomSqlName", customSqlInfo.Name.ToContractName()}
            };

            var entityReferencedResultColumn = customSqlInfo.ResultColumns.FirstOrDefault(x => x.IsReferenceToEntity);

            if (entityReferencedResultColumn != null)
            {
                initialValues[nameof(customSqlInfo.SchemaName)] = customSqlInfo.SchemaName;
                initialValues["CamelCasedResultName"]           = entityReferencedResultColumn.Name.ToContractName();
            }

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.CustomSqlNamingPattern, initialValues);

            Context.customSqlNamingPattern = new CustomSqlNamingPatternContract
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
            Context.database = new SqlDatabase(config.ConnectionString) {CommandTimeout = 1000 * 60 * 60};
        }

        
        #endregion
    }
}