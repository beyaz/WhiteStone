using System;
using System.Collections.Generic;
using BOA.DataFlow;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    [Serializable]
    public class ConfigurationContract
    {
        

        #region Public Properties
        public string SqlSequenceInformationOfTable { get; set; }
        public string                              ConnectionString                  { get; set; }
        public string                              CustomSQL_Get_SQL_Item_Info       { get; set; }
        public string                              CustomSQLNamesDefinedToProfileSql { get; set; }
        public string                              DatabaseEnumName                  { get; set; }
        public string                              EntityContractBase                { get; set; }
        public IReadOnlyDictionary<string, string> NamingPattern                     { get; set; } 
        public string                              SQL_GetProfileIdList              { get; set; }
        #endregion
    }
}