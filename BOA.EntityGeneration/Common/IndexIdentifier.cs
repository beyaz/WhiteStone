using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.Common
{
    public class IndexIdentifier
    {
        #region Public Properties
        public IndexInfo IndexInfo { get; set; }
        public bool      IsUnique  { get; set; }
        public string    Name      { get; set; }
        public string    TypeName  { get; set; }
        #endregion
    }
}