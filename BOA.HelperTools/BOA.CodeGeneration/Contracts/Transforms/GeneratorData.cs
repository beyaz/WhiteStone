﻿using System.Collections.Generic;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class GeneratorData
    {
        #region Public Properties
        public bool IsSupportGetAll { get; set; }

        public bool IsSupportSave { get; set; }

        public string    NamespaceFullName { get; set; }
        public TableInfo TableInfo         { get; set; }
        public string DatabaseEnumName { get; set; }
        public IReadOnlyList<IndexIdentifier> UniqueIndexIdentifiers { get; set; }
        public bool IsSupportSelectByKey { get; set; }
        public bool IsSupportSelectByIndex { get; set; }
        public bool IsSupportSelectByUniqueIndex { get; set; }
        public IReadOnlyList<string> ContractInterfaces { get; set; }
        public IReadOnlyList<IndexIdentifier> NonUniqueIndexIdentifiers { get; set; }
        public bool IsSupportInsert { get; set; }
        public bool IsSupportUpdate { get; set; }
        #endregion
    }
}