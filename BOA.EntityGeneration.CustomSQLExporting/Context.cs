using System;
using BOA.Collections;
using BOA.DatabaseAccess;
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
                OnError = error => { Errors.Add(error.ToString()); }
            };
        }
        #endregion

        #region Public Properties
        public CustomSQLExportingConfig          Config                 { get; set; }
        public CustomSqlInfo                  CustomSqlInfo          { get; set; }
        public CustomSqlNamingPatternContract CustomSqlNamingPattern { get; set; }
        public SqlDatabase                    Database               { get; set; }
        public AddOnlyList<string>            Errors                 { get; } = new AddOnlyList<string>();
        public FileSystem                     FileSystem             { get; } = new FileSystem();
        public MsBuildQueue                   MsBuildQueue           { get; }
        public ProcessContract                ProcessInfo            { get; } = new ProcessContract();
        public string                         ProfileName            { get; set; }
        public ProfileNamingPatternContract   ProfileNamingPattern   { get; set; }
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