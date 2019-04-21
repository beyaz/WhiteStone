namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class GeneratorBase
    {
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