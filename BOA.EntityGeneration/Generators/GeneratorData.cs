using System.Collections.Generic;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.Generators
{
    public class GeneratorData
    {
        #region Public Properties
        public IReadOnlyList<string> ContractInterfaces { get; set; }
        public string                DatabaseEnumName   { get; set; }
        public bool                  IsSupportGetAll    { get; set; }
        public bool                  IsSupportInsert    { get; set; }

        public bool IsSupportSelectByIndex       { get; set; }
        public bool IsSupportSelectByKey         { get; set; }
        public bool IsSupportSelectByUniqueIndex { get; set; }
        public bool IsSupportUpdate              { get; set; }

        public string                         NamespaceFullName         { get; set; }
        public IReadOnlyList<IndexIdentifier> NonUniqueIndexIdentifiers { get; set; }
        public TableInfo                      TableInfo                 { get; set; }
        public IReadOnlyList<IndexIdentifier> UniqueIndexIdentifiers    { get; set; }
        #endregion
    }
}