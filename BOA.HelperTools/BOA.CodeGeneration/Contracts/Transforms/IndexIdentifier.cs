namespace BOA.CodeGeneration.Contracts.Transforms
{
    public class IndexIdentifier
    {
        public string    Name      { get; set; }
        public bool      IsUnique  { get; set; }
        public string    TypeName  { get; set; }
        public IndexInfo IndexInfo { get; set; }
    }
}