using System;
using System.Collections.Generic;

namespace BOA.CodeGeneration.Contracts
{
    [Serializable]
    public class IndexInfo
    {
        #region Public Properties
        public IReadOnlyList<string> ColumnNames    { get; set; }
        public bool                  IsClustered    { get; set; }
        public bool                  IsNonClustered { get; set; }
        public bool                  IsPrimaryKey   { get; set; }
        public bool                  IsUnique       { get; set; }
        public string                Name           { get; set; }
        #endregion
    }
}