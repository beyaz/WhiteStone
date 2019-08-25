using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models;
using Ninject;
using WhiteStone.Helpers;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    public class GeneratorDataCreator
    {
        #region Public Properties
        [Inject]
        public IDatabase Database { get; set; }

        [Inject]
        public TableOverride TableOverride { get; set; }

        [Inject]
        public Config Config { get; set; }
        #endregion

        #region Public Methods
        public TableInfo Create(DbModel.TableInfo tableInfo)
        {
            var uniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && x.IsUnique).ToList();

            var nonUniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && !x.IsUnique).ToList();


            var isSupportSelectByKey         = tableInfo.PrimaryKeyColumns.Any();
            var isSupportSelectByUniqueIndex = uniqueIndexIdentifiers.Any();
            var isSupportSelectByIndex       = nonUniqueIndexIdentifiers.Any();

            var data = JsonHelper.Deserialize<TableInfo>(JsonHelper.Serialize(tableInfo));

            data.UniqueIndexInfoList          = uniqueIndexIdentifiers;
            data.NonUniqueIndexInfoList       = nonUniqueIndexIdentifiers;
            data.IsSupportSelectByKey         = isSupportSelectByKey;
            data.IsSupportSelectByIndex       = isSupportSelectByIndex;
            data.IsSupportSelectByUniqueIndex = isSupportSelectByUniqueIndex;
            data.DatabaseEnumName             = tableInfo.CatalogName;

            if (Config.SqlSequenceInformationOfTable == null)
            {
                data.SequenceList = new List<SequenceInfo>();
            }
            else
            {
                Database.CommandText = Config.SqlSequenceInformationOfTable;
                Database["schema"]    = tableInfo.SchemaName;
                Database["tableName"] = tableInfo.TableName;

                data.SequenceList = Database.ExecuteReader().ToList<SequenceInfo>().Where(x => tableInfo.Columns.Any(c => c.ColumnName == x.TargetColumnName)).ToList();    
 
            }

            

            TableOverride.Override(data);

            return data;
        }
        #endregion
    }
}