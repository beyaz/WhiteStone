using System.Collections.Generic;
using System.Linq;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModelDao;

namespace BOA.EntityGeneration.Generators
{
    public class GeneratorDataCreator
    {
        #region Public Methods
        public static GeneratorData Create(TableInfo tableInfo, IDatabase database)
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
            const bool isSupportInsert = true;
            var        isSupportUpdate = tableInfo.PrimaryKeyColumns.Any();

            var isSupportSelectByKey         = tableInfo.PrimaryKeyColumns.Any();
            var isSupportSelectByUniqueIndex = uniqueIndexIdentifiers.Any();
            var isSupportSelectByIndex       = nonUniqueIndexIdentifiers.Any();

            var interfaces = new List<string>
            {
                Names.ISupportDmlOperationInsert
            };

            if (isSupportUpdate)
            {
                interfaces.Add(Names.ISupportDmlOperationUpdate);
            }

            if (isSupportUpdate)
            {
                interfaces.Add(Names.ISupportDmlOperationDelete);
            }

            if (isSupportGetAll)
            {
                interfaces.Add(Names.ISupportDmlOperationSelectAll);
            }

            if (isSupportSelectByKey)
            {
                interfaces.Add(Names.ISupportDmlOperationSelectByKey);
            }

            if (isSupportSelectByUniqueIndex)
            {
                interfaces.Add(Names.ISupportDmlOperationSelectByUniqueIndex);
            }

            if (isSupportSelectByIndex)
            {
                interfaces.Add(Names.ISupportDmlOperationSelectByIndex);
            }

            var sequenceName = "SEQ_" + tableInfo.TableName;

            var hasSequenceInDatabase = database.HasSequenceLike(tableInfo.SchemaName, sequenceName);

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
                ContractInterfaces           = interfaces,
                UniqueIndexIdentifiers       = uniqueIndexIdentifiers,
                NonUniqueIndexIdentifiers    = nonUniqueIndexIdentifiers,
                TableInfo                    = tableInfo,
                NamespaceFullName            = $"BOA.Types.Kernel.Card.{tableInfo.SchemaName}",
                IsSupportGetAll              = isSupportGetAll,
                IsSupportInsert              = isSupportInsert,
                IsSupportUpdate              = isSupportUpdate,
                IsSupportSelectByKey         = isSupportSelectByKey,
                IsSupportSelectByIndex       = isSupportSelectByIndex,
                IsSupportSelectByUniqueIndex = isSupportSelectByUniqueIndex,
                DatabaseEnumName             = tableInfo.CatalogName,
                SequenceName                 = hasSequence ? tableInfo.SchemaName + "." + sequenceName : null
            };
        }
        #endregion
    }
}