using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.Generators
{
    public class GeneratorBase
    {
        public const string PaddingForComment = "     ";
        #region Public Properties
        public GeneratorData Data      { get; set; }
        public TableInfo     TableInfo => Data.TableInfo;
        #endregion

        #region Public Methods
        public TGenerator Create<TGenerator>() where TGenerator : GeneratorBase, new()
        {
            return new TGenerator
            {
                Data = Data
            };
        }
        #endregion
    }
}