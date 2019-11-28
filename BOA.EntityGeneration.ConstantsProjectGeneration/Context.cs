using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace BOA.EntityGeneration.ConstantsProjectGeneration
{
    public class Context
    {
        public FileSystem FileSystem { get; } = new FileSystem();
        #region Fields
        public   ConfigurationContract Config;
        internal SqlDatabase           Database;
        #endregion
        public ProcessContract processInfo { get; } = new ProcessContract();
        #region Constructors
        public Context()
        {
            var configFilePath = Path.GetDirectoryName(typeof(Context).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.ConstantsProjectGeneration.json";
            Config   = JsonHelper.Deserialize<ConfigurationContract>(System.IO.File.ReadAllText(configFilePath));
            Database = new SqlDatabase(Config.ConnectionString) {CommandTimeout = 1000 * 60 * 60};
        }
        #endregion

        #region Public Properties
        public IReadOnlyList<string>   EnumClassNameList { get; set; }
        public IReadOnlyList<EnumInfo> EnumInfoList      { get; set; }
        public PaddedStringBuilder     File              { get; } = new PaddedStringBuilder();
        #endregion
    }
}