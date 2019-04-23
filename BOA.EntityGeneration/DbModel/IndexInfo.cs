using System;
using System.Collections.Generic;
using System.Linq;

namespace BOA.EntityGeneration.DbModel
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

        #region Public Methods
        public override string ToString()
        {
            var properties = new List<string>();

            if (IsPrimaryKey)
            {
                properties.Add("PrimaryKey");
            }

            if (IsUnique)
            {
                properties.Add("Unique");
            }

            if (IsClustered)
            {
                properties.Add("Clustered");
            }

            if (IsNonClustered)
            {
                properties.Add("NonClustered");
            }

            return $"[ {string.Join(" + ", properties)} ] index on {string.Join(" and ", ColumnNames.Select(x=>'"'+x+'"'))}";
        }
        #endregion
    }
}