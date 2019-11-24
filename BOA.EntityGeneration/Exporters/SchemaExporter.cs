using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.BoaRepositoryFileExporting;
using BOA.EntityGeneration.CsprojFileExporters;
using BOA.EntityGeneration.DataAccess;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.SharedRepositoryFileExporting;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;


namespace BOA.EntityGeneration
{
     class Context : BOA.DataFlow.Context
    {
        public event Action OnTableExportFinished;

        public  event Action OnTableExportStarted;

        public void FireOnTableExportStarted()
        {
            OnTableExportStarted?.Invoke();
        }

        public void FireOnTableExportFinished()
        {
            OnTableExportFinished?.Invoke();
        }
    }
}


namespace BOA.EntityGeneration.Exporters
{
    /// <summary>
    ///     The schema exporter
    /// </summary>
    class SchemaExporter : ContextContainer
    {
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

            schemaName = schemaNametodo;

            InitializeNamingPattern();

            context.FireEvent(SchemaExportStarted);
            context.FireEvent(SchemaExportFinished);

            context.CloseBracket();
        }

        public void InitializeContext()
        {
            Context = new Context();

            InitializeConfig();
            InitializeDatabase();
            InitializeProcessInfo();
            MsBuildQueue = new MsBuildQueue();

            AttachEvents();
        }
        #endregion

        #region Methods
        void AttachEvents()
        {
            var context = Context;
            OnTableExportStarted+=InitializeTableNamingPattern;

            new EntityFileExporter {Context = context}.AttachEvents();
            new SharedFileExporter {Context = context}.AttachEvents();
            new BoaRepositoryFileExporter {Context = context}.AttachEvents();

            OnSchemaExportStarted += ProcessAllTablesInSchema;

            Create<EntityCsprojFileExporter>().AttachEvents();
            Create<RepositoryCsprojFileExporter>().AttachEvents();

            context.AttachEvent(SchemaExportStarted, MsBuildQueue.Build);
        }

        void InitializeConfig()
        {
            config = JsonHelper.Deserialize<ConfigContract>(File.ReadAllText(ConfigFilePath));
        }

        void InitializeDatabase()
        {
            database = new SqlDatabase(config.ConnectionString) {CommandTimeout = 1000 * 60 * 60};
        }

        void InitializeNamingPattern()
        {
            var initialValues = new Dictionary<string, string> {{nameof(Data.SchemaName), schemaName}};

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.NamingPattern, initialValues);

            namingPattern = new NamingPatternContract
            {
                SlnDirectoryPath           = dictionary[nameof(NamingPatternContract.SlnDirectoryPath)],
                EntityNamespace            = dictionary[nameof(NamingPatternContract.EntityNamespace)],
                RepositoryNamespace        = dictionary[nameof(NamingPatternContract.RepositoryNamespace)],
                EntityProjectDirectory     = dictionary[nameof(NamingPatternContract.EntityProjectDirectory)],
                RepositoryProjectDirectory = dictionary[nameof(NamingPatternContract.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines    = dictionary[nameof(NamingPatternContract.BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines           = dictionary[nameof(NamingPatternContract.EntityUsingLines)].Split('|'),
                SharedRepositoryUsingLines = dictionary[nameof(NamingPatternContract.SharedRepositoryUsingLines)].Split('|'),
                EntityAssemblyReferences = dictionary[nameof(NamingPatternContract.EntityAssemblyReferences)].Split('|'),
                RepositoryAssemblyReferences = dictionary[nameof(NamingPatternContract.RepositoryAssemblyReferences)].Split('|'),
        
            };
        }

        void InitializeProcessInfo()
        {
            var processContract = new ProcessContract();

            processInfo = processContract;

            Context.Add(MsBuildQueue.ProcessInfo, processContract);
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

            tableEntityClassNameForMethodParametersInRepositoryFiles = typeContractName;
        }

        bool IsReadyToExport(string tableName)
        {
            var fullTableName = $"{schemaName}.{tableName}";

            var isNotExportable = config.NotExportableTables.Contains(fullTableName);
            if (isNotExportable)
            {
                return false;
            }

            return true;
        }

        void ProcessAllTablesInSchema()
        {
            var tableNames = SchemaInfo.GetAllTableNamesInSchema(database, schemaName).ToList();

            tableNames = tableNames.Where(IsReadyToExport).ToList();

            processInfo.Total   = tableNames.Count;
            processInfo.Current = 0;

            foreach (var tableName in tableNames)
            {
                processInfo.Text = $"Generating codes for table '{tableName}'.";
                processInfo.Current++;

                var tableInfoDao = new TableInfoDao {Database = database, IndexInfoAccess = new IndexInfoAccess {Database = database}};

                var tableInfoTemp = tableInfoDao.GetInfo(config.TableCatalog, schemaName, tableName);

                tableInfo = GeneratorDataCreator.Create(config.SqlSequenceInformationOfTable, config.DatabaseEnumName, database, tableInfoTemp);

                Context.OpenBracket();

                Context.FireOnTableExportStarted();
                Context.FireOnTableExportFinished();

                Context.CloseBracket();
            }
        }
        #endregion
    }
}