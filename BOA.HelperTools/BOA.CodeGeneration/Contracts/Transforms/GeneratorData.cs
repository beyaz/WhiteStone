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
        #endregion
    }
}