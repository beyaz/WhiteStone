using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    public class GeneratorDataCreator
    {
        #region Public Properties
        [Inject]
        public IDatabase Database { get; set; }

        [Inject]
        public TableOverride TableOverride { get; set; }
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
            data.SequenceList = Database.GetSequenceListOfTable(tableInfo.SchemaName, tableInfo.TableName)
                                        .Where(x => tableInfo.Columns.Any(c => c.ColumnName == x.TargetColumnName)).ToList();

            TableOverride.Override(data);

            return data;
        }
        #endregion
    }
}