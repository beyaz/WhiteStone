using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces
{
    public interface ICustomSqlInfoResult
    {
        string           DataType         { get; set; }
        string           DataTypeInDotnet { get; set; }
        string           Name             { get; set; }
        string           NameInDotnet     { get; set; }
        SqlReaderMethods SqlReaderMethod  { get; set; }
        bool             IsNullable       { get; set; }
    }
}