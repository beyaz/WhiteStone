using System.Collections.Generic;
using System.Linq;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class IndexIdentifier
    {
        public string Name { get; set; }
        public bool IsUnique { get; set; }
        public string TypeName { get; set; }
        public IndexInfo IndexInfo { get; set; }
    }
    public class GeneratorDataCreator
    {
        #region Public Methods
        public static GeneratorData Create(TableInfo tableInfo)
        {
            var items = new List<IndexIdentifier>();

            foreach (var indexInfo in tableInfo.IndexInfoList)
            {
                if (indexInfo.IsUnique)
                {
                    items.Add(new IndexIdentifier
                    {
                        Name     = "UniqueIndexOn" + string.Join("And", indexInfo.ColumnNames.Select(x => x.ToContractName())),
                        IsUnique = true,
                        TypeName = "UniqueIndex",
                        IndexInfo = indexInfo
                    });
                }
                else
                {
                    items.Add(new IndexIdentifier
                    {
                        IndexInfo = indexInfo
                        Name     = "IndexOn" + string.Join("And", indexInfo.ColumnNames.Select(x => x.ToContractName())),
                        IsUnique = false,
                        TypeName = "Index"
                    });
                }
                
                
            }

            return new GeneratorData
            {
                IndexIdentifiers = items,
                TableInfo         = tableInfo,
                NamespaceFullName = $"BOA.Types.Kernel.Card.{tableInfo.SchemaName}",
                IsSupportGetAll   = tableInfo.SchemaName == "PRM",
                IsSupportSave     = tableInfo.PrimaryKeyColumns.Any(),
                IsSupportSelectByKey = tableInfo.PrimaryKeyColumns.Any(),
                DatabaseEnumName =tableInfo.CatalogName
            };
        }
        #endregion
    }
}