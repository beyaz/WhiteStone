using System.IO;
using BOA.Common.Helpers;
using BOA.EntityGeneration.CustomSQLExporting.Models;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    public class CustomSqlExporterConfig
    {
        public string SlnDirectoryPath { get; set; }
        public string EntityNamespace { get; set; }
        public string RepositoryNamespace { get; set; }
        public string EntityProjectDirectory { get; set; }
        public string RepositoryProjectDirectory { get; set; }
        public CustomSqlNamingPatternContract CustomSqlNamingPattern { get; set; }
            

        public string InputClassName     { get; set; }
        public string ResultClassName    { get; set; }
        public string RepositoryClassName{ get; set; }

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the custom SQL get SQL item information.
        /// </summary>
        public string CustomSQL_Get_SQL_Item_Info { get; set; }

        /// <summary>
        ///     Gets or sets the custom SQL names defined to profile SQL.
        /// </summary>
        public string CustomSQLNamesDefinedToProfileSql { get; set; }



        
        /// <summary>
        ///     Gets or sets the SQL get profile identifier list.
        /// </summary>
        public string SQL_GetProfileIdList { get; set; }

        public static CustomSqlExporterConfig CreateFromFile(string filePath)
        {
            return YamlHelper.DeserializeFromFile<CustomSqlExporterConfig>(filePath);
        }

        public static CustomSqlExporterConfig CreateFromFile()
        {
            return CreateFromFile(string.Join(Path.DirectorySeparatorChar.ToString(),nameof(Wrapper),nameof(CustomSqlExporterConfig)+".yaml"));
        }
    }
}