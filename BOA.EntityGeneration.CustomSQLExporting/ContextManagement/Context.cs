﻿using System;
using BOA.Collections;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting.ContextManagement
{
    class Context
    {
        #region Fields
        public readonly AddOnlyList<string> EntityAssemblyReferences     = new AddOnlyList<string>();
        public AddOnlyList<string> RepositoryAssemblyReferences { get; } = new AddOnlyList<string>();
        public readonly NamingMap NamingMap = new NamingMap();
        #endregion
        public AddOnlyList<string> RepositoryProjectSourceFileNames { get; } = new AddOnlyList<string>();
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
        public CustomSqlInfo                             CustomSqlInfo                     { get; set; }
        public SqlDatabase                               Database                          { get; set; }
        public AddOnlyList<string>                       Errors                            { get; } = new AddOnlyList<string>();
        public FileSystem                                FileSystem                        { get; } = new FileSystem();
        public MsBuildQueue                              MsBuildQueue                      { get; }
        public ProcessContract                           ProcessInfo                       { get; } = new ProcessContract();
        public string                                    ProfileName                       { get; set; }
        public ReferencedEntityTypeNamingPatternContract ReferencedEntityTypeNamingPattern { get; set; }
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