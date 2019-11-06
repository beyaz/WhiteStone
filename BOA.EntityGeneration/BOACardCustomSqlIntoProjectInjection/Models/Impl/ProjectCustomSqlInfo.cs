using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models
{
    [Serializable]
    public class ProjectCustomSqlInfo : IProjectCustomSqlInfo
    {
        public IReadOnlyList<CustomSqlInfo> CustomSqlInfoList       { get; set; }
        public string                       NamespaceNameOfType     { get; set; }
        public string                       NamespaceNameOfBusiness { get; set; }
        public string                       TypesProjectPath        { get; set; }
        public string                       BusinessProjectPath     { get; set; }
        public string ProfileId { get; set; }
    }
}