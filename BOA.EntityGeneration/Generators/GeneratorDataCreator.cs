using System.Linq;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModelDao;
using Ninject;

namespace BOA.EntityGeneration.Generators
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
            var uniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && x.IsUnique).Select(indexInfo => new IndexIdentifier
            {
                Name      = "UniqueIndexOn" + string.Join("And", indexInfo.ColumnNames.Select(x => x.ToContractName())),
                IsUnique  = true,
                TypeName  = "UniqueIndex",
                IndexInfo = indexInfo
            }).ToList();

            var nonUniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && !x.IsUnique).Select(indexInfo => new IndexIdentifier
            {
                Name      = "IndexOn" + string.Join("And", indexInfo.ColumnNames.Select(x => x.ToContractName())),
                IsUnique  = true,
                TypeName  = "Index",
                IndexInfo = indexInfo
            }).ToList();

            var        isSupportGetAll = tableInfo.SchemaName == "PRM";

            var isSupportSelectByKey         = tableInfo.PrimaryKeyColumns.Any();
            var isSupportSelectByUniqueIndex = uniqueIndexIdentifiers.Any();
            var isSupportSelectByIndex       = nonUniqueIndexIdentifiers.Any();

            var sequenceName = "SEQ_" + tableInfo.TableName;

            var hasSequenceInDatabase = Database.HasSequenceLike(tableInfo.SchemaName, sequenceName);

            var hasSequence = false;
            if (hasSequenceInDatabase && tableInfo.PrimaryKeyColumns.Count == 1)
            {
                if (tableInfo.PrimaryKeyColumns[0].ColumnName == Names.RECORD_ID)
                {
                    hasSequence = true;
                }
            }

            return new GeneratorData
            {
                UniqueIndexIdentifiers          = uniqueIndexIdentifiers,
                NonUniqueIndexIdentifiers       = nonUniqueIndexIdentifiers,
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