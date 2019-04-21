using System.Linq;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class GeneratorDataCreator
    {
        #region Public Methods
        public static GeneratorData Create(TableInfo tableInfo)
        {
            return new GeneratorData
            {
                TableInfo         = tableInfo,
                NamespaceFullName = $"BOA.Types.Kernel.Card.{tableInfo.SchemaName}",
                IsSupportGetAll   = tableInfo.SchemaName == "PRM",
                IsSupportSave     = tableInfo.PrimaryKeyColumns.Any(),
                DatabaseEnumName =tableInfo.CatalogName
            };
        }
        #endregion
    }
}