using System;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;

namespace BOA.EntityGeneration
{
    class Context : BOA.DataFlow.Context
    {
        #region Public Properties
        public MsBuildQueue MsBuildQueue { get; } = new MsBuildQueue();
        #endregion

        #region TableExportFinished
        public event Action TableExportFinished;

        public void FireTableExportFinished()
        {
            TableExportFinished?.Invoke();
        }
        #endregion

        #region TableExportStarted
        public event Action TableExportStarted;

        public void FireTableExportStarted()
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

        public void FireSchemaExportFinished()
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