using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using BOA.EntityGeneration.SchemaToEntityExporting.DataAccess;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.CsprojFileExporters;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.EntityFileExporting;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.SharedFileExporting;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;
using NamingPatternContract = BOA.EntityGeneration.SchemaToEntityExporting.Models.NamingPatternContract;

namespace BOA.EntityGeneration.SchemaToEntityExporting.Exporters
{
    /// <summary>
    ///     The schema exporter
    /// </summary>
    class SchemaExporter : ContextContainer
    {
        #region Static Fields
        internal static readonly SchemaExporterConfig Config = SchemaExporterConfig.CreateFromFile();
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

            InitializeDatabase();

            AttachEvents();
        }
        #endregion

        #region Methods
        void AttachEvents()
        {
            TableExportStarted += InitializeTableNamingPattern;

            Create<EntityFileExporter>().AttachEvents();
            Create<SharedFileExporter>().AttachEvents();

            if (Config.CanExportBoaRepository)
            {
                Create<BoaRepositoryFileExporter>().AttachEvents();    
            }
            if (Config.CanExportAllSchemaInOneClassRepository)
            {
                Create<FileExporter>().AttachEvents();
            }
            
            

            SchemaExportStarted += ProcessAllTablesInSchema;

            Create<EntityCsprojFileExporter>().AttachEvents();
            Create<RepositoryCsprojFileExporter>().AttachEvents();

            SchemaExportFinished += MsBuildQueue.Build;
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
                BoaRepositoryUsingLines      = dictionary[nameof(NamingPatternContract.BoaRepositoryUsingLines)].SplitAndClear("|"),
                EntityUsingLines             = dictionary[nameof(NamingPatternContract.EntityUsingLines)].SplitAndClear("|"),
                SharedRepositoryUsingLines   = dictionary[nameof(NamingPatternContract.SharedRepositoryUsingLines)].SplitAndClear("|"),
                EntityAssemblyReferences     = dictionary[nameof(NamingPatternContract.EntityAssemblyReferences)].SplitAndClear("|"),
                RepositoryAssemblyReferences = dictionary[nameof(NamingPatternContract.RepositoryAssemblyReferences)].SplitAndClear("|"),
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