﻿using System;
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
            processInfo  = new ProcessContract();
            MsBuildQueue = new MsBuildQueue {Trace = trace => { processInfo.Text = trace; }};
            FileSystem   = new FileSystem();
        }
        #endregion

        #region Public Properties
        public ConfigurationContract          config                 { get; set; }
        public CustomSqlInfo                  customSqlInfo          { get; set; }
        public CustomSqlNamingPatternContract customSqlNamingPattern { get; set; }
        public SqlDatabase                    database               { get; set; }
        public FileSystem                     FileSystem             { get; }
        public MsBuildQueue                   MsBuildQueue           { get; }
        public ProcessContract                processInfo            { get; }
        public string                         profileName            { get; set; }
        public ProfileNamingPatternContract   profileNamingPattern   { get; set; }
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