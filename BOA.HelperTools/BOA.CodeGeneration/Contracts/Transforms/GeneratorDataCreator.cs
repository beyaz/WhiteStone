using System.Collections.Generic;
using System.Linq;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class GeneratorDataCreator
    {
        #region Public Methods
        public static GeneratorData Create(TableInfo tableInfo)
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
            // ReSharper disable once RedundantLogicalConditionalExpressionOperand
            var isSupportSave                = isSupportInsert && isSupportUpdate;
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

            if (isSupportSave)
            {
                // remove already supported interfaces
                interfaces.Remove(Names.ISupportDmlOperationUpdate);
                interfaces.Remove(Names.ISupportDmlOperationInsert);

                interfaces.Add(Names.ISupportDmlOperationSave);
            }

            if (isSupportSave)
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

            return new GeneratorData
            {
                ContractInterfaces           = interfaces,
                UniqueIndexIdentifiers       = uniqueIndexIdentifiers,
                NonUniqueIndexIdentifiers    = nonUniqueIndexIdentifiers,
                TableInfo                    = tableInfo,
                NamespaceFullName            = $"BOA.Types.Kernel.Card.{tableInfo.SchemaName}",
                IsSupportGetAll              = isSupportGetAll,
                IsSupportInsert = isSupportInsert,
                IsSupportUpdate = isSupportUpdate,
                IsSupportSave                = isSupportSave,
                IsSupportSelectByKey         = isSupportSelectByKey,
                IsSupportSelectByIndex       = isSupportSelectByIndex,
                IsSupportSelectByUniqueIndex = isSupportSelectByUniqueIndex,
                DatabaseEnumName             = tableInfo.CatalogName
            };
        }
        #endregion

        #region Methods
        static bool ISupportInsert()
        {
            return true;
        }
        #endregion
    }
}