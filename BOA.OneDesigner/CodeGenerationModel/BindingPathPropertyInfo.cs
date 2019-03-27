namespace BOA.OneDesigner.CodeGeneration
{
    class BindingPathPropertyInfo
    {
        public bool IsString          { get; set; }
        public bool IsDecimal         { get; set; }
        public bool IsDecimalNullable { get; set; }
        public bool IsBoolean         { get; set; }
        public bool IsDateTime        { get; set; }
        public bool IsNullableNumber  { get; set; }
        public bool IsNonNullableNumber { get; set; }
        public bool IsValueType{ get; set; }
    }
}