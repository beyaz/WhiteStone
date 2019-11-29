using System;
using BOA.Collections;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.SchemaToEntityExporting.DbModels;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters
{
    class Context
    {


        public AddOnlyList<string> RepositoryAssemblyReferences { get; } = new AddOnlyList<string>();

        #region Constructors
        public Context()
        {
            MsBuildQueue = new MsBuildQueue
            {
                Trace   = trace => { ProcessInfo.Text = trace; },
                OnError = error => { ErrorList.Add(error.ToString()); }
            };
        }
        #endregion

        #region Public Properties
        public IDatabase                  Database                                                 { get; set; }
        public AddOnlyList<string>        EntityProjectSourceFileNames                             { get; } = new AddOnlyList<string>();
        public AddOnlyList<string>        ErrorList                                                { get; } = new AddOnlyList<string>();
        public FileSystem                 FileSystem                                               { get; } = new FileSystem();
        public MsBuildQueue               MsBuildQueue                                             { get; }
        public NamingPatternContract      NamingPattern                                            { get; set; }
        public ProcessContract            ProcessInfo                                              { get; } = new ProcessContract();
        public AddOnlyList<string>        RepositoryProjectSourceFileNames                         { get; } = new AddOnlyList<string>();
        public string                     SchemaName                                               { get; set; }
        public string                     TableEntityClassNameForMethodParametersInRepositoryFiles { get; set; }
        public ITableInfo                 TableInfo                                                { get; set; }
        public TableNamingPatternContract TableNamingPattern                                       { get; set; }
        
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