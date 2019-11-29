using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using BOA.EntityGeneration.SchemaToEntityExporting.DataAccess;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.CsprojFileExporters;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;

namespace BOA.EntityGeneration.SchemaToEntityExporting.Exporters
{
    /// <summary>
    ///     The schema exporter
    /// </summary>
    class SchemaExporter : ContextContainer
    {
        #region Public Properties
        public string ConfigFilePath { get; set; } = Path.GetDirectoryName(typeof(SchemaExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.SchemaToEntityExporting.json";
        #endregion

        #region Public Methods
        /// <summary>
        ///     Exports the specified schema name.
        /// </summary>
        public void Export(string schemaName)
        {
            Context.SchemaName = schemaName;

            InitializeNamingPattern();

            Context.FireSchemaExportStarted();
            Context.OnSchemaExportFinished();
        }

        public void InitializeContext()
        {
            Context = new Context();

            InitializeConfig();
            InitializeDatabase();

            AttachEvents();
        }
        #endregion

        #region Methods
        void AttachEvents()
        {
            var context = Context;
            TableExportStarted += InitializeTableNamingPattern;

            Create<EntityFileExporter>().AttachEvents();
            Create<SharedFileExporter>().AttachEvents();
            Create<BoaRepositoryFileExporter>().AttachEvents();
            Create<AllSchemaInOneClassRepositoryFileExporter>().AttachEvents();

            SchemaExportStarted += ProcessAllTablesInSchema;

            Create<EntityCsprojFileExporter>().AttachEvents();
            Create<RepositoryCsprojFileExporter>().AttachEvents();

            SchemaExportFinished += MsBuildQueue.Build;
        }

        void InitializeConfig()
        {
            Context.Config = JsonHelper.Deserialize<ConfigContract>(File.ReadAllText(ConfigFilePath));
            var configFilePath = Path.GetDirectoryName(typeof(SchemaExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.SchemaToEntityExporting.yaml";
            var c =  new YamlDotNet.Serialization.Deserializer().Deserialize<ConfigContract>(File.ReadAllText(configFilePath));
        }

        void InitializeDatabase()
        {
            Context.Database = new SqlDatabase(Config.ConnectionString) {CommandTimeout = 1000 * 60 * 60};
        }

        void InitializeNamingPattern()
        {
            var initialValues = new Dictionary<string, string> {{nameof(SchemaName), SchemaName}};

            var dictionary = ConfigurationDictionaryCompiler.Compile(Config.NamingPattern, initialValues);

            Context.NamingPattern = new NamingPatternContract
            {
                SlnDirectoryPath             = dictionary[nameof(NamingPatternContract.SlnDirectoryPath)],
                EntityNamespace              = dictionary[nameof(NamingPatternContract.EntityNamespace)],
                RepositoryNamespace          = dictionary[nameof(NamingPatternContract.RepositoryNamespace)],
                EntityProjectDirectory       = dictionary[nameof(NamingPatternContract.EntityProjectDirectory)],
                RepositoryProjectDirectory   = dictionary[nameof(NamingPatternContract.RepositoryProjectDirectory)],
                BoaRepositoryUsingLines      = dictionary[nameof(NamingPatternContract.BoaRepositoryUsingLines)].Split('|'),
                EntityUsingLines             = dictionary[nameof(NamingPatternContract.EntityUsingLines)].Split('|'),
                SharedRepositoryUsingLines   = dictionary[nameof(NamingPatternContract.SharedRepositoryUsingLines)].Split('|'),
                EntityAssemblyReferences     = dictionary[nameof(NamingPatternContract.EntityAssemblyReferences)].Split('|'),
                RepositoryAssemblyReferences = dictionary[nameof(NamingPatternContract.RepositoryAssemblyReferences)].Split('|')
            };


            dictionary = ConfigurationDictionaryCompiler.Compile(Config.AllSchemaInOneClassRepositoryNamingPattern, dictionary);

            Context.AllSchemaInOneClassRepositoryNamingPattern = new AllSchemaInOneClassRepositoryNamingPatternContract()
            {
                NamespaceName             = dictionary[nameof(AllSchemaInOneClassRepositoryNamingPatternContract.NamespaceName)],
                ClassName = dictionary[nameof(AllSchemaInOneClassRepositoryNamingPatternContract.ClassName)],
                UsingLines = dictionary[nameof(AllSchemaInOneClassRepositoryNamingPatternContract.UsingLines)].Split('|'),
                ExtraAssemblyReferences = dictionary[nameof(AllSchemaInOneClassRepositoryNamingPatternContract.ExtraAssemblyReferences)].Split('|')
            };
        }


        void InitializeTableNamingPattern()
        {
            var initialValues = new Dictionary<string, string>
            {
                {nameof(SchemaName), SchemaName},
                {"CamelCasedTableName", TableInfo.TableName.ToContractName()}
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(Config.TableNamingPattern, initialValues);

            Context.TableNamingPattern = new TableNamingPatternContract
            {
                EntityClassName                              = dictionary[nameof(TableNamingPatternContract.EntityClassName)],
                SharedRepositoryClassName                    = dictionary[nameof(TableNamingPatternContract.SharedRepositoryClassName)],
                BoaRepositoryClassName                       = dictionary[nameof(TableNamingPatternContract.BoaRepositoryClassName)],
                SharedRepositoryClassNameInBoaRepositoryFile = dictionary[nameof(TableNamingPatternContract.SharedRepositoryClassNameInBoaRepositoryFile)]
            };

            // TODO: move to usings
            var typeContractName = TableNamingPattern.EntityClassName;
            if (typeContractName == "TransactionLogContract" ||
                typeContractName == "BoaUserContract") // resolve conflig
            {
                typeContractName = $"{NamingPattern.EntityNamespace}.{typeContractName}";
            }

            Context.TableEntityClassNameForMethodParametersInRepositoryFiles = typeContractName;
        }

        bool IsReadyToExport(string tableName)
        {
            var fullTableName = $"{SchemaName}.{tableName}";

            var isNotExportable = Config.NotExportableTables.Contains(fullTableName);
            if (isNotExportable)
            {
                return false;
            }

            return true;
        }

        void ProcessAllTablesInSchema()
        {
            var tableNames = SchemaInfo.GetAllTableNamesInSchema(Database, SchemaName).ToList();

            tableNames = tableNames.Where(IsReadyToExport).ToList();

            ProcessInfo.Total   = tableNames.Count;
            ProcessInfo.Current = 0;

            foreach (var tableName in tableNames)
            {
                ProcessInfo.Text = $"Generating codes for table '{tableName}'.";
                ProcessInfo.Current++;

                var tableInfoDao = new TableInfoDao {Database = Database, IndexInfoAccess = new IndexInfoAccess {Database = Database}};

                var tableInfoTemp = tableInfoDao.GetInfo(Config.TableCatalog, SchemaName, tableName);

                Context.TableInfo = GeneratorDataCreator.Create(Config.SqlSequenceInformationOfTable, Config.DatabaseEnumName, Database, tableInfoTemp);

                Context.OnTableExportStarted();
                Context.OnTableExportFinished();
            }
        }
        #endregion
    }
}