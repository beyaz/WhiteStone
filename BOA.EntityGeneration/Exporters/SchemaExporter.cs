using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.BoaRepositoryFileExporting;
using BOA.EntityGeneration.CsprojFileExporters;
using BOA.EntityGeneration.DataAccess;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.SharedRepositoryFileExporting;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;
using static BOA.EntityGeneration.DataFlow.TableExportingEvent;

namespace BOA.EntityGeneration.Exporters
{
    /// <summary>
    ///     The schema exporter
    /// </summary>
    class SchemaExporter : ContextContainer
    {
        #region Constructors
        public SchemaExporter()
        {
            Context = new Context();

            InitializeConfig();
            InitializeDatabase();
            InitializeProcessInfo();
            MsBuildQueue = new MsBuildQueue();

            AttachEvents();
        }
        #endregion

        #region Public Properties
        public string ConfigFilePath { get; set; } = Path.GetDirectoryName(typeof(SchemaExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
        #endregion

        #region Public Methods
        /// <summary>
        ///     Exports the specified schema name.
        /// </summary>
        public void Export(string schemaNametodo)
        {
            var context = Context;

            context.OpenBracket();

            this.schemaName = schemaNametodo;

            InitializeNamingPattern();

            context.FireEvent(SchemaExportStarted);
            context.FireEvent(SchemaExportFinished);

            context.CloseBracket();
        }
        #endregion
         void InitializeNamingPattern( )
        {

            var initialValues = new Dictionary<string, string> {{nameof(Data.SchemaName), schemaName}};

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.NamingPattern, initialValues);

            namingPattern= new NamingPatternContract
            {
                SlnDirectoryPath           = dictionary[nameof(NamingPatternContract.SlnDirectoryPath)],
                EntityNamespace            = dictionary[nameof(NamingPatternContract.EntityNamespace)],
                RepositoryNamespace        = dictionary[nameof(NamingPatternContract.RepositoryNamespace)],
                EntityProjectDirectory     = dictionary[nameof(NamingPatternContract.EntityProjectDirectory)],
                RepositoryProjectDirectory = dictionary[nameof(NamingPatternContract.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines    = dictionary[nameof(NamingPatternContract.BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines           = dictionary[nameof(NamingPatternContract.EntityUsingLines)].Split('|'),
                SharedRepositoryUsingLines = dictionary[nameof(NamingPatternContract.SharedRepositoryUsingLines)].Split('|')
            };
        }
        #region Methods
         void AttachEvents( )
         {
             var context = Context;
            AttachEvent(TableExportStarted, InitializeTableNamingPattern);

            new EntityFileExporter {Context = context}.AttachEvents();
            new SharedFileExporter {Context = context}.AttachEvents();
            new BoaRepositoryFileExporter {Context = context}.AttachEvents();

            context.AttachEvent(SchemaExportStarted, Events.OnSchemaStartedToExport);

            context.AttachEvent(SchemaExportFinished, EntityCsprojFileExporter.Export);
            context.AttachEvent(SchemaExportFinished, RepositoryCsprojFileExporter.Export);

            context.AttachEvent(SchemaExportStarted, MsBuildQueue.Build);
        }
         void InitializeTableNamingPattern()
         {

             var initialValues = new Dictionary<string, string>
             {
                 {nameof(Data.SchemaName), schemaName},
                 {"CamelCasedTableName", tableInfo.TableName.ToContractName()}
             };

             var dictionary = ConfigurationDictionaryCompiler.Compile(config.TableNamingPattern, initialValues);

             tableNamingPattern = new TableNamingPatternContract
             {
                 EntityClassName                              = dictionary[nameof(TableNamingPatternContract.EntityClassName)],
                 SharedRepositoryClassName                    = dictionary[nameof(TableNamingPatternContract.SharedRepositoryClassName)],
                 BoaRepositoryClassName                       = dictionary[nameof(TableNamingPatternContract.BoaRepositoryClassName)],
                 SharedRepositoryClassNameInBoaRepositoryFile = dictionary[nameof(TableNamingPatternContract.SharedRepositoryClassNameInBoaRepositoryFile)]
             };

             // TODO: move to usings
             var typeContractName = tableNamingPattern.EntityClassName;
             if (typeContractName == "TransactionLogContract" ||
                 typeContractName == "BoaUserContract") // resolve conflig
             {
                 typeContractName = $"{namingPattern.EntityNamespace}.{typeContractName}";
             }

             tableEntityClassNameForMethodParametersInRepositoryFiles= typeContractName;
         }
        

        void InitializeConfig()
        {
            config = JsonHelper.Deserialize<ConfigContract>(File.ReadAllText(ConfigFilePath));
        }

        void InitializeDatabase()
        {
            database = new SqlDatabase(config.ConnectionString) {CommandTimeout = 1000 * 60 * 60};
        }

        void InitializeProcessInfo()
        {
            var processContract = new ProcessContract();

            processInfo = processContract;

            Context.Add(MsBuildQueue.ProcessInfo, processContract);
        }
        #endregion
    }
}