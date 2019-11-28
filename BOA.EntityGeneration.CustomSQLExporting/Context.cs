using System;
using System.Collections.Generic;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    class Context
    {
        #region Constructors
        public Context()
        {
            MsBuildQueue = new MsBuildQueue
            {
                Trace   = trace => { ProcessInfo.Text = trace; },
                OnError = error => { _errors.Add(error.ToString()); }
            };
        }
        #endregion

        #region Public Properties
        public ConfigurationContract          Config                 { get; set; }
        public CustomSqlInfo                  CustomSqlInfo          { get; set; }
        public CustomSqlNamingPatternContract CustomSqlNamingPattern { get; set; }
        public SqlDatabase                    Database               { get; set; }
        public FileSystem                     FileSystem             { get; } = new FileSystem();
        public MsBuildQueue                   MsBuildQueue           { get; }
        public ProcessContract                ProcessInfo            { get; } = new ProcessContract();
        public string                         ProfileName            { get; set; }
        public ProfileNamingPatternContract   ProfileNamingPattern   { get; set; }
        #endregion

        #region Errors
        readonly List<string> _errors = new List<string>();

        public IReadOnlyList<string> Errors => _errors;

        public void AddError(string errorMessage)
        {
            _errors.Add(errorMessage);
        }
        #endregion

        #region CustomSqlInfoInitialized
        public event Action CustomSqlInfoInitialized;

        public void OnCustomSqlInfoInitialized()
        {
            CustomSqlInfoInitialized?.Invoke();
        }
        #endregion

        #region ProfileInfoInitialized
        public event Action ProfileInfoInitialized;

        public void OnProfileInfoInitialized()
        {
            ProfileInfoInitialized?.Invoke();
        }
        #endregion

        #region ProfileInfoRemove
        public event Action ProfileInfoRemove;

        public void OnProfileInfoRemove()
        {
            ProfileInfoRemove?.Invoke();
        }
        #endregion
    }
}