using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;

namespace BOA.EntityGeneration.ConstantsProjectGeneration
{
    public class Context
    {
        #region Fields
        public   ConfigurationContract Config;
        internal SqlDatabase           Database;
        #endregion

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