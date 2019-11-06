using System.Collections.Generic;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models
{
    public interface IProjectCustomSqlInfo
    {
        IReadOnlyList<CustomSqlInfo> CustomSqlInfoList       { get; set; }
        string                       NamespaceNameOfType     { get; set; }
        string                       NamespaceNameOfBusiness { get; set; }
        string                       TypesProjectPath        { get; set; }
        string                       BusinessProjectPath     { get; set; }
        string                       ProfileId               { get; set; }
    }
}