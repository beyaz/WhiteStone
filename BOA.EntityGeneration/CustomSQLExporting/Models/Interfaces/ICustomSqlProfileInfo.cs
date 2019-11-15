using System.Collections.Generic;

namespace BOA.EntityGeneration.CustomSQLExporting.Models.Interfaces
{

    
    
    public interface ICustomSqlProfileInfo
    {
        #region Public Properties
        
        IReadOnlyList<string> ObjectIdList { get; set; }
        #endregion
    }

   
}