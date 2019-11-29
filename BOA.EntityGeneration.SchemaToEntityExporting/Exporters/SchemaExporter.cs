﻿using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using BOA.EntityGeneration.SchemaToEntityExporting.DataAccess;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.BankingRepositoryFileExporting;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.CsprojFileExporters;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;
using NamingPatternContract = BOA.EntityGeneration.SchemaToEntityExporting.Models.NamingPatternContract;

namespace BOA.EntityGeneration.SchemaToEntityExporting.Exporters
{
    class SchemaExporterConfig
    {
        /// <summary>
        ///     Gets or sets the SQL sequence information of table.
        /// </summary>
        public string SqlSequenceInformationOfTable { get; set; }

        /// <summary>
        ///     Gets or sets the table catalog.
        /// </summary>
        public string TableCatalog { get; set; }

        /// <summary>
        ///     Gets or sets the not exportable tables.
        /// </summary>
        public string[] NotExportableTables { get; set; }

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }
        
        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        public string DatabaseEnumName { get; set; }


        /// <summary>
        ///     Gets or sets the naming pattern.
        /// </summary>
        public Dictionary<string, string> NamingPattern { get; set; }

       

       
        
       


        

        /// <summary>
        ///     Gets or sets the table naming pattern.
        /// </summary>
        public Dictionary<string, string> TableNamingPattern { get; set; }

        public static SchemaExporterConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<SchemaExporterConfig>(ContextContainer.ConfigDirectory + nameof(SchemaExporterConfig) + ".yaml");
        }
    }

    /// <summary>
    ///     The schema exporter
    /// </summary>
    class SchemaExporter : ContextContainer
    {
        static readonly SchemaExporterConfig SchemaExporterConfig = SchemaExporterConfig.CreateFromFile();

        

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
            Create<BoaRepositoryFileExporter>().AttachEvents();
            Create<FileExporter>().AttachEvents();

            SchemaExportStarted += ProcessAllTablesInSchema;

            Create<EntityCsprojFileExporter>().AttachEvents();
            Create<RepositoryCsprojFileExporter>().AttachEvents();

            SchemaExportFinished += MsBuildQueue.Build;
        }

        

        void InitializeDatabase()
        {
            Context.Database = new SqlDatabase(SchemaExporterConfig.ConnectionString) {CommandTimeout = 1000 * 60 * 60};
        }

        void InitializeNamingPattern()
        {
            var initialValues = new Dictionary<string, string> {{nameof(SchemaName), SchemaName}};

            var dictionary = ConfigurationDictionaryCompiler.Compile(SchemaExporterConfig.NamingPattern, initialValues);

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



            
        }


        void InitializeTableNamingPattern()
        {
            var initialValues = new Dictionary<string, string>
            {
                {nameof(SchemaName), SchemaName},
                {"CamelCasedTableName", TableInfo.TableName.ToContractName()}
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(SchemaExporterConfig.TableNamingPattern, initialValues);

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

            var isNotExportable = SchemaExporterConfig.NotExportableTables.Contains(fullTableName);
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

                var tableInfoTemp = tableInfoDao.GetInfo(SchemaExporterConfig.TableCatalog, SchemaName, tableName);

                Context.TableInfo = GeneratorDataCreator.Create(SchemaExporterConfig.SqlSequenceInformationOfTable, SchemaExporterConfig.DatabaseEnumName, Database, tableInfoTemp);

                Context.OnTableExportStarted();
                Context.OnTableExportFinished();
            }
        }
        #endregion
    }
}