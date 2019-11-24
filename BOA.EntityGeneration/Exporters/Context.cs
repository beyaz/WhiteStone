using System;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.Models.Interfaces;
using BOA.EntityGeneration.Naming;

namespace BOA.EntityGeneration
{
    class Context
    {
        #region Constructors
        public Context()
        {
            processInfo  = new ProcessContract();
            MsBuildQueue = new MsBuildQueue {Trace = trace => { processInfo.Text = trace; }};
            FileSystem   = new FileSystem();
        }
        #endregion

        #region Public Properties
        public ConfigContract             config                                                   { get; set; }
        public IDatabase                  database                                                 { get; set; }
        public FileSystem                 FileSystem                                               { get; }
        public MsBuildQueue               MsBuildQueue                                             { get; }
        public NamingPatternContract      namingPattern                                            { get; set; }
        public ProcessContract            processInfo                                              { get; }
        public string                     SchemaName                                               { get; set; }
        public string                     tableEntityClassNameForMethodParametersInRepositoryFiles { get; set; }
        public ITableInfo                 tableInfo                                                { get; set; }
        public TableNamingPatternContract tableNamingPattern                                       { get; set; }
        #endregion

        #region TableExportFinished
        public event Action TableExportFinished;

        public void OnTableExportFinished()
        {
            TableExportFinished?.Invoke();
        }
        #endregion

        #region TableExportStarted
        public event Action TableExportStarted;

        public void OnTableExportStarted()
        {
            TableExportStarted?.Invoke();
        }
        #endregion

        #region SchemaExportStarted
        public event Action SchemaExportStarted;

        public void FireSchemaExportStarted()
        {
            SchemaExportStarted?.Invoke();
        }
        #endregion

        #region SchemaExportFinished
        public event Action SchemaExportFinished;

        public void OnSchemaExportFinished()
        {
            SchemaExportFinished?.Invoke();
        }
        #endregion

        #region EntityFileContentCompleted
        public event Action<string> EntityFileContentCompleted;

        public void OnEntityFileContentCompleted(string entityFileContent)
        {
            EntityFileContentCompleted?.Invoke(entityFileContent);
        }
        #endregion

        #region SharedRepositoryFileContentCompleted
        public event Action<string> SharedRepositoryFileContentCompleted;

        public void OnSharedRepositoryFileContentCompleted(string entityFileContent)
        {
            SharedRepositoryFileContentCompleted?.Invoke(entityFileContent);
        }
        #endregion
    }
}