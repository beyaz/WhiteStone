using System.Linq;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class GeneratorDataCreator
    {
        #region Public Properties
        [Inject]
        public IDatabase Database { get; set; }
        #endregion

        #region Public Methods
        public GeneratorData Create(TableInfo tableInfo)
        {
            var uniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && x.IsUnique).ToList();

            var nonUniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && !x.IsUnique).ToList();

            var isSupportGetAll = tableInfo.SchemaName == "PRM";

            var isSupportSelectByKey         = tableInfo.PrimaryKeyColumns.Any();
            var isSupportSelectByUniqueIndex = uniqueIndexIdentifiers.Any();
            var isSupportSelectByIndex       = nonUniqueIndexIdentifiers.Any();

            var sequenceName = "SEQ_" + tableInfo.TableName;

            var hasSequenceInDatabase = Database.HasSequenceLike(tableInfo.SchemaName, sequenceName);

            var hasSequence = false;
            if (hasSequenceInDatabase && tableInfo.PrimaryKeyColumns.Count == 1)
            {
                if (tableInfo.PrimaryKeyColumns[0].ColumnName == Names2.RECORD_ID)
                {
                    hasSequence = true;
                }
            }

            return new GeneratorData
            {
                UniqueIndexInfoList             = uniqueIndexIdentifiers,
                NonUniqueIndexInfoList          = nonUniqueIndexIdentifiers,
                TableInfo                       = tableInfo,
                NamespaceFullNameOfTypeAssembly = $"BOA.Types.Kernel.Card.{tableInfo.SchemaName}",
                IsSupportGetAll                 = isSupportGetAll,
                IsSupportSelectByKey            = isSupportSelectByKey,
                IsSupportSelectByIndex          = isSupportSelectByIndex,
                IsSupportSelectByUniqueIndex    = isSupportSelectByUniqueIndex,
                DatabaseEnumName                = tableInfo.CatalogName,
                SequenceName                    = hasSequence ? tableInfo.SchemaName + "." + sequenceName : null
            };
        }
        #endregion
    }
}