using System.IO;
using BOA.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne
{
    /// <summary>
    ///     All business classes in one
    /// </summary>
    public class AllBusinessClassesInOne
    {
        #region Public Methods
        public static void BeginNamespace(IDataContext context)
        {
            var sb         = context.Get(Data.BoaRepositoryFile);
            var schemaName = context.Get(Data.SchemaName);
            var config     = context.Get(Data.Config);

            sb.AppendLine();
            sb.BeginNamespace(NamingHelper.GetBusinessClassNamespace(schemaName,config));
            ObjectHelperSqlUtilClassWriter.Write(sb,config);

            UtilClass(sb, config);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Utilities the class.
        /// </summary>
        static void UtilClass(PaddedStringBuilder sb, ConfigContract config)
        {
            var path = Path.GetDirectoryName(typeof(AllBusinessClassesInOne).Assembly.Location) + Path.DirectorySeparatorChar + "EmbeddedUtilizationClassForDao.txt";

            var code = FileHelper.ReadFile(path);

            code = code.Replace("{DatabaseEnumName}", config.DatabaseEnumName);
            sb.AppendAll(code.Trim());
            sb.AppendLine();
        }
        #endregion
    }
}