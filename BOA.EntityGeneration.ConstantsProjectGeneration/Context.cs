using System.Collections.Generic;
using System.IO;
using BOA.Collections;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace BOA.EntityGeneration.ConstantsProjectGeneration
{
    public class Context
    {
        #region Constructors
        public Context()
        {
            var configFilePath = Path.GetDirectoryName(typeof(Context).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.ConstantsProjectGeneration.json";

            Config   = JsonHelper.Deserialize<ConfigurationContract>(System.IO.File.ReadAllText(configFilePath));
            Database = new SqlDatabase(Config.ConnectionString) {CommandTimeout = 1000 * 60 * 60};

            MsBuildQueue = new MsBuildQueue
            {
                Trace   = trace => { ProcessInfo.Text = trace; },
                OnError = error => { Errors.Add(error.ToString()); }
            };
        }
        #endregion

        #region Public Properties
        public ConfigurationContract   Config            { get; }
        public Database             Database          { get; }
        public IReadOnlyList<string>   EnumClassNameList { get; set; }
        public IReadOnlyList<EnumInfo> EnumInfoList      { get; set; }
        public AddOnlyList<string>     Errors            { get; } = new AddOnlyList<string>();
        public PaddedStringBuilder     File              { get; } = new PaddedStringBuilder();
        public FileSystem              FileSystem        { get; } = new FileSystem();
        public MsBuildQueue            MsBuildQueue      { get; }
        public ProcessContract         ProcessInfo       { get; } = new ProcessContract();
        #endregion
    }
}