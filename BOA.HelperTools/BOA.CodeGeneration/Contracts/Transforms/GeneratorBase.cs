using System.Linq;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class GeneratorBase
    {
        public TGenerator Create<TGenerator>() where TGenerator : GeneratorBase, new()
        {
            return new TGenerator
            {
                Data = Data
            };
        }
        #region Public Properties
        public GeneratorData Data      { get; set; }
        public TableInfo     TableInfo => Data.TableInfo;
        #endregion
    }

    public class GeneratorData
    {
        #region Public Properties
        public bool IsSupportGetAll { get; set; }

        public bool IsSupportSave { get; set; }

        public string    NamespaceFullName { get; set; }
        public TableInfo TableInfo         { get; set; }
        #endregion
    }

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
                IsSupportSave     = tableInfo.PrimaryKeyColumns.Any()
            };
        }
        #endregion
    }
}